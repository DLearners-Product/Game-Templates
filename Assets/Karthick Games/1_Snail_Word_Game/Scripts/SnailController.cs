using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SnailController : MonoBehaviour
{
    public Vector3 direction;
    public float snailSpeed;
    public float rayDistance;
    public RectTransform frontRay;
    public RectTransform backRay;
    public bool renderRight, renderLeft;

    [SerializeField] private LayerMask borderLayer;
    private Vector3 localScale;
    float speed;

    // Start is called before the first frame update
    void Start()
    {
        localScale = frontRay.localScale;

        direction = Vector3.right;
        snailSpeed = 0.01f;
        rayDistance = 0.5f;
        speed = snailSpeed;
    }


    private void Update()
    {
        CheckAndMove();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("TileBody"))
        // {
        //     ChangeDirection();
        // }
    }

    private void ChangeDirection()
    {
        if (direction == Vector3.left)
        {
            Debug.Log($"Directed to right");
            direction = Vector3.right;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            Debug.Log($"Directed to left");
            direction = Vector3.left;
            transform.localScale = new Vector3(-1, 1, 1);

            // characterRectTransform.localScale = transform.q(new Vector3(-1, 1, 1));
        }
    }

    public void CheckAndMove()
    {
        transform.Translate(direction * speed);

        RaycastHit2D hitLeft = Physics2D.Raycast(frontRay.position, direction, rayDistance, borderLayer);

        if(renderRight)
            Debug.DrawRay(frontRay.position, direction * rayDistance, Color.green);

        Vector2 opposite = new Vector2(direction.x * -1, direction.y);

        Debug.Log($"{opposite}");

        RaycastHit2D hitRight = Physics2D.Raycast(backRay.position, opposite, rayDistance, borderLayer);

        if(renderLeft)
            Debug.DrawRay(backRay.position, opposite * rayDistance, Color.green);

        if(hitLeft.collider != null && hitRight.collider != null){
            speed = 0f;
            Debug.Log($"Don't move");
        }else if(hitLeft.collider != null){
            speed = snailSpeed;
            ChangeDirection();
        }
    }


}

