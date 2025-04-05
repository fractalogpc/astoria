using Unity.Behavior;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CutsceneEnableDisable))]
public class CutsceneEnableDisableEditor : Editor
{
    private CutsceneEnableDisable _cutsceneEnableDisable;
    
    public override void OnInspectorGUI() {
        _cutsceneEnableDisable = (CutsceneEnableDisable)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Start Cutscene", GUILayout.Height(30))) {
            _cutsceneEnableDisable.StartCutscene();
        }
        if (GUILayout.Button("End Cutscene", GUILayout.Height(30))) {
            _cutsceneEnableDisable.EndCutscene();
        }
    }
}
