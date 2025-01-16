using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PreviewObject))]
public class PreviewObjectEditor : Editor
{
  private PreviewObject targetObject;
  private Transform targetTransform;
  private int selectedBoxIndex = -1; // Track the selected box index

  private void OnEnable()
  {
    targetObject = (PreviewObject)target;
    targetTransform = targetObject.transform;
  }

  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    // Height Offset Slider
    EditorGUILayout.LabelField("Height Offset", EditorStyles.boldLabel);
    targetObject.heightOffset = EditorGUILayout.Slider("Offset", targetObject.heightOffset, 0f, 10f); // Adjust range as needed

    // Button to sync HeightOffset to ScriptableObject
    if (GUILayout.Button("Sync HeightOffset to ScriptableObject") && targetObject.Data != null)
    {
      SyncHeightOffset();
    }

    // Button to sync Position and Rotation to ScriptableObject
    if (Application.isPlaying) {
      if (GUILayout.Button("Sync Position and Rotation to ScriptableObject") && targetObject.Data != null)
      {
        SyncPositionAndRotation();
      }
    }


    // Button to add a new box
    if (GUILayout.Button("Add Box"))
    {
      AddBox();
    }

    // Loop through each box and create a collapsible section for each
    for (int i = 0; i < targetObject.boxes.Count; i++)
    {
      EditorGUILayout.Space();

      // Foldout to toggle visibility of box Data
      targetObject.boxes[i].isExpanded = EditorGUILayout.Foldout(targetObject.boxes[i].isExpanded, $"Box {i + 1}", true, EditorStyles.foldout);

      if (targetObject.boxes[i].isExpanded)
      {
        EditorGUI.indentLevel++; // Indent to make the foldout hierarchy clearer

        // Box Properties
        targetObject.boxes[i].position = EditorGUILayout.Vector3Field("Position", targetObject.boxes[i].position);
        targetObject.boxes[i].size = EditorGUILayout.Vector3Field("Size", targetObject.boxes[i].size);
        targetObject.boxes[i].rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", targetObject.boxes[i].rotation.eulerAngles));

        // Box controls
        EditorGUILayout.BeginHorizontal();
        
        // Select Box Button
        if (GUILayout.Button("Select Box"))
        {
          selectedBoxIndex = i;
        }

        // Remove Box Button
        if (GUILayout.Button("Remove Box"))
        {
          RemoveBox(i);
          break; // Stop further execution if box is removed
        }
        
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel--; // Remove the indent
      }
    }

    // Apply changes if anything has been modified
    if (GUI.changed)
    {
      EditorUtility.SetDirty(targetObject);
    }
  }

  private void SyncHeightOffset()
  {
    if (targetObject.Data != null)
    {
      targetObject.Data.HeightOffset = targetObject.heightOffset;
      EditorUtility.SetDirty(targetObject.Data); // Mark the ScriptableObject as dirty to save changes
      Debug.Log("HeightOffset synced to ScriptableObject.");
    }
    else
    {
      Debug.LogWarning("No ScriptableObject linked to sync with.");
    }
  }

  private void SyncPositionAndRotation() {
    if (targetObject.Data != null)
    {
      targetObject.Data.HeldOffsetPosition = targetObject.transform.localPosition;
      targetObject.Data.HeldOffsetRotation = targetObject.transform.localRotation.eulerAngles;
      EditorUtility.SetDirty(targetObject.Data); // Mark the ScriptableObject as dirty to save changes
      Debug.Log("Position and Rotation offset synced to ScriptableObject.");
    }
    else
    {
      Debug.LogWarning("No ScriptableObject linked to sync with.");
    }
  }

  private void AddBox()
  {
    Undo.RecordObject(targetObject, "Add Box");
    targetObject.boxes.Add(new PreviewObject.BoxData());
    EditorUtility.SetDirty(targetObject);
  }

  private void RemoveBox(int index)
  {
    Undo.RecordObject(targetObject, "Remove Box");
    targetObject.boxes.RemoveAt(index);
    EditorUtility.SetDirty(targetObject);
  }

  private void DuplicateBox(int index)
  {
    Undo.RecordObject(targetObject, "Duplicate Box");
    PreviewObject.BoxData boxToDuplicate = targetObject.boxes[index];
    targetObject.boxes.Add(new PreviewObject.BoxData
    {
      position = boxToDuplicate.position + Vector3.right * 0.5f, // Offset the duplicate slightly
      size = boxToDuplicate.size,
      rotation = boxToDuplicate.rotation
    });
    EditorUtility.SetDirty(targetObject);
  }

  private void HandleKeyboardInput()
  {
    Event e = Event.current;
    if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.D)
    {
      if (selectedBoxIndex >= 0 && selectedBoxIndex < targetObject.boxes.Count)
      {
        DuplicateBox(selectedBoxIndex);
        e.Use(); // Consume the event to prevent default duplication behavior
      }
    }
  }

  private void HandleMouseSelection()
  {
    // Handle mouse clicks for selecting a box
    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
    {
      Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
      RaycastHit hit;

      for (int i = 0; i < targetObject.boxes.Count; i++)
      {
        PreviewObject.BoxData box = targetObject.boxes[i];
        Vector3 worldPosition = targetTransform.TransformPoint(box.position);
        Vector3 halfSize = box.size / 2;

        // Create a box collider for the current box
        if (Physics.BoxCast(worldPosition, halfSize, mouseRay.direction, out hit, Quaternion.identity, Mathf.Infinity))
        {
          if (hit.collider != null && hit.collider.transform == targetTransform)
          {
            selectedBoxIndex = i; // Select the box that was clicked
            Event.current.Use(); // Consume the event
            break;
          }
        }
      }
    }
  }
}
