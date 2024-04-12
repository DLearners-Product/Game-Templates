using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaterpillarSortingGame;


public class CaterpillarDemoController : MonoBehaviour
{

    [SerializeField] private GameObject G_AscExample;
    [SerializeField] private GameObject G_DescExample;

    [SerializeField] private CaterpillarGameManager REF_CaterpillarGameManager;



    void Start()
    {
        //to show its ascending or descending demo
        if (REF_CaterpillarGameManager.STR_Mode == "asc")
        {
            G_AscExample.SetActive(true);
            G_DescExample.SetActive(false);
        }
        else if (REF_CaterpillarGameManager.STR_Mode == "desc")
        {
            G_DescExample.SetActive(true);
            G_AscExample.SetActive(false);
        }
    }
}
