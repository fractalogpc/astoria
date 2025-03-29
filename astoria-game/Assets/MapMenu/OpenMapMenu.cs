using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class OpenMapMenu : InputHandlerBase
{

    [SerializeField] private CanvasGroup _mapMenuCanvasGroup;

    protected override void InitializeActionMap()
    {
        RegisterAction(_inputActions.Player.Map, ctx => GetComponent<ToggleUIVisibility>().SetVisibility(true));
    }

}