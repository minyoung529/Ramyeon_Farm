using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRamen : MonoBehaviour
{
    private List<Ingredient> differentIngredients = new List<Ingredient>();
    private bool isAdd;
    private GuestComment guestComment = new GuestComment();

    private Recipe GetRandomRamen()
    {
        Recipe recipe = new Recipe(GameManager.Instance.GetRecipes()[GameManager.Instance.GetRandomRecipeIndex()]);

        if (Random.Range(0, 2) == 0)
        {
            if (Random.Range(0, 3) == 0)
            {
                AddOrRemove(false, recipe);
                GameManager.Instance.SetCurrentRecipe(recipe);
                isAdd = false;
            }
            else
            {
                int cnt = Random.Range(1, 3);

                for (int i = 0; i < cnt; i++)
                {
                    AddOrRemove(true, recipe);
                }
                isAdd = true;
                GameManager.Instance.SetCurrentRecipe(recipe);
            }
        }

        else
        {
            GameManager.Instance.SetCurrentRecipe(recipe);
        }

        return recipe;
    }

    private void AddOrRemove(bool isAdd, Recipe recipe)
    {
        Ingredient ingredient = GameManager.Instance.GetIngredients()[GetRandomIngredientIndex()];

        if (isAdd)
        {
            recipe.AddRecipe(ingredient.name);
        }
        else
        {
            recipe.RemoveRecipe(ingredient.name);
        }

        differentIngredients.Add(ingredient);
    }

    private string GuestComment(Recipe recipe, List<Ingredient> ingredients)
    {
        string comment = "";

        if (ingredients.Count > 0 && !isAdd)
        {
            comment = string.Format(guestComment.GetOrderComments(), recipe.recipeName);
            comment += string.Format(guestComment.GetRemoveComments(), ingredients[0].GetDifferentNames());
        }

        else if (ingredients.Count > 0 && isAdd)
        {
            comment = string.Format(guestComment.GetOrderComments(), recipe.recipeName);

            comment += " Ãß°¡·Î";

            for (int i = 0; i < ingredients.Count; i++)
            {
                if (i == ingredients.Count - 1)
                {
                    comment += string.Format(" {0}", ingredients[i].GetDifferentNames());
                }
                else
                {
                    comment += string.Format(" {0},", ingredients[i].GetDifferentNames());
                }
            }
            comment += guestComment.GetAddComments();
        }

        else
        {
            comment = string.Format(guestComment.GetOrderComments(), recipe.recipeName);
        }

        ResetData();
        return comment;
    }

    public string GetRandomGuestComment()
    {
        Recipe recipe = GetRandomRamen();
        return GuestComment(recipe, differentIngredients);
    }

    private void ResetData()
    {
        differentIngredients.Clear();
        isAdd = false;
    }

    private int GetRandomIngredientIndex()
    {
        int rand;

        do
        {
            rand = Random.Range(0, GameManager.Instance.GetIngredients().Count);
        } while (!GameManager.Instance.CurrentUser.GetIsIngredientsHave()[rand] || rand != 2);

        return rand;
    }
}
