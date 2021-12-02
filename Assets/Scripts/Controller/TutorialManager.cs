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
    private IEnumerator Corountine;
    [SerializeField] private GameObject[] UIPanel;
    private void Awake()
    {
        tutorialNum = PlayerPrefs.GetInt("Tutorial", 0);
        TutorialPanel.SetActive(false);
        Corountine = Typing();
        TutorialNumber(0);
        if (tutorialNum != 19)
        {
            OnTutorial();
            NextBtn.SetActive(false);
            StartTutorial();
        }
        else if (tutorialNum == 19)
            SetIsTutorial(true);
    }
    public void TutorialNumber(int number)
    {
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
                tutorialChange = 0;
                break;
            case 3:
                tutorialNum = 9;
                tutorialChange = 0;
                break;
            case 4:
                tutorialNum = 10;
                tutorialChange = 0;
                break;

        }
        isTutorial = true;
        OnTutorial();
    }
    private bool isTutorialRamen = false;
    private bool isTutorialSoufu = false;
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
        }
        TutorialIngredient();
    }
    private void TutorialIngredient()
    {
        if (!isTutorial && isTutorialRamen == true && isTutorialSoufu == true)
        {
            TutorialNumber(3);
        }
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
        NextBtn.SetActive(false);
        if (tutorialNum == 1)
        {
            GameManager.Instance.GuestMove.StartGoToCounter();
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
        else if (tutorialNum == 19)
            EndTutorial();
        else
            StartTutorial();
        tutorialNum += 1;

        Debug.Log(tutorialNum);

        if (isTutorial && tutorialChange > 0)
            tutorialChange--;
        else if (isTutorial && tutorialChange == 0)
            isTutorial = false;

        StopCoroutine(Typing());
        OnTutorial();
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
    private void EndTutorial()
    {
        for (int i = 0; i < UIPanel.Length; i++)
        {
            UIPanel[i].SetActive(true);
        }
    }
    public void OnApplicationQuit()
    {
        if (tutorialNum < 20)
            PlayerPrefs.SetInt("Tutorial", 0);
        else
            PlayerPrefs.SetInt("Tutorial", 19);
    }

    IEnumerator Typing()
    {
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i <= tutorialText[tutorialNum].Length; i++)
        {
            TutorialText.text = tutorialText[tutorialNum].Substring(0, i);
            yield return new WaitForSeconds(0.08f);
        }
        yield return new WaitForSeconds(0.2f);
        NextBtn.SetActive(true);
        StopCoroutine(Typing());
    }
}
