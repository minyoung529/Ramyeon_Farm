using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ingredient
{
    private int index;
    public string name;
    [TextArea] public string info;
    [SerializeField] private int price;
    public int firstPrice;
    public int upgradePrice;
    public float maxTime;
    public float upgradeOffset;
    public float maxTimeOffset;
    public IngredientType type;
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
        if (type == IngredientType.basic && amount < 0)
        {
            return;
        }

        GameManager.Instance.CurrentUser.AddIngredientsAmounts(index, amount);
    }

    public string GetDifferentNames()
    {
        return differentNames[Random.Range(0, differentNames.Count)];
    }

    public void SetInfo(string name, string info, int firstPrice, int price, float maxTime, int upgradePrice, float upgradeOffset)
    {
        this.name = name;
        this.info = info;
        this.firstPrice = firstPrice;
        this.price = price;
        this.maxTime = maxTime;
        this.upgradePrice = upgradePrice;
        this.upgradeOffset = upgradeOffset;
    }

    public int GetUpgradePrice()
    {
        float price = upgradePrice;
        for (int i = 0; i < GameManager.Instance.CurrentUser.GetIngredientLevel(index) - 1; i++)
        {
            price += price / upgradeOffset;
        }

        return Mathf.RoundToInt(price);
    }

    public float GetMaxTime()
    {
        float maxTime = this.maxTime;

        for (int i = 0; i < GameManager.Instance.CurrentUser.GetIngredientLevel(index); i++)
        {
            maxTime -= 1 / maxTimeOffset;
        }

        return maxTime;
    }

    public int GetAmount()
    {
        int level = GameManager.Instance.CurrentUser.GetIngredientLevel(index);
        int plus = 4;
        if (index == 3) plus = 3;
        return 1 + (level / plus);
    }

    public int GetNextPrice()
    {
        int price = this.price;
        price += Mathf.RoundToInt((GameManager.Instance.CurrentUser.GetIngredientLevel(index)) * price * 0.05f);
        return price;
    }

    public int GetPrice()
    {
        int price = this.price;
        price += Mathf.RoundToInt((GameManager.Instance.CurrentUser.GetIngredientLevel(index) - 1) * price * 0.05f);
        return price;
    }
}
