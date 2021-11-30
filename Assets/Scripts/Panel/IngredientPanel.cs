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
            amountText.text = GameManager.Instance.CurrentUser.GetIngredientsAmounts()[index].ToString();
        }

        image.sprite = GameManager.Instance.GetingredientContainerSprite(index);
        UpdateData();
    }

    public void UpdateData()
    {
        if (!GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index])
        {
            gameObject.SetActive(false);
        }

        else
        {
            gameObject.SetActive(true);
        }

        if (amountText != null)
        {
            amountText.text = GameManager.Instance.CurrentUser.GetIngredientsAmounts()[index].ToString();
        }
    }

    private bool CheckIsOverlap()
    {
        int waterIndex = 2;
        return (ingredient.name == "물" && GameManager.Instance.GetCurrentRamen().Contains(GameManager.Instance.GetIngredients()[waterIndex]));
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentUser.GetIngredientsAmounts()[index] < 1 && ingredient.type != IngredientType.basic) return;
        if (CheckIsOverlap()) return;
        IconInstantiateOrPooling();
        SoundManager.Instance?.PutIngredientSound();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentUser.GetIngredientsAmounts()[index] < 1 && ingredient.type != IngredientType.basic) return;
        if (CheckIsOverlap()) return;

        Vector2 mousePosition = Input.mousePosition;
        mousePosition = GameManager.Instance.mainCam.ScreenToWorldPoint(mousePosition);
        icon.transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (CheckIsOverlap()) return;

        if (GameManager.Instance.CurrentUser.GetIngredientsAmounts()[index] < 1 && ingredient.type != IngredientType.basic)
        {
            GameManager.Instance.SetCurrentIngredientIcon(null);
            return;
        }

        if (!GameManager.Instance.GetCurrentIngredientIcon().isInPot)
        {
            ingredientIcon.Inactive();
        }

        else
        {
            if (pot.IsDonPut())
            {
                GameManager.Instance.UIManager.ErrorMessage("지금은 넣을 수 없습니다.");

                ingredientIcon.Inactive();
                GameManager.Instance.SetCurrentIngredientIcon(null);
                return;
            }

            if (ingredientIcon.IsAuto())
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
            ingredientIcon = GameManager.Instance.GetIngredientIcons().Find(x => x.gameObject == icon.gameObject);
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
        GameManager.Instance.SetCurrentIngredientIcon(ingredientIcon);
    }

    private void InstantiateIcon()
    {
        icon = Instantiate(igdIcon, igdIcon.transform.parent);
        ingredientIcon = icon.GetComponent<IngredientIcon>();
        GameManager.Instance.GetIngredientIcons().Add(ingredientIcon);
    }
}
