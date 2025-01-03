using Mirror;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    // Override OnApplicationQuit to handle cleanup
    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();  // Ensure the base behavior runs
        CleanupNetwork();          // Your custom cleanup code
    }

    // Override OnDestroy to handle cleanup
    public override void OnDestroy()
    {
        base.OnDestroy();         // Ensure the base behavior runs
        CleanupNetwork();          // Your custom cleanup code
    }

    // Custom cleanup method
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
