using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluateRamen : MonoBehaviour
{
    List<int> recipeToInt = new List<int>();
    List<int> myRecipeToInt = new List<int>();

    List<Ingredient> plus = new List<Ingredient>();
    List<Ingredient> minus = new List<Ingredient>();
    List<Ingredient> wrong = new List<Ingredient>();

    int price;

    public int GetRamenPrice()
    {
        Evaluate();
        if (price < 0) price = 0;
        return price;
    }

    public void Evaluate()
    {
        SetRecipeToListInt();

        bool isMatch = CheckIsMatchRecipe();
        price = GetPrice();

        if (isMatch)
        {
            if (myRecipeToInt.Count > recipeToInt.Count)
            {
                Debug.Log("��ġ, ���� ��");
                for (int i = 0; i < plus.Count; i++)
                {
                    price -= Mathf.RoundToInt(plus[i].price * 0.8f);
                }
                //��ᰡ �� ���� �� 
            }

            else if (myRecipeToInt.Count < recipeToInt.Count)
            {
                SettingMinusList();
                for (int i = 0; i < minus.Count; i++)
                {
                    Debug.Log(price);
                    price -= Mathf.RoundToInt(minus[i].price * 1.1f);
                }
                Debug.Log("��ġ, ���� ��");

                //��ᰡ �� ���� �� 
            }
        }

        else
        {
            if (myRecipeToInt.Count > recipeToInt.Count)
            {
                for (int i = 0; i < wrong.Count; i++)
                {
                    price -= Mathf.RoundToInt(wrong[i].price * 1.2f);
                }

                for (int i = 0; i < plus.Count; i++)
                {
                    price += Mathf.RoundToInt(plus[i].price * 1.2f);
                }
                Debug.Log("���ġ, ���� ��");
                //��ᰡ �� ���� �� 
            }

            else
            {
                price = 0;
                Debug.Log("���ġ, ���� ��");
                //��ᰡ �� ���� �� 
            }
        }
    }

    private void SetRecipeToListInt()
    {
        Recipe currentRecipe = GameManager.Instance.GetRecipe();
        List<Ingredient> currentRamen = GameManager.Instance.GetCurrentRamen();

        for (int i = 0; i < currentRecipe.GetIngredients().Count; i++)
        {
            recipeToInt.Add(currentRecipe.GetIngredients().IndexOf(currentRecipe.GetIngredients()[i]));
        }

        for (int i = 0; i < currentRamen.Count; i++)
        {
            myRecipeToInt.Add(currentRamen.IndexOf(currentRamen[i]));
        }
    }

    private bool CheckIsMatchRecipe()
    {
        if (myRecipeToInt.Count == 0) return false;

        int cnt = 0;

        for (int i = 0; i < myRecipeToInt.Count; i++)
        {
            if (recipeToInt.Contains(myRecipeToInt[i]))
            {
                cnt++;
            }

            else
            {
                wrong.Add(GameManager.Instance.GetIngredients().Find(x => x.GetIndex() == myRecipeToInt[i]));
            }
        }

        return (cnt == myRecipeToInt.Count);
    }

    private int GetMyCheckListCount()
    {
        List<int> checkList = new List<int>();

        for (int i = 0; i < recipeToInt.Count; i++)
        {
            for (int j = 0; j < myRecipeToInt.Count; j++)
            {
                if (checkList.Exists(x => x == j)) continue;

                if (myRecipeToInt[j] == recipeToInt[i])
                {
                    checkList.Add(j);
                    break;
                }
            }
        }

        return checkList.Count;
    }

    private int GetPrice()
    {
        int price = 1500;

        List<Ingredient> ingredients = GameManager.Instance.GetCurrentRamen();

        for (int j = 0; j < ingredients.Count; j++)
        {
            if (!ingredients[j].name.Contains("��") && !ingredients[j].name.Contains("���縮") && !ingredients[j].name.Contains("����"))
            {
                plus.Add(ingredients[j]);
            }
        }

        for (int i = 0; i < plus.Count; i++)
        {
            Debug.Log(price);
            price += plus[i].price;
        }

        return (price + (plus.Count - 1) * 100);
    }

    private void SettingMinusList()
    {
        for (int i = 0; i < recipeToInt.Count; i++)
        {
            if (!myRecipeToInt.Contains(recipeToInt[i]))
            {
                minus.Add(GameManager.Instance.GetIngredients()[recipeToInt[i]]);
            }
        }
    }
}