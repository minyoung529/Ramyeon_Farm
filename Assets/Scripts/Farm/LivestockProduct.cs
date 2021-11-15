using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivestockProduct : MonoBehaviour
{
    private LivestockObject livestockObj;
    private Livestock livestock;
    private Ingredient ingredient;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetLiveStock(LivestockObject livestockObj)
    {
        this.livestockObj = livestockObj;
        livestock = livestockObj.GetLivestock();
        spriteRenderer.sprite = GameManager.Instance.ingredientSprites[livestock.GetIngredient().GetIndex()];
    }

    private void OnMouseUp()
    {
        livestockObj.MinusCurCount();
        ingredient = GameManager.Instance.GetIngredients().Find(x => x.GetIndex() == livestock.GetIngredient().GetIndex());
        ingredient.AddAmount(1);
        GameManager.Instance.UIManager.UpdateIngredientPanel();
        GameManager.Instance.QuestManager.AddQuestValue(KeyManager.FARMQUEST_INDEX, 1);
        Despawn();
    }

    private void Despawn()
    {
        gameObject.SetActive(false);
        transform.SetParent(GameManager.Instance.Pool);
    }
}
