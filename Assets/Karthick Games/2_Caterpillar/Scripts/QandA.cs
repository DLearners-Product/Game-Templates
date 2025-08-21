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


        private float _elapsedTime, _desiredDuration = 0.5f;


        void Start()
        {
            REF_CaterpillarGameManager = GameObject.FindObjectOfType<CaterpillarGameManager>();

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
            // //fetching questions and options and assigning them to local list for easy q and a manipulation
            // for (int i = REF_CaterpillarGameManager.I_FirstIndex; i <= REF_CaterpillarGameManager.I_LastIndex; i++)
            // {
            //     GA_Questions.Add(REF_CaterpillarGameManager.STRL_options[i]);
            //     GA_SortedQuestions.Add(REF_CaterpillarGameManager.STRL_options[i]);
            // }

            // // Print the original list
            // Debug.Log("Original Questions: " + string.Join(", ", GA_Questions));

            // //sorting options based on the mdoe
            // if (REF_CaterpillarGameManager.STR_Mode == "asc")
            // {
            //     //ascending order
            //     GA_SortedQuestions.Sort();
            // }
            // else if (REF_CaterpillarGameManager.STR_Mode == "desc")
            // {
            //     //descending order
            //     GA_SortedQuestions.Sort((a, b) => b.CompareTo(a));
            // }

            // // Print the sorted list
            // Debug.Log("Sorted Questions: " + string.Join(", ", GA_SortedQuestions));











            // Clear the lists before adding new elements
            GA_Questions.Clear();
            GA_SortedQuestions.Clear();

            // Fetching questions and options and assigning them to local list for easy q and a manipulation
            for (int i = REF_CaterpillarGameManager.I_FirstIndex; i <= REF_CaterpillarGameManager.I_LastIndex; i++)
            {
                GA_Questions.Add(REF_CaterpillarGameManager.STRL_options[i]);
                GA_SortedQuestions.Add(REF_CaterpillarGameManager.STRL_options[i]);
            }

            // Print the original list
            // Debug.Log("Original Questions: " + string.Join(", ", GA_Questions));

            // Sorting options based on the mode
            if (REF_CaterpillarGameManager.STR_Mode == "asc")
            {
                // Ascending order
                GA_SortedQuestions.Sort((a, b) => int.Parse(a).CompareTo(int.Parse(b)));
            }
            else if (REF_CaterpillarGameManager.STR_Mode == "desc")
            {
                // Descending order
                GA_SortedQuestions.Sort((a, b) => int.Parse(b).CompareTo(int.Parse(a)));
            }

            // Print the sorted list
            // Debug.Log("Sorted Questions: " + string.Join(", ", GA_SortedQuestions));
        }


        private void SetSlotData()
        {
            for (int i = 0; i < GA_Slots.Length; i++)
            {
                GA_Slots[i].name = GA_SortedQuestions[i].ToString();
                Debug.Log(GA_SortedQuestions[i].ToString());
            }
        }


        private void SetDraggableData()
        {
            for (int i = 0; i < GA_Draggables.Length; i++)
            {
                GA_Draggables[i].name = GA_Questions[i].ToString();
                GA_Draggables[i].transform.GetChild(0).GetComponent<Text>().text = GA_Questions[i].ToString();
                GA_Draggables[i].GetComponent<MouseClickAudio>().clip = REF_CaterpillarGameManager.ACA_optionClips[REF_CaterpillarGameManager.I_AudioClipIndex++];
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

            List<int> draggableIndexList = new List<int>();

            for (int i = 0; i < GA_Draggables.Length; i++)
            {
                draggableIndexList.Add(int.Parse(GA_Draggables[i].GetComponentInChildren<Text>().text));
            }

            List<int> indicesInOrder = new List<int>();

            if (REF_CaterpillarGameManager.STR_Mode == "asc")
            {
                indicesInOrder = Enumerable.Range(0, draggableIndexList.Count)
                                            .OrderBy(i => draggableIndexList[i])
                                            .ToList();
            }
            else if (REF_CaterpillarGameManager.STR_Mode == "desc")
            {
                indicesInOrder = Enumerable.Range(0, draggableIndexList.Count)
                                            .OrderByDescending(i => draggableIndexList[i])
                                            .ToList();
            }

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


            REF_CaterpillarGameManager.I_FirstIndex = REF_CaterpillarGameManager.I_LastIndex + 1;
            REF_CaterpillarGameManager.I_LastIndex = REF_CaterpillarGameManager.I_FirstIndex + 5;

            REF_CaterpillarGameManager.RemoveCurrentQuestion();
        }


    }

}
