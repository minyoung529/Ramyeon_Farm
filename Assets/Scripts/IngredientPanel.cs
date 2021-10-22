using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IngredientPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject igdIcon;
    Ingredient ingredient;
    Image image;
    GameObject icon;

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
        ingredient = GameManager.Instance.CurrentUser.ingredients[index];
        image.sprite = GameManager.Instance.GetIngredientContainer(index);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        icon = Instantiate(igdIcon, igdIcon.transform.parent);
        icon.SetActive(true);

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = GameManager.Instance.mainCam.ScreenToWorldPoint(mousePosition);
        icon.transform.position = mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}
