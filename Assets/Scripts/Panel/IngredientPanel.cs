using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngredientPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject igdIcon;
    [SerializeField] private Text amountText;
    [SerializeField] private Pot pot;

    private Ingredient ingredient;
    private IngredientIcon ingredientIcon;
    private Image image;
    private GameObject icon;
    private int index;

    private void Awake()
    {
        image = GetComponent<Image>();

        if (name == "Ramen")
        {
            SetValue(0);
        }

        else if (name == "Soup")
        {
            SetValue(1);
        }

        else if (name == "Water")
        {
            SetValue(2);
        }
    }

    public void SetValue(int index)
    {
        this.index = index;
        ingredient = GameManager.Instance.GetIngredients()[index];

        if (amountText != null)
        {
            amountText.text = GameManager.Instance.CurrentUser.ingredientsAmounts[index].ToString();
        }

        image.sprite = GameManager.Instance.GetingredientContainerSprite(index);
    }

    public void UpdateData()
    {
        if (amountText != null)
        {
            amountText.text = GameManager.Instance.CurrentUser.ingredientsAmounts[index].ToString();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentUser.ingredientsAmounts[index] < 1 && ingredient.state != IngredientState.basic) return;
        IconInstantiateOrPooling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentUser.ingredientsAmounts[index] < 1 && ingredient.state != IngredientState.basic) return;

        Vector2 mousePosition = Input.mousePosition;
        mousePosition = GameManager.Instance.mainCam.ScreenToWorldPoint(mousePosition);
        icon.transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (GameManager.Instance.CurrentUser.ingredientsAmounts[index] < 1 && ingredient.state != IngredientState.basic)
        {
            GameManager.Instance.currentIngredientIcon = null;
            return;
        }

        if (!GameManager.Instance.currentIngredientIcon.isInPot)
        {
            ingredientIcon.Inactive();
        }

        else
        {
            if (pot.IsDonPut())
            {
                ingredientIcon.Inactive();
                GameManager.Instance.currentIngredientIcon = null;
                return;
            }

            if(ingredientIcon.IsAuto())
            {
                ingredientIcon.OnIngredientUp();
                GameManager.Instance.PlusIngredientInPot(ingredientIcon.GetIngredient());
            }

            ingredient.AddAmount(-1);
            GameManager.Instance.UIManager.UpdateIngredientPanel();
        }

        ingredientIcon = null;
    }

    private void IconInstantiateOrPooling()
    {
        if (GameManager.Instance.CheckPool("IngredientIcon"))
        {
            icon = GameManager.Instance.ReturnPoolObject("IngredientIcon");
            icon.transform.SetParent(igdIcon.transform.parent);
            ingredientIcon = GameManager.Instance.ingredientIcons.Find(x => x.gameObject == icon.gameObject);
        }

        else
        {
            InstantiateIcon();
        }

        SetIcon(index);
    }

    public void FollowIcon()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = GameManager.Instance.mainCam.ScreenToWorldPoint(mousePosition);
        icon.transform.position = mousePosition;
    }

    private void SetIcon(int index)
    {
        icon.SetActive(true);
        ingredientIcon.SetValue(index);
        GameManager.Instance.currentIngredientIcon = ingredientIcon;
    }

    private void InstantiateIcon()
    {
        icon = Instantiate(igdIcon, igdIcon.transform.parent);
        ingredientIcon = icon.GetComponent<IngredientIcon>();
        GameManager.Instance.ingredientIcons.Add(ingredientIcon);
    }
}
