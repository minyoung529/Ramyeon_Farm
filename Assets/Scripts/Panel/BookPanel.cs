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
        button.onClick.AddListener(() => OnClickContent());
    }
    public override void SetValue(int index)
    {
        this.index = index;
        SetImage((int)bookType);
    }

    public void SetBookState(BookType type)
    {
        bookType = type;
        SetImage((int)bookType);
    }

    private void SetImage(int enumValue)
    {
        switch (enumValue)
        {
            case 0:
                image.sprite = GameManager.Instance.ingredientSprites[index];
                contentName = GameManager.Instance.CurrentUser.ingredients[index].name;
                info = string.Format("재료 {0}", index);
                break;

            case 1:
                image.sprite = GameManager.Instance.ingredientSprites[0];
                contentName = string.Format("라면 {0}", index);
                info = string.Format("라면 {0}", index);
                break;

            case 2:
                image.sprite = GameManager.Instance.ingredientSprites[1];
                contentName = string.Format("가구 {0}", index);
                info = string.Format("가구 {0}", index);
                break;
        }
    }

    public void OnClickContent()
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