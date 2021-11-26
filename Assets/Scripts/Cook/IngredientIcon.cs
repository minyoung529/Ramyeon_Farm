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

    public bool isInPot { get; private set; }

    private bool isAnimation;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    private void Update()
    {
        if (isAnimation)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.2f)
            {
                PutInPot();
            }
        }
    }

    public void SetValue(int index)
    {
        ingredient = GameManager.Instance.GetIngredients()[index];
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

    private void OnMouseUp()
    {
        OnIngredientUp();
        GameManager.Instance.PlusIngredientInPot(ingredient);
    }

    public void Inactive()
    {
        gameObject.SetActive(false);
        transform.SetParent(GameManager.Instance.Pool);
        animator.Play("DefaultAnimation");
        isInPot = false;
        isAnimation = false;
        animator.enabled = false;

        ingredient = null;
    }

    public bool IsAuto()
    {
        return CheckIsAuto(ingredient.name);
    }

    private bool CheckIsAuto(string ingredient)
    {
        string ingredients = "∞Ì√Â∞°∑Á ∂±";

        if(ingredients.Contains(ingredient))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public void OnIngredientUp()
    {
        if (ingredient.type == IngredientType.basic && isAnimation)
        {
            return;
        }

        animator.enabled = true;
        SoundManager.Instance.PlayIngredientSound(ingredient.GetIndex());
        isAnimation = true;
        OnAnimationStart();
    }

    private void OnAnimationStart()
    {
        animator.Play(ingredient.GetIndex().ToString());
    }

    private void PutInPot()
    {
        gameObject.SetActive(false);
        GameManager.Instance.GetPot().InstantiateIngredientInPot(ingredient.GetIndex());
        Inactive();
    }

    public Ingredient GetIngredient()
    {
        return ingredient;
    }
}
