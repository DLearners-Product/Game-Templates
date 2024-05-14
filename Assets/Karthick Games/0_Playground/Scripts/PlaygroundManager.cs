using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Playground
{
    public class PlaygroundManager : MonoBehaviour
    {



        [SerializeField] private GameObject G_PauseWindow;
        [SerializeField] private GameObject G_PauseWindowBG;




        private bool isPauseWindowActive = false;




        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPauseWindowActive)
                {
                    isPauseWindowActive = false;
                    G_PauseWindow.SetActive(false);
                    G_PauseWindowBG.SetActive(false);
                }
                else
                {
                    isPauseWindowActive = true;
                    G_PauseWindow.SetActive(true);
                    G_PauseWindowBG.SetActive(true);
                }

            }
        }














    }
}