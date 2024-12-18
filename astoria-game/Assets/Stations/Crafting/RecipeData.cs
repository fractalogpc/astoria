using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable Objects/RecipeData", order = 1)]
public class RecipeData : ScriptableObject
{
	public List<ItemSet> _ingredientSets;
	public List<ItemSet> _resultSets;
}
