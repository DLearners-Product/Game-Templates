using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingleton<AudioManager>
{

    [SerializeField] private AudioSource AS_SFX;
    [SerializeField] private AudioSource AS_Music;
    [SerializeField] private AudioSource AS_Voice;

    [Space(10)]


    [SerializeField] private AudioClip AC_IntroMusic;
    [SerializeField] private AudioClip AC_GameMusic;

    [SerializeField] private AudioClip AC_LetterClick;
    [SerializeField] private AudioClip AC_ButtonClick;
    [SerializeField] private AudioClip AC_Correct;
    [SerializeField] private AudioClip AC_Wrong;
    [SerializeField] private AudioClip AC_Swoosh;
    [SerializeField] private AudioClip AC_Yummy;


    public void PlayIntroMusic()
    {
        AS_Music.PlayOneShot(AC_IntroMusic);
    }


    public void PlayGameMusic()
    {
        AS_Music.clip = AC_GameMusic;
        AS_Music.Play();
        AS_Music.loop = true;
    }


    public void PlayButtonClick()
    {
        AS_SFX.PlayOneShot(AC_ButtonClick);
    }

    public void PlayLetterClick()
    {
        AS_SFX.PlayOneShot(AC_LetterClick);
    }


    public void PlayCorrect()
    {
        AS_SFX.PlayOneShot(AC_Correct);
    }


    public void PlayWrong()
    {
        AS_SFX.PlayOneShot(AC_Wrong);
    }


    public void PlaySwoosh()
    {
        AS_SFX.PlayOneShot(AC_Swoosh);
    }


    public void PlayYummy(float delay)
    {
        AS_Voice.clip = AC_Yummy;
        AS_Voice.PlayDelayed(delay);
    }



}
