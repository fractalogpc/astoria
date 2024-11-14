using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConstructionTempObject))]
public class ConstructionTempObjectEditor : Editor
{
  private ConstructionTempObject targetObject;
  private Transform targetTransform;
  private int selectedBoxIndex = -1; // Track the selected box index
  private float handleSize = 0.2f; // Set a smaller handle size

  private void OnEnable()
  {
    targetObject = (ConstructionTempObject)target;
    targetTransform = targetObject.transform;
  }

  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    // Height Offset Slider
    EditorGUILayout.LabelField("Height Offset", EditorStyles.boldLabel);
    targetObject.heightOffset = EditorGUILayout.Slider("Offset", targetObject.heightOffset, 0f, 10f); // Adjust range as needed

    // Button to add a new box
    if (GUILayout.Button("Add Box"))
    {
      AddBox();
    }

    // Loop through each box and create a collapsible section for each
    for (int i = 0; i < targetObject.boxes.Count; i++)
    {
      EditorGUILayout.Space();

      // Foldout to toggle visibility of box data
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


  private void OnSceneGUI()
  {
    HandleKeyboardInput();
    HandleMouseSelection();

    // Iterate through boxes to display handles
    for (int i = 0; i < targetObject.boxes.Count; i++)
    {
      ConstructionTempObject.BoxData box = targetObject.boxes[i];

      // Calculate world position and rotation based on target's transform
      Vector3 worldPosition = targetTransform.TransformPoint(box.position);
      Quaternion worldRotation = targetTransform.rotation * box.rotation;

      // Only show handles for the selected box
      if (i == selectedBoxIndex)
      {
        // Position handle for the selected box
        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(worldPosition, worldRotation);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject(targetObject, "Move Box");
          // Update the local position based on the new world position
          box.position = targetTransform.InverseTransformPoint(newPosition);
          EditorUtility.SetDirty(targetObject);
        }

        // Rotation handle for the selected box
        EditorGUI.BeginChangeCheck();
        Quaternion newRotation = worldRotation;
        newRotation = Handles.RotationHandle(newRotation, worldPosition);
        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject(targetObject, "Rotate Box");
          box.rotation = Quaternion.Inverse(targetTransform.rotation) * newRotation;
          EditorUtility.SetDirty(targetObject);
        }

        // Scaling handles, only show for the selected box
        EditorGUI.BeginChangeCheck();
        Vector3 newSize = box.size;
        Vector3 halfSize = box.size / 2;

        for (int j = 0; j < 3; j++)
        {
          Vector3 direction = Vector3.zero;
          direction[j] = 1;

          // Handle positions for both sides
          Vector3 handlePositionPositive = worldPosition + worldRotation * Vector3.Scale(direction, halfSize);
          Vector3 handlePositionNegative = worldPosition - worldRotation * Vector3.Scale(direction, halfSize);

          // Adjust the handle size used in Handles.Slider
          float effectiveHandleSize = handleSize * HandleUtility.GetHandleSize(handlePositionPositive);

          // Draw handles for both positive and negative directions
          Vector3 newHandlePositionPositive = Handles.Slider(handlePositionPositive, worldRotation * direction, effectiveHandleSize, Handles.CubeHandleCap, 0f);
          Vector3 newHandlePositionNegative = Handles.Slider(handlePositionNegative, worldRotation * -direction, effectiveHandleSize, Handles.CubeHandleCap, 0f);

          // Calculate scale adjustments from both sides
          float scaleChangePositive = (newHandlePositionPositive - handlePositionPositive).magnitude * Mathf.Sign(Vector3.Dot(newHandlePositionPositive - handlePositionPositive, direction));
          float scaleChangeNegative = (newHandlePositionNegative - handlePositionNegative).magnitude * Mathf.Sign(Vector3.Dot(newHandlePositionNegative - handlePositionNegative, -direction));

          // Update size symmetrically for both positive and negative directions
          newSize[j] += scaleChangePositive + scaleChangeNegative;

          // Prevent negative scaling
          if (newSize[j] < 0)
          {
            newSize[j] = 0; // Prevent it from going negative
          }
        }

        if (EditorGUI.EndChangeCheck())
        {
          Undo.RecordObject(targetObject, "Scale Box");
          box.size = newSize;
          EditorUtility.SetDirty(targetObject);
        }

        // Draw box outline in Scene view
        Handles.color = Color.cyan;
        Matrix4x4 handleMatrix = Matrix4x4.TRS(worldPosition, worldRotation, Vector3.one);
        using (new Handles.DrawingScope(handleMatrix))
        {
          Handles.DrawWireCube(Vector3.zero, box.size);
        }
      }
    }
  }

  private void AddBox()
  {
    Undo.RecordObject(targetObject, "Add Box");
    targetObject.boxes.Add(new ConstructionTempObject.BoxData());
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
    ConstructionTempObject.BoxData boxToDuplicate = targetObject.boxes[index];
    targetObject.boxes.Add(new ConstructionTempObject.BoxData
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
        ConstructionTempObject.BoxData box = targetObject.boxes[i];
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
