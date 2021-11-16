using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient
{
    private int index;
    public string name;
    [TextArea] public string info;
    public int price;
    public bool isHaving;
    public IngredientState state;
    public List<string> differentNames;

    public void SetIndex(int i)
    {
        index = i;
    }

    public int GetIndex()
    {
        return index;
    }

    public void AddAmount(int amount)
    {
        if(state == IngredientState.basic && amount < 0)
        {
            return;
        }

        GameManager.Instance.CurrentUser.ingredientsAmounts[index] += amount;
    }

    public string GetDifferentNames()
    {
        return differentNames[Random.Range(0, differentNames.Count)];
    }
}
