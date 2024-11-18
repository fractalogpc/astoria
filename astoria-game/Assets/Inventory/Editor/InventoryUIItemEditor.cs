using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(InventoryItemUI))]
public class InventoryUIItemEditor : Editor
{
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUILayout.Space();
		InventoryItemUI script = (InventoryItemUI) target;
		if (script.Item.Size != Vector2Int.zero) {
			GUILayout.Label($"Prefab Dimensions: {script.Item.Size.x * 96} x {script.Item.Size.y * 96}");
		}
		else {
			GUILayout.Label("Prefab Dimensions: No Item!");
		}
	}
}