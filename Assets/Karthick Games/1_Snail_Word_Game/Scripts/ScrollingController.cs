using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;


public class ScrollingController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject objectToScroll;
    public float scrollSpeed = 1.0f;
    public List<float> topScrollLimits; // Dynamic list of top scroll limits
    public List<float> bottomScrollLimits; // Dynamic list of bottom scroll limits


    [SerializeField] private ParticleSystem upParticles;
    [SerializeField] private ParticleSystem downParticles;


    [SerializeField] private SnailGameManager REF_SnailGameManager;


    private bool scrollingUp = false;
    private bool scrollingDown = false;
    private Vector3 lastScrollPosition;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter.name == "UpButton")
        {
            scrollingUp = true;
            scrollingDown = false;
            upParticles.Play();
        }
        else if (eventData.pointerEnter.name == "DownButton")
        {
            scrollingDown = true;
            scrollingUp = false;
            downParticles.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scrollingUp = false;
        scrollingDown = false;
        lastScrollPosition = objectToScroll.transform.position;
    }

    void Update()
    {
        if (scrollingUp)
        {
            if (objectToScroll.transform.position.y < topScrollLimits[REF_SnailGameManager.I_GridCategory])
            {
                objectToScroll.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            }
        }
        else if (scrollingDown)
        {
            if (objectToScroll.transform.position.y > bottomScrollLimits[REF_SnailGameManager.I_GridCategory])
            {
                objectToScroll.transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);
            }
        }
    }


    public void BUT_Click()
    {
        StartCoroutine(IENUM_AdjustSpeed());
        SnailWordGame.AudioManager.Instance.PlayButtonClick();
    }


    IEnumerator IENUM_AdjustSpeed()
    {
        scrollSpeed = 20f;
        yield return new WaitForSeconds(0.2f);
        scrollSpeed = 2.5f;
    }




}
