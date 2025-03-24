using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrafterCountUI : MonoBehaviour
{
    public bool Initialized = false;
    [SerializeField][ReadOnly] private CrafterCore _crafter;
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
    
    public void Initialize(CrafterCore crafter) {
        if (Initialized) return;
        _crafter = crafter;
        Initialized = true;
    }
    
    public void OnIncrease() {
        // The button shouldn't be interactable if this is the case, but just in case.
        if (!_crafter.CanCraftRecipe(_crafter.SelectedRecipe, _crafter.SelectedCraftCount + 1)) {
            return;
        }
        _crafter.SetCraftCount(_crafter.SelectedCraftCount + 1);
        UpdateUI();
    }
    
    public void OnDecrease() {
        if (_crafter.SelectedCraftCount <= 1) {
            return;
        }
        _crafter.SetCraftCount(_crafter.SelectedCraftCount - 1);
        UpdateUI();
    }
    
    public void UpdateUI() {
        if (_crafter.SelectedRecipe == null) {
            _increaseButton.interactable = false;
            _decreaseButton.interactable = false;
            _countText.text = "0";
            return;
        }
        _decreaseButton.interactable = _crafter.SelectedCraftCount <= 1 ? false : true;
        _increaseButton.interactable = _crafter.CanCraftRecipe(_crafter.SelectedRecipe, _crafter.SelectedCraftCount + 1) ? true : false;
        _countText.text = _crafter.SelectedCraftCount.ToString();
    }
}
