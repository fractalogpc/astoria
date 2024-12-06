using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TogglePlayerInventoryUI : InputHandlerBase
{
  private bool _isInventoryOpen = false;
  [SerializeField] private CanvasGroup _inventoryCanvasGroup;

  protected override void InitializeActionMap()
  {
    RegisterAction(_inputActions.Player.Inventory, ctx => SetVisibility(true));
    RegisterAction(_inputActions.UI.CloseInventory, ctx => SetVisibility(false));
  }
    
  private void SetVisibility(bool show)
  {
    _isInventoryOpen = show;
    _inventoryCanvasGroup.alpha = show ? 1 : 0;
    _inventoryCanvasGroup.blocksRaycasts = show;
    _inventoryCanvasGroup.interactable = show;
    _isInventoryOpen = show;
    if (show) {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
      InputReader.Instance.SwitchInputMap(InputMap.UI);
    }
    else {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
      InputReader.Instance.SwitchInputMap(InputMap.Player);
    }
  }
}
