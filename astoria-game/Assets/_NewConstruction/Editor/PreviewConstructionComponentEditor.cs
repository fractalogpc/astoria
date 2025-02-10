using UnityEngine;
using UnityEditor;
using Construction;

[CustomEditor(typeof(PreviewConstructionComponent))]
public class PreviewConstructionComponentEditor : Editor
{
  private PreviewConstructionComponent component;

  private void OnEnable()
  {
    component = (PreviewConstructionComponent)target;
  }

  public override void OnInspectorGUI()
  {
    DrawDefaultInspector();
  }

  private void OnSceneGUI()
  {
    if (component.edges == null) return;

    for (int i = 0; i < component.edges.Count; i++)
    {
      Edge edge = component.edges[i];

      // Draw a line between the points
      Handles.color = Color.cyan;
      Handles.DrawLine(edge.pointA, edge.pointB, 1f);
    }
  }
}
