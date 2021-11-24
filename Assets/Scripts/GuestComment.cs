using System.Collections.Generic;
using UnityEngine;

public class GuestComment : MonoBehaviour
{
    private List<string> orderComments = new List<string>();
    private List<string> removeComments = new List<string>();
    private List<string> addComments = new List<string>();

    private List<string> goodComments = new List<string>();
    private List<string> commonComments = new List<string>();
    private List<string> badComments = new List<string>();

    public void SetAllComments()
    {
        if (orderComments.Count == 0)
        {
            SetOrderComments();
            SetRemoveComments();
            SetPlusComments();
            SetGoodComments();
            SetBadComments();
            SetCommonComments();
        }
    }

    private void SetOrderComments()
    {
        orderComments.Add("{0}�� ����� ���̳׿�.");
        orderComments.Add("{0}�� ��ĥ���� �����󱸿�.");
        orderComments.Add("{0} �ֳ���?");
        orderComments.Add("���� ���� ������ {0}!");
        orderComments.Add("�ְ��� ����� {0}�� �ƴұ��?");
        orderComments.Add("{0}.");
        orderComments.Add("{0} ���.");
        orderComments.Add("{0} �ֽö���.");
        orderComments.Add("{0} �޶�ϱ��?");
        orderComments.Add("����! {0}!");
        orderComments.Add("{0} ��!!!!!!");
    }
    private void SetRemoveComments()
    {
        removeComments.Add(" {0} ���ּ���~");
        removeComments.Add(" {0} �����!!");
        removeComments.Add(" ��ġ�� {0} �Ⱦ��ؿ�");
        removeComments.Add(" �� {0} �Ⱦ��ؿ�.");
        removeComments.Add(" �ٵ� {0}... ���δ�����");
        removeComments.Add(" ������ {0} ����ؿ�!");
    }
    private void SetPlusComments()
    {
        addComments.Add("�� �߰���~");
        addComments.Add("�߰���~");
        addComments.Add("�� �־��ֽǷ���?");
        addComments.Add("�� �־���!!!!");
        addComments.Add("�� �ּ���!!!!");
        addComments.Add("�� ��!!!!");
        addComments.Add("�� �ּž��ϴ� �� ����?");
        addComments.Add("�� �ּ���~");
        addComments.Add("���� �ʿ��մϴ�.");
        addComments.Add("�� �ʿ��ؿ�!");
    }
    private void SetGoodComments()
    {
        goodComments.Add("�ʹ� ���ƿ�!!!");
        goodComments.Add("���� �Ϻ��� ����̾�!");
        goodComments.Add("�� ����� �� ó������ ������ ����̿���.");
        goodComments.Add("��.¥ ��.��.��.��.��");
        goodComments.Add("��~~ ���ִµ���?");
        goodComments.Add("��ȥ�����");
        goodComments.Add("�� ���� ��� �丮�簡 �Ǿ��ּ���.");
        goodComments.Add("�� �����Ž�~~");
        goodComments.Add("�����¡... �ʹ� �����ݾƿ�!!");
        goodComments.Add("���־�!!!!!!");
    }
    private void SetBadComments()
    {
        badComments.Add("�̰� ����...?");
        badComments.Add("�� ��!! �Ű��Ұž�!!!");
        badComments.Add("��¥ ȭ���� ���� ������.");
        badComments.Add("�� ���� 3�ϸ��� ���´ٿ� �鸸�� �̴ϴ�.");
        badComments.Add("����!! �̰� ����!!");
        badComments.Add("������ �ֱⰡ �� �� ���̰ڳ׿�.");
        badComments.Add("�� ����Ͻô� �ǰ���. �ߴ۰� �ֹ�����.");
        badComments.Add("��� �丮��... ����� �����Ű�����...");
        badComments.Add("�ٽ� �� ��!!!");
        badComments.Add("������!!!!!!!!");
        badComments.Add("������ ���� ����Ѵٸ�, ���� �� ���� �Ű��ϰھ��.");
    }
    private void SetCommonComments()
    {
        commonComments.Add("�׳� �׷��׿�...");
        commonComments.Add("�׳� �׷���.");
        commonComments.Add("��... ����� �ϼ̱���.");
        commonComments.Add("����... ������...!");
        commonComments.Add("������ ���� ������ ���ְڳ׿�.");
        commonComments.Add("�����ؿ�, �׳�.");
        commonComments.Add("���� ���� ���� �ʴ� ��������... �̰�...");
        commonComments.Add("������ �� �� ������!!");
        commonComments.Add("......");
    }

    public string GetOrderComments()
    {
        SetAllComments();
        return orderComments[Random.Range(0, orderComments.Count)];
    }
    public string GetAddComments()
    {
        SetAllComments();
        return addComments[Random.Range(0, addComments.Count)];
    }
    public string GetRemoveComments()
    {
        SetAllComments();
        return removeComments[Random.Range(0, removeComments.Count)];
    }
    public string GetGoodComments()
    {
        SetAllComments();
        return goodComments[Random.Range(0, goodComments.Count)];
    }
    public string GetBadComments()
    {
        SetAllComments();
        return badComments[Random.Range(0, badComments.Count)];
    }
    public string GetCommonComments()
    {
        SetAllComments();
        return commonComments[Random.Range(0, commonComments.Count)];
    }
}
