using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ColorGame
{
    public class ColoringManager : MonoBehaviour
    {

        /*         [SerializeField] private GameObject G_PauseWindow;
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
                } */

        [SerializeField] private Color32[] CLRA_PaletteColors;

        [SerializeField] private Color32[] CLRA_QuestionColors;
        [SerializeField] private Image[] IMGA_QuestionsBG;


        [SerializeField] private Transform[] TA_AnswerPlaceholders;



        [SerializeField] private ColorPicker REF_ColorPicker;


        public Color32 pickedColor;
        [HideInInspector] public int pickedColorIndex;



        public Color32 newColor;


        public void ApplyColor()
        {
            IMGA_QuestionsBG[0].color = newColor;
        }



        void Start()
        {
            pickedColorIndex = 0;
            ColorPicker.OnColorPicked += GetColor;
        }


        public void GetColor(int index)
        {
            pickedColorIndex = index;
            pickedColor = CLRA_PaletteColors[pickedColorIndex];
        }


        public Color32 GetCurrentColor()
        {
            return pickedColor;
        }
















        //===================================================================================
        //!Custom inspector functions

        public void PickQ1Color()
        {

        }


    }
}