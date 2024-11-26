using Mirror;
using UnityEngine;

public class PlayerReady : NetworkBehaviour
{
  public void Start()
  {
    Debug.LogWarning("Matthew please fix this, isLocalPlayer is always false");
    // if (isLocalPlayer)
    // {
      CmdSetPlayerReady();
      Debug.Log("Player is ready.");
    // }
  }

  [Command]
  public void CmdSetPlayerReady()
  {
    if (!isServer) return;
    FindFirstObjectByType<GameStateController>()?.OnPlayerReady();
  }
}
