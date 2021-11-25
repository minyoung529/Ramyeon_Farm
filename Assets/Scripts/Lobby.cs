using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public void GoMainScene()
    {
        SceneManager.LoadScene(KeyManager.GAME_SCENE);
        SoundManager.Instance?.GameSound();
    }
}