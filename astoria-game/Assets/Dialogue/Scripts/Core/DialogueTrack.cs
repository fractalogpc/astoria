using UnityEngine;
using UnityEngine.Timeline;

[TrackBindingType(typeof(DialogueSource))]
[TrackClipType(typeof(DialogueClip))]
public class DialogueTrack : TrackAsset
{
	protected override void OnCreateClip(TimelineClip clip) {
		// Make sure fade in and out are disabled
		clip.blendInDuration = 0;
		clip.blendOutDuration = 0;
	}
}
