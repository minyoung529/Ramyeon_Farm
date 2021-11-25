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
        nameImage.sprite = GameManager.Instance.GetIngredientSprite(index);
        inventoryText.text = string.Format("{0}",GameManager.Instance.CurrentUser.GetIngredientsAmounts()[index]);
    }
}
