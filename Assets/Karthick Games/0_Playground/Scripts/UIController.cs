using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playground
{
    public class UIController : MonoBehaviour
    {

        [SerializeField] private Animator[] ANIMA_SubSettingsButtonAnim;



        [SerializeField] private GameObject G_PauseWindowBG;
        [SerializeField] private GameObject G_PauseWindow;
        [SerializeField] private GameObject G_SettingsWindow;
        [SerializeField] private GameObject[] GA_SubSettings;


        private bool isPauseWindowActive = false;
        private bool isSettingsWindowActive = false;
        private Animator lastClickedSubSettingsButtonAnim = null;


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isSettingsWindowActive)
                {
                    BUT_Back_SettingsWindow();
                }
                else if (isPauseWindowActive)
                {
                    BUT_Back_PauseWindow();
                }
                else
                {
                    BUT_Pause();
                }
            }
        }


        public void BUT_Pause()
        {
            G_PauseWindowBG.SetActive(true);
            G_PauseWindow.SetActive(true);
            isPauseWindowActive = true;
            isSettingsWindowActive = false;
        }


        public void BUT_Settings()
        {
            G_SettingsWindow.SetActive(true);
            BUT_GeneralSettings();
            isSettingsWindowActive = true;
            isPauseWindowActive = false;
        }


        public void BUT_Back_PauseWindow()
        {
            G_PauseWindowBG.SetActive(false);
            G_PauseWindow.SetActive(false);
            isPauseWindowActive = false;
        }


        public void BUT_Back_SettingsWindow()
        {
            G_SettingsWindow.SetActive(false);
            isSettingsWindowActive = false;
        }


        private void DisableSettingsWindow()
        {
            foreach (GameObject subSetting in GA_SubSettings)
            {
                subSetting.SetActive(false);
            }
        }


        public void BUT_GeneralSettings()
        {
            OnClickSubSettingsButton(0);
        }


        public void BUT_VideoSettings()
        {
            OnClickSubSettingsButton(1);
        }


        public void BUT_AudioSettings()
        {
            OnClickSubSettingsButton(2);
        }

        public void BUT_MiscSettings()
        {
            OnClickSubSettingsButton(3);
        }

        public void BUT_AboutSettings()
        {
            OnClickSubSettingsButton(4);
        }


        private void OnClickSubSettingsButton(int index)
        {

            if (lastClickedSubSettingsButtonAnim != ANIMA_SubSettingsButtonAnim[index])
            {
                DisableSettingsWindow();

                GA_SubSettings[index].SetActive(true);

                if (lastClickedSubSettingsButtonAnim != null)
                    lastClickedSubSettingsButtonAnim.SetTrigger("inactive");

                lastClickedSubSettingsButtonAnim = ANIMA_SubSettingsButtonAnim[index];
                lastClickedSubSettingsButtonAnim.SetTrigger("active");
            }
        }




    }
}
