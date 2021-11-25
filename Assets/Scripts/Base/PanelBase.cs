using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    public virtual void SetValue(int index) { }

    public virtual void UpdateUI() { }

    public virtual void SetState(BookType bookType) { }

    public virtual void OnClick() { }

    public virtual bool CheckIsUpdate() { return false; }
}