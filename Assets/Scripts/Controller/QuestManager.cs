using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<Quest> questList = new List<Quest>();
    [SerializeField] private List<Achievement> achievementList = new List<Achievement>();

    [SerializeField] private TextAsset achievementName;
    [SerializeField] private TextAsset achievementInfo;

    private float maxTime = 1f;
    private float curTime = 0f;

    private void Awake()
    {
        for (int i = 0; i < questList.Count; i++)
        {
            questList[i].index = i;
        }

        InputAchievementData();
    }
    private void Start()
    {
        ResetQuest();
    }
    private void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > maxTime)
        {
            AddQuestValue(KeyManager.TIMEQUEST_INDEX, 1);
            GameManager.Instance.CurrentUser.AddSecond();
            curTime = 0;
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            for (int i = 0; i < achievementList.Count; i++)
            {
                UpdateAchievement((AchievementType)i, 1);
            }
        }
    }

    public void AddQuestValue(int index, int value)
    {
        if (Check())
        {
            int index_ = GetQuestListIndex(index);
            if (index_ < 0) return;
            GameManager.Instance.CurrentUser.questList[index_].AddCurrentValue(value);
        }
    }

    private bool Check()
    {
        for (int i = 0; i < GameManager.Instance.QuestManager.questList.Count; i++)
        {
            for (int j = 0; j < KeyManager.QUEST_COUNT; j++)
            {
                if (i == GameManager.Instance.CurrentUser.questList[j].index)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public int GetQuestListIndex(int index)
    {
        for (int i = 0; i < KeyManager.QUEST_COUNT; i++)
        {
            if (GameManager.Instance.CurrentUser.questList[i].index == index)
                return i;
        }

        return -1;
    }

    public void ResetQuest()
    {
        TimeSpan nowTimeSpan = GameManager.Instance.ReturnNowTimeSpan();

        if (GameManager.Instance.CurrentUser.GetUserTimeSpan() < nowTimeSpan.Days)
        {
            foreach (Quest quest in GameManager.Instance.CurrentUser.questList)
            {
                quest.ResetQuest();
            }

            GameManager.Instance.CurrentUser.CheckCurrentQuest();
            GameManager.Instance.UIManager.ResetQuestPanelData();
            GameManager.Instance.CurrentUser.SetUserTimeSpan(nowTimeSpan);
        }
    }

    public bool IsNextDay()
    {
        TimeSpan nowTimeSpan = GameManager.Instance.ReturnNowTimeSpan();

        if (GameManager.Instance.CurrentUser.GetUserTimeSpan() < nowTimeSpan.Days)
        {
            Debug.Log(GameManager.Instance.CurrentUser.GetUserTimeSpan());
            GameManager.Instance.CurrentUser.SetUserTimeSpan(nowTimeSpan);
            return true;
        }

        return false;
    }

    private void InputAchievementData()
    {
        string[] types = achievementName.ToString().Split('\t', '\n');
        int achieveCnt = types.Length / 2;
        for (int i = 0; i < achieveCnt; i++)
        {
            achievementList.Add(new Achievement(i, int.Parse(types[i * 2]), types[i * 2 + 1]));
        }

        string[] infos = achievementInfo.ToString().Split('\t', '\n');
        int increment = 0;

        for (int i = 0; i < achievementList.Count; i++)
        {
            for (int j = 0; j < achievementList[i].achieveCount; j++)
            {
                int offset = (increment + j) * 3;
                achievementList[i].AddData(infos[offset], int.Parse(infos[offset + 1]), int.Parse(infos[offset + 2]));
            }

            increment += achievementList[i].achieveCount;
        }

        if (GameManager.Instance.CurrentUser.currentAchievement.Length == 0)
        {
            GameManager.Instance.CurrentUser.achievementLevel = new int[achieveCnt];
            GameManager.Instance.CurrentUser.currentAchievement = new int[achieveCnt];
        }
    }

    public void UpdateAchievement(AchievementType type, int amount)
    {
        GameManager.Instance.CurrentUser.PlusCurrentAchievement((int)type, amount);
    }

    #region GetSet
    public List<Quest> GetQuestList()
    {
        return questList;
    }
    public List<Achievement> GetAchievements()
    {
        return achievementList;
    }
    #endregion
}
