using Mirror;
using UnityEngine;
using UnityEngine.Android;

// Changed this to initialize only when the player interacts with the station
public class InitializeWithPlayerInventory : NetworkBehaviour
{
    [SerializeField] private InventoryComponent _componentToInitialize;
    
    public void Initialize() {
        InventoryComponent mainPlayerInventory = NetworkClient.localPlayer.gameObject.GetComponentInChildren<InventoryComponent>();
        print(mainPlayerInventory.InventoryData == null);
        _componentToInitialize.CreateInvFromInventoryData(mainPlayerInventory.InventoryData);
        print("Running initalize player inventory for station");
    }
}
