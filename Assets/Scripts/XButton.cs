using UnityEngine;
using UnityEngine.UI;

public class XButton : MonoBehaviour
{
    [SerializeField] GameObject blackPanel;
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => blackPanel.gameObject.SetActive(false));
    }
}
