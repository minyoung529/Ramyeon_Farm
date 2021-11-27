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
        transform.DOScale(new Vector3(0f, 1f, 0f), 0f);
        SoundManager.Instance?.ButtonSound((int)ButtonSoundType.CloseSound);
    }
}
