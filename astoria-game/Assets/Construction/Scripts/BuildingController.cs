using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Construction;

/// <summary>
/// Handles the building menu. Not to be confused with ConstructionCore, which handles the placing and deleting of props and components.
/// This controls the backend of the building menu, designed for selecting components to place. This could also probably work with props, but I haven't tested.
/// </summary>
public class BuildingController : MonoBehaviour, IStartExecution
{
    public ConstructionCore constructionCore;
    public TogglePlayerBuildingUI togglePlayerBuildingUI;

    public GameObject prefabParentContent;
    public GameObject StructureObjectPrefab;

    public ConstructionComponentData[] ConstructableObjects;


    public void InitializeStart()
    {
        togglePlayerBuildingUI.OnBuildingUIOpen.AddListener(OnBuildingUIOpen);

        foreach (ConstructionComponentData data in ConstructableObjects)
        {
            GameObject prefab = Instantiate(StructureObjectPrefab, prefabParentContent.transform);
            prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.name;
            // prefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Cost: " + data.Cost.List[0].ItemCount;

            Button button = prefab.GetComponent<Button>();
            button.onClick.AddListener(() => { constructionCore.SelectData(data); });
            button.onClick.AddListener(() => { togglePlayerBuildingUI.SetVisibility(false); });
        }
    }

    private void OnBuildingUIOpen()
    {
        constructionCore.SetConstructionState(ConstructionCore.ConstructionState.None);
    }
}
