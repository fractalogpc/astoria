using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationCountUI : MonoBehaviour
{
    public bool Initialized = false;
    [SerializeField][ReadOnly] private StationCore _station;
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
    
    public void Initialize(StationCore station) {
        if (Initialized) return;
        _station = station;
        Initialized = true;
    }
    
    public void OnIncrease() {
        // The button shouldn't be interactable if this is the case, but just in case.
        if (!_station.CanCraftRecipe(_station.SelectedRecipe, _station.SelectedCraftCount + 1)) {
            return;
        }
        _station.SetCraftCount(_station.SelectedCraftCount + 1);
        UpdateUI();
    }
    
    public void OnDecrease() {
        if (_station.SelectedCraftCount <= 1) {
            return;
        }
        _station.SetCraftCount(_station.SelectedCraftCount - 1);
        UpdateUI();
    }
    
    public void UpdateUI() {
        if (_station.SelectedRecipe == null) {
            _increaseButton.interactable = false;
            _decreaseButton.interactable = false;
            _countText.text = "0";
            return;
        }
        _decreaseButton.interactable = _station.SelectedCraftCount <= 1 ? false : true;
        _increaseButton.interactable = _station.CanCraftRecipe(_station.SelectedRecipe, _station.SelectedCraftCount + 1) ? true : false;
        _countText.text = _station.SelectedCraftCount.ToString();
    }
}
