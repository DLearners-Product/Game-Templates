using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class DemoController_N : MonoBehaviour
{

    [SerializeField] private AudioSource AS_Voice;
    [SerializeField] private AudioClip[] ACA_VOWebGL;
    [SerializeField] private AudioClip[] ACA_VOMobile;
    [SerializeField] private AudioClip ACA_VOHelp;
    [SerializeField] private TextMeshProUGUI TXT_Controls;



    public string STR_TextWebGL;
    public string STR_TextMobile;


    public AnimationClip AC_demo;
    bool B_CallOnce;
    private List<AudioClip> ACA_VOs = new List<AudioClip>();


    private void Start()
    {
        SetAudio();

        GetComponent<Animator>().speed = 0;
        B_CallOnce = true;
    }


    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == MainController.instance.G_coverPageStart)
        {
            if (B_CallOnce)
            {
                B_CallOnce = false;
                GetComponent<Animator>().speed = 1;
                Invoke("THI_offDemo", AC_demo.length);
            }

        }
    }

    void THI_offDemo()
    {
        gameObject.SetActive(false);

        // Debug.Log("Playing demo");
    }



    private void SetAudio()
    {
        if (MainController.instance.WEB)
        {
            ACA_VOs.Add(ACA_VOWebGL[0]);
            ACA_VOs.Add(ACA_VOWebGL[1]);

            TXT_Controls.text = STR_TextWebGL;
        }
        else if (MainController.instance.MOBILE)
        {
            ACA_VOs.Add(ACA_VOMobile[0]);
            ACA_VOs.Add(ACA_VOMobile[1]);

            TXT_Controls.text = STR_TextMobile;
        }
    }


    public void PlayVO1()
    {
        AS_Voice.clip = ACA_VOs[0];
        AS_Voice.Play();
    }


    public void PlayVO2()
    {
        AS_Voice.clip = ACA_VOs[1];
        AS_Voice.Play();
    }


    public void PlayVOHelp()
    {
        AS_Voice.clip = ACA_VOHelp;
        AS_Voice.Play();
    }



}