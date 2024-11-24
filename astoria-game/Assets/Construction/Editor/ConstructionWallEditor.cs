using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ConstructionWall))]
public class ElliotConstructionWallEditor : Editor
{
  private ConstructionWall wallScript;
  private SerializedProperty wallJointsProp;

  private void OnEnable()
  {
    wallScript = (ConstructionWall)target;
    wallJointsProp = serializedObject.FindProperty("wallJoints");
  }

  public override void OnInspectorGUI()
  {
    // Update the serialized object
    serializedObject.Update();

    // Display the default inspector with the serialized wallJoints
    EditorGUILayout.PropertyField(wallJointsProp, true);

    // Add button for adding new joints
    if (GUILayout.Button("Add New Joint"))
    {
      AddNewJoint();
    }

    // Draw a button for clearing all joints
    if (GUILayout.Button("Clear All Joints"))
    {
      ClearAllJoints();
    }

    // Apply any changes to the serialized object
    serializedObject.ApplyModifiedProperties();
  }

  private void OnSceneGUI()
  {
    // Visualize and move joints in the scene view
    for (int i = 0; i < wallScript.wallJoints.Length; i++)
    {
      // Allow moving the joints
      EditorGUI.BeginChangeCheck();
      Vector3 newJointPos = Handles.PositionHandle(wallScript.wallJoints[i], Quaternion.identity);
      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(wallScript, "Move Joint");
        wallScript.wallJoints[i] = newJointPos;
      }

      // Draw a label next to each joint for clarity
      Handles.Label(wallScript.wallJoints[i], $"Joint {i}");
    }
  }

  private void AddNewJoint()
  {
    Undo.RecordObject(wallScript, "Add New Joint");

    // Add a new joint at the origin or a default position
    Vector3[] newJoints = new Vector3[wallScript.wallJoints.Length + 1];
    for (int i = 0; i < wallScript.wallJoints.Length; i++)
    {
      newJoints[i] = wallScript.wallJoints[i];
    }
    newJoints[wallScript.wallJoints.Length] = Vector3.zero; // Default position of the new joint

    wallScript.wallJoints = newJoints;
    EditorUtility.SetDirty(wallScript);
  }

  private void ClearAllJoints()
  {
    Undo.RecordObject(wallScript, "Clear All Joints");
    wallScript.wallJoints = new Vector3[0];
    EditorUtility.SetDirty(wallScript);
  }
}
