using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPanel : PanelBase
{
    private Button button;

    BookType bookType = BookType.Ingredient;

    private string contentName;
    private string info;

    private int index;

    [SerializeField] private Image image;
    [SerializeField] private SelectedBook SelectedBookContent;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClick());
    }
    public override void SetValue(int index)
    {
        this.index = index;
        SetImage((int)bookType);
    }

    public override void SetState(BookType type)
    {
        bookType = type;
        SetImage((int)bookType);
    }

    private void SetImage(int enumValue)
    {
        switch (enumValue)
        {
            case 0:
                if (!ActiveSelf(GameManager.Instance.GetIngredients().Count)) return;

                Ingredient igd = GameManager.Instance.GetIngredients()[index];
                image.sprite = GameManager.Instance.GetIngredientSprite(index);
                contentName = igd.name;
                info = igd.info;
                break;

            case 1:
                if (!ActiveSelf(GameManager.Instance.GetRecipes().Count)) return;

                image.sprite = GameManager.Instance.GetIngredientSprite(0);
                contentName = string.Format("라면 {0}", index);
                info = string.Format("라면 {0}", index);
                break;

            case 2:
                image.sprite = GameManager.Instance.GetIngredientSprite(1);
                contentName = string.Format("가구 {0}", index);
                info = string.Format("가구 {0}", index);
                break;
        }
    }

    private bool ActiveSelf(int maxIndex)
    {
        if (index > maxIndex - 1)
        {
            gameObject.SetActive(false);
            return false;
        }
        else
        {
            gameObject.SetActive(true);
            return true;
        }
    }
    public override void OnClick()
    {
        SelectedBookContent.SetValue((int)bookType, index, contentName, info);
    }
}


public enum BookType
{
    Ingredient,
    Ramen,
    Furniture,

    Count
}