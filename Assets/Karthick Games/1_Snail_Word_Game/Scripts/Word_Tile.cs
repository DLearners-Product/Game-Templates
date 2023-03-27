using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Word_Tile : MonoBehaviour
{
    public bool isPressed;
    public string letter;

    private Color defaultColor;


    private void Start()
    {
        isPressed = false;
        letter = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        defaultColor = gameObject.GetComponent<Image>().color;
    }

    public void OnClickTile()
    {

        if (isPressed)
        {
            if (Snail_Word_Game_Main.Instance.currWordStack.Peek() == this)
            {
                isPressed = false;
                gameObject.GetComponent<Image>().color = defaultColor;
                Snail_Word_Game_Main.Instance.RemoveFromStack();
            }
        }
        else
        {
            isPressed = true;
            gameObject.GetComponent<Image>().color = Color.green;
            Snail_Word_Game_Main.Instance.AddToStack(this);
        }
    }


}
