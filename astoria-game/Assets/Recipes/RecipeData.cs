using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable Objects/Recipes/RecipeData", order = 1)]
public class RecipeData : ScriptableObject
{
	public Sprite Icon;
	public ItemSetList IngredientSetList;
	public ItemSetList ResultSetList;
	public float CraftTime;
}
