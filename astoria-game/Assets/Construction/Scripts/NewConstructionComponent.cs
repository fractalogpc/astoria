using System.Collections.Generic;
using UnityEngine;

namespace Construction
{
    /// <summary>
    /// Goes on the construction component object.
    /// Handles placing and snapping of components on eachother.
    /// </summary>
    public class ConstructionComponent : MonoBehaviour, IDamageable
    {

        private ConstructionComponentData data; // Assigned by ConstructionCore, used for various stuff
        public PreviewConstructionComponent previewComponent;

        public float inherentStability = 0f;
        public float minimumStability = 50f;
        public float maximumHealth = 100f;

        public List<Edge> edges = new List<Edge>();
        private Dictionary<Edge, Connection> connections = new Dictionary<Edge, Connection>();
        private float stability = 0f;
        public float Stability => stability;
        private float health = 0f;

        [HideInInspector] public bool IsDeleted = false;


        private void Start()
        {
            health = maximumHealth;

            // CreateInitialConnections();
        }

        public void SetData(ConstructionData data)
        {
            this.data = (ConstructionComponentData)data;

            foreach (Edge edge in edges)
            {
                edge.Transform = transform;
                edge.Data = data;
            }
        }

        // For each edge in the component, check if it is colliding with any other component's edge then create a connection between them
        public void CreateInitialConnections()
        {
            // Debug.Log("Creating initial connections...");

            if (data == null)
            {
                Debug.LogError("Data not set!");
                return;
            }

            // Debug.Log($"Checking {edges.Count} edges...");

            foreach (Edge edge in edges)
            {
                // Debug.Log($"Checking edge {edge.position}");
                List<ConstructionComponent> nearbyComponents = edge.GetNearbyComponents(0.1f, new List<Transform> { transform });

                if (nearbyComponents.Count > 0)
                {
                    // If there are nearby components, add connections on THIS component
                    AddConnection(edge, nearbyComponents, data);

                    // Then add connections from the nearby components to this component
                    foreach (ConstructionComponent component in nearbyComponents)
                    {
                        Edge otherEdge = component.GetClosestEdge(edge.position);
                        component.AddConnection(otherEdge, this, data);
                    }

                    // Debug.Log($"Connected {gameObject.name} to {nearbyComponents.Count} components.");
                }
            }
        }

        // Add a single connection to the dictionary
        public void AddConnection(Edge edge, ConstructionComponent component, ConstructionComponentData data)
        {
            if (data.isHorizontal && component.data.isHorizontal)
            {
                edge.usedHorizontally = true;
            }

            if (data.isVertical && component.data.isVertical)
            {
                edge.usedVertically = true;
            }

            if (connections.ContainsKey(edge))
            {
                connections[edge].AddComponent(component);
            }
            else
            {
                connections.Add(edge, new Connection(component));
            }
        }

        // Add a list of connections to the dictionary
        public void AddConnection(Edge edge, List<ConstructionComponent> components, ConstructionComponentData data)
        {
            // Assign data related things
            foreach (ConstructionComponent component in components)
            {
                if (data.isHorizontal && component.data.isHorizontal)
                {
                    edge.usedHorizontally = true;
                }

                if (data.isVertical && component.data.isVertical)
                {
                    edge.usedVertically = true;
                }
            }

            if (connections.ContainsKey(edge))
            {
                connections[edge].AddComponents(components);
            }
            else
            {
                connections.Add(edge, new Connection(components));
            }
        }

        // Evaluate the stability of the component then updates neighbouring components if necessary
        public void EvaluateStability()
        {
            // Debug.Log("Evaluating stability...");
            float currentStability = stability;
            stability = CalculateStability();
            // Debug.Log($"Stability: {stability}");

            if (stability < minimumStability)
            {
                // Debug.Log("Stability too low!");
                Collapse();
                return;
            }

            if (currentStability != stability)
            {
                UpdateNeighbouringStability();
            }
        }

        // Calculate the stability of the component based on the connections
        private float CalculateStability()
        {
            float maximumNeighborStability = 0f;

            List<Edge> edgesToRemove = new List<Edge>();
            foreach (Edge connectionEdge in connections.Keys)
            {
                List<ConstructionComponent> components = connections[connectionEdge].GetComponents();
                if (components.Count == 0)
                {
                    edgesToRemove.Add(connectionEdge);
                    continue;
                }
                foreach (ConstructionComponent component in components)
                {
                    if (component.Stability > maximumNeighborStability)
                    {
                        maximumNeighborStability = component.Stability;
                    }
                }
            }

            /* 
            This is causing issues with foreach loops, if this becomes an issue look here, otherwise it seems fine to have empty lists lying around.
            
            // Clean up empty connections
            foreach (Edge edge in edgesToRemove)
            {
                connections.Remove(edge);
            }
            */

            // If this component has negative inherent stability, it will reduce the maximum neighbor stability
            if (inherentStability < 0)
            {
                return maximumNeighborStability + inherentStability;
            }

            // If this component has a positive inherent stability, take the inherent stability as the stability
            if (inherentStability > 0)
            {
                return Mathf.Max(maximumNeighborStability, inherentStability);
            }

            // If this component has no inherent stability, take the maximum neighbor stability
            return maximumNeighborStability;
        }

        // Update the stability of neighbouring components
        private void UpdateNeighbouringStability()
        {
            foreach (Edge connectionEdge in connections.Keys)
            {
                List<ConstructionComponent> components = connections[connectionEdge].GetComponents();
                foreach (ConstructionComponent component in components)
                {
                    component.EvaluateStability();
                }
            }
        }

        // Get the closest edge to a position
        public Edge GetClosestEdge(Vector3 position)
        {
            Edge closestEdge = null;
            float closestDistance = float.MaxValue;

            foreach (Edge edge in edges)
            {
                float distance = Vector3.Distance(edge.position, position);
                if (distance < closestDistance)
                {
                    closestEdge = edge;
                    closestDistance = distance;
                }
            }

            return closestEdge;
        }

        // Collapse the component
        private void Collapse()
        {
            IsDeleted = true; // Mark the component as deleted to prevent issues with recursion
            stability = 0f;
            UpdateNeighbouringStability();

            // Debug.Log($"{gameObject.name} collapsed!");
            Invoke(nameof(DelayedDestroy), 0f);
        }

        public void TakeDamage(float damage, Vector3 hitPosition)
        {
            health -= damage;
            if (health <= 0)
            {
                Collapse();
            }
        }

        public void Repair(float repairAmount)
        {
            health += repairAmount;
            if (health > maximumHealth)
            {
                health = maximumHealth;
            }
        }

        public void DestroyComponent() {
            Collapse();
        }

        private void DelayedDestroy()
        {
            Destroy(gameObject);
        }
    }

    public class Connection
    {
        public List<ConstructionComponent> components = new List<ConstructionComponent>();


        public Connection(ConstructionComponent component)
        {
            components.Add(component);
        }

        public Connection(List<ConstructionComponent> components)
        {
            this.components = components;
        }

        public List<ConstructionComponent> GetComponents()
        {
            List<ConstructionComponent> validComponents = new List<ConstructionComponent>();
            foreach (ConstructionComponent component in components)
            {
                if (component != null && !component.IsDeleted)
                {
                    validComponents.Add(component);
                }
            }

            components = validComponents;
            return components;
        }

        public void AddComponent(ConstructionComponent component)
        {
            if (!components.Contains(component))
            {
                components.Add(component);
            }
            else
            {
                Debug.LogWarning($"Connection already contains {component.gameObject.name}");
            }
        }

        public void AddComponents(List<ConstructionComponent> components)
        {
            foreach (ConstructionComponent component in components)
            {
                if (!components.Contains(component))
                {
                    components.Add(component);
                }
                else
                {
                    Debug.LogWarning($"Connection already contains {component.gameObject.name}");
                }
            }
        }

    }
}