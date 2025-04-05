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
    private ConstructionComponentData _selectedData;
    public RadialMenu RadialMenu;


    public void InitializeStart()
    {
        togglePlayerBuildingUI.OnBuildingUIOpen.AddListener(OnBuildingUIOpen);
        togglePlayerBuildingUI.OnBuildingUIClose.AddListener(OnBuildingUIClose);

        RadialMenu.OnElementHovered += OnElementHovered;

        // foreach (ConstructionComponentData data in ConstructableObjects)
        // {
        //     GameObject prefab = Instantiate(StructureObjectPrefab, prefabParentContent.transform);
        //     prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.name;
        //     // prefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Cost: " + data.Cost.List[0].ItemCount;

        //     Button button = prefab.GetComponent<Button>();
        //     button.onClick.AddListener(() => { constructionCore.SelectData(data); });
        //     button.onClick.AddListener(() => { togglePlayerBuildingUI.SetVisibility(false); });
        // }
    }

    private void OnDestroy()
    {
        togglePlayerBuildingUI.OnBuildingUIOpen.RemoveListener(OnBuildingUIOpen);
        RadialMenu.OnElementHovered -= OnElementHovered;
    }

    public void OnElementHovered(RadialMenuElement element)
    {
        int index = element.Index;

        // Not great to hard code this but it'll do
        if (index < 10)
        {
            constructionCore.CleanupPreviewObject();
            constructionCore.SelectData(ConstructableObjects[index], true);
            _selectedData = ConstructableObjects[index];
        }
        else if (index == 10)
        {
            // constructionCore.SetConstructionState(ConstructionCore.ConstructionState.Delete);
        }
        else if (index == 11)
        {
            // constructionCore.SetConstructionState(ConstructionCore.ConstructionState.None);
        }
    }

    private void OnBuildingUIOpen()
    {
        RadialMenu.ResetLastSelected();
        _selectedData = null;
        constructionCore.SetDataToNull();
        constructionCore.SetConstructionState(ConstructionCore.ConstructionState.SelectingItem);
    }

    private void OnBuildingUIClose()
    {
        if (_selectedData != null)
        {
            constructionCore.SetConstructionState(ConstructionCore.ConstructionState.PlacingStructure);
        }
        else
        {
            constructionCore.SetConstructionState(ConstructionCore.ConstructionState.None);
        }
    }


}
