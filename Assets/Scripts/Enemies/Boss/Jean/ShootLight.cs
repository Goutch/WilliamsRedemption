﻿using Boss;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Jean
{
    class ShootLight : Capacity
    {
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        [SerializeField] private float shieldCost;
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject projectileSpawnPoint1;
        [SerializeField] private GameObject projectileSpawnPoint2;
        [SerializeField] private float probabilitySpawn1;

        private ShieldManager shieldManager;
        private bool canEnterAsked1Time = false;

        private float lastTimeUsed;

        public override void Finish()
        {
            canEnterAsked1Time = false;

            base.Finish();
        }

        public override void Act()
        {
            Finish();
        }

        public override bool CanEnter()
        {
            if (!canEnterAsked1Time)
            {
                lastTimeUsed = Time.time;
                canEnterAsked1Time = true;
            }

            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();

            shieldManager = GetComponentInChildren<ShieldManager>();

            int random = Random.Range(0, 100);
            Vector2 spawnPosition = random < probabilitySpawn1 ? projectileSpawnPoint1.transform.position : projectileSpawnPoint2.transform.position;
            Instantiate(projectile, spawnPosition, transform.rotation);

            shieldManager.UseShield(shieldCost);

        }
    }
}
