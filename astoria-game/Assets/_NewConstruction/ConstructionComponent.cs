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
        [Tooltip("Required data for component.")]
        public ConstructionData Data;
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
                edge.Data = GetComponent<ConstructionComponent>().Data;
                Debug.Assert(edge.Data != null, "Assign a ConstructionData to the component!");
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
                                if (data.isHorizontal && otherEdge.Data.isHorizontal) // TODO: Fix this
                                {
                                    edge.SetUsedHorizontally(true);
                                    otherEdge.SetUsedHorizontally(true);
                                }
                                if (data.isVertical && otherEdge.Data.isVertical)
                                {
                                    edge.SetUsedVertically(true);
                                    otherEdge.SetUsedVertically(true);
                                }

                                AddConnectionDirect(edge, otherComponent);
                                // otherComponent.AddConnectionDirect(otherEdge, this);
                                // Turns out that I forgot the other edge adds its own connections through DetermineImplicitConnections
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
                Debug.Log("Added connection between " + gameObject.name + " and " + component.gameObject.name, gameObject);
            }
        }

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
            else
            {
                Debug.LogWarning("Connection not found! Something weird happened.");
            }
        }

        private void EvaluateStability()
        {
            float stashedStability = stability;
            stability = CalculateStability();

            if (stability < minimumStability)
            {
                Collapse();
                return;
            }

            // If stability has changed, evaluate stability of neighbors
            if (stashedStability != stability)
            {
                List<Edge> edgesToRemove = new();

                foreach (var connection in connections)
                {
                    if (connection.Value == null || connection.Value.Count == 0)
                    {
                        edgesToRemove.Add(connection.Key);
                        continue;
                    }

                    List<ConstructionComponent> componentsToRemove = new();

                    foreach (ConstructionComponent component in connection.Value)
                    {
                        if (component == null)
                        {
                            componentsToRemove.Add(component);
                            continue;
                        }

                        // Trigger stability check on the next frame
                        Invoke(nameof(DelayedStabilityCheck), 0f);
                    }

                    // Remove null components after iteration
                    foreach (var component in componentsToRemove)
                    {
                        connection.Value.Remove(component);
                    }

                    // Remove the edge if no valid components remain
                    if (connection.Value.Count == 0)
                    {
                        edgesToRemove.Add(connection.Key);
                    }
                }

                // Remove invalid edges after iteration
                foreach (var edge in edgesToRemove)
                {
                    connections.Remove(edge);
                }
            }
        }

        // Helper method to trigger the stability check
        private void DelayedStabilityCheck()
        {
            foreach (var connection in connections)
            {
                foreach (ConstructionComponent component in connection.Value)
                {
                    component?.TriggerStabilityCheck();
                }
            }
        }



        private float CalculateStability()
        {
            // First we get the maximum stability from neighbors
            float maximumNeighborStability = 0f;

            // Collect edges and components to clean up
            List<Edge> edgesToRemove = new();

            foreach (var connection in connections)
            {
                // Collect null components to remove
                List<ConstructionComponent> componentsToRemove = new();

                foreach (ConstructionComponent component in connection.Value)
                {
                    if (component == null)
                    {
                        componentsToRemove.Add(component);
                        continue;
                    }

                    maximumNeighborStability = Mathf.Max(maximumNeighborStability, component.Stability);
                }

                // Remove null components after iteration
                foreach (var component in componentsToRemove)
                {
                    connection.Value.Remove(component);
                }

                // Mark edges for removal if no valid components remain
                if (connection.Value.Count == 0)
                {
                    edgesToRemove.Add(connection.Key);
                }
            }

            // Remove invalid edges after the loop
            foreach (var edge in edgesToRemove)
            {
                connections.Remove(edge);
            }

            // If this component has negative inherent stability, it will reduce the maximum neighbor stability
            if (inherentStability < 0)
            {
                return maximumNeighborStability + inherentStability;
            }

            // If this component has a positive inherent stability, take the inherent stability as the stability
            if (inherentStability > 0)
            {
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

            // Collect connections to be removed
            List<(Edge, ConstructionComponent)> toRemove = new();

            foreach (KeyValuePair<Edge, List<ConstructionComponent>> connection in connections)
            {
                foreach (ConstructionComponent component in connection.Value)
                {
                    if (connection.Key.usedHorizontally)
                    {
                        component.edges.Find(x => x.IsSame(connection.Key))?.SetUsedHorizontally(false);
                        Debug.Log("Removed horizontally");
                    }
                    if (connection.Key.usedVertically)
                    {
                        component.edges.Find(x => x.IsSame(connection.Key))?.SetUsedVertically(false);
                    }

                    // Queue the connection for removal
                    toRemove.Add((connection.Key, component));
                }
            }

            // Remove the connections after iteration
            foreach (var (edge, component) in toRemove)
            {
                component.RemoveConnectionDirect(edge, this);
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