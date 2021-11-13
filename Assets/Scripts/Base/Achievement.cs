using System.Collections.Generic;

[System.Serializable]
public class Achievement
{
    public int achievementType;
    public int achieveCount;
    public string achievementInfo;
    public List<string> achievementNames = new List<string>();
    public List<int> rewards = new List<int>();
    public List<int> conditions = new List<int>();

    public Achievement(int type, int cnt, string info)
    {
        achievementType = type;
        achieveCount = cnt;
        achievementInfo = info;
    }

    public void AddData(string name, int condition, int reward)
    {
        achievementNames.Add(name);
        rewards.Add(reward);
        conditions.Add(condition);
    }

    public void OnClickRewardButton()
    {

    }
}

