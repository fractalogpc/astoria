using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveSystemView : MonoBehaviour
{
	[SerializeField] private EaseElementToPosition _objectivePanelEaseMapUI;
	[SerializeField] private TextMeshProUGUI _titleGameUI;
	[SerializeField] private TextMeshProUGUI _descGameUI;

	public void SetGameUITitle(string text) {
		_titleGameUI.text = text;
	}
	public void SetGameUIDescription(string text) {
		_descGameUI.text = text;
	}
	public void OpenMapPanel() {
		_objectivePanelEaseMapUI.EaseToPosition(1);
	}
	public void CloseMapPanel() {
		_objectivePanelEaseMapUI.EaseToPosition(0);
	}
	
}