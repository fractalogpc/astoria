using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

  [SerializeField] private float _spawnRate = 1f;
  [SerializeField] private GameObject _enemyPrefab;
  [SerializeField] private float _spawnRadius = 1f;

  private float _nextTimeToSpawn = 0f;

  private void Update() {
    if (Time.time >= _nextTimeToSpawn) {
      _nextTimeToSpawn = Time.time + 1f / _spawnRate;
      SpawnEnemy();
    }
  }

  private void SpawnEnemy() {
    Vector3 spawnPosition = transform.position + Random.insideUnitSphere * _spawnRadius;
    spawnPosition.y = 1;
    Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
  }
}
