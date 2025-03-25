
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FilteredInventoryComponent))]
public class FilteredInventoryComponentEditor : InventoryComponentEditor
{
	
	public static IEnumerable<T> FindAssetsByType<T>() where T : UnityEngine.Object {
		string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
		foreach (var t in guids) {
			string assetPath = AssetDatabase.GUIDToAssetPath(t);
			T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
			if (asset != null) {
				yield return asset;
			}
		}
		Debug.Log("Creating a new ItemData type? Make sure to add it to this editor script for the FilteredInventoryComponent.");
	}

	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		// Add horizontal line
		EditorGUILayout.Separator();
		FilteredInventoryComponent filteredInventoryComponent = (FilteredInventoryComponent)target;
		if (GUILayout.Button("Reset Filter")) {
			filteredInventoryComponent.SetFilter(null);
		}
		if (GUILayout.Button("Add all SmeltableData items to filter")) {
			filteredInventoryComponent.SetFilter(FindAssetsByType<SmeltableData>().Cast<ItemData>().ToList());
		}
		if (GUILayout.Button("Add all FuelData items to filter")) {
			filteredInventoryComponent.SetFilter(FindAssetsByType<FuelData>().Cast<ItemData>().ToList());
		}
	}
}
