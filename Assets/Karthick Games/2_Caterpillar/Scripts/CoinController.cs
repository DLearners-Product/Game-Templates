using System.Collections;
using UnityEngine;


namespace CaterpillarSortingGame
{

    public class CoinController : MonoBehaviour
    {

        private Transform T_Points;

        private float elapsedTime = 0f, desiredDuration = 1.5f;


        void Start()
        {
            AudioManager.Instance.PlayCoinSpawn();
            T_Points = GameObject.FindGameObjectWithTag("Points").transform;
            StartCoroutine(IENUM_LerpMoveTile(transform.position, T_Points.GetChild(0).GetChild(1).position));
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

            T_Points.GetComponent<Animator>().SetTrigger("clicked");
            T_Points.GetComponentInChildren<ParticleSystem>().Play();
        }

    }

}