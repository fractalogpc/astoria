using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingCountUI : MonoBehaviour
{
    public bool Initialized = false;
    [SerializeField][ReadOnly] private CraftingStationNetworked _craftingStation;
    [SerializeField] private Button _increaseButton;
    [SerializeField] private Button _decreaseButton;
    [SerializeField] private TextMeshProUGUI _countText;
    
    private void Start() {
        _increaseButton.onClick.AddListener(OnIncrease);
        _decreaseButton.onClick.AddListener(OnDecrease);
    }
    
    private void OnDisable() {
        _increaseButton.onClick.RemoveListener(OnIncrease);
        _decreaseButton.onClick.RemoveListener(OnDecrease);
    }
    
    public void Initialize(CraftingStationNetworked craftingStation) {
        if (Initialized) return;
        _craftingStation = craftingStation;
        Initialized = true;
    }
    
    public void OnIncrease() {
        // The button shouldn't be interactable if this is the case, but just in case.
        if (!_craftingStation.CanCraftRecipe(_craftingStation.SelectedRecipe, _craftingStation.SelectedCraftCount + 1)) {
            return;
        }
        _craftingStation.SetCraftCount(_craftingStation.SelectedCraftCount + 1);
        UpdateUI();
    }
    
    public void OnDecrease() {
        if (_craftingStation.SelectedCraftCount <= 1) {
            return;
        }
        _craftingStation.SetCraftCount(_craftingStation.SelectedCraftCount - 1);
        UpdateUI();
    }
    
    public void UpdateUI() {
        if (_craftingStation.SelectedRecipe == null) {
            _increaseButton.interactable = false;
            _decreaseButton.interactable = false;
            _countText.text = "0";
            return;
        }
        _decreaseButton.interactable = _craftingStation.SelectedCraftCount <= 1 ? false : true;
        _increaseButton.interactable = _craftingStation.CanCraftRecipe(_craftingStation.SelectedRecipe, _craftingStation.SelectedCraftCount + 1) ? true : false;
        _countText.text = _craftingStation.SelectedCraftCount.ToString();
    }
}
