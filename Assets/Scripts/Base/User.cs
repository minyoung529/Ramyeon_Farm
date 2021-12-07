using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class User
{
    [SerializeField] private long money;
    [SerializeField] private int day;
    public List<Livestock> livestocks = new List<Livestock>();

    [SerializeField] private List<bool> isIngredientsHave = new List<bool>();
    [SerializeField] private List<int> ingredientsAmounts = new List<int>();
    [SerializeField] private List<int> ingredientsLevels = new List<int>();

    public Quest[] questList = new Quest[KeyManager.QUEST_COUNT];

    public int[] achievementLevel;
    public int[] currentAchievement;

    [SerializeField] private string userDate;
    [SerializeField] private long userTimeSpan;

    [SerializeField] private int playTime;

    public bool isCompleteTutorial;
    //완료시 user에서 변수 true 시키고
    //앞으로 게임 켰을 때 true이면 튜토리얼 실행 X

    //돈 더해주는 함수, 매개변수에 - 하면 빠짐
    public void AddUserMoney(int addMoney)
    {
        if(addMoney > 0)
        {
            PlusCurrentAchievement((int)AchievementType.Money, addMoney);
        }

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
        GameManager.Instance.UIManager.CheckIsUpdateInMenu();
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
        GameManager.Instance.UIManager.CheckIsUpdateInMenu();
    }

    public bool IsAchievementReward(int index)
    {
        Achievement achievement = GameManager.Instance.QuestManager.GetAchievements()[index];

        for (int i = achievementLevel[index]; i < achievement.achieveCount; i++)
        {
            if (currentAchievement[index] >= achievement.conditions[i])
            {
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

    public List<int> GetIngredientsAmounts()
    {
        return ingredientsAmounts;
    }
    public List<bool> GetIsIngredientsHave()
    {
        return isIngredientsHave;
    }
    public void AddIngredientsAmounts(int index, int amount)
    {
        if (!isIngredientsHave[index]) return;
        ingredientsAmounts[index] += amount;

        GameManager.Instance.UIManager.UpdateInventoryPanel();
    }
    public void SetIsIngredientsHave(int index, bool isTrue)
    {
        isIngredientsHave[index] = isTrue;
        GameManager.Instance.SetUserIndex();
        GameManager.Instance.UIManager.UpdateIngredientPanel();
    }
    public void AddDay(int amount)
    {
        day += amount;
    }
    public int GetDay()
    {
        return day;
    }
    public int GetIngredientLevel(int index)
    {
        return ingredientsLevels[index];
    }
    public void AddIngredientLevel(int index)
    {
        ingredientsLevels[index]++;
    }

    public void AddSecond()
    {
        int index = (int)AchievementType.Time;
        Achievement achievement = GameManager.Instance.QuestManager.GetAchievements()[index];

        if (achievement.conditions[achievementLevel[index]] == achievement.achieveCount) return;

        playTime++;

        if (playTime == 3600)
        {
            PlusCurrentAchievement(index, 1);
            playTime = 0;
        }
    }
    #endregion
}