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

    string d = "{0}æ»≥Á«œººø‰";

    private void Awake()
    {
        for (int i = 0; i < questList.Count; i++)
        {
            questList[i].index = i;
        }
    }
    private void Start()
    {
        InputAchievementData();
        ResetQuest();
    }
    private void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > maxTime)
        {
            AddQuestValue(KeyManager.TIMEQUEST_INDEX, 1);
            curTime = 0;
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

    public List<Quest> GetQuestList()
    {
        return questList;
    }

    private void InputAchievementData()
    {
        string[] types = achievementName.ToString().Split('\t', '\n');

        for (int i = 0; i < types.Length / 2; i++)
        {
            achievementList.Add(new Achievement(i, int.Parse(types[i * 2]), types[i * 2 + 1]));
        }

        string[] infos = achievementInfo.ToString().Split('\t', '\n');
        int increasement = 0;
        for (int i = 0; i < achievementList.Count; i++)
        {
            //1
            for (int j = 0; j < achievementList[i].achieveCount; j++)
            {
                int offset = (increasement + j) * 3;
                Debug.Log(j);
                achievementList[i].AddData(infos[offset], int.Parse(infos[offset + 1]), int.Parse(infos[offset + 2]));
            }

            increasement += achievementList[i].achieveCount;
        }
    }
    // 0 3 6 9 12 15
    // 1 4 7 10 13
    // 2 5 8 11 14
}
