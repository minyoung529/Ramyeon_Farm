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

    //�� �����ִ� �Լ�, �Ű������� - �ϸ� ����
    public void AddUserMoney(int addMoney)
    {
        money += addMoney;
    }

    public void SetMoney(long money)
    {
        this.money = money;
    }

    //�� ������ ���� �Լ�
    public long GetMoney()
    {
        return money;
    }
}
