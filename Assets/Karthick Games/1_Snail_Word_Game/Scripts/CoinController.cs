using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{

    [SerializeField] private Transform T_Points;

    private float elapsedTime = 0f, desiredDuration = 1.5f;


    void Start()
    {
        T_Points = GameObject.FindGameObjectWithTag("Points")?.transform;
        StartCoroutine(IENUM_LerpMoveTile(transform.position, T_Points.position));
    }



    IEnumerator IENUM_LerpMoveTile(Vector3 currentPosition, Vector3 newPosition)
    {
        yield return new WaitForSeconds(2f);

        while (elapsedTime < desiredDuration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;

            transform.position = Vector3.Lerp(currentPosition, newPosition, percentageComplete);
            yield return null;
        }

        //resetting elapsed time back to zero
        elapsedTime = 0f;
    }




}
