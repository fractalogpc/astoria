using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[RequireComponent(typeof(Interactable))]
public class ContainerManager : InputHandlerBase
{
	public UnityEvent OnOpen;
	public UnityEvent OnClose;
	
	[SerializeField] private FadeElementInOut _fadeCanvas;
	[Header("Inventories")]
	[SerializeField] private InventoryComponent _containerInventory;
	[SerializeField] private InventoryComponent _playerInvDisplay;

	private InventoryComponent _playerInventory;

	public void Interact() {
		if (_playerInventory == null) {
			_playerInventory = NetworkClient.localPlayer.gameObject.GetComponentInChildren<InventoryComponent>();
			if (_playerInventory == null) {
				Debug.LogError("ContainerManager: Player Inventory not found! Ensure the player has an InventoryComponent, and that it is a child of the player GameObject.");
				return;
			}
		}
		if (_playerInvDisplay.InventoryData == null) _playerInvDisplay.CreateInvFromInventoryData(_playerInventory.InventoryData);
		
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		InputReader.Instance.SwitchInputMap(InputMap.GenericUI);
		_fadeCanvas.FadeIn();
		OnOpen?.Invoke();
	}

	public void Close() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		InputReader.Instance.SwitchInputMap(InputMap.Player);
		_fadeCanvas.FadeOut();
		OnClose?.Invoke();
	}
	
	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.GenericUI.CloseUI, ctx => Close());
	}

	private void Start() {
		Interactable interactable = GetComponent<Interactable>();
		if (interactable != null) {
			interactable.OnInteract.AddListener(Interact);
		}
	}
}