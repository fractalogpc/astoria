using System;
using Mirror;
using UnityEngine;

public class LocalPlayerReference : MonoBehaviour
{
    public static LocalPlayerReference Instance { get; private set; }
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

    public InventoryComponent Inventory() {
        if (!LocalPlayerExists) {
            Debug.LogError("LocalPlayerReference: Local player does not exist. Please ensure player exists before attempting to access inventory.");
            return null;
        }
        return _localPlayer.GetComponentInChildren<InventoryComponent>();
    }
}
