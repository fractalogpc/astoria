using UnityEngine;
using Construction;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace Construction
{
    /// <summary>
    /// This is the core of the construction system. Not to be confused with CoreController which handles the CORE prop.
    /// Should be on the player somewhere.
    /// This handles the placing and deleting of props and components, nothing more.
    /// </summary>
    public class ConstructionCore : InputHandlerBase
    {

        [Serializable]
        public enum ConstructionState
        {
            None,
            PlacingProp,
            PlacingStructure,
            Deleting
        }
        public ConstructionData DebugData;
        public bool RequireCoreToPlaceStructures;
        public bool HoldComponentAfterPlace = true;

        [Header("Construction State")]
        public ConstructionState State;

        [Header("Placement Settings")]
        public ConstructionSettings Settings;

        [SerializeField] private float _previewObjectLerpSpeed = 10f;
        [SerializeField] private Material _previewValidMaterial;
        [SerializeField] private Material _previewInvalidMaterial;
        [SerializeField] private Material _deletingMaterial;

        [Header("References")]
        [SerializeField] private Transform _cameraTransform;

        [Header("Events")]
        public UnityEvent<ConstructionData> OnDataSelected;
        public UnityEvent<ConstructionData> OnObjectPlaced;
        public UnityEvent OnObjectDeleted;

        // Placing stuff
        private ConstructionData _selectedData;
        private GameObject _previewObject;
        public PreviewObject _previewObjectScript;
        private Vector3 _previewObjectPosition;
        private Vector3 _previewObjectLerpPosition; // Stored seporately for lerping

        // Deleting stuff
        private GameObject _highlightedForDeletionObject;
        private List<Renderer> _highlightedRenderers = new List<Renderer>();
        private List<Material> _highlightedMaterials = new List<Material>();

        public CoreController Core;

        private bool _canPlace;

        protected override void InitializeActionMap()
        {
            RegisterAction(_inputActions.Player.Place, _ => { OnClick(); });
            RegisterAction(_inputActions.Player.Delete, _ => SetConstructionState(ConstructionState.Deleting));
            RegisterAction(_inputActions.Player.FlipBuilding, _ => GlobalVariables.FlipRotation = !GlobalVariables.FlipRotation);
        }

        private void Start()
        {
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
                case ConstructionState.PlacingProp:
                    {
                        ConstructionPropData data = _selectedData as ConstructionPropData;

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
                        if (ConstructionCoreLogic.ValidatePlacementPosition(ray.origin, testDirection, data, _previewObjectScript, Settings, false, out position, out rotation, out isOutOfRange))
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
                                if (ConstructionCoreLogic.ValidatePlacementPosition(ray.origin, subsetOffsetDirection, data, _previewObjectScript, Settings, false, out subsetTestPosition, out subsetTestRotation, out _))
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
                                if (ConstructionCoreLogic.ValidatePlacementPosition(ray.origin, offsetDirection, data, _previewObjectScript, Settings, (true ? false : true), out testPosition, out testRotation, out _))
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
                            ConstructionCoreLogic.ValidatePlacementPosition(ray.origin, testDirection, data, _previewObjectScript, Settings, false, out position, out rotation, out _);
                        }

                        RenderPreviewObject(position, rotation, validPosition);

                        _canPlace = validPosition;
                    }
                    break;

                case ConstructionState.PlacingStructure:
                    {
                        if (Core == null && RequireCoreToPlaceStructures)
                        {
                            SetConstructionState(ConstructionState.None);

                            break;
                        }

                        ConstructionComponentData data = _selectedData as ConstructionComponentData;

                        bool validPosition = false;

                        Vector3 position;
                        Quaternion rotation;

                        PreviewConstructionComponent constructionComponent = _previewObject.GetComponent<PreviewConstructionComponent>();
                        if (constructionComponent == null)
                        {
                            Debug.LogError("PreviewConstructionComponent not found on preview object");
                            SetConstructionState(ConstructionState.None);
                            break;
                        }


                        // Get the initial ray direction (from the camera through the cursor)
                        Ray ray = _cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                        Vector3 testDirection = ray.direction; // Initial ray direction
                        testDirection.y = 0;

                        RaycastHit hit;
                        // If the ray doesn't hit, sphere cast to find the nearest object
                        if (!Physics.Raycast(ray, out hit, Settings.MaxBuildDistance, Settings.PlacementLayerMask))
                        {
                            Physics.SphereCast(ray, 0.4f, out hit, Settings.MaxBuildDistance, Settings.PlacementLayerMask);
                        }

                        // Check if the component can be placed
                        constructionComponent.CanPlace(hit.transform == null ? ray.origin + ray.direction * Settings.MaxBuildDistance : hit.point, Quaternion.LookRotation(testDirection), Settings, data, out position, out rotation, out validPosition);

                        // Check if player has required materials
                        if (validPosition)
                        {
                            if (BackgroundInfo._infBuild)
                            {
                                _canPlace = true;
                            }
                            else
                            {
                                _canPlace = data.Cost.ContainedWithin(PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().GetItems());
                            }
                        }
                        else
                        {
                            _canPlace = false;
                        }

                        RenderPreviewObject(position, rotation, _canPlace);

                    }
                    break;

                case ConstructionState.Deleting:
                    {
                        // Get the initial ray direction (from the camera through the cursor)
                        Ray ray = _cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

                        RaycastHit hit;
                        Physics.Raycast(ray, out hit, Settings.MaxBuildDistance, Settings.ConstructionLayerMask);

                        if (hit.transform != null)
                        {
                            if (hit.transform.GetComponentInParent<ConstructionObject>() != null)
                            {
                                GameObject highlightedObject = hit.transform.GetComponentInParent<ConstructionObject>().gameObject;

                                if (_highlightedForDeletionObject == highlightedObject) break;
                                if (_highlightedForDeletionObject != null) UnhighlightObject(_highlightedForDeletionObject);

                                _highlightedForDeletionObject = highlightedObject;
                                HighlightObject(highlightedObject);
                            }
                        }
                        else
                        {
                            if (_highlightedForDeletionObject != null)
                            {
                                UnhighlightObject(_highlightedForDeletionObject);
                            }
                        }
                    }
                    break;
            }
        }

        private void DebugDrawCross(Vector3 position, float size, Color color)
        {
            Debug.DrawLine(position + Vector3.up * size, position + Vector3.down * size, color);
            Debug.DrawLine(position + Vector3.right * size, position + Vector3.left * size, color);
            Debug.DrawLine(position + Vector3.forward * size, position + Vector3.back * size, color);
        }

        private void RenderPreviewObject(Vector3 position, Quaternion rotation, bool validPosition)
        {
            _previewObjectPosition = position;

            _previewObjectLerpPosition = Vector3.Lerp(_previewObjectLerpPosition, position, Time.deltaTime * _previewObjectLerpSpeed);

            _previewObject.transform.position = _previewObjectLerpPosition;
            _previewObject.transform.rotation = rotation;

            _previewObjectLerpPosition = _previewObject.transform.position;

            Material mat = validPosition ? _previewValidMaterial : _previewInvalidMaterial;

            _previewObjectScript.SetMaterial(mat);
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

            if (data.GetType() == typeof(ConstructionPropData))
            {
                SetConstructionState(ConstructionState.PlacingProp);
            }
            else if (data.GetType() == typeof(ConstructionComponentData))
            {
                SetConstructionState(ConstructionState.PlacingStructure);
            }

            OnDataSelected?.Invoke(_selectedData);

            return true;
        }

        private bool OnClick()
        {
            switch (State)
            {
                case ConstructionState.PlacingProp:
                    if (!_canPlace) break;
                    PlaceObject(_previewObjectPosition, _previewObject.transform.rotation);
                    SetConstructionState(ConstructionState.None);
                    OnObjectPlaced?.Invoke(_selectedData);
                    return true;
                case ConstructionState.PlacingStructure:
                    if (!_canPlace) break;
                    // Debug.Log("Placing structure");
                    GameObject placedObject = PlaceObject(_previewObjectPosition, _previewObject.transform.rotation);

                    ConstructionComponentData data = _selectedData as ConstructionComponentData;

                    placedObject.GetComponent<ConstructionComponent>().AddConnections(data, Settings);

                    // Removing items
                    if (!BackgroundInfo._infBuild)
                    {
                        List<ItemInstance> cost = data.Cost.ToItemsList();
                        foreach (ItemInstance item in cost)
                        {
                            PlayerInstance.Instance.GetComponentInChildren<InventoryComponent>().RemoveItemByData(item.ItemData);
                        }
                    }

                    SetConstructionState(ConstructionState.None);
                    OnObjectPlaced?.Invoke(_selectedData);

                    if (HoldComponentAfterPlace)
                    {
                        SelectData(_selectedData);
                    }
                    return true;
                case ConstructionState.Deleting:
                    if (_highlightedForDeletionObject == null) break;

                    _highlightedForDeletionObject.GetComponent<ConstructionObject>().Delete();

                    OnObjectDeleted?.Invoke();

                    SetConstructionState(ConstructionState.None);
                    break;

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

        // Honestly I can not remember why this function exists
        private GameObject CreateObject()
        {
            Vector3 placedPosition = _previewObject.transform.position;
            Quaternion placedRotation = _previewObject.transform.rotation;

            GameObject placedObject = Instantiate(_selectedData.PlacedPrefab, placedPosition, placedRotation);

            return placedObject;
        }

        private GameObject PlaceObject(Vector3 position, Quaternion rotation)
        {

            // This is where weird server stuff would go

            // Instantiate the object on the server
            GameObject placedObject = Instantiate(_selectedData.PlacedPrefab, position, rotation);

            return placedObject;
        }

        private ConstructionData TryDeleteObject()
        {
            return null;
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

        private void HighlightObject(GameObject obj)
        {
            _highlightedRenderers = new List<Renderer>(obj.GetComponentsInChildren<Renderer>());
            foreach (Renderer r in _highlightedRenderers)
            {
                _highlightedMaterials.Add(r.material);

                r.material = _deletingMaterial;
            }
        }

        private void UnhighlightObject(GameObject obj)
        {
            for (int i = 0; i < _highlightedRenderers.Count; i++)
            {
                _highlightedRenderers[i].material = _highlightedMaterials[i];
            }

            _highlightedRenderers.Clear();
            _highlightedMaterials.Clear();

            _highlightedForDeletionObject = null;
        }

        public void SetConstructionState(ConstructionState state)
        {

            // Cleanup previous state
            switch (State)
            {
                case ConstructionState.PlacingProp:
                    CleanupPreviewObject();
                    break;
                case ConstructionState.PlacingStructure:
                    CleanupPreviewObject();
                    break;
                case ConstructionState.Deleting:
                    UnhighlightObject(_highlightedForDeletionObject);
                    _highlightedForDeletionObject = null;
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
}

namespace Construction
{
    /// <summary>
    /// Static library for functionality with construction. Mainwly about checking for placement or raycasting.
    /// </summary>
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

        public static bool ValidatePlacementPosition(Vector3 origin, Vector3 rotation, ConstructionPropData data, PreviewObject script, ConstructionSettings settings, bool doSphereCast, out Vector3 finalPosition, out Quaternion finalRotation, out bool isOutOfRange)
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

        private static bool ValidateNormal(float angle, ConstructionPropData data, ConstructionSettings settings)
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

        public static Vector3 SnapToGrid(Vector3 point, Vector3 origin, Quaternion rotation, float gridSize = 1f)
        {
            // Translate the point into the grid's local space.
            Vector3 localPoint = Quaternion.Inverse(rotation) * (point - origin);

            // Snap each coordinate of the local point to the nearest grid cell.
            localPoint.x = Mathf.Round(localPoint.x / gridSize) * gridSize;
            localPoint.y = Mathf.Round(localPoint.y / gridSize) * gridSize;
            localPoint.z = Mathf.Round(localPoint.z / gridSize) * gridSize;

            // Transform the snapped local point back to world space.
            Vector3 snappedPoint = rotation * localPoint + origin;

            return snappedPoint;
        }
    }
}

/// <summary>
/// Simple class for storing building settings.
/// I'm not sure if this should be a class or a struct, I guess it doesn't matter? 
/// </summary>
[Serializable]
public class ConstructionSettings
{
    [Tooltip("Maximum distance from the camera position that you can place.")]
    public float MaxBuildDistance = 10f;

    // Sampling settings
    [Tooltip("Number of vertical large steps.")]
    public int MaxLayers = 5;
    [Tooltip("Distance horizontally that the raycast checks.")]
    public float HorizontalStep = 0.5f;
    [Tooltip("Distance vertically that the raycast checks.")]
    public float VerticalStep = 0.5f;
    [Tooltip("Distance vertically for the center line of checks. Should be lower then VerticalStep.")]
    public float VerticalSubStep = 0.01f;
    [Tooltip("Amount that the outer points curve upwards.")]
    public float UpwardCurveFactor = -0.5f;
    [Tooltip("The density of the number of points. Lower number means less points. -1 means all points.")]
    public int DensityFactor = 3;
    [Tooltip("This radius a structure checks for raycasts with other structures.")]
    public float StructureCheckRadius = 3f;
    [Tooltip("The radius a structure checks for snapping with other structures.")]
    public float StructurePlaceRadius = 1f;
    [Tooltip("The distance from a construction object that a new construction object can be placed.")]
    public float ConstructionBufferDistance = 5f;

    [Tooltip("The layer that you can place things on. Should be ground, rocks, other construction etc.")]
    public LayerMask PlacementLayerMask;
    [Tooltip("Should only be the layer of construction objects.")]
    public LayerMask ConstructionLayerMask;
    [Tooltip("Layer that interferes with placing construction objects.")]
    public LayerMask CollisionLayerMask;
    [Tooltip("The ground layer mask")]
    public LayerMask GroundLayerMask;
    [Tooltip("The max distance a foundation can be placed from the ground.")]
    public float FoundationMaxDistanceFromGround = 2f;

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