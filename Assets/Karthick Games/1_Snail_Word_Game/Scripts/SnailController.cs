using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailController : MonoBehaviour
{
    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;
    public float rayDepth = 5f;

    public Transform tileDetection;
    public Transform fallDetection;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(collision.gameObject.name, collision.gameObject);

        if (collision.gameObject.CompareTag("TileBody"))
        {
            dirX *= -1f;
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        RaycastHit2D tileInfo = Physics2D.Raycast(tileDetection.position, Vector2.down, rayDepth);
        RaycastHit2D fallInfo = Physics2D.Raycast(fallDetection.position, Vector2.down, rayDepth);

        if (tileInfo == false && fallInfo.collider == false)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }
}

