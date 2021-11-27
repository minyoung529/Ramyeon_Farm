using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] ButtonSoundType type;

    public void OnPointerUp(PointerEventData eventData)
    {
        SoundManager.Instance?.ButtonSound((int)type);
    }
}

public enum ButtonSoundType
{
    BpSound,
    CloseSound,
    WaterSound,
    PopSound,
    Bbang,
    Count
}
