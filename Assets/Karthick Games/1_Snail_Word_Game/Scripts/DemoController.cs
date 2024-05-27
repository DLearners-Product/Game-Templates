using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SnailWordGame
{

    public class DemoController : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI TXT_FormedWord;
        [SerializeField] private TextMeshProUGUI TXT_Word;
        [SerializeField] private Image IMG_FormedWordBG;
        [SerializeField] private ParticleSystem PS_FormedWord;




        private float elapsedTime, desiredDuration = 0.5f;


        public void Demo1()
        {
            StartCoroutine(IENUM_Demo1());
        }

        private IEnumerator IENUM_Demo1()
        {
            TXT_Word.color = Color.white;
            yield return new WaitForSeconds(3.5f);

            TXT_FormedWord.text = "a";
            StartCoroutine(IENUM_LerpColor(IMG_FormedWordBG, IMG_FormedWordBG.color, SnailGameManager.Instance.CLR_ButtonWrong));

            yield return new WaitForSeconds(2f);

            TXT_FormedWord.text = "ap";

            yield return new WaitForSeconds(2f);


            TXT_FormedWord.text = "app";

            yield return new WaitForSeconds(2f);


            TXT_FormedWord.text = "appl";

            yield return new WaitForSeconds(2f);

            TXT_FormedWord.text = "apple";
            StartCoroutine(IENUM_LerpColor(IMG_FormedWordBG, IMG_FormedWordBG.color, SnailGameManager.Instance.CLR_ButtonCorrect));
        }


        public void WordFormed()
        {
            PS_FormedWord.Play();
            TXT_FormedWord.text = "-";
            TXT_Word.color = SnailGameManager.Instance.CLR_ButtonNormal;
            StartCoroutine(IENUM_LerpColor(IMG_FormedWordBG, IMG_FormedWordBG.color, SnailGameManager.Instance.CLR_ButtonNormal));
        }




        IEnumerator IENUM_LerpColor(Image img, Color32 currentColor, Color32 targetColor)
        {
            //*slowly changing color for background
            while (elapsedTime < desiredDuration)
            {
                elapsedTime += Time.deltaTime;
                float percentageComplete = elapsedTime / desiredDuration;

                img.color = Color.Lerp(currentColor, targetColor, percentageComplete);
                yield return null;
            }

            //resetting elapsed time back to zero
            elapsedTime = 0f;
        }


    }

}