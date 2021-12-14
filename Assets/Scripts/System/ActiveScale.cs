using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActiveScale : MonoBehaviour
{
    public void OnActive()
    {
        gameObject.SetActive(true);
        transform.DOScale(1f, 0.3f).OnComplete(() => transform.DOKill());
    }

    public void OnInactive()
    {
        gameObject.SetActive(false);
        transform.localScale = Vector3.zero;
        SoundManager.Instance?.ButtonSound((int)ButtonSoundType.CloseSound);
    }
}
