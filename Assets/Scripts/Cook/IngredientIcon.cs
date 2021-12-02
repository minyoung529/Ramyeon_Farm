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
    private bool isClick;
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
        if (isClick) return;
        OnIngredientUp();
        GameManager.Instance.PlusIngredientInPot(ingredient);
        isClick = true;
        TutorialIngredient();
    }
    private void TutorialIngredient()
    {
        if (ingredient.name == "물") return;
        else if (ingredient.name == "라면사리") GameManager.Instance.TutorialManager.TutorialIngredient("라면사리", true);
        else if (ingredient.name == "스프") GameManager.Instance.TutorialManager.TutorialIngredient("스프", true);
        else if (ingredient.name == "대파") GameManager.Instance.TutorialManager.TutorialIngredient("대파", true);
    }

    public void Inactive()
    {
        gameObject.SetActive(false);
        transform.SetParent(GameManager.Instance.Pool);
        animator.Play("DefaultAnimation");
        isInPot = false;
        isAnimation = false;
        animator.enabled = false;
        isClick = false;
    }

    public bool IsAuto()
    {
        return false;
    }

    public void OnIngredientUp()
    {
        if (ingredient.type == IngredientType.basic && isAnimation)
        {
            return;
        }

        animator.enabled = true;
        SoundManager.Instance?.PlayIngredientSound(ingredient.GetIndex());
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
