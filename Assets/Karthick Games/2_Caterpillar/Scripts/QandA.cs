using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CaterpillarSortingGame
{

    public class QandA : MonoBehaviour
    {

        [SerializeField] private Animator ANIM_CaterpillarMove;
        [SerializeField] private Animator ANIM_CaterpillarUpDown;

        [Space(10)]
        [SerializeField] private GameObject G_Caterpillar;
        [SerializeField] private GameObject G_QandA;

        [SerializeField] private GameObject[] GA_Slots;
        [SerializeField] private GameObject[] GA_Draggables;
        [SerializeField] private GameObject G_CoinPrefab;

        [Space(10)]

        private CaterpillarGameManager REF_CaterpillarGameManager;


        private List<string> GA_Questions;             //draggable
        private List<string> GA_SortedQuestions;       //slot

        string question = "", answer = "";

        private float _elapsedTime, _desiredDuration = 0.5f;


        void Start()
        {
            REF_CaterpillarGameManager = GameObject.FindObjectOfType<CaterpillarGameManager>();
            //PrepareQuestions();
            //SetSlotData();
            //SetDraggableData();
            //StartCoroutine(IENUM_DisableDraggableAnimator());


            StartCoroutine(IENUM_CaterpillarIn());
        }



        IEnumerator IENUM_CaterpillarIn()
        {

            ANIM_CaterpillarMove.SetTrigger("in");
            ANIM_CaterpillarUpDown.SetTrigger("active");
            AudioManager.Instance.PlayCaterpillarMovement();

            yield return new WaitForSeconds(4f);

            ANIM_CaterpillarUpDown.SetTrigger("inactive");
            G_Caterpillar.SetActive(false);
            G_QandA.SetActive(true);

            //set data
            PrepareQuestions();
            SetSlotData();
            SetDraggableData();
            Invoke("DisableDraggableAnimator", 0.6f);
        }



        private void PrepareQuestions()
        {
            GA_Questions = REF_CaterpillarGameManager.STRL_questions;
            GA_SortedQuestions = REF_CaterpillarGameManager.STRL_answers;

            question = REF_CaterpillarGameManager.STRL_questions[REF_CaterpillarGameManager.I_CurrentIndex];
            answer = REF_CaterpillarGameManager.STRL_answers[REF_CaterpillarGameManager.I_CurrentIndex];

            // for (int i = 0; i < GA_Slots.Length; i++)
            // {
            //     int randomNum = Random.Range(1, 10);
            //     GA_Questions.Add(randomNum);
            //     GA_SortedQuestions.Add(randomNum);
            // }

            //ascending order
            // GA_SortedQuestions.Sort();

            //descending order
            // GA_SortedQuestions.Sort((a, b) => b.CompareTo(a));



        }


        private void SetSlotData()
        {
            //sorted
            /*             for (int i = 0; i < GA_Slots.Length; i++)
                        {
                            GA_Slots[i].name = GA_SortedQuestions[i].ToString();
                        } */

            for (int i = 0; i < GA_Slots.Length; i++)
            {
                GA_Slots[i].name = answer[i].ToString();
            }
        }


        private void SetDraggableData()
        {
            //unsorted
            /*             for (int i = 0; i < GA_Draggables.Length; i++)
                        {
                            GA_Draggables[i].name = GA_Questions[i].ToString();
                            GA_Draggables[i].transform.GetChild(0).GetComponent<Text>().text = GA_Questions[i].ToString();
                        } */

            for (int i = 0; i < GA_Draggables.Length; i++)
            {
                GA_Draggables[i].name = question[i].ToString();
                GA_Draggables[i].transform.GetChild(0).GetComponent<Text>().text = question[i].ToString();
            }
        }


        private void DisableDraggableAnimator()
        {
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
            List<int> ascendingIndexList = new List<int>();

            for (int i = 0; i < GA_Draggables.Length; i++)
            {
                // ascendingIndexList.Add(int.Parse(GA_Draggables[i].transform.GetChild(0).GetComponent<Text>().text));
            }

            ascendingIndexList.Sort();

            for (int i = 0; i < GA_Draggables.Length; i++)
            {
                // GA_Draggables[i].transform.GetChild(0).GetComponent<Text>().text = "";
                Instantiate(G_CoinPrefab, GA_Slots[i].transform.position, Quaternion.identity, transform);
                // GA_Draggables[].GetComponent<Text>().text = "";

                yield return new WaitForSeconds(0.5f);
            }

        }


        public GameObject GetCaterpillar()
        {
            return G_Caterpillar;
        }


        public IEnumerator IENUM_CaterpillarOut()
        {
            yield return new WaitForSeconds(8f);

            G_QandA.SetActive(false);
            G_Caterpillar.SetActive(true);
            ANIM_CaterpillarMove.SetTrigger("out");
            ANIM_CaterpillarUpDown.SetTrigger("active");
            AudioManager.Instance.PlayCaterpillarMovement();

            yield return new WaitForSeconds(4f);

            REF_CaterpillarGameManager.RemoveCurrentQuestion();
        }


    }

}
