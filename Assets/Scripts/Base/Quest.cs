using UnityEngine;

/* ����Ʈ ���̽�,
�̰� ������ User���� ����Ʈ�� ���� ����
��� ���̽�, ������ ���̽��� ���*/

[System.Serializable]
public class Quest
{
    public int index;
    //����Ʈ �̸�
    public string questName;
    public string info;
    //curVal == maxVal�� �� ����Ʈ �޼�
    public int maxValue;
    public int currentValue = 0;

    // ������ Ÿ��(��, ���, ���� ��)
    public RewardType rewardType;

    //���� ����
    public int reward;
    public string ingredientName;

    //������ ���� �� ���� �� Ȱ��ȭ�Ǵ� �Ұ�
    public bool isPerform;

    //������ �� �޾��� �� Ȱ��ȭ�Ǵ� �Ұ�
    public bool isRewarded;

    //currentValue �������ִ� �Լ�
    public void SetCurrentValue(int curVal)
    {
        currentValue = curVal;
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        CheckQuest();
    }

    //currentValue�� �μ���ŭ �����ִ� �Լ�
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

    //����Ʈ ���� Ȯ�����ִ� �Լ�
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