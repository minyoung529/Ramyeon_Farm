using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pot : MonoBehaviour
{
    private const string water = "��";
    private const string soup = "����";
    private const string noodle = "���縮";

    private const float boilTime = 5f;
    private const float soupTime = 2f;
    private const float noodleTime = 4f;
    private const float finishTime = 2f;

    private Animator animator;
    private List<string> dontPutIngredients = new List<string>();

    public bool dontPut { get; private set; }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        DontPut("���縮", "����");
    }

    public void OnIngredientPut(Ingredient ingredient)
    {
        if (ingredient.name == water)
        {
            StartCoroutine(BoilWater());
        }
        else if(ingredient.name == soup)
        {
            StartCoroutine(PutSoup());
        }
        else if(ingredient.name == noodle)
        {
            StartCoroutine(PutNoodle());
        }
    }

    private IEnumerator BoilWater()
    {
        dontPut = true;
        DontPut("���縮", "����");

        yield return new WaitForSeconds(boilTime);
        Debug.Log("���� ����");

        DontPut("");
        dontPut = false;
    }

    private IEnumerator PutSoup()
    {
        if (GameManager.Instance.IsInCurrentRamen())
        {
            StartCoroutine(Finish());
        }

        yield return new WaitForSeconds(soupTime);

        Debug.Log("���� Ǯ����");
    }


    private IEnumerator PutNoodle()
    {

        if (GameManager.Instance.IsInCurrentRamen())
        {
            StartCoroutine(Finish());
        }

        yield return new WaitForSeconds(noodleTime);

        Debug.Log("�� Ǯ������");
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(finishTime);
        Debug.Log("�ϼ�!");
    }
    private void DontPut(params string[] ingredientsName)
    {
        dontPutIngredients.Clear();
        dontPutIngredients = ingredientsName.ToList();
    }

    public bool IsDonPut()
    {
        return (dontPutIngredients.Contains(GameManager.Instance.currentIngredient.name));
        //{
        //    dontPut = true;
        //}

        //else
        //{
        //    dontPut = false;
        //}
    }
}
