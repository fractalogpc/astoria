using System.Collections.Generic;
using UnityEngine;

namespace Construction
{
    public class PreviewConstructionComponent : MonoBehaviour
    {

        [Header("Settings")]
        [Tooltip("The distance to sphere cast to find nearby components to connect to from the transform 0,0.")]
        [SerializeField] private float _sphereRadius = 2f;
        [Tooltip("The layer to sphere cast on to find nearby components to connect to.")]
        [SerializeField] private LayerMask _checkLayer = default;

        public List<Edge> edges; // Public for editor

        // Called by construction system to see if this component can connect to another component
        public bool HasAvailableConnection(out Vector3 snappedPosition, out Quaternion snappedRotation)
        {
            List<(Edge, Edge, float, Transform)> compatibleEdges = new List<(Edge, Edge, float, Transform)>(); // List of compatible edges and their distance to the other component

            foreach (Collider collider in Physics.OverlapSphere(transform.position, _sphereRadius, _checkLayer))
            {
                ConstructionComponent otherComponent = collider.GetComponent<ConstructionComponent>();
                if (otherComponent != null)
                {
                    // Check if the other component has a free connection
                    foreach (Edge edge in edges)
                    {
                        foreach (Edge otherEdge in otherComponent.edges)
                        {
                            float distance = Edge.CalculateEdgeDistance(edge, otherEdge, transform, otherComponent.transform);
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
                
                Edge closestEdge = compatibleEdges[0].Item1;
                Edge closestOtherEdge = compatibleEdges[0].Item2;
                Transform otherTransform = compatibleEdges[0].Item4;

                (Vector3, Quaternion) snappedPositionOffset = Edge.SnapEdgeToEdge(closestEdge, closestOtherEdge, transform, otherTransform);

                snappedPosition = transform.position + snappedPositionOffset.Item1;
                snappedRotation = snappedPositionOffset.Item2;

                return true;
            }

            return false;
        }

    }
}
