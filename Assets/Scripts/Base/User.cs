using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    [SerializeField] private long money;
    public List<Ingredient> ingredients = new List<Ingredient>();
    public List<Recipe> recipes = new List<Recipe>();
    public List<Livestock> livestocks = new List<Livestock>();
    public List<Quest> questList = new List<Quest>();

    //돈 더해주는 함수, 매개변수에 - 하면 빠짐
    public void AddUserMoney(int addMoney)
    {
        money += addMoney;
    }

    public void SetMoney(long money)
    {
        this.money = money;
    }

    //돈 가지고 오는 함수
    public long GetMoney()
    {
        return money;
    }
}
