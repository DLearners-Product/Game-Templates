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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject != gameObject){
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     // if(!GameObject.ReferenceEquals(other.gameObject, gameObject)){
    //     Debug.Log($"{other.gameObject.name} - {other.gameObject.GetInstanceID() == gameObject.GetInstanceID()} other : {other.gameObject.GetInstanceID()} current : {gameObject.GetInstanceID()}", gameObject);
    //     // Debug.Log();
    //     // }
    // }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject != gameObject)
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void CheckBelowObjectDestroyed(){

    }

}
