using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class User
{
    [SerializeField] private long money;
    public List<Ingredient> ingredients = new List<Ingredient>();
    public List<Livestock> livestocks = new List<Livestock>();
    public Quest[] questList = new Quest[KeyManager.QUEST_COUNT];

    [SerializeField] private string userDate;
    [SerializeField] private long userTimeSpan;


    //돈 더해주는 함수, 매개변수에 - 하면 빠짐
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

    //돈 가지고 오는 함수
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

    public long GetUserTimeSpan()
    {
        return userTimeSpan;
    }

    public void SetUserTimeSpan(TimeSpan timeSpan)
    {
        userTimeSpan = timeSpan.Days;
    }

    public void CheckCurrentQuest()
    {
        if (!GameManager.Instance.QuestManager.IsNextDay() && !IsSame()) return;

        int rand;
        List<Quest> quests = GameManager.Instance.QuestManager.GetQuestList();

        for (int i = 0; i < KeyManager.QUEST_COUNT; i++)
        {
            rand = Random.Range(0, quests.Count);
            questList[i] = quests[rand];
        }

        for (int i = 0; i < KeyManager.QUEST_COUNT;)
        {
            for (int j = 0; j < KeyManager.QUEST_COUNT; j++)
            {
                if (questList[i].questName == questList[j].questName && i != j)
                {
                    rand = Random.Range(0, quests.Count);
                    questList[i] = quests[rand];
                    break;
                }

                else if (j == KeyManager.QUEST_COUNT - 1)
                {
                    i++;
                }
            }
        }

        Debug.Log("갱신");
    }

    private bool IsSame()
    {
        return (questList[0].questName == questList[1].questName) &&
            (questList[1].questName == questList[2].questName) &&
            (questList[2].questName == questList[0].questName);
    }
}
