using System;
using Mirror;
using UnityEngine;

public class LocalPlayerReference : MonoBehaviour, IStartExecution
{
  public static LocalPlayerReference Instance { get; private set; }
  public bool LocalPlayerExists => _localPlayer != null;
  public GameObject LocalPlayer => _localPlayer;
  [SerializeField] private GameObject _localPlayer;

  private bool initialized;

  private void Awake()
  {
    if (Instance != null) Destroy(gameObject);
    Instance = this;
  }

  public void InitializeStart()
  {
    ResourceHolder.Instance.GameStateHandler.AddOnStateEnter(GameStateHandler.GameState.Playing, () =>
    {
      Initialize();
    });
  }

  private void Initialize() {
    _localPlayer = GetLocalPlayer();
    initialized = true;
  }

  private void Update()
  {
    if (LocalPlayerExists) return;
    _localPlayer = GetLocalPlayer();
  }

  private GameObject GetLocalPlayer()
  {
    if (!initialized) return null;

    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    foreach (GameObject player in players)
    {
      if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
      {
        return player;
      }
    }
    Debug.LogError("LocalPlayerReference: Could not find local player. You can ignore this if the host player has not spawned yet.");
    return null;
  }

  public InventoryComponent Inventory()
  {
    if (!LocalPlayerExists)
    {
      Debug.LogError("LocalPlayerReference: Local player does not exist. Please ensure player exists before attempting to access inventory.");
      return null;
    }
    return _localPlayer.GetComponentInChildren<InventoryComponent>();
  }
}
