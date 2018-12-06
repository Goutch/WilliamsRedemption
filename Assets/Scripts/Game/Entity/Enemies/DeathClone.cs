using Game.Entity;
using Game.Entity.Enemies;
using Game.Entity.Enemies.Attack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RootMover))]
public class DeathClone : Enemy
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float delayBetweenShot;
    [SerializeField] private Transform projectileSpawnPoint;
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

        if (Time.time - lastShot > delayBetweenShot)
        {
            animator.SetTrigger("PlasmaShoot");
            lastShot = Time.time;
        }
    }


    private void ShootPlasmaProjectile()
    {
        GameObject projectileObject = Instantiate(projectile, projectileSpawnPoint.position, transform.rotation);
        projectileObject.GetComponent<HitStimulus>().Type = HitStimulus.DamageType.Enemy;
        
    }
}