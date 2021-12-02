using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    private static int tutorialNum;
    [SerializeField] private GameObject TutorialPanel;
    [SerializeField] private GameObject NextBtn;
    [SerializeField] private Text TutorialText;
    [SerializeField] private string[] tutorialText;
    private bool isTutorial = false; //true면 넘어가지 않음, false면 넘어감
    private int tutorialChange = 0;
    private IEnumerator corountine;
    [SerializeField] private GameObject[] UIPanel;
    private bool isEndTutorial = false;
    private void Awake()
    {
       
        TutorialPanel.SetActive(false);
        corountine = Typing();
        if (!GameManager.Instance.CurrentUser.isCompleteTutorial)
        {
            TutorialNumber(0);
            NextBtn.SetActive(false);
            StartTutorial();
        }
        else if (GameManager.Instance.CurrentUser.isCompleteTutorial)
        {
            isTutorial = true;
        }
    }
    public void TutorialNumber(int number)
    {
        tutorialChange = 0;

        switch (number)
        {
            case 0:
                tutorialNum = 0;
                tutorialChange = 4;
                break;
            case 1:
                tutorialNum = 5;
                tutorialChange = 2;
                break;
            case 2:
                tutorialNum = 8;
                break;
            case 3:
                tutorialNum = 9;
                break;
            case 4:
                tutorialNum = 10;
                break;
            case 5:
                tutorialNum = 11;
                break;
            case 6:
                tutorialNum = 12;
                break;
            case 7:
                tutorialNum = 13;
                break;
            case 8:
                tutorialNum = 14;
                break;
            case 9:
                tutorialNum = 15;
                
                tutorialChange = 3;
                break;
        }
        isTutorial = true;
        OnTutorial();
    }
    private bool isTutorialRamen = false;
    private bool isTutorialSoufu = false;
    private bool isTutorialDaefa = false;
    public void TutorialIngredient(string name, bool isTutorial)
    {
        switch(name)
        {
            case "라면사리":
                isTutorialRamen = isTutorial;
                break;
            case "스프":
                isTutorialSoufu = isTutorial;
                break;
            case "대파":
                isTutorialDaefa = isTutorial;
                break;
        }
        TutorialIngredient();
    }
    private void TutorialIngredient()
    {
        if (!isTutorial && isTutorialRamen == true && isTutorialSoufu == true)
        {
            if (isTutorialDaefa == true)
                TutorialNumber(8);
            else
                TutorialNumber(3);
        }
    }
    public bool GetEndTutorial()
    {
        return isEndTutorial;
    }
    public int GetTutorialNum()
    {
        return tutorialNum;
    }
    public bool GetIsTutorial()
    {
        return isTutorial;
    }
    public void SetIsTutorial(bool isTutorial)
    {
        this.isTutorial = isTutorial;
    }
    public void NextButton()
    {
        if (isEndTutorial) return;
        NextBtn.SetActive(false);
        if (tutorialNum == 1)
        {
            GameManager.Instance.GuestMove.StartGoToCounter(true);
        }
        else if (tutorialNum == 2)
        {
            GameManager.Instance.GuestMove.SetIsTutorial(false);
        }
        else if (tutorialNum == 4)
        {
            UIPanel[2].SetActive(true);
        }
        else if (tutorialNum == 9)
            UIPanel[2].SetActive(true);
        else if (tutorialNum == 12)
            UIPanel[1].SetActive(true);
        else if(tutorialNum == 14)
            UIPanel[4].SetActive(true);
        else
            StartTutorial();

        tutorialNum += 1;
        StopCoroutine(Typing());
        TutorialPanel.SetActive(isTutorial);
        if (isTutorial && tutorialChange > 0)
            tutorialChange--;
        else if (isTutorial && tutorialChange == 0)
            isTutorial = false;
        if (!GameManager.Instance.CurrentUser.isCompleteTutorial)
            OnTutorial();
        else
            EndTutorial();
        
    }
    public void OnTutorial()
    {
        TutorialPanel.SetActive(isTutorial);
        StartCoroutine(Typing());
    }
    private void StartTutorial()
    {
        for (int i = 0; i < UIPanel.Length; i++)
        {
            UIPanel[i].SetActive(false);
        }
    }
    public void SkipButton()
    {
        GameManager.Instance.CurrentUser.isCompleteTutorial = true;
        EndTutorial();
    }
    private void EndTutorial()
    {
        StopCoroutine(Typing());
        if (GameManager.Instance.CurrentUser.isCompleteTutorial)
        {
            GameManager.Instance.GuestMove.StartGoToCounter(false);
        }
        for (int i = 0; i < UIPanel.Length; i++)
        {
            UIPanel[i].SetActive(true);
        }
        isEndTutorial = true;
        TutorialPanel.SetActive(false);
    }
    public void OnApplicationQuit()
    {
        if (GameManager.Instance.CurrentUser.isCompleteTutorial)
            GameManager.Instance.CurrentUser.isCompleteTutorial =false;
        else
            GameManager.Instance.CurrentUser.isCompleteTutorial = true;
    }

    IEnumerator Typing()
    {
        yield return new WaitForSeconds(0.05f);
        if (tutorialNum >= tutorialText.Length) yield break;
        for (int i = 0; i <= tutorialText[tutorialNum].Length; i++)
        {
            TutorialText.text = tutorialText[tutorialNum].Substring(0, i);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.2f);
        NextBtn.SetActive(true);
        StopCoroutine(Typing());
    }
}
