using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class DropSlot_Caterpillar : MonoBehaviour, IDropHandler
{



    private float _elapsedTime, _desiredDuration = 0.5f;



    public void OnDrop(PointerEventData eventData)
    {
        Draggable_Caterpillar drag = eventData.pointerDrag.GetComponent<Draggable_Caterpillar>();

        if (drag != null)
        {
            if (drag.name == gameObject.name)
            {
                drag.isDropped = true;
                GetComponentInChildren<ParticleSystem>().Play();
                StartCoroutine(IENUM_LerpTransform(drag.rectTransform, drag.rectTransform.anchoredPosition, GetComponent<RectTransform>().anchoredPosition));
            }
            else
            {



            }

        }

    }


    IEnumerator IENUM_LerpTransform(RectTransform obj, Vector3 currentPosition, Vector3 targetPosition)
    {
        while (_elapsedTime < _desiredDuration)
        {
            _elapsedTime += Time.deltaTime;
            float percentageComplete = _elapsedTime / _desiredDuration;

            obj.anchoredPosition = Vector3.Lerp(currentPosition, targetPosition, percentageComplete);
            yield return null;
        }

        //setting parent
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector2.zero;

        //resetting elapsed time back to zero
        _elapsedTime = 0f;
    }

}
