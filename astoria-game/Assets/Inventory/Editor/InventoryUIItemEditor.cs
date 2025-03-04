using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(InventoryStackUI))]
public class InventoryUIItemEditor : Editor
{
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUILayout.Space();
		InventoryStackUI script = (InventoryStackUI) target;
		if (script.ItemStack.Size != Vector2Int.zero) {
			GUILayout.Label($"Prefab Dimensions: {script.ItemStack.Size.x * 96} x {script.ItemStack.Size.y * 96}");
		}
		else {
			GUILayout.Label("Prefab Dimensions: No Item!");
		}
	}
}