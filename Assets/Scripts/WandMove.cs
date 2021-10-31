using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WandMove : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform lever;
    private RectTransform rectTransform;

    [SerializeField] float leverRange = 10f;
    [SerializeField] Text maxCountText;
    [SerializeField] int maxCount;

    private void Awake()
    {
        lever = GetComponent<RectTransform>();
        rectTransform = transform.parent.GetComponent<RectTransform>();
        maxCount = Random.Range(5, 10);
        maxCountText.text = maxCount.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 inputDir = eventData.position - rectTransform.anchoredPosition;
        Vector2 clampedDir = inputDir.magnitude == leverRange ? inputDir : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;
        Debug.Log(lever.transform.position.x);
        if (lever.transform.position.x > -0.2f && lever.transform.position.x < 0f && lever.transform.position.y>0)
        {
            maxCount--;
            maxCountText.text = maxCount.ToString();
        }
    }
}