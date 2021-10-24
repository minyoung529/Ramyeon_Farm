using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivestockObject : MonoBehaviour
{
    [SerializeField] private GameObject livestockProductObject;

    Livestock livestock;
    LivestockProduct livestockProduct;

    private int maxCount = 5;
    private int curCount = 0;

    private float curTime = 0f;
    private float maxTime = 10f;

    private float maxDistanceX;
    private float maxDistanceY;

    private readonly string livestockProductName = "LivestockProduct";
    void Update()
    {
        if (curTime < maxTime && curCount < maxCount)
        {
            curTime += Time.deltaTime;
        }

        else if (curTime > maxTime && curCount < maxCount)
        {
            GameObject obj = InstantiateOrPooling();

            livestockProduct = obj.GetComponent<LivestockProduct>();
            Debug.Log(livestockProduct);
            livestockProduct.SetLiveStock(this);

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

    public void SetValue(Livestock livestock, float distanceX, float distanceY)
    {
        this.livestock = livestock;
        Debug.Log(distanceX + ", " + distanceY);
        maxDistanceX = distanceX;
        maxDistanceY = distanceY;
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
