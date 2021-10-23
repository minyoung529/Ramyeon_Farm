using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        spriteRenderer.sprite = GameManager.Instance.GetIngredientSprite(index);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pot"))
        {
            isInPot = true;
        }
    }

    public void Inactive()
    {
        gameObject.SetActive(false);
        transform.SetParent(GameManager.Instance.Pool);
        isInPot = false;
        ingredient = null;
    }
}
