using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    private AudioSource bgmAudio;
    private AudioSource effectSoundAudio;

    [SerializeField] private AudioClip lobbyBGM;
    [SerializeField] private AudioClip gameBGM;

    [SerializeField] private AudioClip ddiringSound;
    [SerializeField] private AudioClip coinSound;

    [SerializeField] private AudioClip[] buttonSounds;
    [SerializeField] private AudioClip[] ingredientSounds;


    private void Awake()
    {
        DontDestroyOnLoad(this);

        bgmAudio = GetComponent<AudioSource>();
        effectSoundAudio = transform.GetChild(0).GetComponent<AudioSource>();

        LobbySound();
    }

    public void DdiringSound()
        => effectSoundAudio.PlayOneShot(ddiringSound);

    public void CoinSound()
        => effectSoundAudio.PlayOneShot(coinSound);

    public void ButtonSound(int num)
=> effectSoundAudio.PlayOneShot(buttonSounds[num]);

    public void LobbySound()
    {
        bgmAudio.clip = lobbyBGM;
        bgmAudio.Play();
    }

    public void GameSound()
    {
        bgmAudio.clip = gameBGM;
        bgmAudio.Play();
    }

    public void PlayIngredientSound(int index)
    {
        effectSoundAudio.PlayOneShot(ingredientSounds[index]);
    }
}
