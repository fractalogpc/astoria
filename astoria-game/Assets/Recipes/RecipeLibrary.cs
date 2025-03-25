using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeLibrary", menuName = "Scriptable Objects/Recipes/RecipeLibrary", order = 0)]
public class RecipeLibrary : ScriptableObject
{
    public List<RecipeData> Recipes = new List<RecipeData>();

    public List<RecipeData> GetRecipes()
    {
        return Recipes;
    }
}
