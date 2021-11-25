using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : PanelBase
{
    private Quest quest;

    [SerializeField] private Text questNameText;
    [SerializeField] private Text questInfoText;

    [SerializeField] private Button rewardButton;
    private Image questRewardButtonImage;

    [SerializeField] private Image questRewardImage;
    private Text questRewardText;

    [SerializeField] private Image lockImage;

    [SerializeField] private Slider questSlider;
    private Text sliderText;

    private int index;

    private void Awake()
    {
        //GetComponentInChildren => �ڽ� ���� �ڽ� �߿� �ش� ������Ʈ�� ������
        questRewardText = rewardButton.GetComponentInChildren<Text>();
        sliderText = questSlider.GetComponentInChildren<Text>();

        questRewardButtonImage = rewardButton.GetComponent<Image>();

        //��ư OnClick �̺�Ʈ�� �־��ִ� ��. ���ٽ����� Find�� DOTween OnComplete�� �����
        //�԰� �ϸ� �ش� ��ư�� ���� �� �� �Լ��� ����� ó������ ���ִ°ž�
        rewardButton.onClick.AddListener(() => RewardButton());
    }

    // ����Ʈ �� �־��ִ� �Լ�
    public override void SetValue(int index)
    {
        this.index = index;
        quest = GameManager.Instance.CurrentUser.questList[GameManager.Instance.QuestManager.GetQuestListIndex(index)];
        questSlider.maxValue = quest.maxValue;
        UpdateUI();
    }

    // UI ������Ʈ���ִ� �Լ�
    public override void UpdateUI()
    {
        questNameText.text = quest.questName;
        questRewardText.text = quest.reward.ToString();
        questSlider.value = quest.GetCurValue();
        sliderText.text = string.Format("{0} / {1}", quest.GetCurValue(), quest.maxValue);
        questInfoText.text = quest.info;

        questSlider.gameObject.SetActive(!quest.isRewarded);
        rewardButton.gameObject.SetActive(!quest.isRewarded);

        if (quest.isPerform && !quest.isRewarded)
        {
            questRewardButtonImage.color = Color.green;
        }

        else
        {
            questRewardButtonImage.color = Color.gray;
        }

        lockImage.gameObject.SetActive(quest.isRewarded);
    }

    //���� ��ư�� �־��� �Լ�
    public void RewardButton()
    {
        if (!quest.isPerform) return;
        if (quest.isRewarded) return;

        Reward();
    }

    // ���� ������ �޴� �Լ�
    private void Reward()
    {
        if (quest.rewardType == RewardType.money)
        {
            GameManager.Instance.CurrentUser.AddUserMoney(quest.reward);
        }

        else if (quest.rewardType == RewardType.ingredient)
        {
            Ingredient ingredient = GameManager.Instance.GetIngredients().Find(x => x.name == quest.ingredientName);
            ingredient.AddAmount(quest.reward);
        }

        quest.isRewarded = true;

        GameManager.Instance.UIManager.CheckIsUpdateInMenu();
        UpdateUI();
    }

    public override bool CheckIsUpdate()
    {
        return quest.isRewarded;
    }
}