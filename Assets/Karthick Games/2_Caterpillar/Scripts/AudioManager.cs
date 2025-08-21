using UnityEngine;


namespace CaterpillarSortingGame
{

    public class AudioManager : MonoGenericSingleton<AudioManager>
    {

        [SerializeField] private AudioSource AS_SFX;
        [SerializeField] private AudioSource AS_Music;
        [SerializeField] private AudioSource AS_Voice;

        [Space(10)]

        [SerializeField] private AudioClip AC_IntroMusic;
        [SerializeField] private AudioClip AC_GameMusic;
        [SerializeField] private AudioClip AC_GameWon;

        [SerializeField] private AudioClip AC_Correct;
        [SerializeField] private AudioClip AC_Wrong;
        [SerializeField] private AudioClip AC_CaterpillarMovement;
        [SerializeField] private AudioClip AC_CoinSpawn;
        [SerializeField] private AudioClip AC_CoinCollect;


        public void PlayVoice(AudioClip clip)
        {
            AS_Voice.PlayOneShot(clip);
        }


        public void StopVoice()
        {
            AS_Voice.Stop();
        }


        public void PlayIntroMusic()
        {
            AS_Music.PlayOneShot(AC_IntroMusic);
        }


        public void PlayGameMusic()
        {
            AS_Music.clip = AC_GameMusic;
            AS_Music.Play();
            AS_Music.loop = true;
        }


        public bool IsMusicPlaying()
        {
            return AS_Music.isPlaying;
        }


        public void PlayGameWonMusic()
        {
            AS_Music.clip = AC_GameWon;
            AS_Music.Play();
            AS_Music.loop = false;
        }


        public void PlayCorrect()
        {
            AS_SFX.PlayOneShot(AC_Correct);
        }


        public void PlayWrong()
        {
            AS_SFX.PlayOneShot(AC_Wrong);
        }


        public void PlayCaterpillarMovement()
        {
            AS_SFX.PlayOneShot(AC_CaterpillarMovement);
        }


        public void PlayCoinSpawn()
        {
            AS_SFX.PlayOneShot(AC_CoinSpawn);
        }


        public void PlayCoinCollect(float delay)
        {
            AS_SFX.clip = AC_CoinCollect;
            AS_SFX.PlayDelayed(delay);
        }





    }

}
