using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueBehavior : PlayableBehaviour
{
	public DialogueData DialogueData;
	private DialogueSource _dialogueSource;
	private bool _hasStarted;

	public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
		_dialogueSource = playerData as DialogueSource;
		if (_dialogueSource == null) return;
		if (DialogueData == null) return;
		if (_hasStarted || !(info.effectiveWeight > 0f)) return;
		// If the playable is active (weight > 0) and we haven't started yet
		_dialogueSource.Play(DialogueData);
		_hasStarted = true;
	}

	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		if (_dialogueSource == null) return;
		// Reset on pause or stop (so it can play again if the timeline rewinds)
		_dialogueSource.StopPlayingOfData(DialogueData);
		_hasStarted = false;
	}
}
