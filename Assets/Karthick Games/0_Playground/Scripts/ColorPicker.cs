using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace ColorGame
{

    public class ColorPicker : MonoBehaviour
    {

        public static Action<int> OnColorPicked;


        [SerializeField] private Color32[] CLRA_Palette;


        [HideInInspector]
        public Color32 pickedColor;


        public void BUT_PickColor(int index)
        {
            pickedColor = CLRA_Palette[index];
            Debug.Log("color picked " + index);

            //publishing event
            OnColorPicked.Invoke(index);
        }
    }

}