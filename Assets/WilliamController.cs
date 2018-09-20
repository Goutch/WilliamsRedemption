using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilliamController : MonoBehaviour {

    [SerializeField] private float speed;
    private Animator animator;
    private SpriteRenderer sprite;

    private Transform root;

    private float horizontalMovement = 0.0f;

    private float HorizontalMovement
    {
        get
        {
            return horizontalMovement;
        }
        set
        {
            horizontalMovement = value;
            if (horizontalMovement > 0)
                sprite.flipX = false;
            else if (horizontalMovement < 0)
                sprite.flipX = true;
        }
    }

    private void Awake()
    {
        root = gameObject.transform.parent;
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update ()
    {
        HorizontalMovement = Input.GetAxis("Horizontal") * speed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
    }

    private void FixedUpdate()
    {
        root.Translate(horizontalMovement * Time.deltaTime, 0,0);
    }
}
