using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableRecipe : MonoBehaviour
{
	public delegate void RecipeSelected(RecipeData recipe);
	public event RecipeSelected OnRecipeSelected;
	
	[field: ReadOnly] public RecipeData Recipe { get; private set; }
	[SerializeField] private Image _recipeIcon;
	[SerializeField] private CanvasGroup _recipeIconCanvasGroup;
	[SerializeField] private Button _selectRecipeButton;
	[SerializeField][Range(0f, 1f)] private float _alphaWhenDisabled = 0.5f;
	[SerializeField][ColorUsage(true, false)] private Color _disabledColor;

	public void Initialize(RecipeData recipe) {
		Recipe = recipe;
		_recipeIcon.sprite = Recipe.Result.ItemIcon;
		_recipeIcon.preserveAspect = true;
	}
	public void RegisterListener(RecipeSelected callback) {
		OnRecipeSelected += callback;
	}
	public void UnregisterListener(RecipeSelected callback) {
		OnRecipeSelected -= callback;
	}

	public void SetDisabledLook(bool disabled) {
		_recipeIconCanvasGroup.alpha = disabled ? _alphaWhenDisabled : 1f;
		_recipeIcon.color = disabled ? _disabledColor : Color.white;
	}
	
	private void OnSelectRecipe() {
		OnRecipeSelected?.Invoke(Recipe);
	}
	
	private void Start() {
		_selectRecipeButton.onClick.AddListener(OnSelectRecipe);
		_recipeIcon.sprite = Recipe.Result.ItemIcon == null ?  Resources.Load<Sprite>("DefaultItemAssets/NullImage") : Recipe.Result.ItemIcon;
		_recipeIcon.preserveAspect = true;
	}

	private void OnDisable() {
		_selectRecipeButton.onClick.RemoveListener(OnSelectRecipe);
		OnRecipeSelected = null;
	}

	private void OnDestroy() {
		print("SelectableRecipe: Destroyed");
	}
}