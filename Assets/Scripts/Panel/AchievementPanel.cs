using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPanel : PanelBase
{
    Achievement achievement;

    [SerializeField] Text nameText;
    [SerializeField] Text infoText;
    [SerializeField] Text rewardText;

    [SerializeField] Image star;

    private List<Image> starList = new List<Image>();

    private Button rewardButton;
    private int currentLevel = 0;
    private int index;

    public override void SetValue(int index)
    {
        this.index = index;
        achievement = GameManager.Instance.QuestManager.GetAchievements()[index];
        currentLevel = GameManager.Instance.CurrentUser.achievementLevel[index];
        rewardButton = rewardText.transform.parent.GetComponent<Button>();
        rewardButton.onClick.AddListener(() => OnClickReward());
        SetStarList();
        UpdateStars();
        UpdateUI();
    }

    public override void UpdateUI()
    {
        currentLevel = GameManager.Instance.CurrentUser.achievementLevel[index];

        if (GameManager.Instance.CurrentUser.IsAchievementReward(index))
        {
            rewardButton.transform.localScale = Vector3.one;
        }

        else
        {
            rewardButton.transform.localScale = Vector3.zero;
        }

        nameText.text = achievement.achievementNames[currentLevel];
        infoText.text = string.Format(achievement.achievementInfo, achievement.conditions[currentLevel]);
        rewardText.text = achievement.rewards[currentLevel].ToString();
        UpdateStars();
    }

    private void SetStarList()
    {
        GameObject obj;

        for (int i = 0; i < achievement.achieveCount; i++)
        {
            obj = Instantiate(star.gameObject, star.transform.parent);
            starList.Add(obj.GetComponent<Image>());
        }

        star.gameObject.SetActive(false);
    }

    private void UpdateStars()
    {
        for (int i = 0; i < starList.Count; i++)
        {
            if (i < currentLevel)
            {
                starList[i].color = Color.white;
            }

            else
            {
                starList[i].color = Color.gray;
            }
        }
    }

    public void OnClickReward()
    {
        User user = GameManager.Instance.CurrentUser;
        user.AddUserMoney(achievement.rewards[user.achievementLevel[index]]);
        GameManager.Instance.CurrentUser.CheckAchievement(index);
    }

    public override bool CheckIsUpdate()
    {
        return GameManager.Instance.CurrentUser.IsAchievementReward(index);
    }
}
