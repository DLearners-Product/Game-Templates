using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        // string question = "", answer = "";

        private float _elapsedTime, _desiredDuration = 0.5f;

        private static int I_FirstIndex = 0, I_LastIndex = 5;



        void Start()
        {
            REF_CaterpillarGameManager = GameObject.FindObjectOfType<CaterpillarGameManager>();
            //PrepareQuestions();
            //SetSlotData();
            //SetDraggableData();
            //StartCoroutine(IENUM_DisableDraggableAnimator());

            GA_Questions = new List<string>();
            GA_SortedQuestions = new List<string>();

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
            /*             GA_Questions = REF_CaterpillarGameManager.STRL_questions;
                        GA_SortedQuestions = REF_CaterpillarGameManager.STRL_answers;

                        question = REF_CaterpillarGameManager.STRL_questions[REF_CaterpillarGameManager.I_CurrentIndex];
                        answer = REF_CaterpillarGameManager.STRL_answers[REF_CaterpillarGameManager.I_CurrentIndex]; */



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




            for (int i = I_FirstIndex; i <= I_LastIndex; i++)
            {
                GA_Questions.Add(REF_CaterpillarGameManager.STRL_options[i]);
                GA_SortedQuestions.Add(REF_CaterpillarGameManager.STRL_options[i]);
            }

            string mode = "descending";

            //ascending order
            if (mode == "ascending")
            {
                //ascending order
                GA_SortedQuestions.Sort();
            }
            else if (mode == "descending")
            {
                //descending order
                GA_SortedQuestions.Sort((a, b) => b.CompareTo(a));
            }



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
                GA_Slots[i].name = GA_SortedQuestions[i].ToString();
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
                // GA_Draggables[i].name = question[i].ToString();
                // GA_Draggables[i].transform.GetChild(0).GetComponent<Text>().text = question[i].ToString();

                GA_Draggables[i].name = GA_Questions[i].ToString();
                GA_Draggables[i].transform.GetChild(0).GetComponent<Text>().text = GA_Questions[i].ToString();
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
            #region Getting Draggable number's ascending index list

            List<int> ascendingIndexList = new List<int>();

            for (int i = 0; i < GA_Draggables.Length; i++)
            {
                ascendingIndexList.Add(int.Parse(GA_Draggables[i].transform.GetChild(0).GetComponent<Text>().text));
            }

            List<int> indicesInOrder = new List<int>();

            if (REF_CaterpillarGameManager.mode == "ascending")
            {
                indicesInOrder = Enumerable.Range(0, ascendingIndexList.Count)
                                                                       .OrderBy(i => ascendingIndexList[i])
                                                                       .ToList();
            }
            else if (REF_CaterpillarGameManager.mode == "descending")
            {
                indicesInOrder = Enumerable.Range(0, ascendingIndexList.Count)
                                                                          .OrderBy(i => ascendingIndexList[i])
                                                                          .OrderByDescending(i => ascendingIndexList[i])
                                                                          .ToList();
            }

            // List<int> indicesInAscendingOrder = Enumerable.Range(0, ascendingIndexList.Count)
            //                                            .OrderBy(i => ascendingIndexList[i])
            //                                            .ToList();

            #endregion


            for (int i = 0; i < GA_Draggables.Length; i++)
            {
                Instantiate(G_CoinPrefab, GA_Slots[i].transform.position, Quaternion.identity, transform);
                GA_Draggables[indicesInOrder[i]].GetComponentInChildren<Text>().text = "";

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
