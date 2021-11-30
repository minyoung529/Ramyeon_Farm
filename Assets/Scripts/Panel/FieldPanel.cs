using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldPanel : IngredientPurchase
{
    [SerializeField] private Button harvestButton;
    [SerializeField] private Sprite seedSprite;
    [SerializeField] private Sprite FieldSprite;
    [SerializeField] private List<Sprite> daepaSprite;
    [SerializeField] private List<Sprite> pepperSprite;
    [SerializeField] private List<Sprite> garlicSprite;
    [SerializeField] private Image seedFieldImage;

    private Ingredient ingredient;
    private int index;
    private Image harvestButtonImage;
    private BuyFirstIngredient firstIngredient;

    float maxTime = 20f;
    float curTime = 0f;

    private bool isHarvest;
    private bool isSeed = true;
    private bool isGrow = false;

    private void Awake()
    {
        seedFieldImage = GetComponent<Image>();
        harvestButtonImage = harvestButton.transform.GetChild(0).GetComponent<Image>();
        harvestButton.onClick.AddListener(() => OnClickIcon());
        firstIngredient = GetComponent<BuyFirstIngredient>();
        curTime = maxTime;
    }
    void Update()
    {
        if (!GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index]) return;
        if (isSeed) return;

        if (!isHarvest)
        {
            curTime += Time.deltaTime;
        }

        if (curTime > maxTime && !isHarvest)
        {
            UpdateHarvestIcon();
        }
        if(isGrow == false && curTime > maxTime * 0.5f)
        {
            FieldChange(1);
            isGrow = true;
        }
    }
    public void SetValue(int index)
    {
        this.index = index;
        firstIngredient.SetValue(index);
        ingredient = GameManager.Instance.GetIngredients()[index];

        maxTime = ingredient.GetMaxTime();
        UpdateUI();
    }

    public void UpdateHarvestIcon()
    {
        isHarvest = true;
        harvestButton.gameObject.SetActive(true);
        harvestButton.transform.DOScale(1f, 0.4f);
        harvestButtonImage.sprite = GameManager.Instance.GetIngredientSprite(index);
        FieldChange(2);
    }

    public void OnClickIcon()
    {
        Ingredient ingredient = GameManager.Instance.GetIngredients()[index];

        if (isHarvest)
        {
            Harvest(ingredient);
        }

        else
        {
            Seed(ingredient);
        }

        SoundManager.Instance?.ButtonSound((int)ButtonSoundType.Bbang);
    }

    public override void UpdateUI()
    {
        harvestButton.gameObject.SetActive(GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index]);
    }

    private void Harvest(Ingredient ingredient)
    {
        isHarvest = false;
        GameManager.Instance.GetIngredients()[index].AddAmount(ingredient.GetAmount());
        GameManager.Instance.UIManager.UpdateIngredientPanel();
        GameManager.Instance.QuestManager.AddQuestValue(KeyManager.FARMQUEST_INDEX, 1);

        harvestButtonImage.sprite = seedSprite;
        seedFieldImage.sprite = FieldSprite;
        isGrow = false;
        isSeed = true;
    }

    private void Seed(Ingredient ingredient)
    {
        int price = ingredient.GetPrice() / 8;

        if (price > GameManager.Instance.CurrentUser.GetMoney())
        {
            GameManager.Instance.UIManager.ErrorMessage("돈이 부족합니다.");
            return;
        }
        GameManager.Instance.CurrentUser.AddUserMoney(-price);
        curTime = 0f;
        harvestButtonImage.sprite = seedSprite;
        harvestButton.transform.DOScale(0f, 0.2f);
        isSeed = false;
        FieldChange(0);
    }
    void FieldChange(int index)
    {
        switch(ingredient.name) 
        {
            case "마늘":
                seedFieldImage.sprite = garlicSprite[index];
                break;
            case "대파":
                seedFieldImage.sprite = daepaSprite[index];
                break;
            case "청양고추":
                seedFieldImage.sprite = pepperSprite[index];
                break;
            default:

                break;
        }
    }
}
