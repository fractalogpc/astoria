using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeUI : MonoBehaviour
{
	[SerializeField][ReadOnly] private RecipeData _recipe;
	[SerializeField][ReadOnly] private CraftingStationNetworked _craftingStation;
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

	public void Initialize(CraftingStationNetworked craftingStation, RecipeData recipe, RecipeSelected onRecipeSelectedCallback) {
		_craftingStation = craftingStation;
		_recipe = recipe;
		_callback = onRecipeSelectedCallback;
		UpdateRecipe();
	}

	public void UpdateRecipe() {
		Debug.Log("Can change this to be a specific icon in the recipe data later.");
		_recipeIcon.sprite = _recipe._resultSets[0]._item.ItemIcon;
		if (_craftingStation.CanCraftRecipe(_recipe, 1)) {
			_recipeIconCanvasGroup.alpha = 1;
			_selectRecipeButton.interactable = true;
			print($"can craft {_recipe._resultSets[0]._item.ItemName}");
		} else {
			print($"can't craft {_recipe._resultSets[0]._item.ItemName}");
			_recipeIconCanvasGroup.alpha = _alphaWhenDisabled;
			_selectRecipeButton.interactable = false;
		}
	}
	
	public void OnSelectRecipe() {
		_callback?.Invoke(_recipe);
	}
}