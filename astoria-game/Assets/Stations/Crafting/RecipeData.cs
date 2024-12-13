using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scriptable Objects/RecipeData", order = 1)]
public class RecipeData : ScriptableObject
{
	public List<ItemSet> _ingredients;
	public List<ItemSet> _result;
}
