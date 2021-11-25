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

    private GuestComment guestComment = new GuestComment();
    private string comment = "";

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
                SetComment(myRecipeToInt.Count, recipeToInt.Count);
                Debug.Log("매치, 많을 때");
                for (int i = 0; i < plus.Count; i++)
                {
                    price -= Mathf.RoundToInt(plus[i].GetPrice() * 1.5f);
                }

                GameManager.Instance.QuestManager.UpdateAchievement(AchievementType.BadCook, 1);
                //재료가 더 많을 때 
            }

            else if (myRecipeToInt.Count < recipeToInt.Count)
            {
                SettingMinusList();
                SetComment(myRecipeToInt.Count, recipeToInt.Count);

                for (int i = 0; i < minus.Count; i++)
                {
                    Debug.Log(price);
                    price -= Mathf.RoundToInt(minus[i].GetPrice() * 1.1f);
                }
                Debug.Log("매치, 적을 때");
                GameManager.Instance.QuestManager.UpdateAchievement(AchievementType.BadCook, 1);

                //재료가 더 없을 때 
            }

            else
            {
                comment = guestComment.GetGoodComments();
            }
        }

        else
        {
            if (myRecipeToInt.Count > recipeToInt.Count)
            {
                for (int i = 0; i < wrong.Count; i++)
                {
                    price -= Mathf.RoundToInt(wrong[i].GetPrice() * 1.2f);
                }

                for (int i = 0; i < plus.Count; i++)
                {
                    price += Mathf.RoundToInt(plus[i].GetPrice() * 1.2f);
                }
                Debug.Log("노매치, 많을 때");
                comment = guestComment.GetBadComments();

                GameManager.Instance.QuestManager.UpdateAchievement(AchievementType.BadCook, 1);
                //재료가 더 많을 때 
            }

            else
            {
                price = 0;
                Debug.Log("노매치, 적을 때");
                GameManager.Instance.QuestManager.UpdateAchievement(AchievementType.BadCook, 1);
                comment = guestComment.GetBadComments();

                //재료가 더 없을 때 
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
            if (!ingredients[j].name.Contains("물") && !ingredients[j].name.Contains("라면사리") && !ingredients[j].name.Contains("스프"))
            {
                plus.Add(ingredients[j]);
            }
        }

        for (int i = 0; i < plus.Count; i++)
        {
            Debug.Log(price);
            price += plus[i].GetPrice();
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

    public void ResetData()
    {
        recipeToInt.Clear();
        myRecipeToInt.Clear();

        plus.Clear();
        minus.Clear();
        wrong.Clear();

        price = 0;
    }

    private void SetComment(int count1, int count2)
    {
        if (Mathf.Abs(count2 - count1) > 2)
        {
            comment = guestComment.GetBadComments();
        }
        else
        {
            comment = guestComment.GetCommonComments();
        }
    }

    public string GetComment()
    {
        return comment;
    }
}