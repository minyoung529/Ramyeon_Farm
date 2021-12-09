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
    private Door door;

    private bool isStaging = false;

    private bool isTutorial;
    void Start()
    {
        gameObject.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        originScale = spriteRenderer.size;

        transform.position = GameManager.Instance.doorPosition.position - new Vector3(0.4f, 0, 0);
        originPos = transform.localPosition;
        door = GameManager.Instance.doorPosition.GetComponent<Door>();
        delayFadedTime = new WaitForSeconds(phase / (targetSize - 1f) / 2.2f);
        if (GameManager.Instance.CurrentUser.isCompleteTutorial)
            StartGoToCounter(true);
    }
    public void StartGoToCounter(bool isTutorial)
    {
        gameObject.SetActive(true);
        this.isTutorial = isTutorial;
        StartCoroutine(GoToCounter());
    }
    
    private void ToCounter()
    {
        door.OpenDoor();

        GameManager.Instance.QuestManager.AddQuestValue(KeyManager.GUESTQUEST_INDEX, 1);
        spriteRenderer.sprite = GameManager.Instance.GetRandomGuestSprite();
        transform.DOKill();
        transform.DOLocalMove(GameManager.Instance.counterPosition.localPosition, 2f);
        SoundManager.Instance?.DdiringSound();

        
    }

    private IEnumerator GoToCounter()
    {
        ToCounter();

        for (float i = 1f; i < targetSize; i += phase)
        {
            spriteRenderer.size = originScale * i;
            yield return delayFadedTime;
        }

        yield return delay02;

        isStaging = true;

        if(!GameManager.Instance.CurrentUser.isCompleteTutorial)
        {
            yield return StartCoroutine(DelayTutorial());
        }

        GameManager.Instance.UIManager.ShowUpSpeechBubble(true);
    }
    private IEnumerator DelayTutorial()
    {
        if (GameManager.Instance.CurrentUser.isCompleteTutorial) yield break;
        if (!isTutorial)
            StopCoroutine(DelayTutorial());

        while (isTutorial)
        {
            if (GameManager.Instance.CurrentUser.isCompleteTutorial) yield break;
            yield return null;
            if (!isTutorial)
            {
                StopCoroutine(DelayTutorial());
            }
        }
    }
    private void ToDoor()
    {
        GameManager.Instance.UIManager.ShowUpSpeechBubble(false);

        transform.DOKill();
        transform.DOLocalMove(originPos, 2f);
    }
    private IEnumerator Leave()
    {
        isStaging = false;

        ToDoor();

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
        else if (GameManager.Instance.CurrentUser.isCompleteTutorial)
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

    public void SetIsTutorial(bool isTutorial)
    {
        this.isTutorial = isTutorial;
    }
}
