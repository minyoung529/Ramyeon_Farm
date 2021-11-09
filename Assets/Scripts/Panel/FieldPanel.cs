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
        ingredient = GameManager.Instance.CurrentUser.ingredients[index];
        harvestButtonImage.sprite = GameManager.Instance.ingredientSprites[index];
    }

    public void OnClickField()
    {
        if (isHarvest) return;
        timeSlider.transform.DOScale(1f, 0.3f);
        timeSlider.transform.DOScale(1f, 2f).OnComplete(() => timeSlider.transform.DOScale(0f, 0.3f));
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
        GameManager.Instance.CurrentUser.ingredients[index].AddAmount(1);
        GameManager.Instance.UIManager.UpdateIngredientPanel();

    }
}
