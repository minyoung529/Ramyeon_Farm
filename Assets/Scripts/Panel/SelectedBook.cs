using UnityEngine;
using UnityEngine.UI;

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
        infoText.text = info;

        SetImage(type, index);
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