using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    #region 텍스트
    [Header("텍스트")]
    [SerializeField] private Text questTimeText;
    [SerializeField] private Text priceEffectText;
    #endregion

    #region 프로필
    [SerializeField] private Text moneyText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image playerProfile;
    [SerializeField] private Slider levelSlider;

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

    [SerializeField] private GameObject bookPanelObj;
    private List<PanelBase> bookPanels = new List<PanelBase>();

    [SerializeField] private GameObject achievementPanelObj;
    private List<PanelBase> achievementPanels = new List<PanelBase>();

    [SerializeField] private GameObject ingredientUpgradePanelObj;
    private List<PanelBase> ingredientUpgradePanels = new List<PanelBase>();

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

    private int curScreen = 0;
    #endregion
    #region Guest
    [Header("손님")]
    [SerializeField] private Text guestText;
    [SerializeField] private Image speechBubble;
    private GuestMove guest;
    RandomRamen randomRamen;
    #endregion

    [SerializeField] private GameObject quitPanel;
    [SerializeField] private ParticleSystem coinEffect;

    EvaluateRamen evaluateRamen = new EvaluateRamen();

    float currenTime = 0f;

    private void Awake()
    {
        screenWidth = Screen.width;
        ingredientPanelWidth = ingredientPanelObj.GetComponent<RectTransform>().sizeDelta.x;

        distanceX = Mathf.Abs(distanceTransform.position.x) * 2f;
        distanceY = Mathf.Abs(distanceTransform.position.y) * 2f;

        randomRamen = new RandomRamen();
        guest = FindObjectOfType<GuestMove>();
        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.position = new Vector2(distanceX * i, 0);
        }
    }

    void Start()
    {
        InstantiateIngredientPanel();
        InstantiateFarmPanel(fieldPanelObj, IngredientType.vegetable);

        InstantiatePanel(KeyManager.QUEST_COUNT, questPanelObj, questPanels);
        InstantiatePanel(GameManager.Instance.QuestManager.GetAchievements().Count, achievementPanelObj, achievementPanels);
        InstantiatePanel(GameManager.Instance.GetIngredients().Count, ingredientUpgradePanelObj, ingredientUpgradePanels, 3);

        int max = Mathf.Max(GameManager.Instance.GetIngredients().Count, GameManager.Instance.GetRecipes().Count);
        InstantiatePanel(max, bookPanelObj, bookPanels);
        UpdateMoneyText();
    }
    private void Update()
    {
        currenTime += Time.deltaTime;

        if (currenTime > 1f)
        {
            QuestTimeText();
            currenTime = 0f;
        }

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            quitPanel.SetActive(true);
        }
    }

    public void QuestTimeText()
    {
        GameManager.Instance.QuestManager.ResetQuest();
        questTimeText.text = string.Format("{0}시간 {1}분 {2}초", (23 - DateTime.Now.Hour), (59 - DateTime.Now.Minute), (59 - DateTime.Now.Second));
    }

    public void UpdateMoneyText()
    {
        moneyText.text = string.Format("{0}원", GameManager.Instance.CurrentUser.GetMoney());
    }

    #region InstnatiateUIPanel
    public void InstantiateIngredientPanel()
    {
        for (int i = 3; i < GameManager.Instance.GetIngredients().Count; i++)
        {
            GameObject obj = Instantiate(ingredientPanelObj, ingredientPanelObj.transform.parent);
            IngredientPanel igdP = obj.GetComponent<IngredientPanel>();
            ingredientPanels.Add(igdP);
            igdP.SetValue(i);
        }

        ingredientPanelObj.SetActive(false);
    }
    public void InstantiateFarmPanel(GameObject template, IngredientType state)
    {
        List<Ingredient> ingredients = GameManager.Instance.GetIngredients();

        for (int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i].type == state)
            {
                GameObject obj = Instantiate(template, template.transform.parent);
                FieldPanel fieldPanel = obj.GetComponent<FieldPanel>();
                fieldPanels.Add(fieldPanel);
                fieldPanel.SetValue(i);
            }
        }

        template.SetActive(false);
    }
    public void InstantiatePanel(int count, GameObject template, List<PanelBase> panels, int start = 0)
    {
        GameManager.Instance.CurrentUser.CheckCurrentQuest();

        for (int i = start; i < count; i++)
        {
            GameObject obj = Instantiate(template, template.transform.parent);
            PanelBase panel = obj.GetComponent<PanelBase>();

            #region Quest
            if (panels == questPanels)
            {
                panel.SetValue(GameManager.Instance.CurrentUser.questList[i].index);
                panels.Add(panel);
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
        for (int i = 0; i < ingredientPanels.Count; i++)
        {
            ingredientPanels[i].UpdateData();
        }
    }

    public void UpdateQuestPanel(int index)
    {
        if (questPanels.Count == 0) return;
        questPanels[index].UpdateUI();
    }

    public void ResetQuestPanelData()
    {
        for (int i = 0; i < KeyManager.QUEST_COUNT; i++)
        {
            questPanels[i].SetValue(GameManager.Instance.CurrentUser.questList[i].index);
        }
    }

    public void UpdateAchievementPanel()
    {
        for (int i = 0; i < achievementPanels.Count; i++)
        {
            achievementPanels[i].UpdateUI();
        }
    }

    public void UpdateBookPanel(int num)
    {
        for (int i = 0; i < bookPanels.Count; i++)
        {
            bookPanels[i].SetState((BookType)num);
            bookPanels[i].SetValue(i);
        }

        bookPanels[0].OnClick();
    }

    public void UpdateIngredientUpgradePanel()
    {
        for (int i = 0; i < ingredientUpgradePanels.Count; i++)
        {
            ingredientUpgradePanels[i].UpdateUI();
        }
    }
    #endregion

    #region Guest
    public void ShowUpSpeechBubble(bool isShow)
    {
        if (isShow)
        {
            speechBubble.transform.DOScale(0f, 0f);
            RandomOrder();
            speechBubble.transform.DOScale(1f, 0.3f);
        }

        else
        {
            speechBubble.transform.DOScale(0f, 0.3f);
        }
    }

    public void OnClickAccept()
    {
        GameManager.Instance.QuestManager.AddQuestValue(KeyManager.GUESTQUEST_INDEX, 1);
    }

    private void RandomOrder()
    {
        guestText.text = randomRamen.GetRandomGuestComment();
    }

    public void EvaluateCurrentRamen()
    {
        GameManager.Instance.QuestManager.AddQuestValue(KeyManager.COOKQUESST_INDEX, 1);

        int price = evaluateRamen.GetRamenPrice();
        GameManager.Instance.CurrentUser.AddUserMoney(price);
        GameManager.Instance.QuestManager.UpdateAchievement(AchievementType.Cook, 1);
        StartCoroutine(PriceTextEffect(price));

        guest.StartLeave();
    }

    private IEnumerator PriceTextEffect(int price)
    {
        yield return new WaitForSeconds(0.5f);

        if(price != 0)
        {
            coinEffect.Play();
        }
        Vector2 offset = priceEffectText.transform.position;
        priceEffectText.text = string.Format("+{0}", price);

        guestText.text = evaluateRamen.GetComment();

        priceEffectText.gameObject.SetActive(true);
        priceEffectText.transform.DOMoveY(0.5f, 1f);
        priceEffectText.DOFade(0f, 1f).OnComplete(() => ResetPriceText(offset));

        evaluateRamen.ResetData();
    }

    private void ResetPriceText(Vector2 offset)
    {
        priceEffectText.gameObject.SetActive(false);
        priceEffectText.transform.position = offset;
        priceEffectText.color = Color.black;
    }
    #endregion

    #region MoveScreen
    public void NextStage()
    {
        if (isMove) return;
        if (curScreen == stagesUI.Count - 1) return;

        isMove = true;
        curScreen++;
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
        if (curScreen == 0) return;
        isMove = true;
        curScreen--;
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
