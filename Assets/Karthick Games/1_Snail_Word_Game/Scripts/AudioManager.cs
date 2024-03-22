using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingleton<AudioManager>
{

    [SerializeField] private AudioSource AS_SFX;
    [SerializeField] private AudioClip AC_ButtonClick;
    [SerializeField] private AudioClip AC_Correct;
    [SerializeField] private AudioClip AC_Wrong;



    public void PlayButtonClick()
    {
        AS_SFX.PlayOneShot(AC_ButtonClick);
    }


    public void PlayCorrect()
    {
        AS_SFX.PlayOneShot(AC_Correct);
    }


    public void PlayWrong()
    {
        AS_SFX.PlayOneShot(AC_Wrong);
    }






}
