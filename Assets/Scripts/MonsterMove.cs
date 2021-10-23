using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterMove : MonoBehaviour
{
    private float speed = 20f;
    private Vector2 originPos;
    private Vector2 scale;

    void Start()
    {
        scale = transform.localScale;
        transform.position = GameManager.Instance.doorPosition.position;
        originPos = transform.position;
        StartCoroutine(GoToCounter());
    }

    private IEnumerator GoToCounter()
    {
        transform.DOMove(GameManager.Instance.doorPosition.position, 1f);
        yield return new WaitForSeconds(1f);
        transform.DOMove(GameManager.Instance.counterPosition.position, 2f);
        transform.DOScale(scale * 2f, 2f);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.UIManager.ShowUpSpeechBubble();
    }
}
