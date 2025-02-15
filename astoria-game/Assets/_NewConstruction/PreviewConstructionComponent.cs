using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

namespace Construction
{
    /// <summary>
    /// Goes on preview construction components
    /// </summary>
    public class PreviewConstructionComponent : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("The layer to sphere cast on to find nearby components to connect to.")]
        [SerializeField] private LayerMask _checkLayer = default;

        public List<Edge> edges; // Public for editor

        private PreviewObject previewObject;

        private void Awake()
        {
            previewObject = GetComponent<PreviewObject>();
        }

        private void Update()
        {
            // Render lines
            foreach (Edge edge in edges)
            {
                Debug.DrawLine(transform.TransformPoint(edge.pointA), transform.TransformPoint(edge.pointB), Color.red);
            }
        }

        // Called by construction system to see if this component can connect to another component
        public bool CanPlace(Vector3 tryPosition, Quaternion tryRotation, ConstructionSettings settings, ConstructionComponentData data, out Vector3 finalPosition, out Quaternion finalRotation, out bool validPosition)
        {

            // Apply offsets
            tryPosition += data.Offset.PositionOffset;
            tryRotation *= Quaternion.Euler(data.Offset.RotationOffset);

            switch (data.Type) {
                case ConstructionComponentData.StructureType.Foundation:
                    
                    break;
                case ConstructionComponentData.StructureType.Wall:
                    
                    break;
                case ConstructionComponentData.StructureType.Floor:
                    
                    break;
            }
            // If can place component on ground
            if (data.Type == ConstructionComponentData.StructureType.Foundation)
            {

                // Check if there are no nearby components, check for collision
                if (Physics.OverlapSphere(tryPosition, settings.StructurePlaceRadius, _checkLayer).Length == 0)
                {

                    if (previewObject.IsColliding(tryPosition, tryRotation, settings.CollisionLayerMask))
                    {
                        validPosition = false;
                        finalPosition = tryPosition;
                        finalRotation = tryRotation;
                        return false;
                    }

                    validPosition = true;
                    finalPosition = tryPosition;
                    finalRotation = tryRotation;
                    return true;
                }
            }

            // Snap to nearby component
            List<Transform> snappedTransforms;
            if (HasAvailableConnection(tryPosition, tryRotation, settings, data, out finalPosition, out finalRotation, out snappedTransforms))
            {
                // Check for collision
                if (previewObject.IsColliding(finalPosition, finalRotation, settings.CollisionLayerMask, snappedTransforms))
                {
                    validPosition = false;
                    return false;
                }
                validPosition = true;
                return true;
            }

            validPosition = false;
            finalPosition = tryPosition;
            finalRotation = tryRotation;
            return false;
        }

        private bool HasAvailableConnection(Vector3 tryPosition, Quaternion tryRotation, ConstructionSettings settings, ConstructionData data, out Vector3 snappedPosition, out Quaternion snappedRotation, out List<Transform> snappedTransforms)
        {
            List<(Edge, Edge, float, Transform)> compatibleEdges = new List<(Edge, Edge, float, Transform)>(); // List of compatible edges and their distance to the other component

            snappedTransforms = null;

            foreach (Collider collider in Physics.OverlapSphere(tryPosition, settings.StructurePlaceRadius, _checkLayer))
            {
                ConstructionComponent otherComponent = collider.GetComponentInParent<ConstructionComponent>();
                if (otherComponent != null)
                {
                    // Check if the other component has a free connection
                    foreach (Edge edge in edges)
                    {
                        foreach (Edge otherEdge in otherComponent.edges)
                        {
                            float distance = Edge.CalculateEdgeDistance(edge, otherEdge, tryPosition, tryRotation, otherComponent.transform.position, otherComponent.transform.rotation);
                            compatibleEdges.Add((edge, otherEdge, distance, otherComponent.transform));
                        }
                    }
                }
            }

            snappedPosition = Vector3.zero;
            snappedRotation = Quaternion.identity;

            if (compatibleEdges.Count > 0)
            {
                compatibleEdges.Sort((x, y) => x.Item3.CompareTo(y.Item3));

                // Put all items with the same distance in a list
                List<(Edge, Edge, float, Transform)> closestEdges = new List<(Edge, Edge, float, Transform)>();
                closestEdges.Add(compatibleEdges[0]);
                for (int i = 1; i < compatibleEdges.Count; i++)
                {
                    if (Mathf.Abs(compatibleEdges[i].Item3 - closestEdges[0].Item3) < 0.1f)
                    {
                        closestEdges.Add(compatibleEdges[i]);
                    }
                    else
                    {
                        break;
                    }
                }

                // Choose the edge attached to the furthest object from the initial preview object position
                closestEdges.Sort((x, y) => Vector3.Distance(x.Item4.position, tryPosition).CompareTo(Vector3.Distance(y.Item4.position, tryPosition)));

                Edge closestEdge = closestEdges[0].Item1;
                Edge closestOtherEdge = closestEdges[0].Item2;
                Transform otherTransform = closestEdges[0].Item4;

                bool? doSnapping = null;
                // Try to cast data to ConstructionComponentData
                if (data is ConstructionComponentData componentData)
                {
                    
                    if (componentData.Type != ConstructionComponentData.StructureType.Foundation) {
                        doSnapping = GlobalVariables.FlipRotation;
                    }
                }

                (Vector3, Quaternion) snappedPositionOffset = Edge.SnapEdgeToEdge(closestEdge, closestOtherEdge, tryPosition, tryRotation, otherTransform.position, otherTransform.rotation, doSnapping);

                snappedPosition = tryPosition + snappedPositionOffset.Item1;
                snappedRotation = tryRotation * snappedPositionOffset.Item2;

                if (snappedTransforms == null) snappedTransforms = new List<Transform>();
                snappedTransforms.Add(otherTransform); // This doesnt work

                return true;
            }

            return false;
        }

    }
}
