using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaterpillarSortingGame;


namespace CaterpillarSortingGame
{

    public class DemoController : MonoBehaviour
    {

        [SerializeField] private GameObject G_Game;
        [SerializeField] private GameObject G_Demo;
        [SerializeField] private GameObject G_AscExample;
        [SerializeField] private GameObject G_DescExample;

        [Space(10)]

        [SerializeField] private CaterpillarGameManager REF_CaterpillarGameManager;



        public void ChooseExample()
        {
            AudioManager.Instance.PlayGameMusic();

            //to show its ascending or descending demo
            if (REF_CaterpillarGameManager.STR_Mode.Equals("asc"))
            {
                G_AscExample.SetActive(true);
                G_DescExample.SetActive(false);
            }
            else if (REF_CaterpillarGameManager.STR_Mode.Equals("desc"))
            {
                G_DescExample.SetActive(true);
                G_AscExample.SetActive(false);
            }
        }


        public void BUT_Skip()
        {
            G_Demo.SetActive(false);
            G_Game.SetActive(true);
            REF_CaterpillarGameManager.GameInit();
        }

    }
}