using System.Collections.Generic;
using Mirror.BouncyCastle.Asn1.Misc;
using UnityEngine;

namespace Construction
{
    /// <summary>
    /// Goes on the construction component object.
    /// Handles placing and snapping of components on eachother.
    /// </summary>
    public class ConstructionComponent : MonoBehaviour, IDamageable
    {
        [Tooltip("Optional preview component for syncing data.")]
        public PreviewConstructionComponent previewComponent;
        [Header("Stability")]
        // [Tooltip("The stability this component contributes to its neighbors, or how much it supports them. (When they are above it)")]
        // [SerializeField] private float neighborStabilityContributionUpwards = 0.1f;
        // [Tooltip("The stability this component contributes to its neighbors, or how much it supports them. (When they are below it)")]
        // [SerializeField] private float neighborStabilityContributionDownwards = 0.1f;
        // [Tooltip("The stability this component contributes to its neighbors, or how much it supports them. (When they are to the sides of it)")]
        // [SerializeField] private float neighborStabilityContributionHorizontal = 0.1f;
        // [Tooltip("The stability this component has inherently, or how much it supports itself.")]
        // [SerializeField] private float inherentStability = 0.1f;
        // [Tooltip("The minimum stability this component can have before it collapses.")]
        // [SerializeField] private float minimumStability = 0.2f;
        // [Tooltip("The curve that determines the stability of this component (and contributed stability) based on its health.")]
        [Tooltip("This is sort of complicated. Ask Elliot if you want to know what it does.")]
        [SerializeField] private float inherentStability = 0f;
        [Tooltip("The minimum amount of stability this component can have before it collapses.")]
        [SerializeField] private float minimumStability = 50f;
        
        [Header("Health")]
        [Tooltip("The health of this component, or how much damage it can take before it collapses.")]
        [SerializeField] private float maximumHealth = 100f;

        public List<Edge> edges; // Public for editor
        private Dictionary<Edge, List<ConstructionComponent>> connections = new Dictionary<Edge, List<ConstructionComponent>>();
        private float stability = 0f;
        public float Stability => stability;
        private float health;

        private void Awake()
        {
            // Assign edge transforms
            foreach (Edge edge in edges)
            {
                edge.Transform = transform;
            }

            health = maximumHealth;

            TriggerNearbyImplicitConnections(PlayerInstance.Instance.GetComponentInChildren<ConstructionCore>().Settings);
            // EvaluateStability();
        }

        private void Start()
        {
            EvaluateStability();
        }

        // Try to add a connection explicitly (meant for construction by player)
        public void AddConnections(ConstructionData data, ConstructionSettings settings)
        {
            foreach (Edge edge in edges)
            {
                Vector3 worldSpacePosition = transform.position + transform.rotation * edge.position;

                Collider[] colliders = Physics.OverlapSphere(worldSpacePosition, 0.1f, settings.ConstructionLayerMask);
                if (colliders.Length > 0)
                {
                    foreach (Collider collider in colliders)
                    {
                        ConstructionComponent otherComponent = collider.GetComponentInParent<ConstructionComponent>();
                        if (otherComponent != null && otherComponent != this)
                        {
                            if (otherComponent.GetEdge(worldSpacePosition, out Edge otherEdge))
                            {
                                if (data.isHorizontal) // TODO: Fix this
                                {
                                    edge.SetUsedHorizontally(true);
                                    otherEdge.SetUsedHorizontally(true);
                                }
                                if (data.isVertical)
                                {
                                    edge.SetUsedVertically(true);
                                    otherEdge.SetUsedVertically(true);
                                }

                                AddConnectionDirect(edge, otherComponent);
                                otherComponent.AddConnectionDirect(otherEdge, this);
                            }
                        }
                    }
                }
            }
        }

        public bool GetEdge(Vector3 tryPosition, out Edge correctEdge, float threshold = 0.1f)
        {
            foreach (Edge edge in edges)
            {
                if (edge.IsSame(tryPosition, threshold))
                {
                    correctEdge = edge;
                    return true;
                }
            }

            correctEdge = null;
            return false;
        }

        public void TriggerNearbyImplicitConnections(ConstructionSettings settings)
        {
            // Sphere overlap nearby components and run DetermineImplicitConnections on them
            foreach (Collider collider in Physics.OverlapSphere(transform.position, settings.StructurePlaceRadius, settings.ConstructionLayerMask))
            {
                ConstructionComponent otherComponent = collider.GetComponentInParent<ConstructionComponent>();
                if (otherComponent != null && otherComponent != this)
                {
                    // Debug.Log("Found nearby component: " + otherComponent.gameObject.name);
                    DetermineImplicitConnections(otherComponent);
                }
            }
        }

        // Called when a new component is placed nearby to add connections that are not explicitly defined via placement
        public Edge[] DetermineImplicitConnections(ConstructionComponent otherComponent)
        {
            List<Edge> implicitConnections = new List<Edge>();

            // Determine if any connections overlap
            foreach (Edge edge in edges)
            {
                foreach (Edge otherEdge in otherComponent.edges)
                {
                    // Debug.Log("Checking edge: " + edge.pointA + " " + edge.pointB + " against " + otherEdge.pointA + " " + otherEdge.pointB);
                    if (edge.IsSame(otherEdge))
                    {
                        // Debug.Log("Found implicit connection");
                        implicitConnections.Add(edge);
                    }
                }
            }

            // Add connections to our own dictionary
            foreach (Edge edge in implicitConnections)
            {
                AddConnectionDirect(edge, otherComponent);
                // Add connections to the other component's dictionary
                otherComponent.AddConnectionDirect(edge, this);
            }

            // Debug.Log("Implicit connections: " + implicitConnections.Count);

            return implicitConnections.ToArray();
        }

        public void AddConnectionDirect(Edge edge, ConstructionComponent component)
        {
            if (connections.ContainsKey(edge))
            {
                if (!connections[edge].Contains(component))
                {
                    connections[edge].Add(component);
                }
            }
            else
            {
                connections.Add(edge, new List<ConstructionComponent> { component });
            }
        }

        // public void RemoveConnection(Edge otherEdge, ConstructionComponent component)
        // {
        //     // Checks for valid edge
        //     Edge? baseEdge = null;
        //     foreach (Edge edge in edges)
        //     {
        //         if (edge.IsSame(otherEdge))
        //         {
        //             baseEdge = edge;
        //             break;
        //         }
        //     }

        //     if (baseEdge == null) return;

        //     // Remove the connection
        //     RemoveConnectionDirect(baseEdge, component);
        // }

        private void RemoveConnectionDirect(Edge edge, ConstructionComponent component)
        {
            if (connections.ContainsKey(edge))
            {
                connections[edge].Remove(component);
                Debug.Log("Connection Removed");
                
                // If the connection list is empty, remove the key
                if (connections[edge].Count == 0)
                {
                    connections.Remove(edge);
                }

                EvaluateStability();

            }
        }

        private void EvaluateStability()
        {
            float stashedStability = stability;
            stability = CalculateStability();
            // Debug.Log("Stability: " + stability + " Health: " + health + " Identity: " + gameObject.name);
            if (stability < minimumStability)
            {
                Collapse();
            }

            // If stability has changed, evaluate stability of neighbors
            if (stashedStability != stability)
            {
                // Evaluate stability of neighbors
                foreach (KeyValuePair<Edge, List<ConstructionComponent>> connection in connections)
                {
                    foreach (ConstructionComponent component in connection.Value)
                    {
                        component.TriggerStabilityCheck();
                    }
                }
            }
        }

        private float CalculateStability()
        {
            // First we get the maximum stability from neighbors
            float maximumNeighborStability = 0f;

            foreach (KeyValuePair<Edge, List<ConstructionComponent>> connection in connections)
            {
                // Debug.Log("Connection: " + connection.Key.pointA + " " + connection.Key.pointB);
                foreach (ConstructionComponent component in connection.Value)
                {
                    if (component == null) continue;

                    // Determine the stability contribution based on the direction
                    maximumNeighborStability = Mathf.Max(maximumNeighborStability, component.Stability);
                }
            }

            // If this component has negative inherent stability, it will reduce the maximum neighbor stability
            if (inherentStability < 0) {
                return maximumNeighborStability + inherentStability;
            }

            // If this component has a positive inherent stability, take the inherent stability as the stability
            if (inherentStability > 0) {
                return inherentStability;
            }

            // If this component has no inherent stability, take the maximum neighbor stability
            return maximumNeighborStability;
        }

        public void Damage(float damage)
        {
            health -= damage;

            if (health <= 0)
            {
                Collapse();
                return;
            }

            // Evaluate stability of neighbors
            foreach (KeyValuePair<Edge, List<ConstructionComponent>> connection in connections)
            {
                foreach (ConstructionComponent component in connection.Value)
                {
                    component.TriggerStabilityCheck();
                }
            }
        }

        public void DestroyComponent()
        {
            Damage(health);
        }

        public void TriggerStabilityCheck()
        {
            EvaluateStability();
        }

        private void Collapse()
        {
            Debug.Log("Collapsing " + gameObject.name);

            // Propagate collapse with stability
            foreach (KeyValuePair<Edge, List<ConstructionComponent>> connection in connections)
            {

                Debug.Log($"Component {gameObject.name} is collapsing, removing connection {connections.Count}");
                foreach (ConstructionComponent component in connection.Value)
                {
                    if (connection.Key.usedHorizontally)
                    {
                        component.edges.Find(x => x.IsSame(connection.Key)).SetUsedHorizontally(false);
                        Debug.Log("Removed horizontally");
                    }
                    if (connection.Key.usedVertically)
                    {
                        component.edges.Find(x => x.IsSame(connection.Key)).SetUsedVertically(false);
                    }
                    component.RemoveConnectionDirect(connection.Key, this);
                }
            }

            Destroy(gameObject);
        }

        public void TakeDamage(float damage, Vector3 hitPosition)
        {
            health -= damage;

            if (health <= 0)
            {
                Collapse();
                return;
            }
        }
    }
}