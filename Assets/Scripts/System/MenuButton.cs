using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuButton : MonoBehaviour
{
    private bool isActive = true;
    private Button button;
    private List<RectTransform> childTransforms = new List<RectTransform>();

    private WaitForSeconds delay01 = new WaitForSeconds(0.1f);

    private void Start()
    {
        button = GetComponent<Button>();

        for (int i = 0; i < transform.childCount; i++)
        {
            childTransforms.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }

        button.onClick.AddListener(() => OnClick(isActive));
    }

    private void OnClick(bool active)
    {
        if (isActive)
        {
            StartCoroutine(SetMenu(isActive));
        }

        else
        {
            StartCoroutine(SetMenu(isActive));
        }

        isActive = !isActive;
    }

    private IEnumerator SetMenu(bool isActive)
    {
        float adjust = 0;
        const float increment = -160f;
        const float offset = -200f;

        for (int i = 0; i < childTransforms.Count; i++)
        {
            if (isActive)
            {
                childTransforms[i].DOAnchorPos(Vector2.zero, 0.2f);
            }

            else
            {
                childTransforms[i].DOAnchorPosY(offset + adjust, 0.2f);
            }
            yield return delay01;
            childTransforms[i].gameObject.SetActive(!isActive);
            adjust += increment;
        }
    }
}