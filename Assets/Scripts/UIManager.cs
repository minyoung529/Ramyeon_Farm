using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] Transform distanceTransform;
    [SerializeField] private Image speechBubble;
    [SerializeField] private List<Ingredient> ingredients;

    [SerializeField] private GameObject ingredientPanelObj;

    [SerializeField] private List<GameObject> stagesUI;
    [SerializeField] private List<GameObject> stagesObj;

    [SerializeField] private Text guestText;

    private bool isMove;
    private bool isContentMove;

    private float screenWidth;
    private float distanceX;
    private float distanceY;

    private float ingredientPanelWidth;
    private List<IngredientPanel> ingredientPanels = new List<IngredientPanel>();

    private void Awake()
    {
        screenWidth = Screen.width;
        distanceX = Mathf.Abs(distanceTransform.position.x) * 2f;
        distanceY = Mathf.Abs(distanceTransform.position.y) * 2f;
    }

    void Start()
    {
        
        ingredientPanelWidth = ingredientPanelObj.GetComponent<RectTransform>().sizeDelta.x;

        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.position = new Vector2(distanceX * i, 0);
        }

        UpdateIngredientPanel();
    }

    public void UpdateIngredientPanel()
    {
        for (int i = 2; i < GameManager.Instance.CurrentUser.ingredients.Count; i++)
        {
            GameObject obj = Instantiate(ingredientPanelObj, ingredientPanelObj.transform.parent);
            IngredientPanel igdP = obj.GetComponent<IngredientPanel>();
            ingredientPanels.Add(igdP);
            igdP.SetValue(i);
        }
        ingredientPanelObj.SetActive(false);
    }

    public void ShowUpSpeechBubble()
    {
        speechBubble.transform.DOScale(0f, 0f);
        RandomOrder();
        speechBubble.transform.DOScale(1f, 0.3f);
    }

    private void RandomOrder()
    {
        List<Recipe> recipes = GameManager.Instance.CurrentUser.recipes;
        Recipe recipe = recipes[Random.Range(0, recipes.Count)];
        GameManager.Instance.SetCurrentRecipe(recipe);
        guestText.text = string.Format("{0}�� ����� ���ε�...", recipe.recipeName);
    }

    public void NextStage()
    {
        if (isMove) return;

        isMove = true;

        for (int i = 0; i < stagesUI.Count; i++)
        {
            stagesUI[i].transform.DOLocalMoveX(stagesUI[i].transform.localPosition.x - screenWidth, 0.5f).OnComplete(() => isMove = false);
        }

        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.DOMoveX(stagesObj[i].transform.position.x - distanceX, 0.5f);
        }
    }

    public void PreviousStage()
    {
        if (isMove) return;

        isMove = true;

        for (int i = 0; i < stagesUI.Count; i++)
        {
            stagesUI[i].transform.DOLocalMoveX(stagesUI[i].transform.localPosition.x + screenWidth, 0.5f).OnComplete(() => isMove = false);
        }

        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.DOMoveX(stagesObj[i].transform.position.x + distanceX, 0.5f);
        }
    }

    public void PreviousContents(RectTransform rectTransform)
    {
        if (isContentMove) return;
        isContentMove = true;
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + ingredientPanelWidth * 3 + 49 * 3, 0.3f).
            OnComplete(() => isContentMove = false);
    }

    public void NextContents(RectTransform rectTransform)
    {
        if (isContentMove) return;
        isContentMove = true;
        rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x - ingredientPanelWidth * 3 - 49 * 3, 0.3f).
            OnComplete(() => isContentMove = false);
    }

    public void EvaluateCurrentRamen()
    {
        PreviousStage();

        float score = GameManager.Instance.EvaluateRamen(GameManager.Instance.GetRecipe().recipeName);

        if (score > 99f)
        {
            guestText.text = "�Ϻ��ؿ�!";
        }

        else if (score > 59f)
        {
            guestText.text = "�׳� �����Կ�";
        }

        else if (score > 0)
        {
            guestText.text = "�̰Թ���? ��...";
        }

        else
        {
            guestText.text = "�� ������� �ذž�!! ���� �Ű��!!";
        }
    }

    public float DistanceX()
    {
        return distanceX;
    }

    public float DistanceY()
    {
        return distanceY;
    }
}
