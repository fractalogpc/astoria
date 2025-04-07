using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueCaptionUI : MonoBehaviour, IDialogueReceiver
{
	[SerializeField] private FadeElementInOut _fadeElement;
	[SerializeField] private TextMeshProUGUI _captionText;
	private Coroutine _displayCoroutine;
	public event IDialogueReceiver.DialogueReceived OnDialogueReceived;
	public void ReceiveDialogue(Dialogue dialogue) {
		if (!dialogue.DialogueData.AlwaysCaption) {
			// if (PlayerPrefs.GetInt("Subtitles") == 0) return;
			Debug.Log("Add subtitling to options");
		}
		_captionText.text = dialogue.DialogueData.MessageText;
		_fadeElement.FadeIn();
		dialogue.OnDialogueEnd += OnDialogueEnd;
	}
	
	private void Start() {
		_fadeElement.Hide();
	}

	private void OnDialogueEnd(Dialogue dialogue) {
		dialogue.OnDialogueEnd -= OnDialogueEnd;
		_fadeElement.FadeOut();
	}
} 
