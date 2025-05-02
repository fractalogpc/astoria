using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SitePrefabManager))]
public class SitePrefabManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SitePrefabManager manager = (SitePrefabManager)target;

        if (GUILayout.Button("Snap Objects to Ground"))
        {
            SnapObjectsToGround(manager);
        }
    }

    private void SnapObjectsToGround(SitePrefabManager manager)
    {
        foreach (Transform child in manager.transform)
        {
            RaycastHit hit;
            if (Physics.Raycast(child.position, Vector3.down, out hit))
            {
                Undo.RecordObject(child, "Snap Object to Ground");
                child.position = hit.point;
                child.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * child.rotation;
            }
        }

        Debug.Log("Snapped all objects to the ground.");
    }
}