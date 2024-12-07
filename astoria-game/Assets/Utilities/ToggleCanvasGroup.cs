using System;
using UnityEngine;

public class ToggleCanvasGroup : InputHandlerBase
{
    [SerializeField] private CanvasGroup _canvasGroup;
    
    private bool _shown;

    private void Start() {
        SetVisibility(_shown);
    }

    protected override void InitializeActionMap()
    {
        RegisterAction(_inputActions.GenericUI.CloseUI, ctx => {
            // This is already closed, don't need to close it again.
            // This can happen if some other menu is open
            if (!_shown) return;
            SetVisibility(false);
        });
    }
    
    public void ToggleVisibility() {
        SetVisibility(!_shown);
    }
    
    private void SetVisibility(bool show)
    {
        _canvasGroup.alpha = show ? 1 : 0;
        _canvasGroup.blocksRaycasts = show;
        _canvasGroup.interactable = show;
        _shown = show;
        if (show) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            InputReader.Instance.SwitchInputMap(InputMap.GenericUI);
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            InputReader.Instance.SwitchInputMap(InputMap.Player);
        }
    }
}
