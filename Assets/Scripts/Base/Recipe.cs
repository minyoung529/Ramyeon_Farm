using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Recipe
{
    public string recipeName;
    [TextArea] public string ingredient;
    private string[] ingredientList;

    public void SetList()
    {
        ingredientList = ingredient.Split(',');
    }

    public string[] GetIngredients()
    {
        return ingredientList;
    }
}
