using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActiveScale : MonoBehaviour
{
    public void OnActive()
    {
        gameObject.SetActive(true);
        transform.DOScale(1f, 0.3f);
    }

    public void OnEnactive()
    {
        gameObject.SetActive(false);
        transform.DOScale(0f, 0f);
    }
}
