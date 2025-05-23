using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class ContainerManager : Interactable
{
	[Header("Container Settings")]
	public UnityEvent OnOpen;
	public UnityEvent OnClose;
	
	[SerializeField] protected ToggleUIVisibility _toggleUIVisibility;
	[Header("Inventories")]
	[SerializeField] protected InventoryComponent _containerInventory;
	[SerializeField] protected InventoryComponent _playerInvDisplay;

	protected InventoryComponent _playerInventory;

	public override void Interact() {
		if (_playerInventory == null) {
			_playerInventory = PlayerInstance.Instance.gameObject.GetComponentInChildren<InventoryComponent>();
			if (_playerInventory == null) {
				Debug.LogError("ContainerManager: Player Inventory not found! Ensure the player has an InventoryComponent, and that it is a child of the player GameObject.");
				return;
			}
		}
		if (!_playerInvDisplay.Initialized) _playerInvDisplay.CreateInvFromInventoryData(_playerInventory.InventoryData);
		_toggleUIVisibility.SetVisibility(true);
	}
	
	protected virtual void Start() {
		Interactable interactable = GetComponent<Interactable>();
		if (interactable != null) {
			interactable.OnInteract.AddListener(Interact);
		}
	}
}