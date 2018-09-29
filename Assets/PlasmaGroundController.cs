using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaGroundController : MonoBehaviour {
    private FacingSideLeftRight direction;
    private Rigidbody2D rb;
    private new Collider2D collider;
    private float scale = 1;
    private float scaleSpeed = 0.1f;
    private float size;
    private bool grounded = false;
    private bool hitWall = false;
    private float maxWidth = 5;

    private void Awake()
    {
        size = GetComponent<SpriteRenderer>().size.x;
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y);
    }

    private void Update()
    {
        if (grounded && !hitWall)
        {
            goForward();
        }
    }

    private void goForward()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.localPosition - new Vector3(scale * (size / 2), 0), new Vector2(-1, 0), 0, 1 << LayerMask.NameToLayer("Default"));
        if (hit.collider == null)
        {
            if(transform.localScale.x * size < maxWidth)
            {
                transform.localScale = new Vector3(transform.localScale.x + scaleSpeed, transform.localScale.y);
                scale += scaleSpeed;
            }

            transform.localPosition = new Vector3(transform.localPosition.x - scaleSpeed * (size / 2), transform.localPosition.y);
            Debug.DrawLine(transform.localPosition, transform.localPosition - new Vector3(0, 1), Color.green);
            Debug.DrawLine(transform.localPosition - new Vector3(scale * (size / 2), 0), transform.localPosition - new Vector3(scale * (size / 2), 1), Color.red);
        }
        else
        {
            Debug.DrawLine(transform.localPosition - new Vector3(scale * (size / 2), 0), hit.point, Color.red);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Contains("Plateforme"))
        {
            if (!grounded)
            {
                grounded = true;
                rb.constraints = rb.constraints | RigidbodyConstraints2D.FreezePositionY;
                transform.Translate(0,0.02f,0);
            }
            else
            {
                hitWall = true;
                Debug.Log("Hit Wall");
            }
        }
    }
}
