using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluateRamen : MonoBehaviour
{
    List<int> recipeToInt = new List<int>();
    List<int> myRecipeToInt = new List<int>();

    public void Evaluate()
    {
        SetRecipeToListInt();

        bool isMatch = CheckIsMatchRecipe();
        int checkListCnt = GetMyCheckListCount();

        if (isMatch)
        {
            if (myRecipeToInt.Count == recipeToInt.Count)
            {
                Debug.Log("완벽");
                //완벽
            }

            else if (myRecipeToInt.Count > recipeToInt.Count)
            {
                Debug.Log("매치, 많을 때");
                //재료가 더 많을 때 
            }

            else
            {
                Debug.Log("매치, 적을 때");
                //재료가 더 없을 때 
            }
        }

        else
        {
            if (myRecipeToInt.Count > recipeToInt.Count)
            {
                Debug.Log("노매치, 많을 때");
                //재료가 더 많을 때 
            }

            else
            {
                Debug.Log("노매치, 적을 때");
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

        Debug.Log(myRecipeToInt.Count);

        for (int i = 0; i < myRecipeToInt.Count; i++)
        {
            if(!recipeToInt.Contains(myRecipeToInt[i]))
            {
                Debug.Log(i);

                return false;
            }
        }

        return true;
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
}