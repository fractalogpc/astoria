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

    private int _currentCount = 1;
    
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
        _currentCount++;
        UpdateCount();
        if (!_craftingStation.CanCraftSelectedRecipe()) {
            _currentCount--;
            UpdateCount();
        }
    }
    
    public void OnDecrease() {
        _currentCount--;
        if (_currentCount < 1) {
            _currentCount = 1;
        }
        UpdateCount();
    }
    
    private void UpdateCount() {
        _countText.text = _currentCount.ToString();
        _craftingStation.SetCraftCount(_currentCount);
    }
}
