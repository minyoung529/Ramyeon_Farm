using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class User
{
    [SerializeField] private long money;
    public List<Livestock> livestocks = new List<Livestock>();

    public List<bool> isIngredientsHave = new List<bool>();
    public List<int> ingredientsAmounts = new List<int>();

    public Quest[] questList = new Quest[KeyManager.QUEST_COUNT];

    public int[] achievementLevel;
    public int[] currentAchievement;

    [SerializeField] private int level;
    [SerializeField] private int experiencePoint;

    [SerializeField] private string userDate;
    [SerializeField] private long userTimeSpan;

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
    }

    private bool IsSame()
    {
        return (questList[0].questName == questList[1].questName) &&
            (questList[1].questName == questList[2].questName) &&
            (questList[2].questName == questList[0].questName);
    }

    public void PlusCurrentAchievement(int index, int amount)
    {
        currentAchievement[index] += amount;
        GameManager.Instance.UIManager.UpdateAchievementPanel();
    }

    public void CheckAchievement(int index)
    {
        Achievement achievement = GameManager.Instance.QuestManager.GetAchievements()[index];

        for (int i = achievementLevel[index]; i < achievement.achieveCount; i++)
        {
            if (currentAchievement[index] >= achievement.conditions[i])
            {
                achievementLevel[index] = i + 1;
                break;
            }
        }

        GameManager.Instance.UIManager.UpdateAchievementPanel();
    }

    public bool IsAchievementReward(int index)
    {
        Achievement achievement = GameManager.Instance.QuestManager.GetAchievements()[index];

        for (int i = achievementLevel[index]; i < achievement.achieveCount; i++)
        {
            // myLevel: 2   targetLevel: 3
            // 110           100

            if (currentAchievement[index] >= achievement.conditions[i])
            {
                Debug.Log(index + ", " + i);
                return true;
            }
        }

        return false;
    }

    #region GetSet

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

    public void SetLevel(int level)
    {
        this.level = level;
    }
    public int GetLevel()
    {
        return level;
    }
    public void SetEXP(int exp)
    {
        experiencePoint = exp;
    }
    public int GetEXP()
    {
        return experiencePoint;
    }
    #endregion
}