using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory : IngredientPurchase
{
    [SerializeField] Button harvestButton;
    [SerializeField] Image harvestButtonImage;
    [SerializeField] Sprite seedSprite;
    private BuyFirstIngredient firstIngredient;
    private ParticleSystem particle;

    float curTime = 0f;

    private bool isHarvest = false;
    private bool isSeed = true;

    [SerializeField] private string ingredientName;
    void Start()
    {
        ingredient = GameManager.Instance.GetIngredients().Find(x => x.GetIndex() == index);
        harvestButton.transform.position = transform.position + new Vector3(0f, 0.5f, 0f);

        harvestButton.onClick.AddListener(() => OnClickIcon());
        particle = GetComponentInChildren<ParticleSystem>();
        particle.gameObject.SetActive(false);
        index = GameManager.Instance.GetIngredients().Find(x => x.name == ingredientName).GetIndex();
        UpdateUI();
    }

    void Update()
    {
        if (!IsHave()) return;
        if (isSeed) return;
        if (isHarvest) return;

        if (curTime < ingredient.maxTime)
        {
            curTime += Time.deltaTime;
        }

        if (curTime >= ingredient.maxTime)
        {
            UpdateHarvestIcon();
        }
    }

    private void UpdateHarvestIcon()
    {
        Debug.Log("sdf");
        harvestButtonImage.color = Color.white;
        harvestButtonImage.sprite = GameManager.Instance.GetIngredientSprite(index);
        harvestButton.gameObject.SetActive(true);
        isHarvest = true;
    }
    public void OnClickIcon()
    {
        if (isHarvest)
        {
            Harvest();
        }

        else
        {
            Seed();
        }

        SoundManager.Instance?.ButtonSound((int)ButtonSoundType.Bbang);
    }

    public override void UpdateUI()
    {
        harvestButton.gameObject.SetActive(IsHave());
    }

    private void Harvest()
    {
        particle.gameObject.SetActive(false);
        harvestButtonImage.sprite = seedSprite;
        harvestButtonImage.color = Color.black;
        GameManager.Instance.CurrentUser.AddIngredientsAmounts(index, ingredient.GetAmount());

        isHarvest = false;
        isSeed = true;
    }
    private void Seed()
    {
        int price = ingredient.GetPrice() / 8;

        if (price > GameManager.Instance.CurrentUser.GetMoney())
        {
            GameManager.Instance.UIManager.ErrorMessage("돈이 부족합니다.");
            return;
        }

        GameManager.Instance.CurrentUser.AddUserMoney(-price);
        GameManager.Instance.AddDatasOfDay(DataOfDay.SpentIngredientMoney, price);

        curTime = 0f;
        harvestButton.gameObject.SetActive(false);
        particle.gameObject.SetActive(true);
        isSeed = false;
    }
    private bool IsHave()
    {
        return (GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index]);
    }
}
