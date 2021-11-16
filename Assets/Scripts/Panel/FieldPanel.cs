using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldPanel : MonoBehaviour
{
    Ingredient ingredient;
    private int index;
    Slider timeSlider;
    [SerializeField] Button harvestButton;
    Image harvestButtonImage;

    float maxTime = 20f;
    float curTime = 0f;

    private bool isHarvest;

    private void Awake()
    {
        harvestButtonImage = harvestButton.transform.GetChild(0).GetComponent<Image>();
        harvestButton.onClick.AddListener(() => OnClickHarvest());
    }
    void Update()
    {
        if (!isHarvest)
        {
            curTime += Time.deltaTime;
        }

        if (curTime > maxTime && !isHarvest)
        {
            UpdateHarvestIcon();
        }
    }
    public void SetValue(int index)
    {
        this.index = index;
        ingredient = GameManager.Instance.GetIngredients()[index];
        harvestButtonImage.sprite = GameManager.Instance.GetIngredientSprite(index);
    }

    public void OnClickField()
    {
        if(GameManager.Instance.CurrentUser.isIngredientsHave[index])
        {

        }

        else
        {

        }
    }

    public void UpdateHarvestIcon()
    {
        isHarvest = true;
        harvestButton.transform.DOScale(1f, 0.4f);
    }

    public void OnClickHarvest()
    {
        curTime = 0f;
        isHarvest = false;
        harvestButton.transform.DOScale(0f, 0.2f);
        GameManager.Instance.GetIngredients()[index].AddAmount(1);
        GameManager.Instance.UIManager.UpdateIngredientPanel();
    }
}
