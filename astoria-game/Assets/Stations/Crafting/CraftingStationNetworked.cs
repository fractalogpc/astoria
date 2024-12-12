using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/guides/networkbehaviour
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

[Serializable]
public class RecipeData : ScriptableObject
{
	public List<ItemData> _ingredients;
	public ItemData _result;
	public int _resultCount;
}

public class CraftingStationNetworked : NetworkBehaviour
{
	[SerializeField] private InventoryComponent _ingredientInput;
	[SerializeField] private InventoryComponent _outputInventory;
	public List<RecipeData> _recipes;
	
	/// <summary>
	/// Add your validation code here after the base.OnValidate(); call.
	/// </summary>
	protected override void OnValidate() {
		base.OnValidate();
	}

	// NOTE: Do not put objects in DontDestroyOnLoad (DDOL) in Awake.  You can do that in Start instead.

	private void Start() {
		
	}

	/// <summary>
	/// Should be called by a Interactor event.
	/// </summary>
	public void OnInteract() {
		
	}
}