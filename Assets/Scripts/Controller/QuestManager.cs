using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private float maxTime = 1f;
    private float curTime = 0f;

    [SerializeField] private int guestQuest;
    [SerializeField] private int miniGameQuest;
    [SerializeField] private int levelUpQuest;
    [SerializeField] private int timeQuest;


    void Start()
    {
        if(GameManager.Instance.CurrentUser.GetUserData() != DateTime.Now.ToString("yyyyMMdd"))
        {
            foreach(Quest quest in GameManager.Instance.CurrentUser.questList)
            {
                quest.ResetQuest();
            }
        }

        GameManager.Instance.CurrentUser.SetUserData(DateTime.Now.ToString("yyyyMMdd"));
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentUser.questList[timeQuest].isPerform) return;
        if (GameManager.Instance.CurrentUser.questList[timeQuest].isRewarded) return;

        curTime += Time.deltaTime;

        if (curTime > maxTime)
        {
            GameManager.Instance.CurrentUser.questList[timeQuest].AddCurrentValue(1);
            curTime = 0;
        }
    }
}
