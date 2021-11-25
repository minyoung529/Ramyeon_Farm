using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory : IngredientPurchase
{
    [SerializeField] private int index;
    [SerializeField] Button harvestButton;
    private Ingredient ingredient;
    private BuyFirstIngredient firstIngredient;
    private ParticleSystem particle;

    float curTime = 0f;

    private bool isHarvest = false;

    void Start()
    {
        ingredient = GameManager.Instance.GetIngredients().Find(x => x.GetIndex() == index);
        harvestButton.transform.position = transform.position + new Vector3(0f, 0.5f, 0f);

        harvestButton.onClick.AddListener(() => OnClickHarvestButton());
        particle = GetComponentInChildren<ParticleSystem>();

        UpdateUI();

        if (!IsHave())
        {
            firstIngredient = GetComponentInChildren<BuyFirstIngredient>();
        }

        firstIngredient.SetValue(index);
    }

    void Update()
    {
        if (!IsHave()) return;

        if (curTime < ingredient.maxTime && !isHarvest)
        {
            curTime += Time.deltaTime;
        }

        if (curTime >= ingredient.maxTime && !isHarvest)
        {
            harvestButton.gameObject.SetActive(true);
            isHarvest = true;
        }
    }

    public void OnClickHarvestButton()
    {
        harvestButton.gameObject.SetActive(false);
        GameManager.Instance.CurrentUser.AddIngredientsAmounts(index, ingredient.GetAmount());
        curTime = 0f;
        isHarvest = false;
    }

    public override void UpdateUI()
    {
        if (IsHave())
        {
            particle.gameObject.SetActive(true);
        }
        else
        {
            particle.gameObject.SetActive(false);
        }
    }

    private bool IsHave()
    {
        return (GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index]);
    }
}
