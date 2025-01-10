using UnityEngine;
using Construction;
using UnityEngine.Events;

public class NEWConstructionCore : InputHandlerBase
{
    public ConstructionData DebugData;
    [Header("Placement Settings")]
    [SerializeField] private float _maxBuildDistance = 10f;

    [Header("References")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Events")]
    public UnityEvent<ConstructionData> OnDataSelected;
    public UnityEvent<ConstructionData> OnObjectPlaced;

    private ConstructionData _selectedData;

    private GameObject _previewObject;

    private Vector3 _worldSpaceCursorPosition;

    protected override void InitializeActionMap() {
        RegisterAction(_inputActions.Player.Place, _ => { OnPlace(); });
    }

    private void Update() {

        if (DebugData != null) {
            SelectData(DebugData);

            DebugData = null;
        }

        if (_selectedData == null) return;

        _worldSpaceCursorPosition = ConstructionCoreLogic.ValidatePlacementPosition(_cameraTransform.position, ConstructionCoreLogic.GetWorldSpaceCursorPosition());

        _previewObject.transform.position = _worldSpaceCursorPosition;
    }

    public bool SelectData(ConstructionData data) {
        if (!ConstructionCoreLogic.ValidateData(data)) return false;

        if (_selectedData != null && _selectedData != data) {
            DeselectData();
        }

        _selectedData = data;

        _previewObject = CreatePreviewObject();

        OnDataSelected?.Invoke(_selectedData);

        return true;
    }

    public bool DeselectData() {
        if (_selectedData == null) {
            Debug.LogError("No data selected");
            return false;
        }

        _selectedData = null;

        CleanupPreviewObject();

        return true;
    }

    private bool OnPlace() {
        if (_selectedData == null) {
            Debug.LogError("No data selected");
            return false;
        }

        CreateObject();

        OnObjectPlaced?.Invoke(_selectedData);
    
        DeselectData();

        return true;
    }

    private GameObject CreatePreviewObject() {
        GameObject previewObject = Instantiate(_selectedData.PreviewPrefab, _worldSpaceCursorPosition, Quaternion.identity);

        return previewObject;
    }

    private void CreateObject() {
        Vector3 placedPosition = _worldSpaceCursorPosition + _selectedData.PositionOffset;
        Quaternion placedRotation = Quaternion.Euler(_selectedData.RotationOffset);

        GameObject placedObject = Instantiate(_selectedData.PlacedPrefab, placedPosition, placedRotation);
    }

    private void CleanupPreviewObject() {
        if (_previewObject != null) {
            Destroy(_previewObject);
        }

        _previewObject = null;
    }
}

namespace Construction {
    public static class ConstructionCoreLogic {
        public static bool ValidateData(ConstructionData data) {
            if (data == null) {
                Debug.LogError("ConstructionData is null");
                return false;
            }

            return true;
        }

        public static bool ValidatePosition(Vector3 position) {
            if (position == null) {
                Debug.LogError("Position is null");
                return false;
            }

            return true;
        }

        public static Vector3 GetWorldSpaceCursorPosition() {
            Vector2 centerOfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(centerOfScreen);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                return hit.point;
            }

            return Vector3.zero;
        }

        public static Vector3 ValidatePlacementPosition(Vector3 origin, Vector3 position) {
            

            return position;
        }
    }
}