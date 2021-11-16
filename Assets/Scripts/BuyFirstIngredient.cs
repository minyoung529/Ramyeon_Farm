using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyFirstIngredient : MonoBehaviour
{
    int index;
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClick());
    }

    public void SetValue(int index)
    {
        this.index = index;
    }

    public void OnClick()
    {
        Ingredient ingredient = GameManager.Instance.GetIngredients()[index];

    }

    public void PurchaseIngredient()
    {

    }
}
