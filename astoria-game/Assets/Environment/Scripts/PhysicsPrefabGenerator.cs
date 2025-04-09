using UnityEngine;
using System.Collections.Generic;

public class PhysicsPrefabGenerator : MonoBehaviour
{
    
    [SerializeField] private GameObject[] prefabsToGenerate;
    [SerializeField] private int numberOfPrefabsToGenerate = 10;
    [SerializeField] private Vector3 generationCenter;
    [SerializeField] private Vector3 generationSize = new Vector3(10, 10, 10);
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2.0f;
    [SerializeField] private float minRotation = 0f;
    [SerializeField] private float maxRotation = 360f;
    [SerializeField] private float delayBetweenGenerations = 0.5f; // Delay between each prefab generation

    [SerializeField] private float timeBeforeSnapshot = 5f; // Time before taking a snapshot
    [SerializeField] private string snapshotName = "Snapshot"; // Name of the snapshot
    [SerializeField] private string snapshotPath = "Assets/Environment/Prefabs/"; // Path to save the snapshot

    [SerializeField] private GameObject generatedParent; // Parent object to hold the generated prefabs

    private void Start()
    {
        GeneratePrefabs();
    }

    private void OnEnable()
    {
        Invoke("TakeSnapshot", timeBeforeSnapshot);
    }
    
    private void OnDisable()
    {
        CancelInvoke("TakeSnapshot");
    }

    private void TakeSnapshot()
    {
        // Create a new snapshot of the generated prefabs
        string snapshotFilePath = System.IO.Path.Combine(snapshotPath, snapshotName + ".prefab");
        UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(generatedParent, snapshotFilePath, UnityEditor.InteractionMode.UserAction);
        
        // Debug.Log("Snapshot saved at: " + snapshotFilePath);
    }

    private void GeneratePrefabs()
    {
        StartCoroutine(GeneratePrefabsCoroutine());
    }

    private System.Collections.IEnumerator GeneratePrefabsCoroutine()
    {
        for (int i = 0; i < numberOfPrefabsToGenerate; i++)
        {
            GeneratePrefab();
            yield return new WaitForSeconds(delayBetweenGenerations);
        }
    }

    private void GeneratePrefab()
    {
        // Randomly select a prefab from the array
        GameObject prefabToGenerate = prefabsToGenerate[Random.Range(0, prefabsToGenerate.Length)];

        // Generate a random position within the specified area
        Vector3 randomPosition = new Vector3(
            Random.Range(generationCenter.x - generationSize.x / 2, generationCenter.x + generationSize.x / 2),
            Random.Range(generationCenter.y - generationSize.y / 2, generationCenter.y + generationSize.y / 2),
            Random.Range(generationCenter.z - generationSize.z / 2, generationCenter.z + generationSize.z / 2)
        );

        // Instantiate the prefab at the random position with a random rotation and scale
        GameObject generatedPrefab = Instantiate(prefabToGenerate, randomPosition, Quaternion.Euler(
            Random.Range(minRotation, maxRotation),
            Random.Range(minRotation, maxRotation),
            Random.Range(minRotation, maxRotation)
        ));

        // Set a random scale for the generated prefab
        float randomScale = Random.Range(minScale, maxScale);
        generatedPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        generatedPrefab.transform.SetParent(generatedParent.transform); // Set the parent of the generated prefab to the empty transform
    }

}
