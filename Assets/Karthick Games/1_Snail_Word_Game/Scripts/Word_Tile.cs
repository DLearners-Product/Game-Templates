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


    private void Start()
    {
        isPressed = false;
        letter = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }

    public void OnClickTile()
    {
        if (isPressed)
        {
            if (Snail_Word_Game_Main.Instance.currWordStack.Peek() == this)
            {
                isPressed = false;
                gameObject.GetComponent<Image>().color = Color.white;
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

    public static implicit operator Word_Tile(GameObject v)
    {
        return v.GetComponent<Word_Tile>();
    }

}
