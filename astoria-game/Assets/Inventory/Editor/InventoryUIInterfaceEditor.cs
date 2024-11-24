using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(InventoryUI))]
public class InventoryUIInterfaceEditor : Editor
{
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUILayout.Space();
		// Create a button to call InitializeInventoryContainers() on InventoryUI
		if (GUILayout.Button("Create Slots")) {
			InventoryUI inventoryUI = (InventoryUI) target;
			inventoryUI.InitializeInventoryContainers();
		}

    EditorGUILayout.Space();
		// Create a button to call InitializeInventoryContainers() on InventoryUI
		if (GUILayout.Button("Destroy Slots")) {
			InventoryUI inventoryUI = (InventoryUI) target;
			inventoryUI.DestroyInventorySlots();
		}
	}
}