using UnityEngine;
using System;


namespace ColorGame
{

    public class ColorPicker : MonoBehaviour
    {

        public static Action<int> OnColorPicked;

        private int pickedColorIndex;


        public void BUT_PickColor(int index)
        {
            pickedColorIndex = index;

            //publishing event
            OnColorPicked.Invoke(index);
        }

    }

}