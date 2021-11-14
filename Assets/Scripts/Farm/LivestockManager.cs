using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivestockManager : MonoBehaviour
{
    List<LivestockObject> livestockObjects = new List<LivestockObject>();

    void Start()
    {
        SpriteRenderer sr;

        SetLivestockIngredient();

        for (int i = 0; i < transform.childCount; i++)
        {
            sr = transform.GetChild(i).GetComponent<SpriteRenderer>();
            livestockObjects.Add(transform.GetChild(i).GetChild(0).GetComponent<LivestockObject>());
            livestockObjects[i].SetValue(GameManager.Instance.CurrentUser.livestocks[i], sr.size.x, sr.size.y);
        }
    }

    private void SetLivestockIngredient()
    {
        List<Ingredient> ingredients = GameManager.Instance.CurrentUser.ingredients;
        int count = 0;

        for (int i = 0; i < GameManager.Instance.CurrentUser.livestocks.Count; i++)
        {
            for (int j = count; j < ingredients.Count; j++)
            {
                if (ingredients[j].state == IngredientState.meat)
                {
                    count = j + 1;
                    GameManager.Instance.CurrentUser.livestocks[i].SetIngredient(ingredients[j]);
                    break;
                }
            }
        }
    }
}
