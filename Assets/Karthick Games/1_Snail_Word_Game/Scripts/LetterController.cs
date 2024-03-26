using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LetterController : MonoBehaviour
{

    [SerializeField] private Sprite[] SPRA_Tiles;



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
            REF_SnailGameManager.RemoveCoinPos();
            GetComponentInChildren<Image>().sprite = SPRA_Tiles[0];
            REF_SnailGameManager.RemoveLetter();
            isClicked = false;

            GetComponentInChildren<Button>().interactable = true;
        }
        else
        {
            REF_SnailGameManager.AddCoinPos(transform);
            GetComponentInChildren<Image>().sprite = SPRA_Tiles[1];
            REF_SnailGameManager.AddLetter(gameObject);
            isClicked = true;
        }

        AudioManager.Instance.PlayLetterClick();
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


    public void PlayParticles()
    {
        GetComponentInChildren<ParticleSystem>().Play();
    }


    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }



}
