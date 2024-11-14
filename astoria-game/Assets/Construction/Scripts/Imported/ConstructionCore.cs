using UnityEngine;

[System.Serializable]
public struct Structure {
  public StructureType type;
  public GameObject tempObject;
  public GameObject permanentObject;
  public Vector3 rotationOffset;
  public Vector3 positionOffset;

  public readonly Vector3 Bounds {
    get {
      return permanentObject.GetComponent<BoxCollider>().size;
    }
  }
}

public enum StructureType {
  Default,
  Wall,
  Prop
}

public class ConstructionCore : MonoBehaviour
{

  #region Variables

  public bool EnableBuilding;
  public bool EnableRotateOnRightClick;

  [Header("References")]
  [SerializeField] private TEMPORARYPlayerLookNoMultiplayer _playerLook;

  [Header("Structures")]
  [SerializeField] private Structure[] _structures;
  [SerializeField] private int _defaultStructureIndex = 0;

  [Header("General settings")]
  [SerializeField] private float _minBuildDistance = 1f;
  [SerializeField] private float _maxBuildDistance = 5f;
  [SerializeField] private LayerMask _groundLayer;
  [SerializeField] private LayerMask _constructionLayer;
  [SerializeField] private LayerMask _playerLayer;
  [SerializeField] private Material _validMaterial;
  [SerializeField] private Material _invalidMaterial;

  [Header("Wall settings")]
  [SerializeField] private float _wallSnapDistance = 3f;
  [SerializeField] private float _wallAngleTolarence = 35f;
  [SerializeField] private LayerMask _constructionWallLayer;

  // Private variables
  private int _currentStructureIndex;
  private Structure _currentStructure; // The current structure that the player can place
  private GameObject _tempObject; // The temporary object that the player can place
  private Camera _stashedCamera;
  private GameObject _highlightedObject; // The object that the player is currently looking at, used for deleting
  private Material _highlightedMaterial; // The material of the highlighted object, this will cause issues later when objects have multiple materials

  private bool _canPlace;
  private bool _isRotating;
  private bool _isDeleting;

  #endregion


  private void Start() {
    _stashedCamera = Camera.main;
    ChangeStructure(_defaultStructureIndex);
  }

  private void Update() {

    // Toggle building
    if (Input.GetKeyDown(KeyCode.B)) {
      EnableBuilding = !EnableBuilding;
      _isRotating = false;
      _isDeleting = false;
      TryUnhighlightObject();
      _playerLook.canLook = true;
    }

    // If building is disabled, destroy the temp object and return
    if (!EnableBuilding) {
      if (_tempObject != null) {
        Destroy(_tempObject);
        _tempObject = null;
      }
      return;
    }

    if (Input.GetKeyDown(KeyCode.V)) {
      _isDeleting = !_isDeleting;

      if (_isDeleting) {
        TryDestroyTemporaryObject();
        _isRotating = false;
      } else {
        TryUnhighlightObject();
      }
    }

    if (!_isDeleting) {
      if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.R)) && EnableRotateOnRightClick) {
        _isRotating = true;
        _playerLook.canLook = false;
      } else if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.R)) {
        _isRotating = false;
        _playerLook.canLook = true;
      }

      // Handles changing structures with input
      CheckForStructureChange();

      // Update the temp object position
      PlaceTemporaryObject();

      // If the player can place the object, check for input and place it
      if (_canPlace && Input.GetMouseButtonDown(0)) {
        PlacePermanentObject();
      }
    } else {
      TryHighlightObject();
      
      if (Input.GetMouseButtonDown(0)) {
        TryDestroyObject();
      }
    }

  }

  private void PlaceTemporaryObject() {

    Ray ray = _stashedCamera.ScreenPointToRay(Input.mousePosition);

    // heightOffset only work with structures that must be placed on the ground
    if (_currentStructure.type != StructureType.Prop) {
      // Shift the ray based on heightOffset
      float heightOffset = _currentStructure.tempObject.GetComponent<ConstructionTempObject>().heightOffset;
      Vector3 adjustedOrigin = ray.origin - _stashedCamera.transform.up * heightOffset;
      ray.origin = adjustedOrigin;
    }

    RaycastHit hit;
    // Custom raycast logic to handle placeing structures without needing to look at the ground
    if (TryRaycast(ray, out hit)) {
      TryCreateTemporaryObject();

      // Rotate the temp object to face the player
      Vector3 directionTowardsCamera = _stashedCamera.transform.position - hit.point;
      directionTowardsCamera.y = 0;
      Quaternion offsetRotation = Quaternion.LookRotation(directionTowardsCamera) * Quaternion.Euler(_currentStructure.rotationOffset);
    
      if (_isRotating) {
          _tempObject.GetComponent<ConstructionTempObject>().xRotation += Input.GetAxis("Mouse X") * 2; // Rotation speed
          _tempObject.GetComponent<ConstructionTempObject>().SetPositionAndRotation(hit.point + _currentStructure.positionOffset, offsetRotation);
      } else {
        _tempObject.GetComponent<ConstructionTempObject>().SetPositionAndRotation(hit.point + _currentStructure.positionOffset, offsetRotation);
      }


      if (_currentStructure.type == StructureType.Wall) {
        // If the current structure is a wall, snap to the wall
        if (NearOtherWall(hit.point, out ConstructionWall wallScript)) {
          // Handles checking if the wall is placeable
          SnapToWall(hit.point, wallScript);
        } else {
          // Else treat it as a normal structure
          if (CanPlaceStructure()) {
            _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_validMaterial);
            _canPlace = true;
          } else {
            _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_invalidMaterial);
            _canPlace = false;
          }

        }
      } else {
        // If the current structure is not a wall, check if it can be placed
        if (CanPlaceStructure()) {
          _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_validMaterial);
          _canPlace = true;
        } else {
          _tempObject.GetComponent<ConstructionTempObject>().SetMaterial(_invalidMaterial);
          _canPlace = false;
        }
      }

    } else {
      // This shouldn't usually happen, but it's fine if it does
      TryDestroyTemporaryObject();
    }
  }

  private bool TryRaycast(Ray ray, out RaycastHit hit) {
    // Get the current 'player' position
    Vector2 currPositionVector2 = new Vector2(_stashedCamera.transform.position.x, _stashedCamera.transform.position.z);

    // Default test distance to max build distance
    float testDistance = _maxBuildDistance;

    RaycastHit tempHit;

    LayerMask effectiveLayer = _currentStructure.type == StructureType.Prop ? (_groundLayer | _constructionLayer) : _groundLayer;

    // Raycast from the mouse position to the ground, arbitrary ray distance
    if (Physics.Raycast(ray, out tempHit, 100f, effectiveLayer)) {
      // If collides with ground, check if it's within the min and max distance
      Vector2 hitPositionVector2 = new Vector2(tempHit.point.x, tempHit.point.z);
      float distance = Vector2.Distance(currPositionVector2, hitPositionVector2);

      // Handle min and max distance
      if (distance < _minBuildDistance) {
        testDistance = _minBuildDistance;
      } else if (distance > _maxBuildDistance) {
        testDistance = _maxBuildDistance;
      } else {
        hit = tempHit;
        return true;
      }
    }

    Vector3 cameraForward = _stashedCamera.transform.forward;
    if (Mathf.Approximately(cameraForward.y, 1.0f) || Mathf.Approximately(cameraForward.y, -1.0f)) { 
      // If the camera y is close enough to 1 or -1, don't allow building
      hit = new RaycastHit();
      return false;
    }
    cameraForward.y = 0;
    cameraForward = cameraForward.normalized * testDistance;
    Vector3 downRayOrigin = _stashedCamera.transform.position + cameraForward;
    // If the raycast didn't hit anything, try a raycast from the camera to the ground
    if (Physics.Raycast(downRayOrigin, Vector3.down, out hit, 100f, _groundLayer)) {
      return true;
    }

    return false;
  }
  private void TryHighlightObject() {
    Ray ray = _stashedCamera.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 100f, _constructionLayer)) {
      if (_highlightedObject != hit.collider.gameObject) {
        if (_highlightedObject != null) {
          _highlightedObject.GetComponent<Renderer>().material = _highlightedMaterial;
        }
        _highlightedObject = hit.collider.gameObject;
        _highlightedMaterial = _highlightedObject.GetComponent<Renderer>().material;
        _highlightedObject.GetComponent<Renderer>().material = _invalidMaterial;
      }
    } else {
      TryUnhighlightObject();
    }
  }


  private void TryUnhighlightObject() {
    if (_highlightedObject != null) {
      _highlightedObject.GetComponent<Renderer>().material = _highlightedMaterial;
      _highlightedObject = null;
      _highlightedMaterial = null;
    }
  }

  private void TryDestroyObject() {
    if (_highlightedObject != null) {
      Destroy(_highlightedObject);
      _highlightedObject = null;
    }
  }

  private bool NearOtherWall(Vector3 position, out ConstructionWall wallScript) {
    Collider[] colliders = Physics.OverlapSphere(position, _wallSnapDistance, _constructionWallLayer);
    wallScript = null;

    if (colliders.Length == 0) {
      return false;
    }

    float closestDistance = float.MaxValue;
    foreach (Collider c in colliders) {
      ConstructionWall tempWall = c.GetComponent<ConstructionWall>();

      Vector3 closestJoint = tempWall.GetClosestJointWorldSpace(position);
      float distance = Vector3.Distance(closestJoint, position);
      if (distance < closestDistance) {
        closestDistance = distance;
        wallScript = tempWall;
      }
    }

    return true;
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
      Debug.Log(closestTempWallJoint);
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
    LayerMask effectiveLayer = layer ?? (_constructionLayer | _constructionWallLayer | LayerMask.GetMask("Default")); // Not great practice but works
    return !_tempObject.GetComponent<ConstructionTempObject>().IsColliding(effectiveLayer, ignoreTransform);
  }

  private void PlacePermanentObject() {
    // Logic for placing an object (setting a parent, etc) goes here
    
    GameObject permanentObject = Instantiate(_currentStructure.permanentObject, _tempObject.transform.position, _tempObject.transform.rotation);
    
    TryDestroyTemporaryObject();
  }

  private void CheckForStructureChange() {
    // Could be scroll or number keys?
    if (Input.GetKeyDown(KeyCode.Alpha1)) {
      ChangeStructure(0);
    } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
      ChangeStructure(1);
    } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
      ChangeStructure(2);
    } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
      ChangeStructure(3);
    } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
      ChangeStructure(4);
    } else if (Input.GetKeyDown(KeyCode.Alpha6)) {
      ChangeStructure(5);
    } else if (Input.GetKeyDown(KeyCode.Alpha7)) {
      ChangeStructure(6);
    } else if (Input.GetKeyDown(KeyCode.Alpha8)) {
      ChangeStructure(7);
    } else if (Input.GetKeyDown(KeyCode.Alpha9)) {
      ChangeStructure(8);
    } else if (Input.GetKeyDown(KeyCode.Alpha0)) {
      ChangeStructure(9);
    }
  }

  private void TryCreateTemporaryObject() {
    if (_tempObject == null) {
      _tempObject = Instantiate(_currentStructure.tempObject);
    }
  }

  private void TryDestroyTemporaryObject() {
    if (_tempObject != null) {
      Destroy(_tempObject);
      _tempObject = null;
    }
  }

  /// <summary>
  /// Changes the current wall type to the one at the specified index
  /// </summary>
  /// <param name="index">Wall index on ConstructionCore gameobject</param>
  public void ChangeStructure(int index) {
    if (index < 0 || index >= _structures.Length) {
      Debug.LogError("Invalid structure index");
      return;
    }

    _currentStructureIndex = index;
    _currentStructure = _structures[_currentStructureIndex];
    if (_tempObject != null) {
      Destroy(_tempObject);
      _tempObject = null;
    }
  }

}
