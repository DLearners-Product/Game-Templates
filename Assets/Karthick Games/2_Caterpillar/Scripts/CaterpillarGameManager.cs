using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaterpillarGameManager : MonoBehaviour
{



    [SerializeField] private Animator ANIM_Caterpillar;
    [SerializeField] private GameObject G_TransparentScreen;





    public void ShowCurrentQuestion()
    {

    }


    IEnumerator IENUM_ShowCurrentQuestion()
    {
        G_TransparentScreen.SetActive(true);
        ANIM_Caterpillar.SetTrigger("in");

        yield return new WaitForSeconds(4f);
        G_TransparentScreen.SetActive(false);



    }














}
