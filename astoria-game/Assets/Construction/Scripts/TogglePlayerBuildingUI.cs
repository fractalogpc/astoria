using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TogglePlayerBuildingUI : InputHandlerBase
{
  public bool IsInventoryOpen { get; private set; }
  [SerializeField] private CanvasGroup _inventoryCanvasGroup;

  protected override void InitializeActionMap()
  {
    RegisterAction(_inputActions.Player.Build, ctx => SetVisibility(true));
    RegisterAction(_inputActions.BuildingUI.CloseMenu, ctx => SetVisibility(false));
  }
    
  public void SetVisibility(bool show)
  {
    IsInventoryOpen = show;
    _inventoryCanvasGroup.alpha = show ? 1 : 0;
    _inventoryCanvasGroup.blocksRaycasts = show;
    _inventoryCanvasGroup.interactable = show;
    IsInventoryOpen = show;
    if (show) {
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
      InputReader.Instance.SwitchInputMap(InputMap.BuildingUI);
    }
    else {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
      InputReader.Instance.SwitchInputMap(InputMap.Player);
    }
  }
}
