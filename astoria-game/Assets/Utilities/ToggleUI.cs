using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleUI : InputHandlerBase
{
  private bool _isInventoryOpen = false;

  [SerializeField] private CanvasGroup _inventoryCanvasGroup;

  protected override void InitializeActionMap()
  {
    RegisterAction(_inputActions.Player.Inventory, ctx => ToggleInventory());
    RegisterAction(_inputActions.UI.CloseUI, ctx => ToggleInventory());
  }

  private void ToggleInventory() {
    _isInventoryOpen = !_isInventoryOpen;
    
    _inventoryCanvasGroup.alpha = _isInventoryOpen ? 1 : 0;
    _inventoryCanvasGroup.blocksRaycasts = _isInventoryOpen;
    _inventoryCanvasGroup.interactable = _isInventoryOpen;

    Cursor.visible = _isInventoryOpen;
    Cursor.lockState = _isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;

    // Toggle the input map
    InputReader.Instance.SwitchInputMap(_isInventoryOpen ? InputMap.UI : InputMap.Player);
  }
}
