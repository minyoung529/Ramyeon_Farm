using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

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
        animator.enabled = false;
    }

    public void SetValue(int index)
    {
        ingredient = GameManager.Instance.CurrentUser.ingredients[index];
        GameManager.Instance.SetCurrentIngredient(ingredient);
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
        animator.SetInteger(AnimationKey, -1);
        isInPot = false;
        isAnimation = false;
        animator.enabled = false;

        ingredient = null;
    }

    public void OnIngredientUp()
    {
        if (ingredient.state == IngredientState.basic && isAnimation)
        {
            return;
        }

        animator.enabled = true;

        isAnimation = true;
        OnAnimationStart();
    }

    private void OnAnimationStart()
    {
        animator.SetInteger(AnimationKey, ingredient.GetIndex());
    }

    private void Update()
    {
        if(isAnimation)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.2f)
            {
                PutInPot();
            }
        }
    }

    private void PutInPot()
    {
        gameObject.SetActive(false);
        GameManager.Instance.GetPot().InstantiateIngredientInPot(ingredient.GetIndex());
        Inactive();
    }
}
