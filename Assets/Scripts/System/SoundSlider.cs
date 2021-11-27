using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField]
    private int index;
    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void SetVolume()
    {
        SoundManager.Instance.SetVolume(index, slider.value);
    }
}
