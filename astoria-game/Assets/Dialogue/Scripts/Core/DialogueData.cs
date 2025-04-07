using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
	[Header("Settings")]
	[Tooltip("The distance which receivers can hear this dialogue. A distance of 0 means all receivers will hear it.")]
	public float RecieveRange;
	[Tooltip("The layer mask which receivers must be on to hear this dialogue.")]
	public LayerMask RecieveLayerMask;
	[Tooltip("The length of the dialogue in seconds. If shorter than the audio event, it will be cut off.")]
	public float Duration;
	[Header("Sound")]
	[Tooltip("The FMOD audio event to play when this dialogue is played.")]
	public EventReference AudioEvent;
	[Header("Captions")]
	[Tooltip("The text to display when this dialogue is played, if Subtitles is enabled in the options.")]
	[TextArea(4, 12)] public string MessageText;
	[Tooltip("This will force the dialogue to always show a caption, even if the player has subtitles disabled.")]
	public bool AlwaysCaption;

	public virtual Dialogue CreateInstance(DialogueSource dialogueSource) {
		return new Dialogue(this, dialogueSource);
	}
	public virtual Dialogue CreateInstance(DialogueSource dialogueSource, EventInstance soundInstance) {
		return new Dialogue(this, dialogueSource, soundInstance);
	}
}