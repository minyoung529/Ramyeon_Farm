using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientIcon : MonoBehaviour
{
    Ingredient ingredient;
    SpriteRenderer spriteRenderer;
    public bool isInPot { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetValue(int index)
    {
        ingredient = GameManager.Instance.CurrentUser.ingredients[index];
        GameManager.Instance.SetCurrentIngredient(ingredient);
        spriteRenderer.sprite = GameManager.Instance.GetIngredientSprite(index);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pot"))
        {
            isInPot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInPot = false;
    }

    public void Inactive()
    {
        gameObject.SetActive(false);
        GameManager.Instance.currentIngredientIcon = null;
        transform.SetParent(GameManager.Instance.Pool);
        GameManager.Instance.SetCurrentIngredient(null);
        isInPot = false;
        ingredient = null;
    }
}
