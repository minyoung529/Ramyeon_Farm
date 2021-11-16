using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BuyFirstIngredient : MonoBehaviour
{
    int index;
    Button button;
    [SerializeField] private Image ingredientImage;
    [SerializeField] private Text ingredientText;
    [SerializeField] private Text priceText;
    [SerializeField] private ActiveScale panel;

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

        if (GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index])
        {

        }

        else
        {
            ingredientImage.sprite = GameManager.Instance.GetIngredientSprite(index);
            ingredientText.text = string.Format("{0}을 구매하시겠습니까?", ingredient.name);
            priceText.text = string.Format("가격: {0}원", ingredient.firstPrice);
            panel.OnActive();
        }
    }

    public void PurchaseIngredient()
    {
        int price = GameManager.Instance.GetIngredients()[index].firstPrice;

        if (price > GameManager.Instance.CurrentUser.GetMoney()) return;

        GameManager.Instance.CurrentUser.AddUserMoney(-price);
        GameManager.Instance.CurrentUser.SetIsIngredientsHave(index, true);
    }
}
