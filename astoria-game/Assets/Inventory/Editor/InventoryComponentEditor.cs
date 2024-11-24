using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(InventoryComponent))]
public class InventoryComponentEditor : Editor
{
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUILayout.Space();
		// Create a button to call InitializeInventoryContainers() on InventoryComponent
		if (GUILayout.Button("Create Slots")) {
			InventoryComponent inventoryComponent = (InventoryComponent) target;
			inventoryComponent.InitializeInventoryContainers();
		}
		EditorGUILayout.Space();
		// Create a button to call InitializeInventoryContainers() on InventoryComponent
		if (GUILayout.Button("Destroy Slots")) {
			InventoryComponent inventoryComponent = (InventoryComponent) target;
			inventoryComponent.DestroyInventorySlots();
		}
	}
}