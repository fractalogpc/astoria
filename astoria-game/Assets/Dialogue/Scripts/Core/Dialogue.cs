using UnityEngine;

public class Dialogue
{
	public DialogueData DialogueData { get; private set; }
	public delegate void DialogueEnd();
	public event DialogueEnd OnDialogueEnd;
	public float TimeElapsed => _timeElapsed;
	public float DurationLeft => DialogueData.Duration - _timeElapsed;
	public float DurationLeftNormalized => DurationLeft / DialogueData.Duration;
	private float _timeElapsed;

	public Dialogue(DialogueData dialogueData) {
		DialogueData = dialogueData;
	}

	public void Tick(float deltaTime) {
		_timeElapsed += deltaTime;
		if (_timeElapsed > DialogueData.Duration) _timeElapsed = DialogueData.Duration;
	}

	public void End() {
		OnDialogueEnd?.Invoke();
	}
}