using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CrafterRecipeGridUI : MonoBehaviour
{
	public bool Initialized = false;
	[SerializeField] [ReadOnly] private CrafterCore _crafter;
	[SerializeField] private Transform _recipeGridParent;
	[SerializeField] private GameObject _recipePrefab;

	public void Initialize(CrafterCore crafter) {
		if (Initialized) return;
		_crafter = crafter;
		CreateRecipeGrid(_crafter.Recipes.GetRecipes());
		Initialized = true;
	}
	
	public void CreateRecipeGrid(List<RecipeData> recipes) {
		foreach (Transform child in _recipeGridParent) {
			Destroy(child.gameObject);
		}

		foreach (RecipeData recipe in recipes) {
			SelectableRecipe recipeSelectable = Instantiate(_recipePrefab, _recipeGridParent).GetComponent<SelectableRecipe>();
			recipeSelectable.Initialize(_crafter, recipe, OnRecipeClicked);
		}
	}
	
	private void OnRecipeClicked(RecipeData recipe) {
		_crafter.SelectRecipe(recipe);
	}
	
	public void UpdateRecipeGrid() {
		for (int i = 0; i < _recipeGridParent.childCount; i++) {
			SelectableRecipe recipeSelectable = _recipeGridParent.GetChild(i).GetComponent<SelectableRecipe>();
			recipeSelectable.UpdateInteractivity();
		}
	}
}