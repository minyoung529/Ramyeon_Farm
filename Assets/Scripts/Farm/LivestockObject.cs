using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivestockObject : IngredientPurchase
{
    [SerializeField] private GameObject livestockProductObject;

    private Livestock livestock;
    private LivestockProduct livestockProduct;

    private int ingredientIndex;

    private int maxCount;
    private int curCount = 0;

    private float curTime = 0f;
    private float maxTime;

    private float maxDistanceX;
    private float maxDistanceY;

    private readonly string livestockProductName = "LivestockProduct";

    void Update()
    {
        if (!GameManager.Instance.CurrentUser.GetIsIngredientsHave()[ingredientIndex]) return;

        if (curTime < maxTime && curCount < maxCount)
        {
            curTime += Time.deltaTime;
        }

        else if (curTime > maxTime && curCount < maxCount)
        {
            GameObject obj = InstantiateOrPooling();

            livestockProduct = obj.GetComponent<LivestockProduct>();

            LivestockObject livestockObj = this;
            livestockProduct.SetLiveStock(ref livestockObj);

            obj.transform.SetParent(transform);
            obj.gameObject.SetActive(true);

            obj.transform.localPosition = new Vector2(Random.Range(-maxDistanceX * 0.4f, maxDistanceX * 0.4f), Random.Range(-maxDistanceY * 0.4f, maxDistanceY * 0.4f));
            obj.transform.localScale = Vector3.one;

            curCount++;
            curTime = 0f;
        }
    }

    private GameObject InstantiateOrPooling()
    {
        if (GameManager.Instance.CheckPool(livestockProductName))
        {
            return GameManager.Instance.ReturnPoolObject(livestockProductName);
        }

        else
        {
            return Instantiate(livestockProductObject, transform);
        }
    }

    public void SetValue(int index, float distanceX, float distanceY)
    {
        livestock = GameManager.Instance.CurrentUser.livestocks[index];

        ingredientIndex = livestock.GetIngredient().GetIndex();
        Ingredient ingredient = GameManager.Instance.GetIngredients()[ingredientIndex];
        maxTime = ingredient.GetMaxTime();

        maxDistanceX = distanceX;
        maxDistanceY = distanceY;

        maxCount = ingredient.GetAmount();

        transform.parent.GetComponent<BuyFirstIngredient>().SetValue(ingredientIndex);

        UpdateUI();
    }

    public override void UpdateUI()
    {
        gameObject.SetActive(GameManager.Instance.CurrentUser.GetIsIngredientsHave()[ingredientIndex]);
    }
    public void MinusCurCount()
    {
        curCount--;
    }

    public Livestock GetLivestock()
    {
        return livestock;
    }

    public int GetCurCount()
    {
        return curCount;
    }
}
