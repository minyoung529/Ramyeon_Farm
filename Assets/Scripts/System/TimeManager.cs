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

    void Update()
    {
        Timer();
    }

    private void Start()
    {
        time = second / end - begin / 6;
        //StartCoroutine(dd());
    }

    private IEnumerator dd()
    {
        for (int i = begin; i <= end; i++)
        {
            for (int j = 0; j < 60; j += 10)
            {
                Debug.Log(i + ", " + j);
                yield return new WaitForSeconds(time);
            }
        }
    }
    void Timer()
    {
        curTime += Time.deltaTime;

        if (curTime >= maxTime)
        {
            Show();
            curTime = 0;
        }
    }

    void Show()
    {
        Time.timeScale = 0;
        timePanel.OnActive();
        GameManager.Instance.UIManager.AppearCalculatorPanel();
    }
}
