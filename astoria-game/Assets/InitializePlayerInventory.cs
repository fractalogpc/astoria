using UnityEngine;
using UnityEngine.Android;

public class InitializePlayerInventory : MonoBehaviour, IStartExecution
{

    private bool _tryInitialize;
    private bool _initialized;

    public void InitializeStart() {
        ResourceHolder.Instance.GameStateHandler.AddOnStateEnter(GameStateHandler.GameState.Playing, () => {
            _tryInitialize = true;
            Initialize();
        });
    }

    public void Initialize() {
        Debug.Log("Removed InitializePlayerInventory initalization. Reimplement with NetworkClient.localPlayer instead.");
        // if (LocalPlayerReference.Instance.LocalPlayer == null) return;
        // Debug.Log(LocalPlayerReference.Instance.LocalPlayer.GetComponentInChildren<InventoryComponent>().InventoryData);
        // transform.GetComponent<InventoryComponent>().CreateInvFromInventoryData(LocalPlayerReference.Instance.LocalPlayer.GetComponentInChildren<InventoryComponent>().InventoryData);
        // _initialized = true;
    }

    private void Update() {
        if (!_initialized) {
            if (_tryInitialize) {
                Initialize();
            }
        }
    }
}
