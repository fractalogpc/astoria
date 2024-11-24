using UnityEngine;

public class PlayerInventoryReference : LocalPlayerSingleton<PlayerInventoryReference>
{
    public InventoryComponent Inventory => _inventory;
    [SerializeField] private InventoryComponent _inventory;
}
