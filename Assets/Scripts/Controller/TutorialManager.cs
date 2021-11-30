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
    [SerializeField] private RectTransform[] tutorialDisplay;
    private bool isTutorialDisplay = false;
    private void Awake()
    {
        tutorialNum = PlayerPrefs.GetInt("Tutorial", 0);
        TutorialPanel.SetActive(false);
        Corountine = Typing();
        NextBtn.SetActive(false);
        
        if (tutorialNum == 0)
        {
            tutorialChange = 5;
        }
        if(tutorialNum!=19)
            OnTutorial();
        else if(tutorialNum == 19)
            SetIsTutorial(true);
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
        else if(tutorialNum == 4)
        {
        }
        tutorialNum += 1;
        
        Debug.Log(tutorialNum);

        if (isTutorial && tutorialChange > 0)
            tutorialChange--;

        if (isTutorial && tutorialChange == 0)
            isTutorial = false;

        StopCoroutine(Typing());
        OnTutorial();
        if (!isTutorial) TutorialPanel.SetActive(false);
    }
    public void OnTutorial()
    {
        TutorialPanel.SetActive(true);
        StartCoroutine(Typing());
    }
    
    public void OnApplicationQuit()
    {
        //PlayerPrefs.SetInt("Tutorial", tutorialNum);  //이거 써야함
        PlayerPrefs.SetInt("Tutorial", 0);
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
