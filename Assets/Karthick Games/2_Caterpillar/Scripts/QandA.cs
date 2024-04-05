using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CaterpillarSortingGame
{

    public class QandA : MonoBehaviour
    {

        [SerializeField] private GameObject[] GA_Slots;
        [SerializeField] private GameObject[] GA_Draggables;
        [SerializeField] private GameObject G_CoinPrefab;


        private CaterpillarGameManager REF_CaterpillarGameManager;


        private List<int> GA_Questions;             //draggable
        private List<int> GA_SortedQuestions;       //slot

        private float _elapsedTime, _desiredDuration = 0.5f;


        void Start()
        {
            REF_CaterpillarGameManager = GameObject.FindObjectOfType<CaterpillarGameManager>();
            PrepareQuestions();
            SetSlotData();
            SetDraggableData();
            StartCoroutine(IENUM_DisableDraggableAnimation());
        }


        private void PrepareQuestions()
        {
            GA_Questions = new List<int>();
            GA_SortedQuestions = new List<int>();

            for (int i = 0; i < GA_Slots.Length; i++)
            {
                int randomNum = Random.Range(1, 10);
                GA_Questions.Add(randomNum);
                GA_SortedQuestions.Add(randomNum);
            }

            //ascending order
            GA_SortedQuestions.Sort();

            //descending order
            // GA_SortedQuestions.Sort((a, b) => b.CompareTo(a));
        }


        private void SetSlotData()
        {
            //sorted
            for (int i = 0; i < GA_Slots.Length; i++)
            {
                GA_Slots[i].name = GA_SortedQuestions[i].ToString();
            }

        }


        private void SetDraggableData()
        {
            //unsorted
            for (int i = 0; i < GA_Draggables.Length; i++)
            {
                GA_Draggables[i].name = GA_Questions[i].ToString();
                GA_Draggables[i].transform.GetChild(0).GetComponent<Text>().text = GA_Questions[i].ToString();
            }

        }


        IEnumerator IENUM_DisableDraggableAnimation()
        {
            yield return new WaitForSeconds(0.6f);

            for (int i = 0; i < GA_Draggables.Length; i++)
            {
                GA_Draggables[i].GetComponent<Animator>().enabled = false;
            }

        }


        public void SpawnCoins()
        {
            StartCoroutine(IENUM_SpawnCoins());
        }


        IEnumerator IENUM_SpawnCoins()
        {
            for (int i = 0; i < GA_Draggables.Length; i++)
            {
                GA_Draggables[i].transform.GetChild(0).GetComponent<Text>().text = "";
                Instantiate(G_CoinPrefab, GA_Draggables[i].transform.position, Quaternion.identity, transform);
                yield return new WaitForSeconds(0.5f);
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

            //resetting elapsed time back to zero
            _elapsedTime = 0f;
        }



    }



}
