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

    [SerializeField] private GameObject inventoryPanelObj;
    private List<PanelBase> inventoryPanels = new List<PanelBase>();

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

    #region Lotto
    [SerializeField] private Text lottoStatusText;
    [SerializeField] private Text lottoRewardText;
    [SerializeField] private ActiveScale lottoPanel;
    [SerializeField] private GameObject blackUI;

    private ParticleSystem lottoParticle;
    #endregion

    [SerializeField] private GameObject quitPanel;
    [SerializeField] private ParticleSystem coinEffect;

    [SerializeField] private GameObject errorPanel;
    private Text errorText;


    EvaluateRamen evaluateRamen = new EvaluateRamen();
    MenuButton menuButton;

    float currenTime = 0f;

    private void Awake()
    {
        screenWidth = Screen.width;
        ingredientPanelWidth = ingredientPanelObj.GetComponent<RectTransform>().sizeDelta.x;

        distanceX = Mathf.Abs(distanceTransform.position.x) * 2f;
        distanceY = Mathf.Abs(distanceTransform.position.y) * 2f;
        errorText = errorPanel.GetComponentInChildren<Text>();
        randomRamen = new RandomRamen();
        menuButton = FindObjectOfType<MenuButton>();
        guest = FindObjectOfType<GuestMove>();
        lottoParticle = lottoPanel.GetComponentInChildren<ParticleSystem>();
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
        InstantiatePanel(GameManager.Instance.GetIngredients().Count, inventoryPanelObj, inventoryPanels, 3);

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

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            quitPanel.SetActive(!quitPanel.activeSelf);
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
        for(int i = 0; i<questPanels.Count; i++)
        {
            questPanels[i].UpdateUI();
        }
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

    public void UpdateInventoryPanel()
    {
        for (int i = 0; i < inventoryPanels.Count; i++)
        {
            inventoryPanels[i].UpdateUI();
        }
    }
    #endregion

    #region Guest
    public void ShowUpSpeechBubble(bool isShow)
    {
        if (isShow)
        {
            SoundManager.Instance?.ButtonSound((int)ButtonSoundType.PopSound);
            speechBubble.transform.localScale = Vector3.zero;
            RandomOrder();
            speechBubble.transform.DOKill();
            speechBubble.transform.DOScale(1f, 0.3f);
        }

        else
        {
            speechBubble.transform.DOKill();
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

        if (price != 0)
        {
            coinEffect.Play();
        }
        Vector2 offset = priceEffectText.transform.position;
        priceEffectText.text = string.Format("+{0}", price);
        guestText.text = evaluateRamen.GetComment();

        priceEffectText.gameObject.SetActive(true);
        priceEffectText.DOKill();
        priceEffectText.transform.DOKill();
        priceEffectText.transform.DOMoveY(0.5f, 1f);
        priceEffectText.DOFade(0f, 1f).OnComplete(() => ResetPriceText(offset));

        evaluateRamen.ResetData();
        SoundManager.Instance?.CoinSound();
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
            stagesUI[i].transform.DOLocalMoveX(stagesUI[i].transform.localPosition.x - 1440, 0.5f).OnComplete(() =>
            {
                isMove = false;
                stagesUI[i].transform.DOKill();
            });
        }

        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.DOMoveX(stagesObj[i].transform.position.x - distanceX, 0.5f).OnComplete(() => stagesObj[i].transform.DOKill());
        }

        SoundManager.Instance?.BoilingWaterSound();
    }

    public void PreviousStage()
    {
        if (isMove) return;
        if (curScreen == 0) return;
        isMove = true;
        curScreen--;
        for (int i = 0; i < stagesUI.Count; i++)
        {
            stagesUI[i].transform.DOLocalMoveX(stagesUI[i].transform.localPosition.x + 1440, 0.5f)
                .OnComplete(() =>
                {
                    isMove = false;
                    stagesUI[i].transform.DOKill();
                });
        }

        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.DOMoveX(stagesObj[i].transform.position.x + distanceX, 0.5f).OnComplete(() => stagesObj[i].transform.DOKill());
        }

        SoundManager.Instance?.BoilingWaterSound();
    }

    public bool isKitchenScene()
    {
        return (curScreen == 1);
    }
    #endregion

    #region MoveContents
    public void PreviousContents(RectTransform rectTransform)
    {
        if (isContentMove) return;
        isContentMove = true;
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + ingredientPanelWidth * 3 + 6 * 3, 0.3f).
            OnComplete(() => isContentMove = false);
    }

    public void NextContents(RectTransform rectTransform)
    {
        if (isContentMove) return;
        isContentMove = true;
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x - ingredientPanelWidth * 3 - 6 * 3, 0.3f).
            OnComplete(() => isContentMove = false);
    }
    #endregion

    #region Check
    public bool CheckIsReward_Quest()
    {
        for (int i = 0; i < questPanels.Count; i++)
        {
            if (questPanels[i].CheckIsUpdate())
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckIsReward_Achievement()
    {
        for (int i = 0; i < achievementPanels.Count; i++)
        {
            if (achievementPanels[i].CheckIsUpdate())
            {
                return true;
            }
        }
        return false;
    }

    public void CheckIsUpdateInMenu()
    {
        menuButton.CheckIsUpdate();
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
    public GuestMove GetGuest()
    {
        return guest;
    }
    #endregion

    #region Message
    private IEnumerator ErrorCoroutine(string info)
    {
        errorText.text = info;
        errorPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        errorPanel.gameObject.SetActive(false);
    }

    public void ErrorMessage(string info)
    {
        StartCoroutine(ErrorCoroutine(info));
    }
    #endregion

    #region Lotto
    public void Lotto()
    {
        if (lottoPanel.gameObject.activeSelf && lottoPanel.transform.localScale == Vector3.one) return;

        int reward;
        int grade;


        if (GameManager.Instance.CurrentUser.GetMoney() < 1000)
        {
            ErrorMessage("돈이 부족합니다.");
            return;
        }

        GameManager.Instance.CurrentUser.AddUserMoney(-1000);
        GameManager.Instance.QuestManager.UpdateAchievement(AchievementType.MiniGame, 1);
        lottoPanel.OnActive();
        blackUI.gameObject.SetActive(true);

        float random = Random.Range(0f, 100f);

        if (random < 0.0007f)
        {
            reward = 5000000;
            grade = 1;
        }

        else if (random < 0.1f)
        {
            reward = 1000000;
            grade = 2;
        }

        else if (random < 2f)
        {
            reward = 50000;
            grade = 3;
        }

        else if (random < 4f)
        {
            reward = 10000;
            grade = 4;
        }

        else if (random < 10f)
        {
            reward = 5000;
            grade = 5;
        }

        else
        {
            grade = -1;
            reward = 0;
        }

        if (grade != -1)
        {
            lottoStatusText.text = string.Format("{0}등 당첨!", grade);
            lottoRewardText.text = string.Format("보상금 +{0}원", reward);
            GameManager.Instance.CurrentUser.AddUserMoney(reward);

            SoundManager.Instance?.ZzaraSound();
        }

        else
        {
            lottoStatusText.text = "꽝입니다!";
            lottoRewardText.text = "";

            SoundManager.Instance?.RewardSound();
        }

        lottoParticle.gameObject.SetActive(grade != -1);
    }
    #endregion
}
