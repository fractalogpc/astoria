using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class BuildingController : MonoBehaviour, IStartExecution
{
    public ConstructionCore constructionCore;
    public TogglePlayerBuildingUI togglePlayerBuildingUI;

    public GameObject prefabParentContent;
    public GameObject StructureObjectPrefab;


    public void InitializeStart()
    {
        togglePlayerBuildingUI.OnBuildingUIOpen.AddListener(OnBuildingUIOpen);

        foreach (ConstructableObjectData data in constructionCore.ConstructableObjectsPublic)
        {
            GameObject prefab = Instantiate(StructureObjectPrefab, prefabParentContent.transform);
            prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.name;

            Button button = prefab.GetComponent<Button>();
            button.onClick.AddListener(() => { if (constructionCore.TryGiveObject(data)) togglePlayerBuildingUI.SetVisibility(false); });
        }
    }

    private void OnBuildingUIOpen()
    {
        constructionCore.TryRemoveObject();
    }
}
