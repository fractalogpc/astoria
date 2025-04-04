using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueCaptionUI : MonoBehaviour
{
	[SerializeField] private DialogueReceiver _attachedReciever;
	[SerializeField] private FadeElementInOut _fadeElement;
	[SerializeField] private TextMeshProUGUI _captionText;
	private Coroutine _displayCoroutine;
	
	private void Start() {
		_attachedReciever.OnDialogueRecieved += OnDialogueReceived;
	}
	
	private void OnDialogueReceived(Dialogue dialogue) {
		if (!dialogue.DialogueData.AlwaysCaption) {
			if (PlayerPrefs.GetInt("Subtitles") == 0) return;
		}
		_captionText.text = dialogue.DialogueData.MessageText[0];
		_displayCoroutine = StartCoroutine(OnDialogueReceiveCoroutine(dialogue));
	}
	
	private IEnumerator OnDialogueReceiveCoroutine(Dialogue dialogue) {
		_fadeElement.FadeIn();
		yield return new WaitForSeconds(dialogue.DialogueData.Duration);
		_fadeElement.FadeOut();
	}
}
