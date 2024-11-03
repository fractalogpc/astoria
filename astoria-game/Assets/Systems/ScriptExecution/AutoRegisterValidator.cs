using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class AutoRegisterValidator : Editor
{
  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    // Ensure the target is a MonoBehaviour
    MonoBehaviour monoBehaviour = target as MonoBehaviour;
    if (monoBehaviour == null) return;

    // Check if the target MonoBehaviour implements either required interface
    bool requiresAutoRegister = monoBehaviour is IOnEnableExecution || monoBehaviour is IStartExecution;

    // Ensure AutoRegister is present if required
    if (requiresAutoRegister)
    {
      if (monoBehaviour.GetComponent<AutoRegister>() == null)
      {
        EditorGUILayout.HelpBox("This component requires AutoRegister to be attached.", MessageType.Error);

        // Optional: Add a button to add AutoRegister automatically
        if (GUILayout.Button("Add AutoRegister"))
        {
          monoBehaviour.gameObject.AddComponent<AutoRegister>();
        }
      }
    }
  }
}
