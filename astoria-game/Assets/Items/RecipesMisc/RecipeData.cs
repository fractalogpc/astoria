using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable Objects/Crafting/RecipeData", order = 1)]
public class RecipeData : ScriptableObject
{
	public ItemSetList _ingredientSetList;
	public ItemSetList _resultSetList;
}
