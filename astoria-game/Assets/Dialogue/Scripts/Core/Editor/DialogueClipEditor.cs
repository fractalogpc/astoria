using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;

#if UNITY_EDITOR
[CustomEditor(typeof(DialogueClip))]
public class DialogueClipEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector(); // Draws the regular fields

		DialogueClip asset = (DialogueClip)target;

		if (GUILayout.Button("Sync Clip Name/Duration to match DialogueData"))
		{
			UpdateClipDuration(asset);
		}
	}

	private void UpdateClipDuration(DialogueClip asset)
	{
		if (asset.DialogueData == null)
		{
			Debug.LogWarning("ClipData is not assigned.");
			return;
		}

		// Find all TimelineClips using this asset
		TimelineAsset[] timelines = Resources.FindObjectsOfTypeAll<TimelineAsset>();

		foreach (var timeline in timelines)
		{
			foreach (var track in timeline.GetOutputTracks())
			{
				foreach (var clip in track.GetClips())
				{
					if (clip.asset == asset)
					{
						Undo.RecordObject(track, "Update Timeline Clip To Match Data");
						clip.duration = asset.DialogueData.Duration;
						clip.displayName = asset.DialogueData.MessageText;
						Debug.Log($"Updated name/duration of clip on track {track.name} to {clip.displayName.Substring(0, Mathf.Min(8, clip.displayName.Length))} & {clip.duration}");
						EditorUtility.SetDirty(track);
					}
				}
			}
		}
	}
}
#endif