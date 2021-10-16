using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterMove : MonoBehaviour
{
    private float speed = 20f;
    void Start()
    {
        StartCoroutine(GoToCounter());
    }

    private IEnumerator GoToCounter()
    {
        transform.DOMove(GameManager.Instance.doorPosition.position + new Vector3(0.5f, 0f), 2f);
        yield return new WaitForSeconds(2f);
        transform.DOMove(GameManager.Instance.counterPosition.position, 2f);
        transform.DOScale(4f, 2f);
        yield return new WaitForSeconds(2f);
        GameManager.Instance.UIManager.ShowUpSpeechBubble();
    }
}
