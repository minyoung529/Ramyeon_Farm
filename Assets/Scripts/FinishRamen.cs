using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FinishRamen : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!GameManager.Instance.TutorialManager.GetIsTutorial() && GameManager.Instance.TutorialManager.GetTutorialNum() == 15)
        {
            GameManager.Instance.TutorialManager.TutorialNumber(9);
        }

        if (!GameManager.Instance.UIManager.GetGuest().GetIsStaging())
        {
            GameManager.Instance.UIManager.ErrorMessage("���� �մ��� �����ϴ�.");
            return;
        }

        GameManager.Instance.UIManager.EvaluateCurrentRamen();
        
        GameManager.Instance.GetPot().ResetPot();
        GameManager.Instance.UIManager.PreviousStage();
    }
}
