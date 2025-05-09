using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.BouncyCastle.Asn1;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class CrafterPresenter
{
	private CrafterView _view;
	private RecipeData _selectedRecipe;
	private int _selectedCraftCount;
	private CrafterModel _model;
	private float _timer;
	
	public CrafterPresenter(CrafterModel model, CrafterView view) {
		_model = model;
		_view = view;
		_model.OnQueueUpdated += OnQueueUpdated;
		_view.OnRecipeSelected += OnRecipeSelected;
		_view.OnCraftButtonClicked += OnCraftButtonClicked;
		_view.OnCraftCountUp += OnCraftCountUp;
		_view.OnCraftCountDown += OnCraftCountDown;
		_selectedCraftCount = 1;
		_view.SetCraftCount(1);
		_view.SetCraftCountUpInteractable(false);
		_view.SetCraftCountDownInteractable(false);
		_view.SetCraftButtonInteractable(false);
		_view.SetCraftQueue(new List<CraftingSession>());
		_view.SetIngredientList(new ItemSetList());
		_view.SetRecipeInfo(null);
		_view.SetAvailableRecipeList(_model.Recipes.ToList());
		UpdateRecipeCraftability();
		Debug.Log("CrafterPresenter: Crafter Presenter initialized");
	}
	
	public void Update(float deltaTime) {
		UpdateRecipeCraftability();
		if (_model.CraftQueue.Count <= 0) return;
		_model.DecrementCraftTimeFromFirstInQueue(deltaTime);
		CraftingSession session = _model.CraftQueue[0];
		if (!session.IsDone) return;
		if (session.CraftCount > 1) {
			session.DecrementCraftCount();
			session.ResetCraftTime();
			_view.SetCraftQueue(_model.CraftQueue);
			_model.PlayerInventory.AddItemByData(session.Recipe.Result);
			return;
		} 
		_model.RemoveFromCraftQueue(session);
		_model.PlayerInventory.AddItemByData(session.Recipe.Result);
	}

	public void Dispose() {
		_model.OnQueueUpdated -= OnQueueUpdated;
		_view.OnRecipeSelected -= OnRecipeSelected;
		_view.OnCraftButtonClicked -= OnCraftButtonClicked;
		_view.OnCraftCountUp -= OnCraftCountUp;
		_view.OnCraftCountDown -= OnCraftCountDown;
	}

	private void UpdateRecipeCraftability() {
		foreach (RecipeData recipe in _model.Recipes.ToList()) {
			_view.SetRecipeDisabled(recipe, !CanCraft(recipe, _selectedCraftCount));
		}
		if (_selectedRecipe == null) return;
		_view.SetCraftButtonInteractable(CanCraft(_selectedRecipe, _selectedCraftCount));
		_view.SetCraftCountDownInteractable(_selectedCraftCount > 1);
		_view.SetCraftCountUpInteractable(CanCraft(_selectedRecipe, _selectedCraftCount + 1));
	}
	
	private void OnQueueUpdated(List<CraftingSession> craftQueue) {
		_view.SetCraftQueue(craftQueue);
	}
	private void OnRecipeSelected(RecipeData recipe) {
		_selectedRecipe = recipe;
		_view.SetRecipeInfo(recipe);
		_view.SetIngredientList(recipe.IngredientSetList);
		_selectedCraftCount = 1;
		_view.SetCraftCount(_selectedCraftCount);
		UpdateRecipeCraftability();
	}
	private void OnCraftButtonClicked() {
		if (_selectedRecipe == null) return;
		_model.AddToCraftQueue(_selectedRecipe, _selectedCraftCount);
		_model.PlayerInventory.RemoveItemsByData(_selectedRecipe.IngredientSetList.ToDatasList());
	}
	private void OnCraftCountUp() {
		_selectedCraftCount++;
		UpdateRecipeCraftability();
		_view.SetCraftCount(_selectedCraftCount);
		_view.SetIngredientList(_selectedRecipe.IngredientSetList, _selectedCraftCount);
	}
	private void OnCraftCountDown() {
		_selectedCraftCount--;
		UpdateRecipeCraftability();
		_view.SetCraftCount(_selectedCraftCount);
		_view.SetIngredientList(_selectedRecipe.IngredientSetList, _selectedCraftCount);
	}
	private bool CanCraft(RecipeData recipe, int count) {
		if (BackgroundInfo._infCraft) return true;
		return recipe.IngredientSetList.ContainedWithin(_model.PlayerInventory.GetItems(), count);
	}
}