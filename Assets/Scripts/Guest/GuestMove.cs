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

    private bool isStaging = false;

    private IEnumerator Corountine;
    
    private bool isTutorial;
    void Start()
    {
        gameObject.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        originScale = spriteRenderer.size;
        Corountine = DelayTutorial();

        transform.position = GameManager.Instance.doorPosition.position - new Vector3(0.4f, 0, 0);
        originPos = transform.localPosition;

        delayFadedTime = new WaitForSeconds(phase / (targetSize - 1f) / 2.2f);
    }
    public void StartGoToCounter(bool isTutorial)
    {
        gameObject.SetActive(true);
        this.isTutorial = isTutorial;
        StartCoroutine(GoToCounter());
    }
    public void SetIsTutorial(bool isTutorial)
    {
        this.isTutorial = isTutorial;
    }
    private IEnumerator GoToCounter()
    {
        GameManager.Instance.QuestManager.AddQuestValue(KeyManager.GUESTQUEST_INDEX, 1);
        spriteRenderer.sprite = GameManager.Instance.GetRandomGuestSprite();
        transform.DOKill();
        transform.DOLocalMove(GameManager.Instance.counterPosition.localPosition, 2f);
        SoundManager.Instance?.DdiringSound();

        for (float i = 1f; i < targetSize; i += phase)
        {
            spriteRenderer.size = originScale * i;
            yield return delayFadedTime;
        }

        yield return delay02;
        isStaging = true;
        yield return StartCoroutine(DelayTutorial());
        GameManager.Instance.UIManager.ShowUpSpeechBubble(true);
    }
    private IEnumerator DelayTutorial()
    {
        if (!isTutorial)
            StopCoroutine(DelayTutorial());

        while (isTutorial)
        {
            yield return null;
            if (!isTutorial)
            {
                StopCoroutine(DelayTutorial());
            }
        }
    }
    private IEnumerator Leave()
    {
        isStaging = false;

        yield return delay02;
        GameManager.Instance.UIManager.ShowUpSpeechBubble(false);

        transform.DOKill();
        transform.DOLocalMove(originPos, 2f);

        for (float i = targetSize; i > 1f; i -= phase)
        {
            spriteRenderer.size = originScale * i;
            yield return delayFadedTime;
        }

        yield return new WaitForSeconds(Random.Range(3f, 5f));

        if(GameManager.Instance.TutorialManager.GetEndTutorial())
        {
            StartCoroutine(GoToCounter());
        }
    }

    public void StartLeave()
    {
        StartCoroutine(Leave());
    }

    public bool GetIsStaging()
    {
        return isStaging;
    }
}
