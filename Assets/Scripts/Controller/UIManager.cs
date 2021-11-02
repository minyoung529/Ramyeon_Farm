using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    #region 텍스트
    [Header("텍스트")]
    [SerializeField] private Text moneyText;
    [SerializeField] private Text questTimeText;
    #endregion
    #region ContentPanels
    [Header("패널")]
    [SerializeField] private GameObject ingredientPanelObj;
    private List<IngredientPanel> ingredientPanels = new List<IngredientPanel>();
    private float ingredientPanelWidth;

    [SerializeField] private GameObject fieldPanelObj;
    private List<FieldPanel> fieldPanels = new List<FieldPanel>();

    [SerializeField] private GameObject questPanelObj;
    private List<PanelBase> questPanels = new List<PanelBase>();
    private bool isContentMove;
    #endregion
    #region ScreenMoving
    [Header("스크린 무빙")]
    [SerializeField] private List<GameObject> stagesUI;
    [SerializeField] private List<GameObject> stagesObj;
    [SerializeField] Transform distanceTransform;

    private bool isMove;

    private float screenWidth;
    private float distanceX;
    private float distanceY;
    #endregion
    #region Guest
    [Header("손님")]
    [SerializeField] private Text guestText;
    [SerializeField] private Image speechBubble;
    #endregion

    float currenTime = 0f;

    private void Awake()
    {
        screenWidth = Screen.width;
        distanceX = Mathf.Abs(distanceTransform.position.x) * 2f;
        distanceY = Mathf.Abs(distanceTransform.position.y) * 2f;
    }

    private void Update()
    {
        currenTime += Time.deltaTime;

        if (currenTime > 1f)
        {
            QuestTimeText();
            currenTime = 0f;
        }
    }

    public void QuestTimeText()
    {
        GameManager.Instance.QuestManager.CheckNextDay();
        questTimeText.text = string.Format("{0}시간 {1}분 {2}초", (24 - DateTime.Now.Hour), (60 - DateTime.Now.Minute), (60 - DateTime.Now.Second));
    }

    public void UpdateMoneyText()
    {
        moneyText.text = string.Format("{0}원", GameManager.Instance.CurrentUser.GetMoney());
    }

    void Start()
    {
        ingredientPanelWidth = ingredientPanelObj.GetComponent<RectTransform>().sizeDelta.x;

        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.position = new Vector2(distanceX * i, 0);
        }

        InstantiateIngredientPanel();
        InstantiateFarmPanel(fieldPanelObj, IngredientState.vegetable);

        InstantiatePanel(3, questPanelObj, questPanels);
    }

    #region InstnatiateUIPanel
    public void InstantiateIngredientPanel()
    {
        for (int i = 3; i < GameManager.Instance.CurrentUser.ingredients.Count; i++)
        {
            GameObject obj = Instantiate(ingredientPanelObj, ingredientPanelObj.transform.parent);
            IngredientPanel igdP = obj.GetComponent<IngredientPanel>();
            ingredientPanels.Add(igdP);
            igdP.SetValue(i);
        }

        ingredientPanelObj.SetActive(false);
    }
    public void InstantiateFarmPanel(GameObject template, IngredientState state)
    {
        List<Ingredient> ingredients = GameManager.Instance.CurrentUser.ingredients;

        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].state == state)
            {
                GameObject obj = Instantiate(template, template.transform.parent);
                FieldPanel fieldPanel = obj.GetComponent<FieldPanel>();
                fieldPanels.Add(fieldPanel);
                fieldPanel.SetValue(i);
            }
        }

        template.SetActive(false);
    }
    public void InstantiatePanel(int count, GameObject template, List<PanelBase> panels)
    {

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(template, template.transform.parent);
            PanelBase panel = obj.GetComponent<PanelBase>();

            #region Quest
            
            if (panels == questPanels)
            {

                //questIndex[i] = randomIndex;
                //panel.SetValue(randomIndex);
                //panels.Add(panel);
                continue;
            }
            #endregion

            panel.SetValue(i);
            panels.Add(panel);
        }

        template.SetActive(false);
    }

    #endregion

    #region RefreshData
    public void UpdateIngredientPanel()
    {
        foreach (IngredientPanel panel in ingredientPanels)
        {
            panel.UpdateData();
        }
    }

    public void UpdateQuestPanel()
    {
        foreach (PanelBase panel in questPanels)
        {
            panel.UpdateUI();
        }
    }
    #endregion

    #region Guest
    public void ShowUpSpeechBubble()
    {
        speechBubble.transform.DOScale(0f, 0f);
        RandomOrder();
        speechBubble.transform.DOScale(1f, 0.3f);
    }

    public void OnClickAccept()
    {
        GameManager.Instance.QuestManager.AddQuestValue(GameManager.Instance.QuestManager.guestQuest, 1);
    }

    private void RandomOrder()
    {
        List<Recipe> recipes = GameManager.Instance.CurrentUser.recipes;
        Recipe recipe = recipes[Random.Range(0, recipes.Count)];
        GameManager.Instance.SetCurrentRecipe(recipe);
        guestText.text = string.Format("{0}이 땡기는 날인데...", recipe.recipeName);
    }

    public void EvaluateCurrentRamen()
    {
        PreviousStage();

        float score = GameManager.Instance.EvaluateRamen(GameManager.Instance.GetRecipe().recipeName);

        if (score > 99f)
        {
            guestText.text = "완벽해요!";
        }

        else if (score > 59f)
        {
            guestText.text = "그냥 먹을게요";
        }

        else if (score > 0)
        {
            guestText.text = "이게뭐죠? 참...";
        }

        else
        {
            guestText.text = "뭘 먹으라고 준거야!! 당장 신고야!!";
        }
    }

    private bool CheckCurrentQuest(int index)
    {
        if (GameManager.Instance.QuestManager.GetNextDay()) return false;

        int[] questIndex = GameManager.Instance.CurrentUser.questIndex;
        int randomIndex = Random.Range(0, GameManager.Instance.CurrentUser.questList.Count);

        for (int j = 0; j < questIndex.Length; j++)
        {
            if (questIndex[j] == randomIndex)
            {
                randomIndex = Random.Range(0, GameManager.Instance.CurrentUser.questList.Count);
                j = 0;
            }

            else
            { }
            questIndex[j] = randomIndex;
        }

        return false;
    }
    #endregion

    #region MoveScreen
    public void NextStage()
    {
        if (isMove) return;

        isMove = true;

        for (int i = 0; i < stagesUI.Count; i++)
        {
            stagesUI[i].transform.DOLocalMoveX(stagesUI[i].transform.localPosition.x - 1440, 0.5f).OnComplete(() => isMove = false);
        }

        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.DOMoveX(stagesObj[i].transform.position.x - distanceX, 0.5f);
        }
    }

    public void PreviousStage()
    {
        if (isMove) return;

        isMove = true;

        for (int i = 0; i < stagesUI.Count; i++)
        {
            stagesUI[i].transform.DOLocalMoveX(stagesUI[i].transform.localPosition.x + 1440, 0.5f).OnComplete(() => isMove = false);
        }

        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.DOMoveX(stagesObj[i].transform.position.x + distanceX, 0.5f);
        }
    }
    #endregion

    #region MoveContents
    public void PreviousContents(RectTransform rectTransform)
    {
        if (isContentMove) return;
        isContentMove = true;
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + ingredientPanelWidth * 3 + 49 * 3, 0.3f).
            OnComplete(() => isContentMove = false);
    }

    public void NextContents(RectTransform rectTransform)
    {
        if (isContentMove) return;
        isContentMove = true;
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x - ingredientPanelWidth * 3 - 49 * 3, 0.3f).
            OnComplete(() => isContentMove = false);
    }
    #endregion

    #region GetSet
    public float DistanceX()
    {
        return distanceX;
    }

    public float DistanceY()
    {
        return distanceY;
    }
    #endregion
}
