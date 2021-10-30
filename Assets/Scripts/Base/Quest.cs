
/* 퀘스트 베이스,
이걸 가지고 User에서 리스트로 만들어서 관리
재료 베이스, 레시피 베이스랑 비슷*/

[System.Serializable]
public class Quest
{
    //퀘스트 이름
    public string questName;

    //curVal == maxVal일 때 퀘스트 달성
    public int maxValue;
    int currentValue = 0;

    // 리워드 타입(돈, 재료, 가구 등)
    public RewardType rewardType;

    //보상 변수
    public int reward;

    public bool isPerform;

    //currentValue 설정해주는 함수
    public void SetCurrentValue(int curVal)
    {
        currentValue = curVal;
    }

    //currentValue를 인수만큼 더해주는 함수
    public void AddCurrentValue(int curVal)
    {
        currentValue += curVal;
    }

    public int GetCurValue()
    {
        return currentValue;
    }
}