using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityControlableController : MonoBehaviour {
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpForce;
    protected Animator animator;
    protected SpriteRenderer sprite;
    protected Rigidbody2D rb;

    protected Transform root;

    protected float inputHorizontalMovement = 0.0f;
    protected bool inputJump = false;

    protected bool isOnGround = true;

    private void Awake()
    {
        root = gameObject.transform.parent;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        inputHorizontalMovement = Input.GetAxis("Horizontal") * speed;
        if (inputHorizontalMovement > 0)
            sprite.flipX = false;
        else if (inputHorizontalMovement < 0)
            sprite.flipX = true;

        inputJump = Input.GetButton("Jump");


        animator.SetFloat("Speed", Mathf.Abs(inputHorizontalMovement));
    }

    private void FixedUpdate()
    {
        root.Translate(inputHorizontalMovement * Time.deltaTime, 0, 0);

        if (inputJump && isOnGround && rb.velocity.y == 0)
        {
            rb.velocity = new Vector2(0,0);
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            isOnGround = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isOnGround = true;
    }
}
