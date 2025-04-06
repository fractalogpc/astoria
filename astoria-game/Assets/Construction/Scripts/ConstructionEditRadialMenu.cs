using System.Collections.Generic;
using Construction;
using UnityEngine;

public class ConstructionEditRadialMenu : InputHandlerBase
{
    private bool activated = false;

    private ConstructionComponentData _selectedData;

    protected override void InitializeActionMap()
    {
        RegisterAction(_inputActions.Player.Attack, _ => { }, OnRelease);
    }

    void Start()
    {
        GetComponentInChildren<RadialMenu>().OnElementHovered += OnElementHovered;

    }

    public void PopulateData(List<ConstructionRadialMenuElement> datas)
    {
        GetComponentInChildren<RadialMenu>().CreateElements(datas.ConvertAll(data => (RadialMenuElement)data));

        Debug.Log("Populated Edit Menu Data");
    }

    private GameObject objectToDelete = null;
    public void Activate(GameObject highlightedObject)
    {
        if (activated) return;

        objectToDelete = highlightedObject;

        Cursor.visible = true;
        InputReader.Instance.InputActions.FindAction("Look").Disable();
        InputReader.Instance.InputActions.FindAction("AttackSecondary").Disable();
        Cursor.lockState = CursorLockMode.Confined;

        GetComponentInChildren<RadialMenu>().Enabled = true;

        activated = true;
    }

    private void OnRelease()
    {
        if (!activated) return;

        if (_selectedData != null)
        {
            PlayerInstance.Instance.GetComponentInChildren<ConstructionCore>().ReplaceObject(objectToDelete, _selectedData);
        }

        Cursor.visible = false;
        InputReader.Instance.InputActions.FindAction("Look").Enable();
        InputReader.Instance.InputActions.FindAction("AttackSecondary").Enable();
        Cursor.lockState = CursorLockMode.Locked;

        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<CanvasGroup>().interactable = false;

        GetComponentInChildren<RadialMenu>().Enabled = false;

        activated = false;
    }

    private void OnElementHovered(RadialMenuElement element)
    {
        if (element is ConstructionRadialMenuElement constructionElement)
        {
            if (constructionElement.Data != null)
            {
                _selectedData = constructionElement.Data;
            }
        }

    }
}
