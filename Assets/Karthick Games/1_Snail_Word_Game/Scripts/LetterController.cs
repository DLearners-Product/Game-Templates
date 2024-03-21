using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LetterController : MonoBehaviour
{

    private bool isClicked;
    private SnailGameManager REF_SnailGameManager;
    private float elapsedTime, desiredDuration = 0.5f;


    void Start()
    {
        REF_SnailGameManager = FindObjectOfType<SnailGameManager>();
        isClicked = false;
    }

    public void BUT_Click()
    {
        if (isClicked)
        {
            GetComponentInChildren<Image>().color = Color.white;
            REF_SnailGameManager.RemoveLetter();
            isClicked = false;

            GetComponentInChildren<Button>().interactable = true;
        }
        else
        {
            GetComponentInChildren<Image>().color = Color.green;
            REF_SnailGameManager.AddLetter(gameObject);
            isClicked = true;
        }
    }


    public void Move(Vector3 currentPos, Vector3 targetPos)
    {
        StartCoroutine(IENUM_LerpMoveTile(currentPos, targetPos));
    }


    IEnumerator IENUM_LerpMoveTile(Vector3 currentPosition, Vector3 newPosition)
    {
        while (elapsedTime < desiredDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;

            transform.position = Vector3.Lerp(currentPosition, newPosition, percentageComplete);
            yield return null;
        }

        //resetting elapsed time back to zero
        elapsedTime = 0f;
    }











}
