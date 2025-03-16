using System.Collections.Generic;
using UnityEngine;

namespace Construction
{
    /// <summary>
    /// Goes on the construction component object.
    /// Handles placing and snapping of components on eachother.
    /// </summary>
    public class NewConstructionComponent : MonoBehaviour, IDamageable
    {

        private ConstructionData data; // Assigned by ConstructionCore, used for various stuff
        public PreviewConstructionComponent previewComponent;

        public float inherentStability = 0f;
        public float minimumStability = 50f;
        public float maximumHealth = 100f;

        public List<Edge> edges = new List<Edge>();
        private Dictionary<Edge, Connection> connections = new Dictionary<Edge, Connection>();
        private float stability = 0f;
        public float Stability => stability;
        private float health = 0f;


        private void Start()
        {
            foreach (Edge edge in edges)
            {
                edge.Transform = transform;
                edge.Data = data;
            }

            health = maximumHealth;

            CreateInitialConnections();
        }

        public void SetData(ConstructionData data)
        {
            this.data = data;
        }

        // For each edge in the component, check if it is colliding with any other component's edge then create a connection between them
        public void CreateInitialConnections()
        {

            if (data == null)
            {
                Debug.LogError("Data not set!");
                return;
            }

            foreach (Edge edge in edges)
            {
                List<NewConstructionComponent> nearbyComponents = edge.GetNearbyComponents(0.1f, new List<Transform> { transform });

                if (nearbyComponents.Count > 0)
                {
                    // If there are nearby components, add connections on THIS component
                    AddConnection(edge, nearbyComponents);

                    // Then add connections from the nearby components to this component
                    foreach (NewConstructionComponent component in nearbyComponents)
                    {
                        component.AddConnection(edge, this);
                    }
                }
            }
        }

        // Add a single connection to the dictionary
        public void AddConnection(Edge edge, NewConstructionComponent component)
        {
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
        public void AddConnection(Edge edge, List<NewConstructionComponent> components)
        {
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
            float currentStability = stability;
            stability = CalculateStability();

            if (stability < minimumStability)
            {
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
                List<NewConstructionComponent> components = connections[connectionEdge].GetComponents();
                if (components.Count == 0)
                {
                    edgesToRemove.Add(connectionEdge);
                    continue;
                }
                foreach (NewConstructionComponent component in components)
                {
                    if (component.Stability > maximumNeighborStability)
                    {
                        maximumNeighborStability = component.Stability;
                    }
                }
            }

            // Clean up empty connections
            foreach (Edge edge in edgesToRemove)
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

        // Update the stability of neighbouring components
        private void UpdateNeighbouringStability()
        {
            foreach (Edge connectionEdge in connections.Keys)
            {
                List<NewConstructionComponent> components = connections[connectionEdge].GetComponents();
                foreach (NewConstructionComponent component in components)
                {
                    component.EvaluateStability();
                }
            }
        }

        // Collapse the component
        private void Collapse()
        {
            stability = 0f;
            UpdateNeighbouringStability();

            Debug.Log($"{gameObject.name} collapsed!");
            Destroy(gameObject);
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
    }

    public class Connection
    {
        public List<NewConstructionComponent> components = new List<NewConstructionComponent>();


        public Connection(NewConstructionComponent component)
        {
            components.Add(component);
        }

        public Connection(List<NewConstructionComponent> components)
        {
            this.components = components;
        }

        public List<NewConstructionComponent> GetComponents()
        {
            List<NewConstructionComponent> validComponents = new List<NewConstructionComponent>();
            foreach (NewConstructionComponent component in components)
            {
                if (component != null)
                {
                    validComponents.Add(component);
                }
            }

            components = validComponents;
            return components;
        }

        public void AddComponent(NewConstructionComponent component)
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

        public void AddComponents(List<NewConstructionComponent> components)
        {
            foreach (NewConstructionComponent component in components)
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