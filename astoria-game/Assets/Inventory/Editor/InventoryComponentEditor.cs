using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(InventoryComponent))]
public class InventoryComponentEditor : AutoRegisterValidator
{
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		EditorGUILayout.Space();
		if (GUILayout.Button("Preview Slots")) {
			InventoryComponent inventoryComponent = (InventoryComponent) target;
			inventoryComponent.CreateAndAttachContainersTo(new InventoryData(inventoryComponent.AssignedInventorySize.x, inventoryComponent.AssignedInventorySize.y));
		}
		EditorGUILayout.Space();
		if (GUILayout.Button("Destroy Slots")) {
			InventoryComponent inventoryComponent = (InventoryComponent) target;
			inventoryComponent.DestroyInventoryContainers();
		}
	}
}