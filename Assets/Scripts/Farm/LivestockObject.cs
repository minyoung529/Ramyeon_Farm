using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivestockObject : IngredientPurchase
{
    [SerializeField] private GameObject livestockProductObject;

    private Livestock livestock;
    private LivestockProduct livestockProduct;

    private int maxCount;
    private int curCount = 0;

    private float curTime = 0f;
    private float maxTime;

    private float maxDistanceX;
    private float maxDistanceY;


    void Update()
    {
        if (!GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index]) return;

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
        if (GameManager.Instance.CheckPool(KeyManager.LIVESTOCK_PRODUCT_NAME))
        {
            return GameManager.Instance.ReturnPoolObject(KeyManager.LIVESTOCK_PRODUCT_NAME);
        }

        else
        {
            return Instantiate(livestockProductObject, transform);
        }
    }

    public void SetValue(int index, float distanceX, float distanceY)
    {
        livestock = GameManager.Instance.CurrentUser.livestocks[index];

        this.index = livestock.GetIngredient().GetIndex();
        Ingredient ingredient = GameManager.Instance.GetIngredients()[this.index];
        maxTime = ingredient.GetMaxTime();

        maxDistanceX = distanceX;
        maxDistanceY = distanceY;

        maxCount = ingredient.GetAmount();

        transform.parent.GetComponent<BuyFirstIngredient>().SetValue(this.index);

        UpdateUI();
    }

    public override void UpdateUI()
    {
        gameObject.SetActive(GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index]);
    }
    public void MinusCurCount()
    {
        curCount--;
    }

    public Livestock GetLivestock()
    {
        return livestock;
    }
}