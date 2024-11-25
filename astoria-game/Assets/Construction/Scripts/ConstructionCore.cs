using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConstructionCore : InputHandlerBase, IStartExecution
{
  public static ConstructionCore Instance => Singleton<ConstructionCore>.Instance;

  [SerializeField] private bool EnableDebugging; // Doesn't currently work

  #region Variables

  public bool HasObject; // If the player is currently holding an object

  [Header("Construction Data")]
  [SerializeField] private ConstructableObjectData[] ConstructableObjects;

  [Header("General settings")]
  [SerializeField] private float _minBuildDistance = 1f;
  [SerializeField] private float _maxBuildDistance = 5f;
  [SerializeField] private LayerMask _groundLayer;
  [SerializeField] private LayerMask _constructionLayer;
  [SerializeField] private Material _validMaterial;
  [SerializeField] private Material _invalidMaterial;
  [SerializeField] private Material _destroyMaterial;

  [Header("Wall settings")]
  [SerializeField] private float _wallSnapDistance = 3f;
  [SerializeField] private float _wallAngleTolarence = 35f;

  private int _currentStructureIndex;
  public ConstructableObjectData _currentStructureData;
  private GameObject _tempObject;
  private GameObject _heldObject;
  private GameObject _baseHighlightedObject; // This is the gameObject with the collider and the renderer on it
  private GameObject _highlightedObject; // This is the head parent of the baseHighlightedObject
  private Material _highlightedObjectMaterial;
  private Camera _stashedCamera;

  private PlayerCamera _playerLook;

  private bool _canPlace; // If the currrently held object can be placed
  [SerializeField] private bool _isRotating;
  [SerializeField] private bool _isDeleting;

  #endregion

  protected override void InitializeActionMap()
  {
    _actionMap = new Dictionary<InputAction, Action<InputAction.CallbackContext>>();

    RegisterAction(_inputActions.Player.Attack, _ => OnClick());

    RegisterAction(_inputActions.Player.Rotate, _ => ToggleRotating(true), () => ToggleRotating(false));
    RegisterAction(_inputActions.Player.Delete, _ => ToggleDeleting());

    // Debugging
    if (!EnableDebugging) return;
    
    RegisterAction(_inputActions.Player.Build, _ => ToggleBuilding());

    RegisterAction(_inputActions.Player.KeyOne, _ => ChangeActiveObject(0));
    RegisterAction(_inputActions.Player.KeyTwo, _ => ChangeActiveObject(1));
    RegisterAction(_inputActions.Player.KeyThree, _ => ChangeActiveObject(2));
    RegisterAction(_inputActions.Player.KeyFour, _ => ChangeActiveObject(3));
    RegisterAction(_inputActions.Player.KeyFive, _ => ChangeActiveObject(4));
    RegisterAction(_inputActions.Player.KeySix, _ => ChangeActiveObject(5));
  }

  private void ToggleBuilding()
  {
    HasObject = !HasObject;

    if (!HasObject)
    {
      _isRotating = false;
      _isDeleting = false;
      TryDestroyTempObject();
      TryDestroyHeldObject();
      _currentStructureData = null;
      _playerLook.canLook = true;
    }
  }

  private void ToggleRotating(bool enable)
  {
    if (!HasObject) return;

    _isRotating = enable;
    _isDeleting = false;

    _playerLook.canLook = !enable;
  }

  private void ToggleDeleting()
  {
    if (HasObject) return;

    _isDeleting = !_isDeleting;

    if (!_isDeleting)
    {
      TryUnhighlightObject();
    }
  }

  public void InitializeStart()
  {
    _stashedCamera = ResourceHolder.Instance.MainCamera;

    _playerLook = _stashedCamera.GetComponent<PlayerCamera>();
  }

  private void Update()
  {
    if (HasObject) {
      PlaceTemporaryObject();
    }

    if (_isDeleting) {
      TryHighlightObject();
    }
  }

  private void PlaceTemporaryObject()
  {
    Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    Ray ray = _stashedCamera.ScreenPointToRay(screenCenter); // This is probably fine, but might cause issues?

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

  private void CreateHeldObject() {
    if (_heldObject == null) {
      _heldObject = Instantiate(_currentStructureData.TemporaryPrefab);

      // Remove the box collider from the held object
      Collider[] colliders = _heldObject.GetComponentsInChildren<Collider>();
      foreach (Collider c in colliders) {
        Destroy(c);
      }

      _heldObject.transform.parent = _stashedCamera.transform;
      _heldObject.transform.localPosition = _currentStructureData.HeldOffsetPosition;
      _heldObject.transform.localRotation = Quaternion.Euler(_currentStructureData.HeldOffsetRotation);
    }
  }

  private void OnClick()
  {
    if (HasObject) {
      if (_canPlace) {
        PlacePermanentObject();

        // Reset player build state
        TryDestroyHeldObject();
        _currentStructureData = null;
        HasObject = false;
        _playerLook.canLook = true;
      }
    }

    if (_isDeleting) {
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
      if (_baseHighlightedObject != hit.collider.gameObject) {
        if (_baseHighlightedObject != null) {
          _baseHighlightedObject.GetComponent<Renderer>().material = _highlightedObjectMaterial;
        }
        _baseHighlightedObject = hit.collider.gameObject;
        _highlightedObject = _baseHighlightedObject.GetComponent<ReferenceParent>().ParentScript.gameObject;
        _highlightedObjectMaterial = _baseHighlightedObject.GetComponent<Renderer>().material;
        _baseHighlightedObject.GetComponent<Renderer>().material = _destroyMaterial;
      }
    } else {
      TryUnhighlightObject();
    }
  }

  private void TryUnhighlightObject() {
    if (_highlightedObject != null) {
      _baseHighlightedObject.GetComponent<Renderer>().material = _highlightedObjectMaterial;
      _baseHighlightedObject = null;
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

  private void TryDestroyHeldObject() {
    if (_heldObject != null) {
      Destroy(_heldObject);
      _heldObject = null;
    }
  }

  private void TryDestroyObject() {
    if (_highlightedObject != null) {
      
      // If the selected object is falling, ignore click
      if (_highlightedObject.GetComponent<ConstructionFallingObject>()) {
        return;
      }

      ConstructionPermObject script = _highlightedObject.GetComponent<ConstructionPermObject>();
      ConstructableObjectData objectType = script.Data;
      
      script.OnObjectRemoved();

      MakeObjectsFall(script.GetObjects(0));

      Destroy(_highlightedObject.gameObject);
      _highlightedObject = null;
      _highlightedObjectMaterial = null;
      _baseHighlightedObject = null;

      _isDeleting = false;
      TryGiveObject(objectType);
    }
  }

  private void MakeObjectsFall(List<GameObject> objects) {

    foreach (GameObject obj in objects) {
      ConstructableObjectData data = obj.GetComponent<ConstructionPermObject>().Data;
      GameObject fallingObject = Instantiate(data.FallingPrefab, obj.transform.position, obj.transform.rotation);

      fallingObject.GetComponent<ConstructionFallingObject>().Data = data;

      Destroy(obj);
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
    // if (_currentStructureData.CarveNavmesh) {
    //   NavMeshObstacle obstacle = permanentObject.AddComponent<NavMeshObstacle>();
    //   obstacle.carving = true;
    //   obstacle.radius = permanentObject.transform.GetChild(0).GetComponent<MeshRenderer>().bounds.size.x / 2;
    // }
    // If the object is a prop, call the OnPlaced method
    if (_currentStructureData.Type == ConstructableObjectData.ConstructableType.Prop) {
      permanentObject.GetComponent<ConstructionPermObject>().OnObjectPlaced();
    }

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

    _currentStructureData = null;
    TryDestroyHeldObject();
    TryDestroyTempObject();

    _currentStructureIndex = index;
    _currentStructureData = ConstructableObjects[_currentStructureIndex];
    
    CreateHeldObject();
  }

  /// <summary>
  /// Tries to give the player an object to place
  /// </summary>
  /// <param name="data">The object data to give the player</param>
  public void TryGiveObject(ConstructableObjectData data)
  {
    if (_currentStructureData != null) return;

    _isDeleting = false;
    TryUnhighlightObject();

    _currentStructureData = data;
    HasObject = true;

    CreateHeldObject();
  }

  public bool CanGiveObject(ConstructableObjectData data, out string errorText)
  {
    errorText = "";

    if (HasObject)
    {
      errorText = "You are already holding an object";
      return false;
    }

    if (data == null)
    {
      errorText = "No object selected";
      return false;
    }

    if (data.Cost.Count > 0)
    {
      foreach (ConstructionObjectCost cost in data.Cost)
      {
        if (!LocalPlayerReference.Instance.Inventory().ItemCountOrMoreInInventory(cost.Item, cost.Amount))
        {
          errorText = "You do not have the required resources";
          return false;
        }
      }
    }

    return true;
  }
}
