using UnityEngine;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _wandererPrefab;
    private bool _night = false;
    private GameObject[] _players;
    private List<GameObject> _wanderers;

    void Start() {
        _players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update() {
        if (_night && (1200 - Time.time) < 900 && (1200 - Time.time) % 30 == 0) {
            foreach (GameObject player in _players) {
                _wanderers.Add(SpawnWanderer(SpawnPosition(player.transform.position)));
            }
        }

        foreach (GameObject wanderer in _wanderers) {
            foreach (GameObject player in _players) {
                if (Vector3.Distance(wanderer.transform.position, player.transform.position) < 300) {
                    _wanderers.Remove(wanderer);
                    Destroy(wanderer);
                }
            }
        }
    }

    public GameObject SpawnWanderer(Vector3 position) {
        Debug.Log("Spawning wanderer at " + position);
        GameObject wanderer = Instantiate(_wandererPrefab, position, Quaternion.identity);
        return wanderer;
    }

    public void OnNight() {
        _night = true;
        foreach (GameObject player in _players) {
            for (int i = 0; i < 5; i++) {
                _wanderers.Add(SpawnWanderer(SpawnPosition(player.transform.position)));
            }
        }
    }

    public void OnDay() {
        _night = false;
    }

    private Vector3 SpawnPosition(Vector3 position) {
        float x = position.x + Random.Range(-100, 100);
        float z = position.z + Random.Range(-100, 100);
        float y = 0;
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, 10000, z), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"))) {
            y = 10000 - hit.distance; 
        }
        return new Vector3(x, y, z);
    }
}
