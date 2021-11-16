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
    public int firstPrice;
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

        GameManager.Instance.CurrentUser.AddIngredientsAmounts(index, amount);
    }

    public string GetDifferentNames()
    {
        return differentNames[Random.Range(0, differentNames.Count)];
    }

    public void SetInfo(string name, string info, int firstPrice, int price)
    {
        this.name = name;
        this.info = info;
        this.firstPrice = firstPrice;
        this.price = price;
    }
}
