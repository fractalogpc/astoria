using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationRecipeUI : MonoBehaviour
{
	[SerializeField][ReadOnly] private RecipeData _recipe;
	[SerializeField][ReadOnly] private StationCore _station;
	[SerializeField] private Image _recipeIcon;
	[SerializeField] private CanvasGroup _recipeIconCanvasGroup;
	[SerializeField] private Button _selectRecipeButton;
	[SerializeField][Range(0f, 1f)] private float _alphaWhenDisabled = 0.5f;
	
	public delegate void RecipeSelected(RecipeData recipe);
	private RecipeSelected _callback;

	private void Start() {
		_selectRecipeButton.onClick.AddListener(OnSelectRecipe);
	}

	private void OnDisable() {
		_selectRecipeButton.onClick.RemoveListener(OnSelectRecipe);
	}

	public void Initialize(StationCore station, RecipeData recipe, RecipeSelected onRecipeSelectedCallback) {
		_station = station;
		_recipe = recipe;
		_callback = onRecipeSelectedCallback;
		UpdateInteractivity();
	}

	public void UpdateInteractivity() {
		Debug.Log("Can change this to be a specific icon in the recipe data later.");
		_recipeIcon.sprite = _recipe._resultSetList.List[0].ItemData.ItemIcon;
		_recipeIcon.preserveAspect = true;
		if (_station.CanCraftRecipe(_recipe, 1)) {
			_recipeIconCanvasGroup.alpha = 1;
			_selectRecipeButton.interactable = true;
			
		} 
		else {
			_recipeIconCanvasGroup.alpha = _alphaWhenDisabled;
			_selectRecipeButton.interactable = false;
		}
	}
	
	public void OnSelectRecipe() {
		_callback?.Invoke(_recipe);
	}
}