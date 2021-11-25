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
        //GetComponentInChildren => 자신 포함 자식 중에 해당 컴포넌트를 가져옴
        questRewardText = rewardButton.GetComponentInChildren<Text>();
        sliderText = questSlider.GetComponentInChildren<Text>();

        questRewardButtonImage = rewardButton.GetComponent<Image>();

        //버튼 OnClick 이벤트에 넣어주는 거. 람다식으로 Find랑 DOTween OnComplete랑 비슷함
        //입걸 하면 해당 버튼을 누를 때 이 함수가 실행됨 처음부터 해주는거야
        rewardButton.onClick.AddListener(() => RewardButton());
    }

    // 퀘스트 값 넣어주는 함수
    public override void SetValue(int index)
    {
        this.index = index;
        quest = GameManager.Instance.CurrentUser.questList[GameManager.Instance.QuestManager.GetQuestListIndex(index)];
        questSlider.maxValue = quest.maxValue;
        UpdateUI();
    }

    // UI 업데이트해주는 함수
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

    //보상 버튼에 넣어줄 함수
    public void RewardButton()
    {
        if (!quest.isPerform) return;
        if (quest.isRewarded) return;

        Reward();
    }

    // 직접 보상을 받는 함수
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