using UnityEngine;
using System.Collections.Generic;

public class MiningRockArea : MonoBehaviour
{

    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private Transform[] rockSpawnPoints;
    [SerializeField] private int maxRocks = 5;
    [SerializeField] private float spawnTime = 300f;

    private Dictionary<Transform, GameObject> spawnedRocks = new Dictionary<Transform, GameObject>();
    private float timeSinceLastSpawn = 0f;
    private int currentRocks = 0;

    void Start()
    {
        for (int i = 0; i < maxRocks; i++)
        {
            Transform spawnPoint = rockSpawnPoints[Random.Range(0, rockSpawnPoints.Length)];
            GameObject rock = Instantiate(rockPrefab, spawnPoint.position, spawnPoint.rotation, transform);
            spawnedRocks.Add(spawnPoint, rock);
        }
        currentRocks = maxRocks;
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnTime)
        {
            timeSinceLastSpawn = 0f;
            CheckRocks();
            SpawnRock();
        }
    }

    private void CheckRocks()
    {
        // Because rocks destroy themselves when they are mined, we need to remove them from the dictionary when they are destroyed
        List<Transform> rocksToRemove = new List<Transform>();
        foreach (KeyValuePair<Transform, GameObject> rock in spawnedRocks)
        {
            if (rock.Value == null)
            {
                rocksToRemove.Add(rock.Key);
            }
        }

        foreach (Transform rock in rocksToRemove)
        {
            spawnedRocks.Remove(rock);
            currentRocks--;
        }
    }

    private void SpawnRock()
    {
        if (currentRocks >= maxRocks)
        {
            return;
        }
        Transform spawnPoint = rockSpawnPoints[Random.Range(0, rockSpawnPoints.Length)];
        if (spawnedRocks.ContainsKey(spawnPoint))
        {
            return;
        }
        GameObject rock = Instantiate(rockPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedRocks.Add(spawnPoint, rock);
        currentRocks++;
    }

}
