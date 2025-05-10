using System;
using UnityEngine;

/// <summary>
/// Use this class to toggle the visibility of a UI menu. This will handle fading, cursor visibility and input map switching for you.
/// You only need to specify when to show the menu. This will automatically hide/disable the UI when UIClose actions are triggered.
/// </summary>
[RequireComponent(typeof(FadeElementInOut))]
[RequireComponent(typeof(CanvasGroup))]
public class ToggleUIVisibility : InputHandlerBase
{
	[SerializeField] private FadeElementInOut _fadeElementInOut;
	
	private bool _shown;

	public void ToggleVisibility() {
		SetVisibility(!_shown);
	}

	public void SetVisibility(bool show) {
		_shown = show;
		if (show) {
			_fadeElementInOut.FadeIn();
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			InputReader.Instance.SwitchInputMap(InputMap.GenericUI);
			print("Switching Input map to GenericUI");
		}
		else {
			_fadeElementInOut.FadeOut();
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			InputReader.Instance.SwitchInputMap(InputMap.Player);
		}
	}
	
	protected override void InitializeActionMap() {
		RegisterAction(_inputActions.GenericUI.CloseUI, ctx => {
			// This is already closed, don't need to close it again.
			// This can happen if some other menu is open
			if (!_shown) return;
			SetVisibility(false);
		});
	}
	
	private void OnValidate() {
		_fadeElementInOut = GetComponent<FadeElementInOut>();
	}

	private void Start() {
		// SetVisibility(_shown);
	}



}