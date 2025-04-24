using UnityEngine;
using Construction;

public class ConstructionBuilder : MonoBehaviour
{

    public static ConstructionBuilder Instance { get; private set; }

    [SerializeField] private ConstructionComponentData[] constructionComponents;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BuildConstruction(SaveSystem.BuildingComponent[] componentsToBuild) {
        int numberOfComponents = componentsToBuild.Length;

        ConstructionComponent[] components = new ConstructionComponent[numberOfComponents];

        // Instantiate all components in the scene
        for (int i = 0; i < numberOfComponents; i++)
        {
            var componentData = GetDataFromIndex(componentsToBuild[i].componentIndex);

            GameObject componentObject = Instantiate(componentData.PlacedPrefab, componentsToBuild[i].position, componentsToBuild[i].rotation);
            componentObject.GetComponent<ConstructionComponent>().SetHealth(componentsToBuild[i].health);
            componentObject.GetComponent<ConstructionComponent>().SetData(componentData);

            components[i] = componentObject.GetComponent<ConstructionComponent>();
        }

        // Make edges between all components
        for (int i = 0; i < numberOfComponents; i++) {
            components[i].CreateInitialConnections();
        }

        // Initialize all components
        for (int i = 0; i < numberOfComponents; i++) {
            components[i].EvaluateStability();
        }
    }

    private ConstructionComponentData GetDataFromIndex(int index) {
        foreach (var component in constructionComponents)
        {
            if (component.ComponentIndex == index)
            {
                return component;
            }
        }

        Debug.LogError($"Component with index {index} not found.");
        return null;
    }
}
