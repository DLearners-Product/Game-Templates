using System.Collections;
using UnityEngine;


namespace SnailWordGame
{
    public class IntroController : MonoBehaviour
    {
        [SerializeField] private Animator ANIM_Intro;


        [SerializeField] private GameObject G_Intro;
        [SerializeField] private GameObject G_Game;


        [SerializeField] private ParticleSystem PS_Effects;


        void Start()
        {
            Invoke(nameof(DisableGame), 1f);
        }


        public void THI_PlayParticles()
        {
            PS_Effects.Play();
        }


        public void BUT_Play()
        {
            AudioManager.Instance.PlayButtonClick();
            ANIM_Intro.SetTrigger("exit");
            StartCoroutine(IENUM_EnableGame());
            AudioManager.Instance.PlayMusic();
        }


        private void DisableGame()
        {
            G_Game.SetActive(false);
        }


        IEnumerator IENUM_EnableGame()
        {
            yield return new WaitForSeconds(2f);
            AudioManager.Instance.PlaySwoosh();
            G_Intro.SetActive(false);
            G_Game.SetActive(true);
            G_Game.GetComponent<Animator>().SetTrigger("active");
            Invoke(nameof(DisableAnimator), 2f);

        }


        private void DisableAnimator()
        {
            G_Game.GetComponent<Animator>().enabled = false;
        }

    }
}