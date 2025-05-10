using UnityEngine;
using System.Collections.Generic;

public class TornadoSpawner : MonoBehaviour
{

    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float alpha = 0.4f;
    [SerializeField] private float spawnDelayMin = 0.5f;
    [SerializeField] private float spawnDelayMax = 1.5f;
    [SerializeField] private int maxTornadoes = 5;
    [SerializeField] private float tornadoMinSpeed = 1f;
    [SerializeField] private float tornadoMaxSpeed = 5f;
    [SerializeField] private float tornadoLifetime = 5f;
    [SerializeField] private float tornadoScaleMin = 0.5f;
    [SerializeField] private float tornadoScaleMax = 2f;
    [SerializeField] private float tornadoSpawnHeight = 0f;

    private float spawnDelay;
    private int tornadoCount = 0;
    private float nextSpawnTime = 0f;
    private List<GameObject> tornadoes = new List<GameObject>();

    private void Start()
    {
        spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
        nextSpawnTime = Time.time + spawnDelay;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime && tornadoCount < maxTornadoes)
        {
            SpawnTornado();
            spawnDelay = Random.Range(spawnDelayMin, spawnDelayMax);
            nextSpawnTime = Time.time + spawnDelay;
        }

        for (int i = tornadoes.Count - 1; i >= 0; i--)
        {
            if (tornadoes[i] == null)
            {
                tornadoes.RemoveAt(i);
                tornadoCount--;
            }
        }
    }

    private void SpawnTornado()
    {
        Vector3 spawnPosition = spawnCenter.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = tornadoSpawnHeight;

        GameObject tornado = Instantiate(tornadoPrefab, spawnPosition, Quaternion.identity);
        tornado.transform.localScale = Vector3.one * Random.Range(tornadoScaleMin, tornadoScaleMax);

        Tornado tornadoScript = tornado.GetComponent<Tornado>();
        tornadoScript.SetSpeed(Random.Range(tornadoMinSpeed, tornadoMaxSpeed));
        tornadoScript.SetLifetime(tornadoLifetime);

        tornadoes.Add(tornado);
        tornadoCount++;
    }

}
