using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GuestMove : MonoBehaviour
{
    private Vector2 originPos;
    private Vector2 originScale;
    private SpriteRenderer spriteRenderer;

    private float targetSize = 1.7f;
    private float phase = 0.025f;

    private WaitForSeconds delay02 = new WaitForSeconds(2f);
    private WaitForSeconds delayFadedTime = new WaitForSeconds(2f);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originScale = spriteRenderer.size;

        transform.position = GameManager.Instance.doorPosition.position;
        originPos = transform.localPosition;

        delayFadedTime = new WaitForSeconds(phase / (targetSize - 1f) / 2.2f);

        StartCoroutine(GoToCounter());
    }

    private IEnumerator GoToCounter()
    {
        transform.DOLocalMove(GameManager.Instance.counterPosition.localPosition, 2f);

        for (float i = 1f; i < targetSize; i += phase)
        {
            spriteRenderer.size = originScale * i;
            yield return delayFadedTime;
        }

        yield return delay02;
        GameManager.Instance.UIManager.ShowUpSpeechBubble();
    }

    private IEnumerator Leave()
    {
        yield return delay02;
        transform.DOLocalMove(GameManager.Instance.doorPosition.localPosition, 2f);

        for (float i = targetSize; i > 1f; i -= phase)
        {
            spriteRenderer.size = originScale * i;
            yield return delayFadedTime;
        }

        yield return delay02;
        GameManager.Instance.UIManager.ShowUpSpeechBubble();
    }

    public void StartLeave()
    {
        StartCoroutine(Leave());
    }
}
