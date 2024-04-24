using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using CaterpillarSortingGame;


public class DropSlot_Caterpillar : MonoBehaviour, IDropHandler
{
    private float _elapsedTime, _desiredDuration = 0.5f;
    private static int answerCount = 0;
    private CaterpillarGameManager REF_CaterpillarGameManager;
    private QandA REF_QandA;
    private string emptyString = " ";
    

    void Start()
    {
        REF_CaterpillarGameManager = FindObjectOfType<CaterpillarGameManager>();
        REF_QandA = FindObjectOfType<QandA>();
    }


    public void OnDrop(PointerEventData eventData)
    {
        Draggable_Caterpillar drag = eventData.pointerDrag.GetComponent<Draggable_Caterpillar>();

        if (drag != null)
        {
            if (drag.name == gameObject.name)
            {
                //*correct answer
                drag.isDropped = true;
                answerCount++;

                AudioManager.Instance.PlayCorrect();
                GetComponentInChildren<ParticleSystem>().Play();
                StartCoroutine(IENUM_LerpTransform(drag.rectTransform, drag.rectTransform.anchoredPosition, GetComponent<RectTransform>().anchoredPosition));
                gameObject.GetComponent<DropSlot_Caterpillar>().enabled = false;
                REF_CaterpillarGameManager.IncrementPoints();
                REF_CaterpillarGameManager.STR_currentSelectedAnswer += drag.name + emptyString;

                if (answerCount == 6)
                {
                    AudioManager.Instance.PlayGameWonMusic();
                    REF_QandA.Invoke("SpawnCoins", 1f);
                    REF_QandA.StartCoroutine(REF_QandA.IENUM_CaterpillarOut());
                    REF_CaterpillarGameManager.UpdateScore(4.5f);
                    answerCount = 0;

                    // REF_CaterpillarGameManager.STR_currentSelectedAnswer = "";
                }
            }
            else
            {
                //!wrong answer
                AudioManager.Instance.PlayWrong();
                REF_CaterpillarGameManager.DecrementPoints();
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

