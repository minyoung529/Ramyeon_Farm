using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Recipe
{
    public string recipeName;
    [TextArea] public string ingredient;
    private List<string> ingredientList;
    private string info;

    public Recipe(string recipeName_, string ingredient_)
    {
        recipeName = recipeName_;
        ingredient = ingredient_;

        SetList();
    }

    public Recipe(Recipe recipe)
    {
        recipeName = recipe.recipeName;
        ingredient = recipe.ingredient;
        SetList();
    }

    public void SetList()
    {
        ingredientList = ingredient.Split(',').ToList();
    }

    public List<string> GetIngredients()
    {
        return ingredientList;
    }

    public void AddRecipe(string ingredient)
    {
        ingredientList.Add(ingredient);
    }

    public void RemoveRecipe(string ingredient)
    {
        ingredientList.Remove(ingredient);
    }
}
