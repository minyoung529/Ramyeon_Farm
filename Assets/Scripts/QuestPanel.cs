using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : PanelBase
{
    private Quest quest;

    [SerializeField] private Text questNameText;

    [SerializeField] private Button rewardButton;
    private Image questRewardButtonImage;

    [SerializeField] private Image questRewardImage;
    private Text questRewardText;

    private Slider questSlider;
    private Text sliderText;

    private void Awake()
    {
        //GetComponentInChildren => 자신 포함 자식 중에 해당 컴포넌트를 가져옴
        questRewardText = rewardButton.GetComponentInChildren<Text>();
        questSlider = GetComponentInChildren<Slider>();
        sliderText = questSlider.GetComponentInChildren<Text>();
        questRewardButtonImage = rewardButton.GetComponent<Image>();
        rewardButton.onClick.AddListener(() => RewardButton());
    }

    public override void SetValue(int index)
    {
        quest = GameManager.Instance.CurrentUser.questList[index];
        questSlider.maxValue = quest.maxValue;
        UpdateUI();
    }

    public override void UpdateUI()
    {
        questNameText.text = quest.questName;
        questRewardText.text = quest.reward.ToString();
        questSlider.value = quest.GetCurValue();
        sliderText.text = string.Format("{0} / {1}", quest.GetCurValue(), quest.maxValue);

        if(quest.isPerform)
        {
            questRewardButtonImage.color = Color.gray;
        }
        else
        {
            questRewardButtonImage.color = Color.white;
        }
    }

    public void RewardButton()
    {
        if (quest.isPerform) return;
    }
}
