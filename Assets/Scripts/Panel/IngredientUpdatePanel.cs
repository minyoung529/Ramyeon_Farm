using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUpdatePanel : PanelBase
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text upgradePriceText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text ingredientPriceText;
    [SerializeField] private Image ingredientImage;

    private int index;

    public override void SetValue(int index_)
    {
        index = index_;
        UpdateUI();
    }

    public override void UpdateUI()
    {
        gameObject.SetActive(GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index]);
        Ingredient ingredient = GameManager.Instance.GetIngredients()[index];
        ingredientImage.sprite = GameManager.Instance.GetIngredientSprite(index);

        int level = GameManager.Instance.CurrentUser.GetIngredientLevel(index);
        levelText.text = string.Format("Lv.{0}  -> Lv.{1}", level, level + 1);
        ingredientPriceText.text = string.Format("{0}¿ø  -> {1}¿ø", ingredient.GetPrice(), ingredient.GetNextPrice());

        upgradePriceText.text = ingredient.GetUpgradePrice().ToString();
        nameText.text = ingredient.name;
    }

    public void EnforceButton()
    {
        Ingredient ingredient = GameManager.Instance.GetIngredients()[index];

        if (ingredient.GetUpgradePrice() <= GameManager.Instance.CurrentUser.GetMoney())
        {
            GameManager.Instance.CurrentUser.AddUserMoney(-ingredient.GetUpgradePrice());
            GameManager.Instance.AddDatasOfDay(DataOfDay.SpentIngredientMoney, ingredient.GetUpgradePrice());

            GameManager.Instance.CurrentUser.AddIngredientLevel(index);
            UpdateUI();
        }
    }
}
