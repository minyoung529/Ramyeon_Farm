using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientIcon : MonoBehaviour
{
    private Ingredient ingredient;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private const string AnimationKey = "Index";

    public bool isInPot { get; private set; }

    private bool isAnimation;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetValue(int index)
    {
        ingredient = GameManager.Instance.CurrentUser.ingredients[index];
        GameManager.Instance.SetCurrentIngredient(ingredient);
        Debug.Log(index);
        spriteRenderer.sprite = GameManager.Instance.ingredientSprites[index];
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
        isAnimation = false;
        ingredient = null;
    }

    private void OnMouseUp()
    {
        if (ingredient.state == IngredientState.basic && isAnimation) return;

        isAnimation = true;
        OnAnimationStart();
    }

    private void OnAnimationStart()
    {
        animator.SetInteger("Index", ingredient.GetIndex());
    }
}
