using System;
using UnityEngine;

public class InputReader : Singleton<InputReader> {
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

  private void Start() => SwitchInputMap(DEFAULT_MAP);

  private void SwitchInputMap(InputMap newInputMap) {
    if (newInputMap == CurrentMap) {
      Debug.LogWarning($"Trying to change to the same input map: {newInputMap}");
      return;
    }
    
    OnBeforeInputMapChange?.Invoke(newInputMap);

    CurrentMap = newInputMap;
    switch (newInputMap) {
      case InputMap.UI:
        InputActions.Player.Disable();
        InputActions.UI.Enable();
        break;
      case InputMap.Player:
        InputActions.UI.Disable();
        InputActions.Player.Enable();
        break;
      case InputMap.Null:
        InputActions.UI.Disable();
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
  UI = 0,
  Player = 1,
}