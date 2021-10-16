using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image speechBubble;
    [SerializeField] private List<Ingredient> ingredients;

    void Start()
    {
        
    }

    public void ShowUpSpeechBubble()
    {
        speechBubble.transform.DOScale(0f, 0f);
        speechBubble.transform.DOScale(1f, 0.3f);
    }
}
