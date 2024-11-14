using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructionCore : InputHandlerBase, IStartExecution
{
  #region Variables

  public bool EnableBuilding;
  public bool EnableRotateOnRightClick;

  [Header("Construction Data")]
  [SerializeField] private ConstructableObjectData[] ConstructableObjects;
  [SerializeField] private int DefaultObjectIndex;

  [Header("General settings")]
  [SerializeField] private float _minBuildDistance = 1f;
  [SerializeField] private float _maxBuildDistance = 5f;
  [SerializeField] private LayerMask _groundLayer;
  [SerializeField] private LayerMask _constructionLayer;
  [SerializeField] private LayerMask _playerLayer;
  [SerializeField] private Material _validMaterial;
  [SerializeField] private Material _invalidMaterial;
  [SerializeField] private Material _destroyMaterial;

  [Header("Wall settings")]
  [SerializeField] private float _wallSnapDistance = 3f;
  [SerializeField] private float _wallAngleTolarence = 35f;

  private int _currentStructureIndex;
  private ConstructableObjectData _currentStructureData;
  private GameObject _tempObject;
  private GameObject _highlightedObject;
  private Material _highlightedObjectMaterial;
  private Camera _stashedCamera;

  private TEMPORARYPlayerLookNoMultiplayer _playerLook;

  private bool _canPlace;
  [SerializeField] private bool _isRotating;
  [SerializeField] private bool _isDeleting;

  #endregion

  protected override void InitializeActionMap()
  {
    _actionMap = new Dictionary<InputAction, Action<InputAction.CallbackContext>>();

    RegisterAction(_inputActions.Player.Build, _ => ToggleBuilding());
    RegisterAction(_inputActions.Player.Rotate, _ => ToggleRotating(true), () => ToggleRotating(false));
    RegisterAction(_inputActions.Player.Delete, _ => ToggleDeleting());

    RegisterAction(_inputActions.Player.Attack, _ => OnClick());

    // TODO: Implement this
    // RegisterAction(_inputActions.Player.Scroll, ctx => ChangeActiveObject((int)ctx.ReadValue<float>()));
    RegisterAction(_inputActions.Player.KeyOne, _ => ChangeActiveObject(0));
    RegisterAction(_inputActions.Player.KeyTwo, _ => ChangeActiveObject(1));
    RegisterAction(_inputActions.Player.KeyThree, _ => ChangeActiveObject(2));
    RegisterAction(_inputActions.Player.KeyFour, _ => ChangeActiveObject(3));
    RegisterAction(_inputActions.Player.KeyFive, _ => ChangeActiveObject(4));
    RegisterAction(_inputActions.Player.KeySix, _ => ChangeActiveObject(5));
  }

  private void ToggleBuilding()
  {
    EnableBuilding = !EnableBuilding;

    if (!EnableBuilding)
    {
      _isRotating = false;
      _isDeleting = false;
      TryDestroyTempObject();
      // Enable player looking
    }
  }

  private void ToggleRotating(bool enable)
  {
    if (!EnableBuilding) return;

    _isRotating = enable;
    _isDeleting = false;

    _playerLook.canLook = !enable;
  }

  private void ToggleDeleting()
  {
    if (!EnableBuilding) return;
    if (_isRotating) return;

    _isDeleting = !_isDeleting;
  }

  public void InitializeStart()
  {
    _stashedCamera = ResourceHolder.Instance.MainCamera;
    ChangeActiveObject(DefaultObjectIndex);

    _playerLook = GetComponent<TEMPORARYPlayerLookNoMultiplayer>();
  }

  private void Update()
  {
    if (!EnableBuilding) return;

    if (_isDeleting)
    {
      TryDestroyTemporaryObject();
      TryHighlightObject();
    } else {
      TryUnhighlightObject();
      PlaceTemporaryObject();
    }
  }

  private void PlaceTemporaryObject()
  {
    Ray ray = _stashedCamera.ScreenPointToRay(Input.mousePosition);

    // heightOffset doesn't work with props
    if (_currentStructureData.Type != ConstructableObjectData.ConstructableType.Prop)
    {
      // Shift the ray based on heightOffset
      float heightOffset = _currentStructureData.HeightOffset;
      Vector3 adjustedOrigin = ray.origin - _stashedCamera.transform.up * heightOffset;
      ray.origin = adjustedOrigin;
    }

    RaycastHit hit;
    // Custom raycast logic to handle placeing structures without needing to look at the ground
    if (TryRaycast(ray, out hit))
    {
      TryCreateTemporaryObject();

      // Rotate the temp object to face the player
      Vector3 directionTowardsCamera = _stashedCamera.transform.position - hit.point;
      directionTowardsCamera.y = 0;
      Quaternion offsetRotation = Quaternion.LookRotation(directionTowardsCamera) * Quaternion.Euler(_currentStructureData.RotationOffset);

      if (_isRotating)
      {
        _tempObject.GetComponent<ConstructionTempObject>().xRotation += Input.GetAxis("Mouse X") * 2; // Rotation speed
        _tempObject.GetComponent<ConstructionTempObject>().SetPositionAndRotation(hit.point + _currentStructureData.PositionOffset, offsetRotation);
      }
      else
      {
        _tempObject.GetComponent<ConstructionTempObject>().SetPositionAndRotation(hit.point + _currentStructureData.PositionOffset, offsetRotation);
      }


      if (_currentStructureData.Type == ConstructableObjectData.ConstructableType.Wall)
      {
        // If the current structure is a wall, snap to the wall
        if (NearOtherWall(hit.point, out ConstructionWall wallScript))
        {
          // Handles checking if the wall is placeable
          SnapToWall(hit.point, wallScript);
        }
        else
        {
          // Else treat it as a normal structure
          if (CanPlaceStructure())
          {
            _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_validMaterial);
            _canPlace = true;
          }
          else
          {
            _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_invalidMaterial);
            _canPlace = false;
          }

        }
      }
      else
      {
        // If the current structure is not a wall, check if it can be placed
        if (CanPlaceStructure())
        {
          _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_validMaterial);
          _canPlace = true;
        }
        else
        {
          _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_invalidMaterial);
          _canPlace = false;
        }
      }

    }
    else
    {
      // This shouldn't usually happen, but it's fine if it does
      TryDestroyTemporaryObject();
    }
  }

  private bool TryRaycast(Ray ray, out RaycastHit hit)
  {
    // Get the current 'player' position
    Vector2 currPositionVector2 = new Vector2(_stashedCamera.transform.position.x, _stashedCamera.transform.position.z);

    // Default test distance to max build distance
    float testDistance = _maxBuildDistance;

    RaycastHit tempHit;

    LayerMask effectiveLayer = _currentStructureData.Type == ConstructableObjectData.ConstructableType.Prop ? (_groundLayer | _constructionLayer) : _groundLayer;

    // Raycast from the mouse position to the ground, arbitrary ray distance
    if (Physics.Raycast(ray, out tempHit, 100f, effectiveLayer))
    {
      // If collides with ground, check if it's within the min and max distance
      Vector2 hitPositionVector2 = new Vector2(tempHit.point.x, tempHit.point.z);
      float distance = Vector2.Distance(currPositionVector2, hitPositionVector2);

      // Handle min and max distance
      if (distance < _minBuildDistance)
      {
        testDistance = _minBuildDistance;
      }
      else if (distance > _maxBuildDistance)
      {
        testDistance = _maxBuildDistance;
      }
      else
      {
        hit = tempHit;
        return true;
      }
    }

    Vector3 cameraForward = _stashedCamera.transform.forward;
    if (Mathf.Approximately(cameraForward.y, 1.0f) || Mathf.Approximately(cameraForward.y, -1.0f))
    {
      // If the camera y is close enough to 1 or -1, don't allow building
      hit = new RaycastHit();
      return false;
    }
    cameraForward.y = 0;
    cameraForward = cameraForward.normalized * testDistance;
    Vector3 downRayOrigin = _stashedCamera.transform.position + cameraForward;
    // If the raycast didn't hit anything, try a raycast from the camera to the ground
    if (Physics.Raycast(downRayOrigin, Vector3.down, out hit, 100f, _groundLayer))
    {
      return true;
    }

    return false;
  }

  private void OnClick()
  {
    if (!EnableBuilding) return;

    if (_canPlace)
    {
      PlacePermanentObject();
    } else if (_isDeleting)
    {
      TryDestroyObject();
    }
  }

  private void TryCreateTemporaryObject () {
    if (_tempObject == null) {
      _tempObject = Instantiate(_currentStructureData.TemporaryPrefab);
    }
  }

  private void TryHighlightObject() {
    Ray ray = _stashedCamera.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 100f, _constructionLayer)) {
      if (_highlightedObject != hit.collider.gameObject) {
        if (_highlightedObject != null) {
          _highlightedObject.GetComponent<Renderer>().material = _highlightedObjectMaterial;
        }
        _highlightedObject = hit.collider.gameObject;
        _highlightedObjectMaterial = _highlightedObject.GetComponent<Renderer>().material;
        _highlightedObject.GetComponent<Renderer>().material = _destroyMaterial;
      }
    } else {
      TryUnhighlightObject();
    }
  }

  private void TryUnhighlightObject() {
    if (_highlightedObject != null) {
      _highlightedObject.GetComponent<Renderer>().material = _highlightedObjectMaterial;
      _highlightedObject = null;
      _highlightedObjectMaterial = null;
    }
  }

  private void TryDestroyTemporaryObject() {
    if (_tempObject != null) {
      Destroy(_tempObject);
      _tempObject = null;
    }
  }

  private void TryDestroyObject() {
    if (_highlightedObject != null) {
      Destroy(_highlightedObject);
      _highlightedObject = null;
    }
  }

  private bool NearOtherWall(Vector3 position, out ConstructionWall wallScript) {
    Collider[] colliders = Physics.OverlapSphere(position, _wallSnapDistance, _constructionLayer);
    wallScript = null;

    float closestDistance = float.MaxValue;
    foreach (Collider c in colliders) {
      // Skip if the collider is not a wall
      if (c.GetComponent<ConstructionWall>() == null) {
        continue;
      }
      ConstructionWall tempWall = c.GetComponent<ConstructionWall>();

      Vector3 closestJoint = tempWall.GetClosestJointWorldSpace(position);
      float distance = Vector3.Distance(closestJoint, position);
      if (distance < closestDistance) {
        closestDistance = distance;
        wallScript = tempWall;
      }
    }

    if (closestDistance == float.MaxValue) {
      return false;
    } else {
      return true;
    }
  } 

  private int stashedTempJointIndex = 0;
  private bool SnapToWall(Vector3 position, ConstructionWall wallScript) {
    Vector3 closestOtherWallJoint = wallScript.GetClosestJointWorldSpace(position, out int otherJointIndex);
    Vector3 closestTempWallJoint;
    int tempJointIndex;
    if (_isRotating) {
      closestTempWallJoint = _tempObject.GetComponent<ConstructionWall>().GetJointWorldSpace(stashedTempJointIndex);
      tempJointIndex = stashedTempJointIndex;
    } else {
      closestTempWallJoint = _tempObject.GetComponent<ConstructionWall>().GetClosestJointWorldSpace(closestOtherWallJoint, out tempJointIndex);
      stashedTempJointIndex = tempJointIndex;
    }

    Vector3 tempWallOffset = _tempObject.transform.position - closestTempWallJoint;

    _tempObject.transform.position = closestOtherWallJoint + tempWallOffset;

    Vector3 otherWallDirection = wallScript.GetJointWorldSpace((otherJointIndex == 0) ? 0 : 1) - wallScript.GetJointWorldSpace((otherJointIndex == 0) ? 1 : 0);
    Vector3 tempWallDirection = _tempObject.GetComponent<ConstructionWall>().GetJointWorldSpace((tempJointIndex == 0) ? 0 : 1) - _tempObject.GetComponent<ConstructionWall>().GetJointWorldSpace((tempJointIndex == 0) ? 1 : 0);

    float angle = Vector3.Angle(otherWallDirection, tempWallDirection);
    if (angle > _wallAngleTolarence) {
      if (CanPlaceStructure(ignoreTransform: wallScript.transform)) {
        _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_validMaterial);
        _canPlace = true;
      } else {
        _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_invalidMaterial);
        _canPlace = false;
      }
    } else {
      _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_invalidMaterial);
      _canPlace = false;
    }

    return true;
  }

  private bool CanPlaceStructure(LayerMask? layer = null, Transform ignoreTransform = null) {
    // If the structure is a wall, don't check for other walls
    LayerMask effectiveLayer = layer ?? (_constructionLayer | LayerMask.GetMask("Default")); // Not great practice but works
    return !_tempObject.GetComponent<ConstructionTempObject>().IsColliding(effectiveLayer, ignoreTransform);
  }

  private void PlacePermanentObject() {
    // Logic for placing an object (setting a parent, etc) goes here
    
    GameObject permanentObject = Instantiate(_currentStructureData.FinalPrefab, _tempObject.transform.position, _tempObject.transform.rotation);
    
    TryDestroyTempObject();
  }

  private void TryDestroyTempObject()
  {
    if (_tempObject != null)
    {
      Destroy(_tempObject);
      _tempObject = null;
    }
  }

  /// <summary>
  /// Changes the current wall type to the one at the specified index
  /// </summary>
  /// <param name="index">Wall index on ConstructionCore gameobject</param>
  public void ChangeActiveObject(int index)
  {
    if (index < 0 || index >= ConstructableObjects.Length)
    {
      Debug.LogError("Invalid structure index");
      return;
    }

    _currentStructureIndex = index;
    _currentStructureData = ConstructableObjects[_currentStructureIndex];
    if (_tempObject != null)
    {
      Destroy(_tempObject);
      _tempObject = null;
    }
  }
}
