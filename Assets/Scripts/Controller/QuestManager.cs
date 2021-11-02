using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private float maxTime = 1f;
    private float curTime = 0f;

    public int guestQuest;
    public int miniGameQuest;
    public int starQuest;
    public int timeQuest;
    public int cookQuest;
    public int moneyQuest;
    public int farmQuest;


    private void Start()
    {
        ResetQuest();
    }
    private void Update()
    {
        if (GameManager.Instance.CurrentUser.questList[timeQuest].isPerform) return;
        if (GameManager.Instance.CurrentUser.questList[timeQuest].isRewarded) return;

        curTime += Time.deltaTime;

        if (curTime > maxTime)
        {
            AddQuestValue(timeQuest, 1);
            curTime = 0;
        }
    }

    public void AddQuestValue(int index, int value)
    {
        GameManager.Instance.CurrentUser.questList[index].AddCurrentValue(value);
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

            GameManager.Instance.CurrentUser.SetUserTimeSpan(nowTimeSpan);
        }
    }

    public bool IsNextDay()
    {
        TimeSpan nowTimeSpan = GameManager.Instance.ReturnNowTimeSpan();

        if (GameManager.Instance.CurrentUser.GetUserTimeSpan() < nowTimeSpan.Days)
        {
            return true;
        }

        return false;
    }
}
