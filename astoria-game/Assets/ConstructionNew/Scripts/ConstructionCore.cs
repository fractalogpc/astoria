using UnityEngine;
using Construction;
using UnityEngine.Events;
using System;

public class ConstructionCore : InputHandlerBase
{

    [Serializable]
    public enum ConstructionState
    {
        None,
        Placing,
        Rotating,
        Deleting
    }

    public static ConstructionCore Instance;

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

    private bool _canPlace;

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

                // Get the initial ray direction (from the camera through the cursor)
                Ray ray = _cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                Vector3 testDirection = ray.direction.normalized; // Initial ray direction

                // Debug the initial ray direction
                Debug.DrawLine(ray.origin, ray.origin + testDirection * Settings.MaxBuildDistance, Color.white, 1f);

                bool isOutOfRange;
                // Test the initial raycast direction
                if (ConstructionCoreLogic.ValidatePlacementPosition(ray.origin, testDirection, _selectedData, _previewObjectScript, Settings, false, out position, out rotation, out isOutOfRange))
                {
                    validPosition = true;
                }

                int steps = 0;
                int maxLayers = Settings.MaxLayers; // Maximum number of downward layers
                float horizontalStep = Settings.HorizontalStep; // Horizontal spread between rays in world units
                float verticalStep = Settings.VerticalStep; // Vertical distance between layers in world units
                float verticalSubStep = Settings.VerticalSubStep;
                float upwardCurveFactor = Settings.UpwardCurveFactor; // Controls the amount of upward bending
                int densityFactor = Settings.DensityFactor; // Controls the density of the downward layers

                for (int layer = 0; layer < maxLayers && !validPosition; layer++)
                {
                    // Test vertical substeps
                    Vector3 subsetOffsetDirection;

                    int numberOfSteps = Mathf.FloorToInt(Settings.VerticalStep / Settings.VerticalSubStep);
                    for (int i = numberOfSteps; i > 0; i--)
                    {
                        float verticalOffset = (-layer - 1) * verticalStep + i * verticalSubStep;
                        subsetOffsetDirection = ray.direction + (_cameraTransform.up * verticalOffset);
                        subsetOffsetDirection.Normalize();

                        Vector3 subsetTestPosition;
                        Quaternion subsetTestRotation;
                        if (ConstructionCoreLogic.ValidatePlacementPosition(ray.origin, subsetOffsetDirection, _selectedData, _previewObjectScript, Settings, false, out subsetTestPosition, out subsetTestRotation, out _))
                        {
                            position = subsetTestPosition;
                            rotation = subsetTestRotation;
                            validPosition = true;

                            Debug.DrawLine(ray.origin, ray.origin + subsetOffsetDirection * Settings.MaxBuildDistance, Color.blue, 0.1f);
                            DebugDrawCross(ray.origin + subsetOffsetDirection * Settings.MaxBuildDistance, 0.1f, Color.blue);

                            break;
                        }
                        else
                        {
                            Debug.DrawLine(ray.origin, ray.origin + subsetOffsetDirection * Settings.MaxBuildDistance, Color.red, 0.1f);
                            DebugDrawCross(ray.origin + subsetOffsetDirection * Settings.MaxBuildDistance, 0.1f, Color.black);
                        }
                    }

                    // If the initial raycast doesn't hit anything, we assume there aren't any objects near so we simplify by just sampling the horizontal slice.
                    if (validPosition) break;

                    // If the object is out of range, we don't need to sample the horizontal slice
                    if (isOutOfRange) continue;

                    int pointsInLayer = 1 + (layer * 2); // Number of points in the current layer (1, 3, 5, ...)

                    // Start with the center, then alternate left-right
                    int directionToggle = 0; // 0 = center, 1 = left, 2 = right
                    for (int i = 0; i < pointsInLayer && !validPosition; i++)
                    {
                        float horizontalOffset = 0f;

                        // Calculate offset for alternating left-right pattern
                        if (directionToggle == 0)
                        {
                            // Center point (first)
                            horizontalOffset = 0f;
                            directionToggle = 1; // Next point will go left
                        }
                        else if (directionToggle == 1)
                        {
                            // Left point
                            horizontalOffset = -(i / 2 + 1) * horizontalStep;
                            directionToggle = 2; // Next point will go right
                        }
                        else
                        {
                            // Right point
                            horizontalOffset = (i / 2) * horizontalStep;
                            directionToggle = 1; // Next point will go left again
                        }

                        float layerOffset = -layer * verticalStep; // Downward bias for this layer

                        // Smooth upward curve: Use a quadratic curve for smoothness
                        float normalizedOffset = Mathf.Abs(horizontalOffset) / (pointsInLayer * horizontalStep); // Normalize to range [0, 1]
                        float upwardOffset = -Mathf.Pow(normalizedOffset, 2) * upwardCurveFactor; // Quadratic curve for smoothness

                        // Adjust the direction based on the offsets
                        Vector3 offsetDirection = ray.direction
                          + (_cameraTransform.up * (layerOffset + upwardOffset)) // Apply downward bias and smooth upward curve
                          + (_cameraTransform.right * horizontalOffset); // Apply horizontal bias
                        offsetDirection.Normalize();

                        // Cull ray based on Density Factor
                        if (densityFactor == 0) continue;
                        if (densityFactor != -1f)
                        {
                            if (i % densityFactor != 0)
                            {
                                continue;
                            }
                        }

                        // Test this direction
                        Vector3 testPosition;
                        Quaternion testRotation;

                        // Don't sphere cast for the layers near the center of the screen because it causes issues with the vertical subset steps
                        if (ConstructionCoreLogic.ValidatePlacementPosition(ray.origin, offsetDirection, _selectedData, _previewObjectScript, Settings, (true ? false : true), out testPosition, out testRotation, out _))
                        {
                            position = testPosition;
                            rotation = testRotation;
                            validPosition = true;

                            // Draw the valid ray in blue
                            Debug.DrawLine(ray.origin, ray.origin + offsetDirection * Settings.MaxBuildDistance, Color.blue, 0.1f);

                            DebugDrawCross(ray.origin + offsetDirection * Settings.MaxBuildDistance, 0.1f, Color.blue);
                        }
                        else
                        {
                            // Draw invalid rays in a gradient from green to red
                            float t = (float)steps / (maxLayers * maxLayers); // Normalize step count
                            Color rayColor = Color.Lerp(Color.green, Color.red, t);
                            Debug.DrawLine(ray.origin, ray.origin + offsetDirection * Settings.MaxBuildDistance, rayColor, Time.deltaTime);

                            DebugDrawCross(ray.origin + offsetDirection * Settings.MaxBuildDistance, 0.1f, Color.black);

                        }

                        steps++; // Increment the step count
                    }
                }

                // If none of the positions are valid, position and rotation are set by the initial test
                if (!validPosition)
                {
                    ConstructionCoreLogic.ValidatePlacementPosition(ray.origin, testDirection, _selectedData, _previewObjectScript, Settings, false, out position, out rotation, out _);
                }

                RenderPreviewObject(position, rotation);

                Material mat = validPosition ? _previewValidMaterial : _previewInvalidMaterial;

                _previewObjectScript.SetMaterial(mat);

                _canPlace = validPosition;

                break;
        }
    }

    private void DebugDrawCross(Vector3 position, float size, Color color)
    {
        Debug.DrawLine(position + Vector3.up * size, position + Vector3.down * size, color);
        Debug.DrawLine(position + Vector3.right * size, position + Vector3.left * size, color);
        Debug.DrawLine(position + Vector3.forward * size, position + Vector3.back * size, color);
    }

    private void RenderPreviewObject(Vector3 position, Quaternion rotation)
    {
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

        if (!_canPlace) return false;

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

        // Corrects and validates the position of the object

        public static bool ValidatePlacementPosition(Vector3 origin, Vector3 rotation, ConstructionData data, PreviewObject script, BuildingSettings settings, bool doSphereCast, out Vector3 finalPosition, out Quaternion finalRotation, out bool isOutOfRange)
        {
            ConstructionOffset offset = data.Offset;

            // Initial values for failed validation
            finalPosition = origin + offset.PositionOffset + rotation * settings.MaxBuildDistance;
            finalRotation = GetRotationTowardsCamera(finalPosition, origin, offset.RotationOffset);
            isOutOfRange = false;

            RaycastHit hit;
            // Check if the raycast hits anything
            if (doSphereCast)
            {
                if (!Physics.SphereCast(origin, 0.2f, rotation, out hit, settings.MaxBuildDistance, settings.PlacementLayerMask | (data.CanBePlacedOnStructures ? (int)settings.ConstructionLayerMask : 0)))
                {
                    isOutOfRange = true;
                    return false;
                }
            }
            else
            {
                if (!Physics.Raycast(origin, rotation, out hit, settings.MaxBuildDistance, settings.PlacementLayerMask | (data.CanBePlacedOnStructures ? (int)settings.ConstructionLayerMask : 0)))
                {
                    isOutOfRange = true;
                    return false;
                }
            }

            Vector3 newPosition = hit.point + offset.PositionOffset;
            Vector3 normal = hit.normal;

            // Calculate the angle between the normal of the surface and the up vector
            float angle = Vector3.Angle(Vector3.up, normal);

            // Check if the normal of the surface is within the allowed range
            if (!ValidateNormal(angle, data, settings)) return false;

            // Calculate the new rotation of the object
            Quaternion newRotation = GetRotationTowardsCamera(newPosition, origin, offset.RotationOffset);

            // Rotate the object to match the normal of the surface
            newRotation = Quaternion.FromToRotation(Vector3.up, normal) * newRotation;

            finalPosition = newPosition;
            finalRotation = newRotation;

            // Check if the object is not colliding with anything
            if (script.IsColliding(newPosition, newRotation, settings.CollisionLayerMask))
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

        private static bool ValidateNormal(float angle, ConstructionData data, BuildingSettings settings)
        {
            if (data.CanBePlacedOnGround)
            {
                if (angle >= settings.MinGroundNormal && angle <= settings.MaxGroundNormal) return true;
            }
            if (data.CanBePlacedOnWalls)
            {
                if (angle >= settings.MinWallNormal && angle <= settings.MaxWallNormal) return true;
            }
            if (data.CanBePlacedOnCeilings)
            {
                if (angle >= settings.MinCeilingNormal && angle <= settings.MaxCeilingNormal) return true;
            }

            return false;
        }
    }

    [Serializable]
    public class BuildingSettings
    {
        public float MaxBuildDistance = 10f;

        // Sampling settings
        public int MaxLayers = 5;
        public float HorizontalStep = 0.5f;
        public float VerticalStep = 0.5f;
        public float VerticalSubStep = 0.01f;
        public float UpwardCurveFactor = -0.5f;
        public int DensityFactor = 3;

        public LayerMask PlacementLayerMask;
        public LayerMask ConstructionLayerMask;
        public LayerMask CollisionLayerMask;

        [Header("Normal settings")]
        // Angle from 0 to 180 degrees
        // 0 is straight up, 90 is straight forward, 180 is straight down
        public float MinGroundNormal = 0.0f;
        public float MaxGroundNormal = 45f;
        public float MinWallNormal = 45f;
        public float MaxWallNormal = 135f;
        public float MinCeilingNormal = 135f;
        public float MaxCeilingNormal = 180f;
    }
}