using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lotto : MonoBehaviour
{
    int count = 0;
    Button button;
    [SerializeField] Image fillImage;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClickLotto());
    }
    private void OnClickLotto()
    {
        if (count >= KeyManager.LOTTO_MAX_COUNT)
        {
            GameManager.Instance.UIManager.ErrorMessage(string.Format("복권은 하루 {0}회만 가능합니다", KeyManager.LOTTO_MAX_COUNT));
            return;
        }

        GameManager.Instance.UIManager.Lotto();
        fillImage.fillAmount =((float)++count / (float)KeyManager.LOTTO_MAX_COUNT);
    }
}
