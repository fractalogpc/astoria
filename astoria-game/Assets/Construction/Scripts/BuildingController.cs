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

    public ConstructionComponentData[] ConstructableObjects;
    private ConstructionComponentData _selectedData;
    private bool deleting = false;
    private bool editing = false;
    public RadialMenu RadialMenu;


    public void InitializeStart()
    {
        togglePlayerBuildingUI.OnBuildingUIOpen.AddListener(OnBuildingUIOpen);
        togglePlayerBuildingUI.OnBuildingUIClose.AddListener(OnBuildingUIClose);

        RadialMenu.OnElementHovered += OnElementHovered;
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
        
        // These are the components
        if (index < 10)
        {
            constructionCore.CleanupPreviewObject();
            constructionCore.SelectData(ConstructableObjects[index], true);
            _selectedData = ConstructableObjects[index];
        }
        // Edit
        else if (index == 10)
        {
            _selectedData = null;
            editing = true;
            constructionCore.SetConstructionState(ConstructionCore.ConstructionState.WantingToEdit);
        }
        // Delete
        else if (index == 11)
        {
            _selectedData = null;
            deleting = true;
            constructionCore.SetConstructionState(ConstructionCore.ConstructionState.Deleting);
        }
    }

    private void OnBuildingUIOpen()
    {
        editing = false;
        deleting = false;

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
        else if (!editing && !deleting)
        {
            constructionCore.SetConstructionState(ConstructionCore.ConstructionState.None);
        }
    }
}
