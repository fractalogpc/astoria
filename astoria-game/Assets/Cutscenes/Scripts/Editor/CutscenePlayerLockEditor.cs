using Unity.Behavior;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CutscenePlayerLock))]
public class CutscenePlayerLockEditor : Editor
{
    private CutscenePlayerLock _cutscenePlayerLock;
    
    public override void OnInspectorGUI() {
        _cutscenePlayerLock = (CutscenePlayerLock)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Start Cutscene", GUILayout.Height(30))) {
            _cutscenePlayerLock.StartCutscene();
        }
        if (GUILayout.Button("End Cutscene", GUILayout.Height(30))) {
            _cutscenePlayerLock.EndCutscene();
        }
    }
}
