using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class OpenMapMenu : InputHandlerBase
{
	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.Player.Map, ctx => GetComponent<ToggleUIVisibility>().SetVisibility(true));
	}
}