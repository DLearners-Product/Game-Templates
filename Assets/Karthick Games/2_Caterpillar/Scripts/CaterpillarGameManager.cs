using System.Collections;
using UnityEngine;


namespace CaterpillarSortingGame
{

    public class CaterpillarGameManager : MonoBehaviour
    {

        [SerializeField] private Animator ANIM_CaterpillarMove;
        [SerializeField] private Animator ANIM_CaterpillarUpDown;



        [SerializeField] private GameObject G_QandAPrefab;
        [SerializeField] private GameObject G_Caterpillar;
        [SerializeField] private GameObject G_TransparentScreen;



        [SerializeField] private Transform T_QandAParent;



        private GameObject _InstantiatedQandA;

        private int I_CurrentIndex;



        void Start()
        {
            I_CurrentIndex = 0;
            Invoke(nameof(ShowNextQuestion), 3f);
        }


        public void ShowNextQuestion()
        {
            AudioManager.Instance.PlayGameMusic();
            StartCoroutine(IENUM_ShowCurrentQuestion());
        }


        IEnumerator IENUM_ShowCurrentQuestion()
        {
            // G_Caterpillar.SetActive(true);
            // G_TransparentScreen.SetActive(true);
            // ANIM_CaterpillarMove.SetTrigger("in");
            // ANIM_CaterpillarUpDown.SetTrigger("active");
            // AudioManager.Instance.PlayCaterpillarMovement();

            // yield return new WaitForSeconds(4f);

            // ANIM_CaterpillarUpDown.SetTrigger("inactive");
            // G_TransparentScreen.SetActive(false);
            // G_Caterpillar.SetActive(false);
            // G_QandA.SetActive(true);

            _InstantiatedQandA = Instantiate(G_QandAPrefab, G_QandAPrefab.transform.position, Quaternion.identity, T_QandAParent);
            G_Caterpillar = _InstantiatedQandA.GetComponent<QandA>().GetCaterpillar();

            yield return new WaitForSeconds(4f);

        }


        public void RemoveCurrentQuestion()
        {
            Destroy(_InstantiatedQandA.gameObject);
        }





    }





}
