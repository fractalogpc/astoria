using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable Objects/Recipes/RecipeData", order = 1)]
public class RecipeData : ScriptableObject
{
	public ItemSetList IngredientSetList;
	public ItemData Result;
	public float CraftTime;
}
