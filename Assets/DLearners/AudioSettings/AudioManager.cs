using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DLearners
{

    public class AudioManager : MonoGenericSingleton<AudioManager>
    {
       
    #region =======================================user input=======================================
    public float Initial_Music_Value = 0.1f;
    public float Initial_SFX_Value = 0.6f;
    public float Initial_VO_Value = 1f;

    //!end of region - user input
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion


    #region =======================================unity reference variables=======================================


    [Header("IMAGE---------------------------------------------------------")]

    [SerializeField] private AudioSource AS_Music;
    [SerializeField] private AudioSource AS_SFX;
    [SerializeField] private AudioSource AS_Voice;

    [Space(10)]
    [Header("SPRITE---------------------------------------------------------")]
    [SerializeField] private Sprite[] SPRA_Music;
    [SerializeField] private Sprite[] SPRA_SFX;
    [SerializeField] private Sprite[] SPRA_Voice;

    [Space(10)]
    [Header("IMAGE---------------------------------------------------------")]

    [SerializeField] private Image IMG_Music;
    [SerializeField] private Image IMG_SFX;
    [SerializeField] private Image IMG_Voice;
    [SerializeField] private Image IMG_FillMusic;
    [SerializeField] private Image IMG_FillSFX;
    [SerializeField] private Image IMG_FillVoice;

    [Space(10)]
    [Header("LINE RENDERER---------------------------------------------------------")]
    [SerializeField] private Slider SL_Music;
    [SerializeField] private Slider SL_SFX;
    [SerializeField] private Slider SL_Voice;

    [Space(10)]
    [Header("GRADIENT---------------------------------------------------------")]
    [SerializeField] private Gradient GR_Slider;


    [Header("GAME OBJECT---------------------------------------------------------")]
    [SerializeField] private GameObject G_AudioSettingsLayout;


    //!end of region - unity reference variables
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion


    #region =======================================local variables=======================================
    private float elapsedTime, desiredDuration = 0.5f;
    private float musicFillValue, sfxFillValue, voiceFillValue;
    private Color32 musicColor, sfxColor, voiceColor;
    private bool isNotActive = true;


    //!end of region - local variables-------------------------------------------------------------------
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    #endregion


    public void THI_ResetAudioSettings()
    {
        AS_Music.volume = Initial_Music_Value;
        AS_SFX.volume = Initial_SFX_Value;
        AS_Voice.volume = Initial_VO_Value;

        SL_Music.value = Initial_Music_Value;
        SL_SFX.value = Initial_SFX_Value;
        SL_Voice.value = Initial_VO_Value;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            THI_AudioSettings();

        }
    }


    public void THI_AudioSettings()
    {
        if (isNotActive)
        {
            isNotActive = false;
            G_AudioSettingsLayout.SetActive(false);
        }
        else
        {
            isNotActive = true;
            G_AudioSettingsLayout.SetActive(true);
        }
    }


    void Start()
    {
        THI_ResetAudioSettings();

        SL_Music.onValueChanged.AddListener(OnMusicSliderValueChanged);
        SL_SFX.onValueChanged.AddListener(OnSFXSliderValueChanged);
        SL_Voice.onValueChanged.AddListener(OnVoiceSliderValueChanged);

        UpdateColor(SL_Music.value, IMG_Music, IMG_FillMusic);
        UpdateColor(SL_SFX.value, IMG_SFX, IMG_FillSFX);
        UpdateColor(SL_Voice.value, IMG_Voice, IMG_FillVoice);
    }


    void OnMusicSliderValueChanged(float value)
    {
        if (value == 0)
        {
            IMG_Music.sprite = SPRA_Music[0];
        }
        else
        {
            IMG_Music.sprite = SPRA_Music[1];
        }

        UpdateColor(value, IMG_Music, IMG_FillMusic);
        UpdateVolume(value, AS_Music);
    }


    void OnSFXSliderValueChanged(float value)
    {
        if (value == 0)
        {
            IMG_SFX.sprite = SPRA_SFX[0];
        }
        else
        {
            IMG_SFX.sprite = SPRA_SFX[1];
        }

        UpdateColor(SL_SFX.value, IMG_SFX, IMG_FillSFX);
        UpdateVolume(value, AS_SFX);
    }


    void OnVoiceSliderValueChanged(float value)
    {
        if (value == 0)
        {
            IMG_Voice.sprite = SPRA_Voice[0];
        }
        else
        {
            IMG_Voice.sprite = SPRA_Voice[1];
        }

        UpdateColor(SL_Voice.value, IMG_Voice, IMG_FillVoice);
        UpdateVolume(value, AS_Voice);
    }


    void UpdateVolume(float value, AudioSource audioSource)
    {
        audioSource.volume = value;
    }


    void UpdateColor(float value, Image icon, Image fill)
    {
        Color color = GR_Slider.Evaluate(value);

        icon.color = color;
        fill.color = color;
    }


    public void PlayMusic(AudioClip clip)
    {
        AS_Music.clip = clip;
        AS_Music.Play();
    }


    public void PlaySFX(AudioClip clip)
    {
        AS_SFX.PlayOneShot(clip);
    }


    public void PlayVoice(AudioClip clip)
    {
        AS_Voice.PlayOneShot(clip);
    }


    public void StopVoice()
    {
        AS_Voice.Stop();
    }

    public void StopSFX()
    {
        AS_SFX.Stop();
    }

    public void StopMusic()
    {
        AS_Music.Stop();
    }




    }

}