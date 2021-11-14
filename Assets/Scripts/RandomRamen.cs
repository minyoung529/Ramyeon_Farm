using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRamen : MonoBehaviour
{
    public Recipe GetRandomRamen()
    {
        Recipe recipe;

        if (Random.Range(0, 2) == 0)
        {
            //recipe = new Recipe("",);
            return recipe;
        }

        else
        {
            recipe = GameManager.Instance.GetRecipes()[GameManager.Instance.GetRandomRecipeIndex()];
            return recipe;
        }
    }
}
