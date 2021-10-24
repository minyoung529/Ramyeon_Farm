using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient
{
    public string name;
    public int amount;
    public bool isHaving;
    public IngredientState state;
    private int index;

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
        this.amount += amount;
    }
}
