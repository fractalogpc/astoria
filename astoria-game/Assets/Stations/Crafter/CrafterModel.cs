using System;
using System.Collections.Generic;
using System.Text;
using Mirror.BouncyCastle.Asn1;
using UnityEngine;
using UnityEngine.Android;

public class CraftingSession 
{
	public RecipeData Recipe;
	public int CraftCount;
	public float CraftTimeLeft;
	public bool IsDone => CraftTimeLeft <= 0f;
	
	public CraftingSession(RecipeData recipe, int craftCount) {
		Recipe = recipe;
		CraftCount = craftCount;
		CraftTimeLeft = Recipe.CraftTime;
	}
	
	public void DecrementCraftTime(float deltaTime) {
		CraftTimeLeft -= deltaTime;
	}
	
	public void ResetCraftTime() {
		CraftTimeLeft = Recipe.CraftTime;
	}
	
	public void DecrementCraftCount() {
		CraftCount--;
	}
}

public class CrafterModel
{
	public delegate void QueueUpdated(List<CraftingSession> craftQueue);
	public event QueueUpdated OnQueueUpdated;
	public InventoryComponent PlayerInventory { get; private set; }
	public RecipeLibrary Recipes { get; private set; }
	public List<CraftingSession> CraftQueue { get; private set; }

	public CrafterModel(InventoryComponent playerInventory, RecipeLibrary recipeLibrary) {
		PlayerInventory = playerInventory;
		Recipes = recipeLibrary;
		CraftQueue = new List<CraftingSession>();
	}
	public void DecrementCraftTimeFromFirstInQueue(float deltaTime) {
		if (CraftQueue.Count <= 0) return; // not logic, just avoiding index out of range
		CraftQueue[0].DecrementCraftTime(deltaTime);
		OnQueueUpdated?.Invoke(CraftQueue);
	}
	// Unused due to direct CraftQueue[0].count access and decrement by CrafterPresenter
	//
	// public void DecrementCraftCountFromFirstInQueue() {
	// 	if (CraftQueue.Count <= 0) return; // not logic, just avoiding index out of range
	// 	CraftQueue[0].DecrementCraftCount();
	// 	OnQueueUpdated?.Invoke(CraftQueue);
	// }
	// public void ResetCraftTimeFromFirstInQueue() {
	// 	if (CraftQueue.Count <= 0) return; // not logic, just avoiding index out of range
	// 	CraftQueue[0].ResetCraftTime();
	// 	OnQueueUpdated?.Invoke(CraftQueue);
	// }
	public void AddToCraftQueue(RecipeData recipe, int count) {
		CraftQueue.Add(new CraftingSession(recipe, count));
		OnQueueUpdated?.Invoke(CraftQueue);
	}
	public void RemoveFromCraftQueue(CraftingSession session) {
		CraftQueue.Remove(session);
		OnQueueUpdated?.Invoke(CraftQueue);
	}
	public void ClearCraftQueue() {
		CraftQueue.Clear();
		OnQueueUpdated?.Invoke(CraftQueue);
	}
}