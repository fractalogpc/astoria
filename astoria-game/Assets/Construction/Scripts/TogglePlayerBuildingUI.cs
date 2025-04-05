using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simply toggles building menu.
/// </summary>
public class TogglePlayerBuildingUI : InputHandlerBase
{
    public UnityEvent OnBuildingUIOpen;
    public UnityEvent OnBuildingUIClose;

    public RadialMenu RadialMenu;

    public bool IsInventoryOpen { get; private set; }
    [SerializeField] private CanvasGroup _inventoryCanvasGroup;

    protected override void InitializeActionMap()
    {
        // RegisterAction(_inputActions.Player.Build, ctx => SetVisibility(true));
        // RegisterAction(_inputActions.BuildingUI.CloseMenu, ctx => SetVisibility(false));
    }

  private void Start()
  {
    SetVisibility(false);
  }

  public void SetVisibility(bool show)
    {
        IsInventoryOpen = show;
        _inventoryCanvasGroup.alpha = show ? 1 : 0;
        _inventoryCanvasGroup.blocksRaycasts = show;
        _inventoryCanvasGroup.interactable = show;
        IsInventoryOpen = show;
        if (show)
        {
            RadialMenu.Enabled = true;
            OnBuildingUIOpen?.Invoke();
            Cursor.visible = true;
            InputReader.Instance.InputActions.FindAction("Look").Disable();
            Cursor.lockState = CursorLockMode.Confined;
            // InputReader.Instance.SwitchInputMap(InputMap.BuildingUI);
        }
        else
        {
            RadialMenu.Enabled = false;
            OnBuildingUIClose?.Invoke();
            Cursor.visible = false;
            InputReader.Instance.InputActions.FindAction("Look").Enable();
            Cursor.lockState = CursorLockMode.Locked;
            // InputReader.Instance.SwitchInputMap(InputMap.Player);
        }
    }

    public void ToggleVisibility()
    {
        SetVisibility(!IsInventoryOpen);
    }
}
