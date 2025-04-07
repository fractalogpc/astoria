using FMOD.Studio;
using UnityEngine;

public class Dialogue
{
	public DialogueSource DialogueSource { get; private set; }
	public DialogueData DialogueData { get; private set; }
	public EventInstance SoundInstance { get; private set; }
	public delegate void DialogueEnd(Dialogue dialogue);
	public event DialogueEnd OnDialogueEnd;
	public float TimeElapsed => _timeElapsed;
	public float DurationLeft => DialogueData.Duration - _timeElapsed;
	public float DurationLeftNormalized => DurationLeft / DialogueData.Duration;
	public bool Complete { get; private set; }
	
	private float _timeElapsed;

	public Dialogue(DialogueData dialogueData, DialogueSource dialogueSource) {
		DialogueData = dialogueData;
		DialogueSource = dialogueSource;
	}
	
	public Dialogue(DialogueData dialogueData, DialogueSource dialogueSource, EventInstance soundInstance) {
		DialogueData = dialogueData;
		DialogueSource = dialogueSource;
		SoundInstance = soundInstance;
	}

	public void Tick(float deltaTime) {
		_timeElapsed += deltaTime;
		if (!(_timeElapsed > DialogueData.Duration)) return;
		_timeElapsed = DialogueData.Duration;
		End();
	} 

	public void End() {
		Complete = true;
		OnDialogueEnd?.Invoke(this);
		if (SoundInstance.isValid()) {
			SoundInstance.stop(STOP_MODE.ALLOWFADEOUT);
			SoundInstance.release();
		}
	}
}