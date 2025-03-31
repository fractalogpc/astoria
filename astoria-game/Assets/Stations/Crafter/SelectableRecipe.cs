using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableRecipe : MonoBehaviour
{
	[SerializeField][ReadOnly] private RecipeData _recipe;
	[SerializeField][ReadOnly] private CrafterCore _crafter;
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

	public void Initialize(CrafterCore crafter, RecipeData recipe, RecipeSelected onRecipeSelectedCallback) {
		_crafter = crafter;
		_recipe = recipe;
		_callback = onRecipeSelectedCallback;
		UpdateInteractivity();
	}

	public void UpdateInteractivity() {
		_recipeIcon.sprite = _recipe.Icon;
		_recipeIcon.preserveAspect = true;
		if (_crafter.CanCraftRecipe(_recipe, 1)) {
			_recipeIconCanvasGroup.alpha = 1;
		} 
		else {
			_recipeIconCanvasGroup.alpha = _alphaWhenDisabled;
		}
	}
	
	public void OnSelectRecipe() {
		_callback?.Invoke(_recipe);
	}
}