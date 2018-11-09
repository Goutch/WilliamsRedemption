using Game.Entity;
using Game.Entity.Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RootMover))]
public class DeathClone : Enemy {

    [SerializeField] private GameObject projectile;
    [SerializeField] private float delayBetweenShot;

    private RootMover mover;
    private float lastShot;

    private new void Awake()
    {
        base.Awake();

        mover = GetComponent<RootMover>();

        float randomInitialDelay = UnityEngine.Random.Range(0, delayBetweenShot);
        lastShot = Time.time - randomInitialDelay;
    }

    protected override void Init()
    {

    }

    private void Update()
    {
        mover.LookAtPlayer();

        if(Time.time - lastShot > delayBetweenShot)
        {
            ShootProjectile();
            lastShot = Time.time;
        }
    }

    protected override void OnHit(HitStimulus other)
    {
        base.OnHit(other);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitStimulus other;
        if (other = collision.GetComponent<HitStimulus>())
            base.OnHit(other);
    }

    private void ShootProjectile()
    {
        Instantiate(projectile, transform.position, transform.rotation);
    }
}
