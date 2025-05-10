using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{

    [System.Serializable]
    private struct Enemy
    {

        public GameObject prefab;
        public float difficultyPoints;

    }

    [SerializeField] private Enemy[] _enemies;
    [SerializeField] private float _spawnRadius = 100;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Difficulty scaling, into function y = ax^2 + b")]
    [SerializeField] private float _difficultyCurveATerm = 0.05f;
    [SerializeField] private float _difficultyCurveBTerm = 10f;
    [SerializeField] private float _maxProportionPerEnemy = 0.4f;

    [Header("Patrollster settings")]
    [SerializeField] private Transform enemyContainer;

    private GameObject _player;
    private List<GameObject> _monsters;
    private int _nightsSurvived = 1;

    void Start() {
        _player = GameObject.FindWithTag("Player");
        foreach (Transform patrollster in enemyContainer) {
            patrollster.gameObject.GetComponent<EnemyCore>().SetPlayer(_player.transform);
        }
    }

    void Update() {
        
    }

    public GameObject SpawnWanderer(Vector3 position, GameObject _enemyToSpawn) {
        // Debug.Log("Spawning enemy at " + position);
        GameObject wanderer = Instantiate(_enemyToSpawn, position, Quaternion.identity, _enemyContainer);
        wanderer.GetComponent<EnemyCore>().SetPlayer(_player.transform);
        return wanderer;
    }

    public void SpawnWave(int waveNumber) {
        float difficultyPoints = _difficultyCurveATerm * Mathf.Pow(waveNumber, 2) + _difficultyCurveBTerm;

        List<Enemy> enemiesToSpawn = new List<Enemy>();
        foreach (Enemy enemy in _enemies) {
            Debug.Log(enemy.difficultyPoints);
            Debug.Log(difficultyPoints);
            Debug.Log(_difficultyCurveBTerm);
            if (enemy.difficultyPoints <= difficultyPoints * _maxProportionPerEnemy) {
                enemiesToSpawn.Add(enemy);
            }
        }

        // Sort enemies by difficulty points
        enemiesToSpawn.Sort((a, b) => a.difficultyPoints.CompareTo(b.difficultyPoints));

        float minEnemySpawnCost = enemiesToSpawn[0].difficultyPoints;

        while (difficultyPoints >= minEnemySpawnCost) {
            // Create weighted list of enemies to spawn
            List<(Enemy, float)> weightedEnemies = new List<(Enemy, float)>();

            float sum = 0;
            foreach (Enemy enemy in enemiesToSpawn) {
                float weight = enemy.difficultyPoints;
                weightedEnemies.Add((enemy, weight));
                sum += weight;
            }

            for (int i = 0; i < weightedEnemies.Count; i++) {
                weightedEnemies[i] = (weightedEnemies[i].Item1, weightedEnemies[i].Item2 / sum);
            }

            float randomValue = Random.value;

            float cumulative = 0;
            for (int i = 0; i < weightedEnemies.Count; i++) {
                cumulative += weightedEnemies[i].Item2;
                if (randomValue <= cumulative) {
                    // Spawn enemy
                    GameObject enemy = SpawnWanderer(SpawnPosition(_player.transform.position), weightedEnemies[i].Item1.prefab);
                    difficultyPoints -= weightedEnemies[i].Item1.difficultyPoints;
                    break;
                }
            }
        }
    }

    public void OnNight() {
        SpawnWave(_nightsSurvived);
    }

    public void OnDay() {
        _nightsSurvived++;
    }

    private Vector3 SpawnPosition(Vector3 position) {
        Vector2 xzSpawnPosition = Random.insideUnitCircle.normalized * _spawnRadius;
        float x = position.x + xzSpawnPosition.x;
        float z = position.z + xzSpawnPosition.y;
        float y = 0;
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, 10000, z), Vector3.down, out hit, Mathf.Infinity, _groundLayer)) {
            y = 10000 - hit.distance; 
        }
        return new Vector3(x, y, z);
    }
    
    public void OnHourChanged() {
        if (Random.value < 0.3f) {
            SpawnWanderer(SpawnPosition(_player.transform.position), _enemies[1].prefab);
        }
    }
}