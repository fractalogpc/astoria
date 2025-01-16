using UnityEngine;
using Construction;
using UnityEngine.Events;
using System;

public class NEWConstructionCore : InputHandlerBase
{

    [Serializable]
    public enum ConstructionState
    {
        None,
        Placing,
        Rotating,
        Deleting
    }

    public static NEWConstructionCore Instance;

    public ConstructionData DebugData;

    [Header("Construction State")]
    public ConstructionState State;

    [Header("Placement Settings")]
    public BuildingSettings Settings;

    [SerializeField] private float _previewObjectLerpSpeed = 10f;
    [SerializeField] private Material _previewValidMaterial;
    [SerializeField] private Material _previewInvalidMaterial;

    [Header("References")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Events")]
    public UnityEvent<ConstructionData> OnDataSelected;
    public UnityEvent<ConstructionData> OnObjectPlaced;

    private ConstructionData _selectedData;

    private GameObject _previewObject;
    public PreviewObject _previewObjectScript;
    private Vector3 _previewObjectPosition;

    protected override void InitializeActionMap()
    {
        RegisterAction(_inputActions.Player.Place, _ => { OnClick(); });
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of NEWConstructionCore detected");
        }

        SetConstructionState(ConstructionState.None);
    }

    private void Update()
    {

        if (DebugData != null)
        {
            SelectData(DebugData);

            DebugData = null;
        }

        switch (State)
        {
            case ConstructionState.Placing:
                Vector3 position;
                Quaternion rotation;

                bool validPosition = false;

                Vector3 testDirection = _cameraTransform .forward;

                // Test initial position
                if (ConstructionCoreLogic.ValidatePlacementPosition(_cameraTransform.position, testDirection, _selectedData.Offset, out position, out rotation)) {
                    validPosition = true;
                }

                int steps = 0;
                while (!validPosition && Vector3.Distance(testDirection.normalized, Vector3.down) > 0.1f || steps > 100)
                {
                    Vector3 testPosition;
                    Quaternion testRotation;
                    if (ConstructionCoreLogic.ValidatePlacementPosition(_cameraTransform.position, testDirection, _selectedData.Offset, out testPosition, out testRotation)) {
                        position = testPosition;
                        rotation = testRotation;
                        validPosition = true;

                    }
                    else
                    {
                        // Rotate the direction and test again
                        Vector3 rotationAxis = Vector3.Cross(testDirection, Vector3.down);
                        Quaternion stepRotation = Quaternion.AngleAxis(Settings.RotationStep, rotationAxis);
                        testDirection = stepRotation * testDirection;
                    }
                    steps++;
                }

                // If none of the positions are valid, position and rotation are set by the initial test
                RenderPreviewObject(position, rotation);

                Material mat = validPosition ? _previewValidMaterial : _previewInvalidMaterial;

                _previewObjectScript.SetMaterial(mat);

                break;
        }
    }

    private void RenderPreviewObject(Vector3 position, Quaternion rotation) {
        _previewObjectPosition = Vector3.Lerp(_previewObjectPosition, position, Time.deltaTime * _previewObjectLerpSpeed);

        _previewObject.transform.position = _previewObjectPosition;
        _previewObject.transform.rotation = rotation;

        _previewObjectPosition = _previewObject.transform.position;
    }

    public bool SelectData(ConstructionData data)
    {
        if (!ConstructionCoreLogic.ValidateData(data)) return false;

        if (_selectedData != null && _selectedData != data)
        {
            SetConstructionState(ConstructionState.None);
        }

        _selectedData = data;

        _previewObject = CreatePreviewObject();

        SetConstructionState(ConstructionState.Placing);

        OnDataSelected?.Invoke(_selectedData);

        return true;
    }

    private bool OnClick()
    {

        switch (State)
        {
            case ConstructionState.Placing:

                CreateObject();

                SetConstructionState(ConstructionState.None);

                OnObjectPlaced?.Invoke(_selectedData);

                SetConstructionState(ConstructionState.None);

                return true;
        }

        return false;
    }

    private GameObject CreatePreviewObject()
    {
        GameObject localPreviewObject = Instantiate(_selectedData.PreviewPrefab);

        _previewObjectScript = localPreviewObject.GetComponent<PreviewObject>();

        if (_previewObjectScript == null)
        {
            Debug.LogError("PreviewObject script not found on preview object");
            return null;
        }

        _previewObjectScript.SetMaterial(_previewValidMaterial);

        return localPreviewObject;
    }

    private GameObject CreateObject()
    {
        Vector3 placedPosition = _previewObject.transform.position;
        Quaternion placedRotation = _previewObject.transform.rotation;

        GameObject placedObject = Instantiate(_selectedData.PlacedPrefab, placedPosition, placedRotation);

        return placedObject;
    }

    private void CleanupPreviewObject()
    {
        if (_previewObject != null)
        {
            Destroy(_previewObject);
        }

        _previewObject = null;
        _previewObjectScript = null;
    }

    public void SetConstructionState(ConstructionState state)
    {

        // Cleanup previous state
        switch (State)
        {
            case ConstructionState.Placing:
                CleanupPreviewObject();
                break;
        }

        State = state;

        // Initialize new state
        switch (State)
        {
            case ConstructionState.None:
                break;
        }
    }
}

namespace Construction
{
    public static class ConstructionCoreLogic
    {
        public static bool ValidateData(ConstructionData data)
        {
            if (data == null)
            {
                Debug.LogError("ConstructionData is null");
                return false;
            }

            return true;
        }

        public static bool ValidatePosition(Vector3 position)
        {
            if (position == null)
            {
                Debug.LogError("Position is null");
                return false;
            }

            return true;
        }

        public static Vector3 GetWorldSpaceCursorPosition()
        {
            Vector2 centerOfScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(centerOfScreen);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        // Corrects and validates the position of the object
        public static bool ValidatePlacementPosition(Vector3 origin, Vector3 rotation, ConstructionOffset offset, out Vector3 finalPosition, out Quaternion finalRotation)
        {
            finalPosition = Vector3.zero;
            finalRotation = Quaternion.identity;

            // Check if the raycast hits anything
            if (!Physics.Raycast(origin, rotation, out RaycastHit hit, NEWConstructionCore.Instance.Settings.PlacementLayerMask))
            {
                return false;
            }

            Vector3 newPosition = hit.point + offset.PositionOffset;

            // Check if the distance is within the limits
            float distance = Vector3.Distance(origin, newPosition);
            if (distance > NEWConstructionCore.Instance.Settings.MaxBuildDistance || distance < NEWConstructionCore.Instance.Settings.MinBuildDistance)
            {
                return false;
            }

            // Calculate the new rotation of the object
            Quaternion newRotation = GetRotationTowardsCamera(newPosition, origin, offset.RotationOffset);

            // Check if the object is not colliding with anything
            if (NEWConstructionCore.Instance._previewObjectScript.IsColliding(newPosition, newRotation, NEWConstructionCore.Instance.Settings.PlacementLayerMask))
            {
                return false;
            }

            finalPosition = newPosition;
            finalRotation = newRotation;
            return true;
        }
    
        public static Quaternion GetRotationTowardsCamera(Vector3 origin, Vector3 target, Vector3 rotationOffset = default)
        {
            Vector3 directionTowardsCamera = origin - target;
            directionTowardsCamera.y = 0;
            return Quaternion.LookRotation(directionTowardsCamera) * Quaternion.Euler(rotationOffset);
        }
    }

    [Serializable]
    public class BuildingSettings
    {
        public float MaxBuildDistance = 10f;
        public float MinBuildDistance = 2f;
        public float RotationStep = 1f;

        public LayerMask PlacementLayerMask;
    }
}