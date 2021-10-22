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

    private int stageIndex = 0;
    [SerializeField] private List<GameObject> stagesUI;
    [SerializeField] private List<GameObject> stagesObj;
    private bool isMove;

    private float width;
    private float distanceX;
    private List<IngredientPanel> ingredientPanels = new List<IngredientPanel>();

    void Start()
    {
        width = Screen.width;
        distanceX = Mathf.Abs(distanceTransform.position.x) * 2f;
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
        speechBubble.transform.DOScale(1f, 0.3f);
    }

    public void NextStage()
    {
        if (isMove) return;

        stageIndex++;
        isMove = true;

        for (int i = 0; i < stagesUI.Count; i++)
        {
            stagesUI[i].transform.DOLocalMoveX(stagesUI[i].transform.localPosition.x - width, 0.5f).OnComplete(() => isMove = false);
        }

        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.DOMoveX(-distanceX, 0.5f);
        }
    }

    public void PreviousStage()
    {
        if (isMove) return;

        stageIndex--;
        isMove = true;

        for (int i = 0; i < stagesUI.Count; i++)
        {
            stagesUI[i].transform.DOLocalMoveX(stagesUI[i].transform.localPosition.x + width, 0.5f).OnComplete(() => isMove = false);
        }

        for (int i = 0; i < stagesObj.Count; i++)
        {
            stagesObj[i].transform.DOMoveX(stagesObj[i].transform.position.x + distanceX, 0.5f);
        }
    }


}
