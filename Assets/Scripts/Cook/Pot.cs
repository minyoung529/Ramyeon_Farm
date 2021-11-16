using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Pot : MonoBehaviour
{
    #region constant
    private const string water = "물";
    private const string soup = "스프";
    private const string noodle = "라면사리";

    private const float boilTime = 5f;
    private const float soupTime = 2f;
    private const float noodleTime = 4f;
    private const float finishTime = 2f;
    #endregion

    [SerializeField] private GameObject ingredientInPot;
    [SerializeField] Sprite[] potSprites;

    private SpriteRenderer boilingWater;
    private Animator potAnimator;
    private Animator waterAnimator;
    private List<string> dontPutIngredients = new List<string>();
    private List<SpriteRenderer> potObjs = new List<SpriteRenderer>();

    private SpriteRenderer spriteRenderer;

    private bool isStop = false;
    private bool isBoil = false;

    Color soupColor = new Color32(255, 0, 0, 89);
    Color originColor = new Color32(0, 0, 0, 89);

    private float curTime = 0f;
    private float maxTime = 0f;

    public bool dontPut { get; private set; }

    private void Start()
    {
        waterAnimator = GetComponentInChildren<Animator>();
        boilingWater = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        DontPut("라면사리", "스프");
    }

    private void Update()
    {
        if (isStop)
        {
            StopAllCoroutines();
            isStop = false;
        }

        if (curTime < maxTime && !isBoil)
        {
            curTime += Time.deltaTime;
        }

        else if(curTime > maxTime && !isBoil)
        {
            BoilWaterAnimation();
        }
    }
    public void OnIngredientPut(Ingredient ingredient)
    {
        if (ingredient.name == water)
        {
            BoilWater();
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

    private void BoilWater()
    {
        dontPut = true;
        DontPut("라면사리", "스프");
        boilingWater.transform.DOScale(1f, 2f);
        ResetTimer();
    }

    private void ResetTimer()
    {
        maxTime = boilTime;
        curTime = 0f;
    }

    private void BoilWaterAnimation()
    {
        isBoil = true;
        dontPut = false;

        waterAnimator.Play("Water_boil");
        DontPut("");
        Debug.Log("물이 끓음");
    }

    private IEnumerator PutSoup()
    {
        if (GameManager.Instance.IsInCurrentRamen())
        {
            StartCoroutine(Finish());
        }

        boilingWater.DOColor(soupColor, soupTime);
        yield return new WaitForSeconds(soupTime);

        Debug.Log("스프 풀어짐");
    }


    private IEnumerator PutNoodle()
    {

        if (GameManager.Instance.IsInCurrentRamen())
        {
            StartCoroutine(Finish());
        }

        yield return new WaitForSeconds(noodleTime);

        Debug.Log("면 풀ㄷ어짐");
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(finishTime);
        Debug.Log("완성!");
    }
    private void DontPut(params string[] ingredientsName)
    {
        dontPutIngredients.Clear();
        dontPutIngredients = ingredientsName.ToList();
    }

    public bool IsDonPut()
    {
        return (dontPutIngredients.Contains(GameManager.Instance.currentIngredientIcon.GetIngredient().name));
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

        if (spRenderer == null) return;

        if (GameManager.Instance.GetingredientInPotSprite(index) != null)
        {
            spRenderer.sprite = GameManager.Instance.GetingredientInPotSprite(index);
        }
        spRenderer.sortingOrder = SetOrder(index);
        obj.SetActive(true);
    }

    private int SetOrder(int index)
    {
        if (index == 0)
        {
            return 0;
        }
        else
        {
            return 2;
        }
    }

    private void DespawnObjs()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(1).gameObject;
            obj.transform.SetParent(GameManager.Instance.Pool);
            obj.SetActive(false);
        }
    }

    public void ResetPot()
    {
        isStop = true;
        isBoil = false;

        curTime = 0f;
        maxTime = 0f;

        spriteRenderer.sprite = potSprites[0];
        GameManager.Instance.ClearCurrentRamen();

        boilingWater.gameObject.SetActive(false);
        boilingWater.transform.localScale = Vector2.zero;
        boilingWater.color = originColor;
        waterAnimator.Play("WaterIdle");

        DespawnObjs();
    }
}
