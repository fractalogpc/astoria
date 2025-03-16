using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Construction;


[CustomEditor(typeof(NewConstructionComponent))]
public class NewConstructionComponentEditor : Editor
{
  private NewConstructionComponent component;

  private void OnEnable()
  {
    component = (NewConstructionComponent)target;
  }

  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    // Button to sync edges
    if (GUILayout.Button("Sync Edges to Preview") && !EditorApplication.isPlaying)
    {
      SyncEdgesToPreview();
    }
  }

  private void SyncEdgesToPreview()
  {
    if (component.previewComponent == null)
    {
      Debug.LogWarning("PreviewConstructionComponent reference is missing!");
      return;
    }

    Undo.RecordObject(component.previewComponent, "Sync Edges");

    // Copy edges list
    component.previewComponent.edges = new List<Edge>(component.edges);

    EditorUtility.SetDirty(component.previewComponent);
    Debug.Log("Edges synced to PreviewConstructionComponent.");
  }

  private void OnSceneGUI()
  {
    if (EditorApplication.isPlaying) return;

    if (component.edges == null) return;

    Transform objectTransform = component.transform;

    for (int i = 0; i < component.edges.Count; i++)
    {
      Edge edge = component.edges[i];

      // Convert world positions to local space
      Vector3 localPosition = objectTransform.InverseTransformPoint(edge.position);

      // Display handles in local space (without unnecessary transformations)
      Vector3 newLocalPosition = Handles.PositionHandle(objectTransform.position + localPosition, Quaternion.identity);

      // Convert modified positions back to world space
      Vector3 newWorldPoint = objectTransform.TransformPoint(newLocalPosition - objectTransform.position);

      // Update only if positions changed
      if (newWorldPoint != edge.position)
      {
        Undo.RecordObject(component, "Move Edge Point");

        // Copy edge and update list
        var edgeCopy = component.edges[i];
        edgeCopy.position = newWorldPoint;
        component.edges[i] = edgeCopy;

        EditorUtility.SetDirty(component);
      }

      // Draw a line between the points
      Handles.color = Color.cyan;
      Handles.DrawLine(edge.position, edge.position + edge.normal * 0.2f, 1f);
    }

    SceneView.RepaintAll();
  }
}
