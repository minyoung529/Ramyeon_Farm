using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BuyFirstIngredient : MonoBehaviour
{
    int index;
    Button button;

    [SerializeField] private Text ingredientName;

    [SerializeField] private Image ingredientPanelImage;
    [SerializeField] private Text ingredientPanelText;
    [SerializeField] private Text priceText;
    [SerializeField] private ActiveScale panel;
    [SerializeField] private Button purchaseButton;

    private IngredientPurchase ingredientPurchase;

    private void Awake()
    {
        ingredientPurchase = GetComponentInChildren<IngredientPurchase>();
    }
    private void Start()
    {
        button = GetComponent<Button>();
        button?.onClick.AddListener(() => OnClick());
    }

    public void SetValue(int index)
    {
        this.index = index;

        if (ingredientName != null)
        {
            ingredientName.text = GameManager.Instance.GetIngredients()[index].name;
        }
    }

    public void OnClick()
    {
        Ingredient ingredient = GameManager.Instance.GetIngredients()[index];

        if (GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index]) { }

        else
        {
            SoundManager.Instance?.ButtonSound((int)ButtonSoundType.Bbang);
            ingredientPanelImage.sprite = GameManager.Instance.GetIngredientSprite(index);
            string name="";

            if (ingredient.type == IngredientType.meat) name = "농장";
            else if (ingredient.type == IngredientType.vegetable) name = "밭";
            else if (ingredient.type == IngredientType.processed) name = "공장";

            ingredientPanelText.text = string.Format("{0} {1}을 구매하시겠습니까?", ingredient.name, name);
            priceText.text = string.Format("가격: {0}원", ingredient.firstPrice);
            AssignButtonListner();
            panel.OnActive();
        }
    }
    private void OnMouseUp()
    {
        if (button == null)
        {
            OnClick();
        }
    }
    public void PurchaseIngredient()
    {
        int price = GameManager.Instance.GetIngredients()[index].firstPrice;

        if (price > GameManager.Instance.CurrentUser.GetMoney()) return;

        GameManager.Instance.CurrentUser.AddUserMoney(-price);
        GameManager.Instance.CurrentUser.SetIsIngredientsHave(index, true);
        GameManager.Instance.UIManager.UpdateIngredientUpgradePanel();

        ingredientPurchase.UpdateUI();
        panel.OnEnactive();
    }

    private void AssignButtonListner()
    {
        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() => PurchaseIngredient());
    }
}
