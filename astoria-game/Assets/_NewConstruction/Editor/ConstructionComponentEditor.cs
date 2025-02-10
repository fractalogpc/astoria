using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Construction;

[CustomEditor(typeof(ConstructionComponent))]
public class ConstructionComponentEditor : Editor
{
  private ConstructionComponent component;

  private void OnEnable()
  {
    component = (ConstructionComponent)target;
  }

  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();

    // Button to sync edges
    if (GUILayout.Button("Sync Edges to Preview"))
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
    if (component.edges == null) return;

    Transform objectTransform = component.transform;

    for (int i = 0; i < component.edges.Count; i++)
    {
      Edge edge = component.edges[i];

      // Convert world positions to local space
      Vector3 localPointA = objectTransform.InverseTransformPoint(edge.pointA);
      Vector3 localPointB = objectTransform.InverseTransformPoint(edge.pointB);

      // Display handles in local space (without unnecessary transformations)
      Vector3 newLocalPointA = Handles.PositionHandle(objectTransform.position + localPointA, Quaternion.identity);
      Vector3 newLocalPointB = Handles.PositionHandle(objectTransform.position + localPointB, Quaternion.identity);

      // Convert modified positions back to world space
      Vector3 newWorldPointA = objectTransform.TransformPoint(newLocalPointA - objectTransform.position);
      Vector3 newWorldPointB = objectTransform.TransformPoint(newLocalPointB - objectTransform.position);

      // Update only if positions changed
      if (newWorldPointA != edge.pointA || newWorldPointB != edge.pointB)
      {
        Undo.RecordObject(component, "Move Edge Point");

        // Copy edge and update list
        var edgeCopy = component.edges[i];
        edgeCopy.pointA = newWorldPointA;
        edgeCopy.pointB = newWorldPointB;
        component.edges[i] = edgeCopy;

        EditorUtility.SetDirty(component);
      }

      // Draw a line between the points
      Handles.color = Color.cyan;
      Handles.DrawLine(edge.pointA, edge.pointB, 1f);
    }

    SceneView.RepaintAll();
  }
}
