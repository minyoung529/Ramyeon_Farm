using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    [SerializeField] private GameObject quitPanel;

    public void GoMainScene()
    {
        SceneManager.LoadScene(KeyManager.GAME_SCENE);
        SoundManager.Instance?.GameSound();
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            quitPanel.SetActive(!quitPanel.activeSelf);
        }
    }
}