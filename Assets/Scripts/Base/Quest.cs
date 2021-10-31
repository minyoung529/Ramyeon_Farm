
/* ����Ʈ ���̽�,
�̰� ������ User���� ����Ʈ�� ���� ����
��� ���̽�, ������ ���̽��� ���*/

[System.Serializable]
public class Quest
{
    //����Ʈ �̸�
    public string questName;

    //curVal == maxVal�� �� ����Ʈ �޼�
    public int maxValue;
    int currentValue = 0;

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
        CheckQuest();
    }

    //currentValue�� �μ���ŭ �����ִ� �Լ�
    public void AddCurrentValue(int curVal)
    {
        currentValue += curVal;
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
}