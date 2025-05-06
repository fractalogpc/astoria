using System.Collections;
using UnityEngine;

public class LightningController : MonoBehaviour
{

    public Transform goalTransform;

    public GameObject lightningPrefab;

  private void Awake()
  {
    timer = Random.Range(0f, SPAWN_FREQUENCY); // Randomize the initial timer
  }

  private void SpawnLightning(Vector3 start, Vector3 goal)
    {
        GameObject lightning = Instantiate(lightningPrefab, start, Quaternion.identity);
        lightning.GetComponent<LightningMeshGeneration>().Initialize(start, goal);
    }

    [SerializeField] private float START_RANGE = 500f;
    [SerializeField] private float GOAL_RANGE = 200f;
    [SerializeField] private float HEIGHT = 1500f;
    [SerializeField] private float SPAWN_TIME_RANGE = 5f;
    [SerializeField] private float SPAWN_FREQUENCY = 30f; // Frequency of lightning strikes
    [SerializeField] private int WAVE_COUNT = 10; // Time between lightning strikes in a wave
    private IEnumerator SpawnLightningWithDelay(Vector3 start, Vector3 goal, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnLightning(start, goal);
    }

    private void SpawnWave(Vector3 goal)
    {
        for (int i = 0; i < WAVE_COUNT; i++) // Number of lightning strikes in the wave
        {
            Vector3 randomDirection = Random.onUnitSphere * GOAL_RANGE;
            randomDirection.y = 0; // Keep the lightning strikes horizontal
            goal += randomDirection; // Randomize the goal position
            Vector3 randomOffset = Random.onUnitSphere * START_RANGE;
            float spawnDelay = Random.Range(0f, SPAWN_TIME_RANGE);
            Vector3 spawnPosition = goal + randomOffset + Vector3.up * HEIGHT; // Adjust height as needed

            StartCoroutine(SpawnLightningWithDelay(spawnPosition, goal, spawnDelay));
        }
    }

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > SPAWN_FREQUENCY) // Adjust the frequency of lightning strikes as needed
        {
            timer = 0f;
            SpawnWave(goalTransform.position); // Replace with your desired position
        }
    }

}
