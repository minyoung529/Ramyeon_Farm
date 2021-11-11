using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Pot : MonoBehaviour
{
    #region constant
    private const string water = "��";
    private const string soup = "����";
    private const string noodle = "���縮";

    private const float boilTime = 5f;
    private const float soupTime = 2f;
    private const float noodleTime = 4f;
    private const float finishTime = 2f;
    #endregion

    [SerializeField] private GameObject ingredientInPot;

    private SpriteRenderer boilingWater;
    private Animator animator;
    private List<string> dontPutIngredients = new List<string>();
    private List<SpriteRenderer> potObjs = new List<SpriteRenderer>();

    public bool dontPut { get; private set; }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        boilingWater = transform.GetChild(0).GetComponent<SpriteRenderer>();
        DontPut("���縮", "����");
    }

    public void OnIngredientPut(Ingredient ingredient)
    {
        if (ingredient.name == water)
        {
            StartCoroutine(BoilWater());
        }
        else if (ingredient.name == soup)
        {
            StartCoroutine(PutSoup());
        }
        else if (ingredient.name == noodle)
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

        boilingWater.DOColor(Color.red, soupTime);
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
    }

    public void InstantiateIngredientInPot(int index)
    {
        GameObject obj;
        SpriteRenderer spRenderer;
        if (GameManager.Instance.CheckPool("IngredientInPot"))
        {
            obj = GameManager.Instance.ReturnPoolObject("IngredientInPot");
            obj.transform.SetParent(transform);
            spRenderer = potObjs.Find(x => x.gameObject == obj);
        }

        else
        {
            obj = Instantiate(ingredientInPot, transform);
            spRenderer = obj.GetComponent<SpriteRenderer>();
            potObjs.Add(spRenderer);
        }

        spRenderer.sprite = GameManager.Instance.ingredientInPotSprites[index];
        obj.SetActive(true);
    }

    public void DespawnObjs()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(1).gameObject;
            obj.transform.SetParent(GameManager.Instance.Pool);
            obj.SetActive(false);
        }
    }
}
