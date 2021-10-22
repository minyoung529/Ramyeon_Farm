using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientIcon : MonoBehaviour
{
    Ingredient ingredient;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetValue(int index)
    {
        ingredient = GameManager.Instance.CurrentUser.ingredients[index];
        spriteRenderer.sprite = GameManager.Instance.GetIngredientContainer(index);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
