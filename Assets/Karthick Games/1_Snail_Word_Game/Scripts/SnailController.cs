using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SnailController : MonoBehaviour
{
    public Vector3 direction;
    public float snailSpeed;
    public float rayDepth = 5f;

    public RectTransform characterRectTransform;
    public Transform fallDetection;

    public LayerMask borderLayer;




    public float moveSpeed = 5f;
    public float blockDetectDistance = 0.5f;

    private Rigidbody2D rb;
    private bool movingRight = true;
    private Transform leftBlock;
    private Transform rightBlock;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.right;
        snailSpeed = 0.01f;
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


        transform.Translate(direction * snailSpeed);







        /*        RaycastHit2D frontHit = Physics2D.Raycast(characterRectTransform.position, direction, rayDepth, borderLayer);
                RaycastHit2D backHit = Physics2D.Raycast(characterRectTransform.position, -direction, rayDepth, borderLayer);

                Debug.Log("front " + frontHit);
                Debug.Log("front collider" + frontHit.collider);
                Debug.Log(frontHit.collider == null);

                Debug.Log("back " + backHit);
                Debug.Log("back collider " + backHit.collider);

                if (frontHit != true && frontHit.collider.gameObject.CompareTag("TileBody"))
                {
                    Debug.Log("front");

                    Debug.DrawRay(characterRectTransform.position, direction, Color.green);
                    ChangeDirection();
                    return;
                }
                else if (backHit != true && backHit.collider.gameObject.CompareTag("TileBody"))
                {
                    Debug.Log("back");

                    Debug.DrawRay(characterRectTransform.position, -direction, Color.red);
                    ChangeDirection();
                    return;
                }

                transform.Translate(direction * snailSpeed);*/


        /*if (frontHit.collider != null)
        {
            if (frontHit.collider.CompareTag("TileBody"))
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


            Debug.DrawRay(characterRectTransform.position, direction, Color.green);
            transform.Translate(direction * snailSpeed);
        }
        else
        {

        }*/


        /* Ray downray = new Ray(fallDetection.position, Vector3.down);
         RaycastHit downHitInfo;
         if (Physics.Raycast(downray, out downHitInfo, rayDepth))
         {
             //if ray hits Wall, don't move the player
             if (!downHitInfo.collider.CompareTag("TileBody"))
             {
                 Debug.Log("bottom ray hitting");

                 return;
             }
         }*/


        transform.Translate(direction * snailSpeed);
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {

    }
}

