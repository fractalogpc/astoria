using UnityEngine;
using Construction;
using UnityEngine.Events;
using System;
using Palmmedia.ReportGenerator.Core;

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

    [SerializeField] private Material _previewValidMaterial;
    [SerializeField] private Material _previewInvalidMaterial;

    [Header("References")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Events")]
    public UnityEvent<ConstructionData> OnDataSelected;
    public UnityEvent<ConstructionData> OnObjectPlaced;

    private ConstructionData _selectedData;

    private GameObject _previewObject;
    private PreviewObject _previewObjectScript;

    private Vector3 _fixedCursorPosition = Vector3.zero;

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
                bool pass1 = ConstructionCoreLogic.ValidatePlacementPositionPass1(_cameraTransform.position, ConstructionCoreLogic.GetWorldSpaceCursorPosition(), out _fixedCursorPosition);

                _previewObject.transform.position = UpdatePreviewObjectPosition();
                _previewObject.transform.rotation = UpdatePreviewObjectRotation();

                bool pass2 = pass1 ? ConstructionCoreLogic.ValidatePlacementPositionPass2(_previewObjectScript) : false;

                Material mat = pass2 ? _previewValidMaterial : _previewInvalidMaterial;

                _previewObjectScript.SetMaterial(mat);

                break;
        }
    }

    private Vector3 UpdatePreviewObjectPosition()
    {
        Vector3 position = _fixedCursorPosition;
        position += _selectedData.PositionOffset;

        return position;
    }

    private Quaternion UpdatePreviewObjectRotation()
    {
        Vector3 directionTowardsCamera = _cameraTransform.transform.position - _previewObject.transform.position;
        directionTowardsCamera.y = 0;
        Quaternion rotation = Quaternion.LookRotation(directionTowardsCamera) * Quaternion.Euler(_selectedData.RotationOffset);

        return rotation;
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
        GameObject localPreviewObject = Instantiate(_selectedData.PreviewPrefab, _fixedCursorPosition, Quaternion.identity);

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
        Vector3 placedPosition = _fixedCursorPosition + _selectedData.PositionOffset;
        Quaternion placedRotation = Quaternion.Euler(_selectedData.RotationOffset);

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
        _fixedCursorPosition = Vector3.zero;
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
                _fixedCursorPosition = Vector3.zero;
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
        public static bool ValidatePlacementPositionPass1(Vector3 origin, Vector3 initPosition, out Vector3 finalPosition)
        {
            finalPosition = initPosition;

            // Check if the distance between the origin and the initPosition is within the acceptable distance
            float distance = Vector3.Distance(origin, initPosition);
            if (distance > NEWConstructionCore.Instance.Settings.MaxBuildDistance || distance < NEWConstructionCore.Instance.Settings.MinBuildDistance)
            {
                return false;
            }
            return true;
        }

        // Validates the position without moving the object
        public static bool ValidatePlacementPositionPass2(PreviewObject previewObjectScript)
        {
            // Check if the object is colliding with anything
            if (previewObjectScript.IsColliding(NEWConstructionCore.Instance.Settings.PlacementLayerMask))
            {
                return false;
            }

            return true;
        }
    }

    [Serializable]
    public class BuildingSettings
    {
        public float MaxBuildDistance = 10f;
        public float MinBuildDistance = 2f;

        public LayerMask PlacementLayerMask;
    }
}