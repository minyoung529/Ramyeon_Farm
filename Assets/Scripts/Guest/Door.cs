using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    private float scaleX;
    private float scaleY;

    private bool isOpen = false;

    private void Start()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(KeyManager.GUEST_TAG))
        {
            if (isOpen) return;
            isOpen = true;
            transform.DOScale(new Vector2(0f, scaleY), 1.2f).OnComplete(() => transform.DOScale(new Vector2(scaleX, scaleY), 0.5f));
        }
    }

}