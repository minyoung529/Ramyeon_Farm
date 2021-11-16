using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRamen : MonoBehaviour
{
    private List<Ingredient> differentIngredients = new List<Ingredient>();
    private bool isAdd;

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
            ingredient = GameManager.Instance.GetIngredients()[GetRandomIngredientIndex()];
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
            comment = string.Format("{0} 주세용~", recipe.recipeName);
            comment += string.Format(" {0} 빼고요!!", ingredients[0].GetDifferentNames());
        }

        else if (ingredients.Count > 0 && isAdd)
        {
            comment = string.Format("{0} 주세여", recipe.recipeName);

            comment += " 추가로";

            for (int i = 0; i < ingredients.Count; i++)
            {
                if (i == ingredients.Count - 1)
                {
                    comment += string.Format(" {0}", ingredients[i].GetDifferentNames());
                }
                else
                {
                    comment += string.Format(" {0}랑", ingredients[i].GetDifferentNames());
                }
            }
            comment += "도 넣어주세여~~";
        }

        else
        {
            comment = string.Format("{0} 주시라구요", recipe.recipeName);
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
        int maxCount = GameManager.Instance.GetIngredients().Count;
        int rand;

        do
        {
            rand = Random.Range(0, maxCount);

        } while (!GameManager.Instance.CurrentUser.GetIsIngredientsHave()[rand] && rand != 3);

        return rand;
    }
}
