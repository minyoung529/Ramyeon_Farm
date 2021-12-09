using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldPanel : IngredientPurchase
{
    [SerializeField] private Button harvestButton;
    [SerializeField] private Sprite seedSprite;
    [SerializeField] private Sprite FieldSprite;
    [SerializeField] private List<Sprite> daepaSprite;
    [SerializeField] private List<Sprite> pepperSprite;
    [SerializeField] private List<Sprite> garlicSprite;
    [SerializeField] private List<Material> materials;
    [SerializeField] private Image seedFieldImage;

    private ParticleSystem particle;
    private ParticleSystemRenderer pr;

    private Image harvestButtonImage;
    private BuyFirstIngredient firstIngredient;

    float maxTime = 20f;
    float curTime = 0f;

    private bool isHarvest;
    private bool isSeed = true;
    private bool isGrow = false;

    private void Awake()
    {
        seedFieldImage = GetComponent<Image>();
        harvestButtonImage = harvestButton.transform.GetChild(0).GetComponent<Image>();
        harvestButton.onClick.AddListener(() => OnClickIcon());
        firstIngredient = GetComponent<BuyFirstIngredient>();
        particle = GetComponentInChildren<ParticleSystem>();
        pr = GetComponentInChildren<ParticleSystemRenderer>();
        curTime = maxTime;
    }
    void Update()
    {
        if (!GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index]) return;
        if (isSeed) return;

        if (!isHarvest)
        {
            curTime += Time.deltaTime;
        }

        if (curTime > maxTime && !isHarvest)
        {
            UpdateHarvestIcon();
        }
        if (isGrow == false && curTime > maxTime * 0.5f)
        {
            FieldChange(1);
            isGrow = true;
        }
    }
    public void SetValue(int index)
    {
        this.index = index;
        firstIngredient.SetValue(index);
        ingredient = GameManager.Instance.GetIngredients()[index];
        pr.material = materials[GetIndex()];
        maxTime = ingredient.GetMaxTime();
        UpdateUI();
    }

    public void UpdateHarvestIcon()
    {
        if (!GameManager.Instance.CurrentUser.isCompleteTutorial && !GameManager.Instance.TutorialManager.GetIsTutorial() && !GameManager.Instance.TutorialManager.GetEndTutorial())
            GameManager.Instance.TutorialManager.TutorialNumber(5);
        isHarvest = true;
        harvestButton.gameObject.SetActive(true);
        harvestButton.transform.DOScale(1f, 0.4f);
        harvestButtonImage.sprite = GameManager.Instance.GetIngredientSprite(index);
        FieldChange(2);
    }

    public void OnClickIcon()
    {
        Ingredient ingredient = GameManager.Instance.GetIngredients()[index];
        if (isHarvest)
        {
            Harvest(ingredient);
            if (!GameManager.Instance.CurrentUser.isCompleteTutorial && !GameManager.Instance.TutorialManager.GetIsTutorial())
                GameManager.Instance.TutorialManager.TutorialNumber(6);
        }

        else
        {
            Seed(ingredient);
        }

        SoundManager.Instance?.ButtonSound((int)ButtonSoundType.Bbang);
    }

    public override void UpdateUI()
    {
        harvestButton.gameObject.SetActive(GameManager.Instance.CurrentUser.GetIsIngredientsHave()[index]);
    }

    private void Harvest(Ingredient ingredient)
    {
        isHarvest = false;
        GameManager.Instance.GetIngredients()[index].AddAmount(ingredient.GetAmount());
        GameManager.Instance.UIManager.UpdateIngredientPanel();
        GameManager.Instance.QuestManager.AddQuestValue(KeyManager.FARMQUEST_INDEX, 1);

        harvestButtonImage.sprite = seedSprite;
        seedFieldImage.sprite = FieldSprite;

        particle.Play();

        isGrow = false;
        isSeed = true;
    }

    private void Seed(Ingredient ingredient)
    {
        int price = ingredient.GetPrice() / 8;

        if (price > GameManager.Instance.CurrentUser.GetMoney())
        {
            GameManager.Instance.UIManager.ErrorMessage("돈이 부족합니다.");
            return;
        }
        GameManager.Instance.CurrentUser.AddUserMoney(-price);
        GameManager.Instance.AddDatasOfDay(DataOfDay.SpentIngredientMoney, price);
        curTime = 0f;
        harvestButtonImage.sprite = seedSprite;
        harvestButton.transform.DOScale(0f, 0.2f);
        isSeed = false;
        FieldChange(0);
    }
    void FieldChange(int index)
    {
        switch (ingredient.name)
        {
            case "마늘":
                seedFieldImage.sprite = garlicSprite[index];
                break;
            case "대파":
                seedFieldImage.sprite = daepaSprite[index];
                break;
            case "청양고추":
                seedFieldImage.sprite = pepperSprite[index];
                break;
            default:

                break;
        }
    }

    int GetIndex()
    {
        switch (ingredient.name)
        {
            case "대파":
                return 0;
            case "마늘":
                return 1;
            case "청양고추":
                return 2;
            default:

                break;
        }

        return -1;
    }
}
