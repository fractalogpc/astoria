using System;
using UnityEngine;

public class InputReader : Singleton<InputReader>, IStartExecution {
  public PlayerInputActions InputActions { get; private set; }
  
  public static event Action<InputMap> OnBeforeInputMapChange;
  public static event Action<InputMap> OnAfterInputMapChange;
  
  private const InputMap DEFAULT_MAP = InputMap.Player;
  public InputMap CurrentMap { get; private set; } = InputMap.Null;
  
  protected override void Awake() {
    base.Awake();
    
    InputActions = new PlayerInputActions();
    InputActions.Enable();
  }

  public void InitializeStart() => SwitchInputMap(DEFAULT_MAP);

  public void SwitchInputMap(InputMap newInputMap) {
    if (newInputMap == CurrentMap) {
      Debug.LogWarning($"Trying to change to the same input map: {newInputMap}");
      return;
    }
    
    OnBeforeInputMapChange?.Invoke(newInputMap);

    CurrentMap = newInputMap;
    switch (newInputMap) {
      case InputMap.GenericUI:
        InputActions.Player.Disable();
        InputActions.InventoryUI.Disable();
        InputActions.GenericUI.Enable();
        break;
      case InputMap.Player:
        InputActions.GenericUI.Disable();
        InputActions.InventoryUI.Disable();
        InputActions.Player.Enable();
        break;
      case InputMap.InventoryUI:
        InputActions.Player.Disable();
        InputActions.GenericUI.Disable();
        InputActions.InventoryUI.Enable();
        break;
      case InputMap.ConsoleUI:
        InputActions.Player.Disable();
        InputActions.GenericUI.Disable();
        InputActions.InventoryUI.Disable();
        break;
      case InputMap.Null:
        InputActions.GenericUI.Disable();
        InputActions.InventoryUI.Disable();
        InputActions.Player.Disable();
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(newInputMap), newInputMap, null);
    }
    
    OnAfterInputMapChange?.Invoke(newInputMap);
    
    Debug.Log($"Changed Input Map: {newInputMap}");
  }
}

[Serializable]
public enum InputMap {
  Null = -1,
  GenericUI = 0,
  Player = 1,
  InventoryUI = 2,
  ConsoleUI = 3
}