using UnityEngine;
using System.Collections.Generic;

public class GroundHarvestableSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] harvestablePrefabs;
    [SerializeField] private int maxHarvestables = 10;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private float spawnDelayVariation = 2f;
    [SerializeField] private LayerMask groundLayer;

    private float nextSpawnTime;
    private float timeSinceLastSpawn;
    private List<Transform> spawnedHarvestables = new List<Transform>();

    private void Start()
    {
        for (int i = 0; i < maxHarvestables; i++)
        {
            SpawnHarvestable();
        }
    }

    private void SpawnHarvestable()
    {
        // Check if all harvestables exist
        for (int i = spawnedHarvestables.Count - 1; i >= 0; i--)
        {
            if (spawnedHarvestables[i] == null)
            {
                spawnedHarvestables.RemoveAt(i);
            }
        }

        if (spawnedHarvestables.Count >= maxHarvestables)
        {
            return;
        }

        Vector3 randomPoint = Random.insideUnitSphere * spawnRadius;
        randomPoint.y = 0;
        randomPoint += transform.position;

        if (Physics.Raycast(randomPoint + Vector3.up * 25f, Vector3.down, out RaycastHit hit, 50f, groundLayer))
        {
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            GameObject harvestable = Instantiate(harvestablePrefabs[Random.Range(0, harvestablePrefabs.Length)], hit.point, randomRotation, transform);
            // harvestable.transform.up = hit.normal;
            spawnedHarvestables.Add(harvestable.transform);
        }
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= nextSpawnTime)
        {
            timeSinceLastSpawn = 0;
            SpawnHarvestable();
            nextSpawnTime = spawnDelay + Random.Range(-spawnDelayVariation, spawnDelayVariation);
        }
    }

}
