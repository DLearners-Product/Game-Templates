using System.Collections;
using UnityEngine;


namespace CaterpillarSortingGame
{

    public class CaterpillarGameManager : MonoBehaviour
    {

        [SerializeField] private Animator ANIM_CaterpillarMove;
        [SerializeField] private Animator ANIM_CaterpillarUpDown;

        [SerializeField] private GameObject G_Caterpillar;
        [SerializeField] private GameObject G_QandA;
        [SerializeField] private GameObject G_TransparentScreen;



        void Start()
        {
            Invoke(nameof(ShowCurrentQuestion), 3f);
        }


        public void ShowCurrentQuestion()
        {
            AudioManager.Instance.PlayGameMusic();
            StartCoroutine(IENUM_ShowCurrentQuestion());
        }


        IEnumerator IENUM_ShowCurrentQuestion()
        {
            G_Caterpillar.SetActive(true);
            G_TransparentScreen.SetActive(true);
            ANIM_CaterpillarMove.SetTrigger("in");
            ANIM_CaterpillarUpDown.SetTrigger("active");
            AudioManager.Instance.PlayCaterpillarMovement();

            yield return new WaitForSeconds(4f);

            ANIM_CaterpillarUpDown.SetTrigger("inactive");
            G_TransparentScreen.SetActive(false);
            G_Caterpillar.SetActive(false);
            G_QandA.SetActive(true);
        }


        public void HideCurrentQuestion()
        {
            StartCoroutine(IENUM_HideCurrentQuestion());
        }


        IEnumerator IENUM_HideCurrentQuestion()
        {
            yield return new WaitForSeconds(4f);


            G_QandA.SetActive(false);
            G_Caterpillar.SetActive(true);
            ANIM_CaterpillarMove.SetTrigger("out");
            ANIM_CaterpillarUpDown.SetTrigger("active");
            AudioManager.Instance.PlayCaterpillarMovement();

            yield return new WaitForSeconds(4f);



        }





    }





}
