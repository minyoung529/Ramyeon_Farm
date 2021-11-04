using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<Quest> questList = new List<Quest>();

    private float maxTime = 1f;
    private float curTime = 0f;

    public int guestQuest;
    public int miniGameQuest;
    public int starQuest;
    public int timeQuest;
    public int cookQuest;
    public int moneyQuest;
    public int farmQuest;

    private void Awake()
    {
        for (int i = 0; i < questList.Count; i++)
        {
            questList[i].index = i;
        }
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
            AddQuestValue(timeQuest, 1);
            curTime = 0;
        }
    }

    public void AddQuestValue(int index, int value)
    {
        if (Check())
        {
            int index_ = GetIndex(index);
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

    public int GetIndex(int index)
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
            GameManager.Instance.UIManager.UpdateQuestPanel();

            GameManager.Instance.CurrentUser.SetUserTimeSpan(nowTimeSpan);
        }
    }



    public bool IsNextDay()
    {
        TimeSpan nowTimeSpan = GameManager.Instance.ReturnNowTimeSpan();

        if (GameManager.Instance.CurrentUser.GetUserTimeSpan() < nowTimeSpan.Days)
        {
            Debug.Log(GameManager.Instance.CurrentUser.GetUserTimeSpan());
            Debug.Log(nowTimeSpan.Days);
            GameManager.Instance.CurrentUser.SetUserTimeSpan(nowTimeSpan);
            return true;
        }

        return false;
    }

    public List<Quest> GetQuestList()
    {
        return questList;
    }
}
