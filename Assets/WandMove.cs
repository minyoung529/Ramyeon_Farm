using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WandMove : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform lever;
    private RectTransform rectTransform;
    [SerializeField] float leverRange = 10f;
    private void Awake()
    {
        lever = GetComponent<RectTransform>();
        rectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 inputDir = eventData.position - rectTransform.anchoredPosition;
        Vector2 clampedDir = inputDir.magnitude < leverRange ? inputDir : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;
    }
}