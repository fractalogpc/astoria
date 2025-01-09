using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LootContainer))]
public class LootContainerEditor : Editor
{
    private LootContainer _script;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        _script = (LootContainer) target;
        foreach (LootPool lootPool in _script.LootTable.LootPools) {
            EditorGUILayout.LabelField("Pool " + _script.LootTable.LootPools.IndexOf(lootPool) + " Total Weight: " + lootPool.TotalWeight);
        }
    }
}
