using UnityEngine;

/* 퀘스트 베이스,
이걸 가지고 User에서 리스트로 만들어서 관리
재료 베이스, 레시피 베이스랑 비슷*/

[System.Serializable]
public class Quest
{
    public int index;
    //퀘스트 이름
    public string questName;
    public string info;
    //curVal == maxVal일 때 퀘스트 달성
    public int maxValue;
    public int currentValue = 0;

    // 리워드 타입(돈, 재료, 가구 등)
    public RewardType rewardType;

    //보상 변수
    public int reward;
    public string ingredientName;

    //보상을 받을 수 있을 때 활성화되는 불값
    public bool isPerform;

    //보상을 다 받았을 때 활성화되는 불값
    public bool isRewarded;

    //currentValue 설정해주는 함수
    public void SetCurrentValue(int curVal)
    {
        currentValue = curVal;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        CheckQuest();
    }

    //currentValue를 인수만큼 더해주는 함수
    public void AddCurrentValue(int curVal)
    {
        currentValue += curVal;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        CheckQuest();
    }

    public int GetCurValue()
    {
        return currentValue;
    }

    //퀘스트 조건 확인해주는 함수
    private void CheckQuest()
    {
        GameManager.Instance.UIManager.UpdateQuestPanel();

        if (currentValue >= maxValue)
        {
            isPerform = true;
        }
    }

    public void ResetQuest()
    {
        currentValue = 0;
        isRewarded = false;
        isPerform = false;
    }
}