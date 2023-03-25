using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        Snail_Word_Game_Main.Instance.OnClickTile(gameObject);
    }

    public static implicit operator Word_Tile(GameObject v)
    {
        return v.GetComponent<Word_Tile>();
    }
}
