using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


public class DialogueClip : PlayableAsset
{
	public DialogueData DialogueData;
	
	public DialogueClip() { }

	public DialogueClip(DialogueData dialogueData) {
		DialogueData = dialogueData;
	}
	
	public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
		DialogueBehavior behaviour = new();
		behaviour.DialogueData = DialogueData;
		return ScriptPlayable<DialogueBehavior>.Create(graph, behaviour);
	}
}