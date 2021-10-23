using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngredientPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject igdIcon;
    Ingredient ingredient;
    IngredientIcon ingredientIcon;
    Image image;
    GameObject icon;
    int index;

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
    }

    public void SetValue(int index)
    {
        this.index = index;
        ingredient = GameManager.Instance.CurrentUser.ingredients[index];
        image.sprite = GameManager.Instance.GetIngredientContainer(index);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IconInstantiateOrPooling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = GameManager.Instance.mainCam.ScreenToWorldPoint(mousePosition);
        icon.transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.currentIngredientIcon.isInPot)
        {
            ingredientIcon.Inactive();
        }
    }

    private void IconInstantiateOrPooling()
    {
        if (GameManager.Instance.CheckPool("IngredientIcon"))
        {
            icon = GameManager.Instance.ReturnPoolObject("IngredientIcon");
            icon.transform.SetParent(null);
            ingredientIcon = GameManager.Instance.ingredientIcons.Find(x => x.gameObject == icon.gameObject);
            SetIcon(index);
        }

        else
        {
            InstantiateIcon();
            SetIcon(index);
        }
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
