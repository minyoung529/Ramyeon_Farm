using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldPanel : IngredientPurchase
{
    [SerializeField] private Button harvestButton;
    [SerializeField] private Sprite seedSprite;

    private int index;
    private Image harvestButtonImage;
    private BuyFirstIngredient firstIngredient;

    float maxTime = 20f;
    float curTime = 0f;

    private bool isHarvest;
    private bool isSeed = true;

    private void Awake()
    {
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
    }
    public void SetValue(int index)
    {
        this.index = index;
        firstIngredient.SetValue(index);
        Ingredient ingredient = GameManager.Instance.GetIngredients()[index];

        maxTime = ingredient.GetMaxTime();
        UpdateUI();
    }

    public void UpdateHarvestIcon()
    {
        isHarvest = true;
        harvestButton.gameObject.SetActive(true);
        harvestButton.transform.DOScale(1f, 0.4f);
        harvestButtonImage.sprite = GameManager.Instance.GetIngredientSprite(index);
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
    }
}
