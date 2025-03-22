using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class StationRecipeGridUI : MonoBehaviour
{
	public bool Initialized = false;
	[SerializeField] [ReadOnly] private StationCore _station;
	[SerializeField] private Transform _recipeGridParent;
	[SerializeField] private GameObject _recipePrefab;

	public void Initialize(StationCore station) {
		if (Initialized) return;
		_station = station;
		CreateRecipeGrid(_station.Recipes.GetRecipes());
		Initialized = true;
	}
	
	public void CreateRecipeGrid(List<RecipeData> recipes) {
		foreach (Transform child in _recipeGridParent) {
			Destroy(child.gameObject);
		}

		foreach (RecipeData recipe in recipes) {
			StationRecipeUI recipeUI = Instantiate(_recipePrefab, _recipeGridParent).GetComponent<StationRecipeUI>();
			recipeUI.Initialize(_station, recipe, OnRecipeClicked);
		}
	}
	
	private void OnRecipeClicked(RecipeData recipe) {
		_station.SelectRecipe(recipe);
	}
	
	public void UpdateRecipeGrid() {
		for (int i = 0; i < _recipeGridParent.childCount; i++) {
			StationRecipeUI recipeUI = _recipeGridParent.GetChild(i).GetComponent<StationRecipeUI>();
			recipeUI.UpdateInteractivity();
		}
	}
}