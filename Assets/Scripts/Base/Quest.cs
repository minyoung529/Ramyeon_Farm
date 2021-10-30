
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

    public bool isPerform;

    //currentValue �������ִ� �Լ�
    public void SetCurrentValue(int curVal)
    {
        currentValue = curVal;
    }

    //currentValue�� �μ���ŭ �����ִ� �Լ�
    public void AddCurrentValue(int curVal)
    {
        currentValue += curVal;
    }

    public int GetCurValue()
    {
        return currentValue;
    }
}