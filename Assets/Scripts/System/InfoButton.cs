using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour
{
    [SerializeField] private GameObject d585;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClick());
    }

    public void OnClick()
    {
        d585.SetActive(!d585.activeSelf);
    }
}
