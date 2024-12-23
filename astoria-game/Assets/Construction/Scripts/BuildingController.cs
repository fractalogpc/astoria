using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class BuildingController : MonoBehaviour, IStartExecution
{
    public ConstructionCore constructionCore;
    public TogglePlayerBuildingUI togglePlayerBuildingUI;

    public GameObject prefabParentContent;
    public GameObject StructureObjectPrefab;

    public ConstructableObjectData[] constructableObjectDatas;

    public void InitializeStart()
    {
        foreach (ConstructableObjectData data in constructableObjectDatas)
        {
            GameObject prefab = Instantiate(StructureObjectPrefab, prefabParentContent.transform);
            prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.name;

            Button button = prefab.GetComponent<Button>();
            button.onClick.AddListener(() => { if (constructionCore.TryGiveObject(data)) togglePlayerBuildingUI.SetVisibility(false); });
        }
    }
}
