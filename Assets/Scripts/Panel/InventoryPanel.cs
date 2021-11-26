using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : PanelBase
{
    [SerializeField] private Image nameImage;
    [SerializeField] private Text inventoryText;

    private int index;

    public override void SetValue(int index_)
    {
        index = index_;
        UpdateUI();
    }

    public override void UpdateUI()
    {
        int amount = GameManager.Instance.CurrentUser.GetIngredientsAmounts()[index];
        if (amount == 0) { gameObject.SetActive(false); }
        else { gameObject.SetActive(true); }
        nameImage.sprite = GameManager.Instance.GetIngredientSprite(index);
        inventoryText.text = amount.ToString();
    }
}
