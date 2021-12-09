using System;
using MinLibrary;
using UnityEngine;
using UnityEngine.UI;

public class CalculatePanel : PanelBase
{
    [SerializeField] Text nameText;
    [SerializeField] Text moneyText;
    DataOfDay data;
    private int index;

    public override void SetValue(int index)
    {
        this.index = index;
        data = MinConvert.FindEnumByInt<DataOfDay>(index);
        UpdateUI();
    }

    public override void UpdateUI()
    {
        string sign = "+";
        nameText.text = GameManager.Instance.calculatedFactors[index];

        if (data.ToString().Contains(KeyManager.SPENT_KEY))
        {
            sign = "-";
        }

        moneyText.text = string.Format("{0}{1}", sign, GameManager.Instance.GetDataofDay(index));
    }

    public override int GetMoney()
    {
        return int.Parse(moneyText.text);
    }
}
