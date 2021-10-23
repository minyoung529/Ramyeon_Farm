using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    [SerializeField] private long money;
    public List<Ingredient> ingredients = new List<Ingredient>();
    public List<Recipe> recipes = new List<Recipe>();

    public void AddUserMoney(int addMoney)
    {
        money += addMoney;
    }

    public long GetMoney()
    {
        return money;
    }
}
