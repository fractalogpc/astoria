using System.Collections.Generic;
using UnityEngine;

namespace Construction
{
    public class ConstructionComponent : MonoBehaviour
    {
        [Tooltip("Optional preview component for syncing data.")]
        public PreviewConstructionComponent previewComponent;
        [Header("Stability")]
        [Tooltip("The stability this component contributes to its neighbors, or how much it supports them. (When they are above it)")]
        [SerializeField] private float neighborStabilityContributionUpwards = 0.1f;
        [Tooltip("The stability this component contributes to its neighbors, or how much it supports them. (When they are below it)")]
        [SerializeField] private float neighborStabilityContributionDownwards = 0.1f;
        [Tooltip("The stability this component contributes to its neighbors, or how much it supports them. (When they are to the sides of it)")]
        [SerializeField] private float neighborStabilityContributionHorizontal = 0.1f;
        [Tooltip("The stability this component has inherently, or how much it supports itself.")]
        [SerializeField] private float inherentStability = 0.1f;
        [Tooltip("The minimum stability this component can have before it collapses.")]
        [SerializeField] private float minimumStability = 0.2f;
        [Tooltip("The curve that determines the stability of this component (and contributed stability) based on its health.")]
        [SerializeField] private AnimationCurve stabilityHealthCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Health")]
        [Tooltip("The health of this component, or how much damage it can take before it collapses.")]
        [SerializeField] private float maximumHealth = 100f;

        [Header("Settings")]
        [Tooltip("The distance to sphere cast to find nearby components to connect to from the transform 0,0.")]
        [SerializeField] private float _sphereRadius = 2f;
        [Tooltip("The layer to sphere cast on to find nearby components to connect to.")]
        [SerializeField] private LayerMask _checkLayer = default;

        public List<Edge> edges; // Public for editor
        private Dictionary<Edge, List<ConstructionComponent>> connections = new Dictionary<Edge, List<ConstructionComponent>>();
        private float stability;
        private float health;

        private void Awake()
        {
            health = maximumHealth;

            TriggerNearbyImplicitConnections();
            EvaluateStability();
        }

        // Try to add a connection explicitly (meant for construction by player)
        public void AddConnection(Edge otherEdge, ConstructionComponent component)
        {
            // Checks for valid edge
            Edge? baseEdge = null;
            foreach (Edge edge in edges)
            {
                if (edge.IsSame(otherEdge))
                {
                    baseEdge = edge;
                    break;
                }
            }

            if (baseEdge == null) return;

            // Add the connection
            AddConnectionDirect(baseEdge.Value, component);
        }

        public void TriggerNearbyImplicitConnections()
        {
            // Sphere overlap nearby components and run DetermineImplicitConnections on them
            foreach (Collider collider in Physics.OverlapSphere(transform.position, _sphereRadius, _checkLayer))
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
                    if (edge.IsSame(otherEdge, transform.position, otherComponent.transform.position, transform.rotation, otherComponent.transform.rotation))
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

        public void RemoveConnection(Edge otherEdge, ConstructionComponent component)
        {
            // Checks for valid edge
            Edge? baseEdge = null;
            foreach (Edge edge in edges)
            {
                if (edge.IsSame(otherEdge))
                {
                    baseEdge = edge;
                    break;
                }
            }

            if (baseEdge == null) return;

            // Remove the connection
            RemoveConnectionDirect(baseEdge.Value, component);
        }

        private void RemoveConnectionDirect(Edge edge, ConstructionComponent component)
        {
            if (connections.ContainsKey(edge))
            {
                connections[edge].Remove(component);
                if (connections[edge].Count == 0)
                {
                    connections.Remove(edge);
                    // Recalculate stability
                    EvaluateStability();
                }
            }
        }

        private void EvaluateStability()
        {
            stability = CalculateStability();
            // Debug.Log("Stability: " + stability + " Health: " + health + " Identity: " + gameObject.name);
            if (stability < minimumStability)
            {
                Collapse();
            }
        }

        private float CalculateStability()
        {

            // Debug.Log("Calculating stability for " + gameObject.name);
            // Debug.Log("Connections: " + connections.Count);
            foreach (KeyValuePair<Edge, List<ConstructionComponent>> connection in connections)
            {
                // Debug.Log("Connection: " + connection.Key.pointA + " " + connection.Key.pointB);
                foreach (ConstructionComponent component in connection.Value)
                {
                    // Debug.Log("Component: " + component.gameObject.name);
                }
            }
            // Calculate stability based on connections
            float stability = inherentStability * stabilityHealthCurve.Evaluate(health / maximumHealth);

            // Add stability from neighbors
            foreach (KeyValuePair<Edge, List<ConstructionComponent>> connection in connections)
            {
                foreach (ConstructionComponent component in connection.Value)
                {
                    // Determine the direction of the connection
                    Vector3 direction = transform.position - component.transform.position;
                    direction.Normalize();

                    // Determine the stability contribution based on the direction
                    stability += component.GetStabilityContribution(direction);
                }
            }

            return stability;
        }

        public float GetStabilityContribution(Vector3 direction)
        {
            float stabilityMultiplier = stabilityHealthCurve.Evaluate(health / maximumHealth);
            switch (direction.y)
            {
                case 1:
                    return neighborStabilityContributionUpwards * stabilityMultiplier;
                case -1:
                    return neighborStabilityContributionDownwards * stabilityMultiplier;
                default:
                    return neighborStabilityContributionHorizontal * stabilityMultiplier;
            }
        }

        public void Damage(float damage)
        {
            health -= damage;
            // Evaluate stability of neighbors
            foreach (KeyValuePair<Edge, List<ConstructionComponent>> connection in connections)
            {
                foreach (ConstructionComponent component in connection.Value)
                {
                    component.TriggerStabilityCheck();
                }
            }

            if (health <= 0)
            {
                Collapse();
            }
        }
        
        public void TriggerStabilityCheck()
        {
            EvaluateStability();
        }

        private void Collapse()
        {
            // Propagate collapse with stability
            foreach (KeyValuePair<Edge, List<ConstructionComponent>> connection in connections)
            {
                foreach (ConstructionComponent component in connection.Value)
                {
                    component.RemoveConnectionDirect(connection.Key, this);
                }
            }

            Destroy(gameObject);
        }

        private static bool Vector3Approximately(Vector3 a, Vector3 b)
        {
            if (Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z))
            {
                return true;
            }
            return false;
        }

    }
}