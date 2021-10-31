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

    [SerializeField] private string userDate;

    //�� �����ִ� �Լ�, �Ű������� - �ϸ� ����
    public void AddUserMoney(int addMoney)
    {
        money += addMoney;
        GameManager.Instance.UIManager.UpdateMoneyText();
    }

    public void SetMoney(long money)
    {
        this.money = money;
        GameManager.Instance.UIManager.UpdateMoneyText();
    }

    //�� ������ ���� �Լ�
    public long GetMoney()
    {
        return money;
    }

    public string GetUserData()
    {
        return userDate;
    }

    public void SetUserData(string data)
    {
        userDate = data;
    }
}
