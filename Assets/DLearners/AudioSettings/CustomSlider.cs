using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CustomSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.parent.parent.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.parent.parent.parent.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
    }

}

