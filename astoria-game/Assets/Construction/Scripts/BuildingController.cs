using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Construction;
using System.Collections.Generic;

public class BuildingController : MonoBehaviour, IStartExecution
{
    public ConstructionCore constructionCore;
    public TogglePlayerBuildingUI togglePlayerBuildingUI;

    public GameObject prefabParentContent;
    public GameObject StructureObjectPrefab;

    public ConstructionData[] ConstructableObjects;


    public void InitializeStart()
    {
        togglePlayerBuildingUI.OnBuildingUIOpen.AddListener(OnBuildingUIOpen);

        foreach (ConstructionData data in ConstructableObjects)
        {
            GameObject prefab = Instantiate(StructureObjectPrefab, prefabParentContent.transform);
            prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.name;

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
