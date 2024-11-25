using System;
using Mirror;
using UnityEngine;

public class LocalPlayerReference : NetworkBehaviour
{
    public LocalPlayerReference Instance { get; private set; }
    public bool LocalPlayerExists => _localPlayer != null;
    public GameObject LocalPlayer => _localPlayer;
    [SerializeField] private GameObject _localPlayer;

    private void Awake() {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    private void Update() {
        if (LocalPlayerExists) return;
        _localPlayer = GetLocalPlayer();
    }

    private GameObject GetLocalPlayer() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players) {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer) {
                return player;
            }
        }
        Debug.LogError("LocalPlayerReference: Could not find local player.");
        return null;
    }
}
