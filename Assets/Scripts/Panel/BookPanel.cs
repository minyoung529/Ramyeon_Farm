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
    [SerializeField] private GameObject pot;

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
        info = "";

        switch (enumValue)
        {
            case 0:
                if (!ActiveSelf(GameManager.Instance.GetIngredients().Count)) return;

                Ingredient igd = GameManager.Instance.GetIngredients()[index];
                gameObject.SetActive(GameManager.Instance.CurrentUser.GetIsIngredientsHave()[igd.GetIndex()]);

                image.sprite = GameManager.Instance.GetIngredientSprite(index);
                contentName = igd.name;
                info = igd.info;
                pot.gameObject.SetActive(false);
                break;

            case 1:
                if (!ActiveSelf(GameManager.Instance.GetRecipes().Count)) return;
                gameObject.SetActive(GameManager.Instance.IsUserRecipe(index));

                image.sprite = GameManager.Instance.GetRecipeSprite(index);
                contentName = GameManager.Instance.GetRecipes()[index].recipeName;
                List<string> ingredients = GameManager.Instance.GetRecipes()[index].GetIngredients();
                for (int i = 0; i < ingredients.Count; i++)
                {
                    info += string.Format("{0}, ", ingredients[i]);
                }
                pot.gameObject.SetActive(true);
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