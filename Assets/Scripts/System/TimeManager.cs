using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float curTime = 0;
    [SerializeField] private float maxTime = 20f;
    [SerializeField] GameObject TenM;

    void Update()
    {
        Timer();
    }


    void Timer()
    {
        curTime += Time.deltaTime;
        if(curTime >= maxTime)
        {
            Show();
            curTime = 0;
        }
    }

    void Show()
    {
        Time.timeScale = 0;
        TenM.SetActive(true);
    }
}
