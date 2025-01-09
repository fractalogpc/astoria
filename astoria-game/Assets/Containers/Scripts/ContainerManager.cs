using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class ContainerManager : Interactable
{
	public UnityEvent OnOpen;
	public UnityEvent OnClose;
	
	[SerializeField] private FadeElementInOut _fadeCanvas;
	[Header("Inventories")]
	[SerializeField] private InventoryComponent _containerInventory;
	[SerializeField] private InventoryComponent _playerInvDisplay;

	private InventoryComponent _playerInventory;
	private bool _isOpen;
	
	public override void Interact() {
		base.Interact();
		if (_playerInventory == null) {
			_playerInventory = NetworkClient.localPlayer.gameObject.GetComponentInChildren<InventoryComponent>();
			if (_playerInventory == null) {
				Debug.LogError("ContainerManager: Player Inventory not found! Ensure the player has an InventoryComponent, and that it is a child of the player GameObject.");
				return;
			}
		}
		if (_playerInvDisplay.InventoryData == null) _playerInvDisplay.CreateInvFromInventoryData(_playerInventory.InventoryData);
		
		if (_isOpen) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			_fadeCanvas.FadeOut();
			_isOpen = false;
			OnClose?.Invoke();
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			_fadeCanvas.FadeIn();
			_isOpen = true;
			OnOpen?.Invoke();
		}
	}
}