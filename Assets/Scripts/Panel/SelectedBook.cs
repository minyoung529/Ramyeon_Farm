using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SelectedBook : MonoBehaviour
{
    BookType bookType;

    [SerializeField] Text nameText;
    [SerializeField] Text infoText;
    [SerializeField] Image image;
    [SerializeField] Image pot;


    public void SetValue(int type, int index, string name, string info)
    {
        nameText.text = name;
        if(type == (int)BookType.Ramen)
        {
            infoText.text = ModifyText(info);
        }
        else
        {
            infoText.text = info;
        }

        SetImage(type, index);
    }

    private string ModifyText(string text)
    {
        List<char> chars = text.ToCharArray().ToList();
        int count = chars.Count;

        for (int i = 0; i < count; i++)
        {
            if(i == count-2)
            {
                chars[i] = ' ';
                break;
            }
        }

        return string.Concat(chars);
    }

    private void SetImage(int type, int index)
    {
        switch (type)
        {
            case 0:
                image.sprite = GameManager.Instance.GetIngredientSprite(index);
                pot.gameObject.SetActive(false);
                break;

            case 1:
                image.sprite = GameManager.Instance.GetRecipeSprite(index);
                pot.gameObject.SetActive(true);
                break;
        }
    }
}