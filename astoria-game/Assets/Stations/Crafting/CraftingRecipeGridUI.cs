using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CraftingRecipeGridUI : MonoBehaviour
{
	public bool Initialized = false;
	[SerializeField] [ReadOnly] private CraftingStationNetworked _craftingStation;
	[SerializeField] private Transform _recipeGridParent;
	[SerializeField] private GameObject _recipePrefab;

	public void Initialize(CraftingStationNetworked craftingStation) {
		if (Initialized) return;
		_craftingStation = craftingStation;
		CreateRecipeGrid(_craftingStation.Recipes);
		Initialized = true;
	}
	
	public void CreateRecipeGrid(List<RecipeData> recipes) {
		foreach (Transform child in _recipeGridParent) {
			Destroy(child.gameObject);
		}

		foreach (RecipeData recipe in recipes) {
			CraftingRecipeUI recipeUI = Instantiate(_recipePrefab, _recipeGridParent).GetComponent<CraftingRecipeUI>();
			recipeUI.Initialize(_craftingStation, recipe, OnRecipeClicked);
		}
	}
	
	private void OnRecipeClicked(RecipeData recipe) {
		_craftingStation.SelectRecipe(recipe);
	}
	
	public void UpdateRecipeGrid() {
		for (int i = 0; i < _recipeGridParent.childCount; i++) {
			CraftingRecipeUI recipeUI = _recipeGridParent.GetChild(i).GetComponent<CraftingRecipeUI>();
			recipeUI.UpdateRecipe();
		}
	}
}