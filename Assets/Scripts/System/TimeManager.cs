using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float curTime = 0;
    [SerializeField] private float maxTime = 60f;
    [SerializeField] ActiveScale timePanel;

    int begin = 9;
    int end = 16;
    float second = 30;
    float time;

    private void Start()
    {
        time = second / end - begin / 6;
        //StartCoroutine(dd());
    }

    void Show()
    {
        Time.timeScale = 0;
        GameManager.Instance.UIManager.UpdateDayText();
        timePanel.OnActive();
        GameManager.Instance.CurrentUser.AddDay();
        GameManager.Instance.SaveToJson();
        GameManager.Instance.UIManager.AppearCalculatorPanel();
    }
}
