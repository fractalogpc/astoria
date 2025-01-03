using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
  private void OnApplicationQuit()
  {
    CleanupNetwork();
  }

  private void OnDestroy()
  {
    CleanupNetwork();
  }

  private void CleanupNetwork()
  {
    if (NetworkServer.active)
    {
      Debug.Log("Shutting down NetworkServer...");
      NetworkServer.Shutdown();
    }

    if (NetworkClient.isConnected)
    {
      Debug.Log("Disconnecting NetworkClient...");
      NetworkClient.Disconnect();
    }
  }
}
