using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Construction
{
    /// <summary>
    /// Goes on preview construction components
    /// </summary>
    public class PreviewConstructionComponent : MonoBehaviour
    {
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
                Debug.DrawLine(transform.TransformPoint(edge.position), transform.TransformPoint(edge.position + edge.normal * 0.2f), Color.red);
            }
        }

        bool doDebug = false; // This is purely for testing and me being lazy with commenting the debug messages

        // Called by construction system to see if this component can connect to another component
        public bool CanPlace(Vector3 tryPosition, Quaternion tryRotation, ConstructionSettings settings, ConstructionComponentData data, out Vector3 finalPosition, out Quaternion finalRotation, out bool validPosition)
        {

            Debug.DrawLine(tryPosition, tryPosition + Vector3.up * 0.1f, Color.green, 1f);
            Debug.DrawLine(tryPosition, tryPosition + Vector3.right * 0.1f, Color.blue, 1f);
            Debug.DrawLine(tryPosition, tryPosition + Vector3.forward * 0.1f, Color.red, 1f);

            // Apply offsets
            // tryPosition += data.Offset.PositionOffset;
            // tryRotation *= Quaternion.Euler(data.Offset.RotationOffset);

            List<Transform> snappedTransforms; // Don't check collisions with the object we are snapping to

            switch (data.Type)
            {
                case ConstructionComponentData.StructureType.Foundation:
                    // Don't check for collisions with the ground for foundations
                    LayerMask foundationLayerMask = settings.CollisionLayerMask & ~settings.GroundLayerMask;

                    // Check for nearby snapping
                    if (HasAvailableConnection(tryPosition, settings, data, out finalPosition, out finalRotation, out snappedTransforms))
                    {
                        // Check for collision
                        if (previewObject.IsColliding(finalPosition, finalRotation, foundationLayerMask, snappedTransforms))
                        {
                            validPosition = false;
                            if (doDebug) Debug.Log("Colliding");
                        }
                        else
                        {
                            // Check for distance from ground
                            if (Physics.Raycast(tryPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, 100f, settings.GroundLayerMask))
                            {
                                if (hit.distance > settings.FoundationMaxDistanceFromGround + 10)
                                {
                                    // Too far from ground
                                    validPosition = false;
                                    if (doDebug) Debug.Log("Too far from ground");
                                }
                                else
                                {
                                    // Valid position
                                    validPosition = true;
                                    if (doDebug) Debug.Log("Valid position");
                                }
                            }
                            else
                            {
                                // No ground found
                                validPosition = false;
                                if (doDebug) Debug.Log("No ground found");
                            }
                        }
                    }
                    else
                    {
                        // No snapping found
                        finalPosition = tryPosition;
                        finalRotation = tryRotation;

                        // Check for collision
                        if (previewObject.IsColliding(finalPosition, finalRotation, foundationLayerMask, snappedTransforms))
                        {
                            validPosition = false;
                            if (doDebug) Debug.Log($"Colliding");
                        }
                        else
                        {
                            // Check for distance from ground
                            if (Physics.Raycast(tryPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, 100f, settings.GroundLayerMask))
                            {
                                if (hit.distance > settings.FoundationMaxDistanceFromGround + 10)
                                {
                                    // Too far from ground
                                    validPosition = false;
                                    if (doDebug) Debug.Log("Too far from ground");
                                }
                                else
                                {
                                    // Valid position
                                    validPosition = true;
                                    if (doDebug) Debug.Log("Valid position");
                                }
                            }
                            else
                            {
                                // No ground found
                                validPosition = false;
                                if (doDebug) Debug.Log("No ground found");
                            }
                        }
                    }
                    return validPosition;
                    break;
                case ConstructionComponentData.StructureType.Wall:
                    // Check for nearby snapping
                    if (HasAvailableConnection(tryPosition, settings, data, out finalPosition, out finalRotation, out snappedTransforms))
                    {
                        // Check for collision
                        if (previewObject.IsColliding(finalPosition, finalRotation, settings.CollisionLayerMask, snappedTransforms))
                        {
                            validPosition = false;
                            if (doDebug) Debug.Log("Colliding");
                        }
                        else
                        {
                            // Valid position
                            validPosition = true;
                            if (doDebug) Debug.Log("Valid position");
                        }
                    }
                    else
                    {

                        if (doDebug) Debug.Log("Need a foundation to snap to");

                        finalPosition = tryPosition;
                        finalRotation = tryRotation;
                        validPosition = false;
                    }

                    return validPosition;
                    break;
            }
            finalPosition = tryPosition;
            finalRotation = tryRotation;
            validPosition = false;
            return false;
        }

        bool hasFlipped = false;
        private bool HasAvailableConnection(Vector3 tryPosition, ConstructionSettings settings, ConstructionData data, out Vector3 snappedPosition, out Quaternion snappedRotation, out List<Transform> snappedTransforms)
        {
            snappedTransforms = new List<Transform>();

            List<(Edge, float, Transform)> possibleEdges = new();
            foreach (Collider collider in Physics.OverlapSphere(tryPosition, settings.StructureCheckRadius, settings.ConstructionLayerMask, QueryTriggerInteraction.Ignore))
            {
                ConstructionComponent otherComponent = collider.GetComponentInParent<ConstructionComponent>();
                if (otherComponent != null)
                {
                    // Find the closest free edge that's in range
                    foreach (Edge edge in otherComponent.edges)
                    {
                        // Don't check for an edge if it is already used
                        if (data.isHorizontal && edge.usedHorizontally || data.isVertical && edge.usedVertically) continue;

                        float distance = edge.DistanceFromEdge(tryPosition, collider.transform, 1.5f, settings.StructurePlaceRadius);
                        if (distance == -1f) continue; // Edge was too far away
                        if (distance <= settings.StructurePlaceRadius)
                        {
                            possibleEdges.Add((edge, distance, collider.transform));
                            if (doDebug) Debug.Log($"Found possible edge: {edge.position} at distance {distance}");
                        }
                    }
                }
            }

            snappedPosition = Vector3.zero;
            snappedRotation = Quaternion.identity;

            Edge? edgeToSnapTo = null; // The other edge
            Edge? edgeToSnapFrom = null; // The edge on this component
            Transform otherTransform = null;
            if (possibleEdges.Count > 0)
            {
                // Sorts the list by distance
                possibleEdges.Sort((x, y) => x.Item2.CompareTo(y.Item2));

                // I'm not sure it's important to use a foreach here, it's probably fine to just choose the closest edge
                // There may be a case where we want to check multiple edges, but I can't think of one right now
                foreach ((Edge, float, Transform) otherEdge in possibleEdges)
                {
                    Vector3 edgeDirection = otherEdge.Item1.WorldSpaceRotation(otherEdge.Item3);

                    List<(Edge, float)> possibleEdgesOnThisComponent = new();
                    foreach (Edge tryEdge in edges)
                    {
                        Vector3 tryEdgeNormal = tryEdge.WorldSpaceRotation(transform);

                        float dot = Vector3.Dot(Vector3.forward, tryEdge.normal);
                        possibleEdgesOnThisComponent.Add((tryEdge, dot));

                        if (doDebug) Debug.Log($"Found possible edge on this component: {tryEdge.position} with dot {dot}");
                    }

                    // Sort by dot product
                    if (possibleEdgesOnThisComponent.Count > 0)
                    {
                        // if (GlobalVariables.FlipRotation)
                        // {
                        //     possibleEdgesOnThisComponent.Sort((x, y) => y.Item2.CompareTo(x.Item2));
                        //     // if (!hasFlipped)
                        //     // {
                        //     //     hasFlipped = true;
                        //     //     possibleEdgesOnThisComponent.Sort((x, y) => y.Item2.CompareTo(x.Item2)); // Descending
                        //     // }
                        //     // else
                        //     // {
                        //     //     possibleEdgesOnThisComponent.Sort((x, y) => x.Item2.CompareTo(y.Item2)); // Ascending
                        //     // }
                        // }
                        // else
                        // {
                        //     hasFlipped = false;
                        //     possibleEdgesOnThisComponent.Sort((x, y) => x.Item2.CompareTo(y.Item2)); // Ascending
                        // }

                        possibleEdgesOnThisComponent.Sort((x, y) => x.Item2.CompareTo(y.Item2)); // Ascending

                        edgeToSnapTo = otherEdge.Item1;
                        edgeToSnapFrom = possibleEdgesOnThisComponent[0].Item1;
                        otherTransform = otherEdge.Item3;

                        // Here we finally choose the edge we are snapping to and from.
                        snappedTransforms.Add(otherTransform);
                        break;
                    }
                }

                // Couldn't find compatible edges
                if (edgeToSnapTo == null || edgeToSnapFrom == null || otherTransform == null)
                {
                    if (doDebug) Debug.Log("Couldn't find compatible edges");
                    hasFlipped = false;
                    return false;
                }

                (Vector3, Quaternion) snappedPositionOffset = Edge.SnapEdgeToEdge((Edge)edgeToSnapFrom, (Edge)edgeToSnapTo, transform, otherTransform);
                snappedPosition = snappedPositionOffset.Item1;
                snappedRotation = snappedPositionOffset.Item2;


                if (doDebug) Debug.Log($"Snapped to edge: {edgeToSnapTo.position} from edge: {edgeToSnapFrom.position}");

                return true;
            }

            return false;
        }
    }
}
