using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SnailController : MonoBehaviour
{
    public Vector3 direction;
    public float snailSpeed;
    public float rayDistance;
    public RectTransform characterRectTransform;
    public Transform fallDetection;

    private LayerMask borderLayer;
    private Vector3 localScale;

    // Start is called before the first frame update
    void Start()
    {
        localScale = characterRectTransform.localScale;

        direction = Vector3.right;
        snailSpeed = 0.01f;
        rayDistance = 0.5f;
    }


    private void Update()
    {
        CheckAndMove();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TileBody"))
        {
            if (direction == Vector3.left)
            {
                direction = Vector3.right;
                characterRectTransform.localScale = new Vector3(1, 1, 1);

            }
            else
            {
                direction = Vector3.left;
                characterRectTransform.localScale = new Vector3(-1, 1, 1);

            }
        }
    }

    public void CheckAndMove()
    {
        RaycastHit2D hit = Physics2D.Raycast(characterRectTransform.position, direction, rayDistance, borderLayer);

        /*
                if (hit.collider.CompareTag("TileBody"))
                {
                    //hits a collider and that collider is TileBody
                    transform.Translate(direction * snailSpeed);
                    Debug.DrawRay(characterRectTransform.position, direction * rayDistance, Color.green);

                }
                else if (!hit.collider.CompareTag("TileBody"))
                {
                    //collider detected | dont move
                    //transform.Translate(direction * snailSpeed);
                    Debug.DrawRay(characterRectTransform.position, direction * rayDistance, Color.red);


                }
                else
                {
                    //no collider detected | move
                    transform.Translate(direction * snailSpeed);
                    Debug.DrawRay(characterRectTransform.position, direction * rayDistance, Color.green);
                }*/


       

    }


}

