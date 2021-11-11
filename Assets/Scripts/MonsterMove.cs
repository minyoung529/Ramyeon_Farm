using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterMove : MonoBehaviour
{
    private Vector2 originPos;
    private Vector2 scale;

    void Start()
    {
        scale = transform.localScale;
        transform.position = GameManager.Instance.doorPosition.position;
        originPos = transform.localPosition;
        StartCoroutine(GoToCounter());
    }

    private IEnumerator GoToCounter()
    {
        transform.DOLocalMove(GameManager.Instance.doorPosition.localPosition, 1f);
        yield return new WaitForSeconds(1f);
        transform.DOLocalMove(GameManager.Instance.counterPosition.localPosition, 2f);
        transform.DOScale(scale * 2f, 2f);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.UIManager.ShowUpSpeechBubble();
    }
}
