using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private float curTime = 0;
    private int begin = 7;
    private int end = 22;
    private float time;
    private float second = 30;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private ActiveScale timePanel;

    void Start()
    {
        Timer();
        StartCoroutine(DayTime());
        time = second / end - begin / 6;
    }

    IEnumerator DayTime()
    {
        for(int i = begin; i <= end; i++)
        {
            for(int j = 0; j < 60; j += 10)
            {
                Debug.Log(i + ", " + j);
                yield return new WaitForSeconds(time);
                if (i <= 12)
                {
                    timeText.text = string.Format(" {0}시 {0}분 ", i, j);
                }
                else
                {
                    timeText.text = string.Format(" {0}시 {0}분 ", i / 12, j);
                }

            }
        }
                Show();
    }

    void Update()
    {
        
    }

    void Timer()
    {
        curTime += Time.deltaTime;
    }

    void Show()
    {
        Time.timeScale = 0;
        GameManager.Instance.CurrentUser.AddDay(1);
        GameManager.Instance.SaveToJson();
        timePanel.OnActive();
        //GameManager.Instance.UIManager.AppearCalulatorPanel();
        curTime = 0;
    }
}
