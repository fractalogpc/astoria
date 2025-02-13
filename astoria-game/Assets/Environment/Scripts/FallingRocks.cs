using UnityEngine;

public class FallingRocks : MonoBehaviour
{

    [SerializeField] private Transform[] rockSpawnPoints;
    [SerializeField] private GameObject rockPrefab;

    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private float minSpawnChance = 0.1f;
    [SerializeField] private float maxSpawnChance = 0.5f;
    [SerializeField] private float rockLifetime = 30f;
    // TODO: Add randomization to spawn interval if anyone cares

    private float timeSinceLastSpawn = 0;
    private float spawnIntensity = 1;

    public void SetSpawnIntensity(float newSpawnIntensity)
    {
        spawnIntensity = newSpawnIntensity;
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = 0;

            if (Random.value < spawnIntensity)
            {
                Vector3 spawnPoint = rockSpawnPoints[Random.Range(0, rockSpawnPoints.Length)].position;
                spawnPoint += new Vector3(Random.Range(-spawnRadius, spawnRadius), spawnHeight, Random.Range(-spawnRadius, spawnRadius));

                Destroy(Instantiate(rockPrefab, spawnPoint, Quaternion.identity), rockLifetime);
            }
        }
    }

}
