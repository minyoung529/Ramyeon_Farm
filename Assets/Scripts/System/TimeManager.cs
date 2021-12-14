using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private int begin = 7;
    private int end = 22;
    private int time;

    void Start()
    {
        Timer();
    }

    IEnumerator DayTime()
    {
        for(int i = begin; i <= end; i++)
        {
            for(int j = 0; j < 60; j += 10)
            {
                Debug.Log(i + ", " + j);
                yield return new WaitForSeconds(time);
                if(i == 12)
                {
                    
                }
            }
        }
    }

    void Update()
    {
        
    }

    void Timer()
    {

    }

    void Show()
    {
        Time.timeScale = 0;
    }
}
