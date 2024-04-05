using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace CaterpillarSortingGame
{

    public class Draggable_Caterpillar : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [HideInInspector] public RectTransform rectTransform;
        [HideInInspector] public bool isDropped;

        private Canvas canvas;
        private Image img;

        private float _elapsedTime, _desiredDuration = 0.5f;
        private Vector2 _initialPos;


        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            img = GetComponent<Image>();

            _initialPos = rectTransform.anchoredPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _initialPos = rectTransform.anchoredPosition;
            img.raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Update the position of the dragged object based on the mouse position
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isDropped)
            {
                StartCoroutine(IENUM_LerpTransform(rectTransform, rectTransform.anchoredPosition, _initialPos));
            }

            img.raycastTarget = true;
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

            //resetting elapsed time back to zero
            _elapsedTime = 0f;
        }


    }

}