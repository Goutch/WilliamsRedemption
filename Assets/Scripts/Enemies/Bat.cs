using System;
using System.Collections;
using System.Collections.Generic;
using Playmode.EnnemyRework;
using UnityEngine;

public class Bat : Enemy
{
    private RootMover rootMover;
    private int direction = 1;
    private Animator animator;
    private bool isTriggered;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 exponentialFonction;
    [SerializeField] private float fonctionYOffSet = .32f;

    protected void Fly()
    {
        exponentialFonction.x += 1 * Time.deltaTime;
        exponentialFonction.y = Mathf.Pow(exponentialFonction.x, 2) + fonctionYOffSet;
        rootMover.FlyToward(
            new Vector2(exponentialFonction.x * direction + transform.position.x,
                exponentialFonction.y + transform.position.y), Speed);
    }

    public void OnTriggered()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            animator.SetTrigger("Fly");
            Destroy(this.gameObject, 10);
            direction = PlayerController.instance.transform.position.x - transform.root.position.x > 0
                ? 1
                : -1;
            if (direction == 1)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }
    }

    protected override void Init()
    {
        rootMover = GetComponent<RootMover>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (isTriggered)
        {
            Fly();
        }
    }
}