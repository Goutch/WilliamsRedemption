using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilliamController : MonoBehaviour {

    [SerializeField] private float speed;
    private Animator animator;

    private Transform root;

    private float horizontalMovement = 0.0f;

    private void Awake()
    {
        root = gameObject.transform.parent;
        animator = GetComponent<Animator>();
    }

    private void Update ()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * speed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
    }

    private void FixedUpdate()
    {
        root.Translate(horizontalMovement * Time.deltaTime, 0,0);
    }
}
