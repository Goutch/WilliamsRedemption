using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boss;
using Harmony;
using UnityEngine;

namespace Edgar
{
    class ProjectileShoot : Capacity
    {
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;

        [Header("Multiple projectile config")]
        [SerializeField] private GameObject bullet;

        private float lastTimeCapacityUsed;

        private new void Awake()
        {
            base.Awake();
            if (capacityUsableAtStart)
                lastTimeCapacityUsed = -cooldown;
        }

        public override void Act()
        {
            Debug.Log(this);
        }

        public override bool CanTransit()
        {
            if (Time.time - lastTimeCapacityUsed > cooldown)
                return true;
            else
                return false;
        }

        public override void Transite()
        {
            base.Transite();
            lastTimeCapacityUsed = Time.time;

            ShootProjectile();
        }

        public void ShootProjectile()
        {
            Quaternion direction = PlayerDirection();
            GameObject projectile = Instantiate(bullet, transform.position, direction);
            projectile.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Ennemy);

            Finish();
        }

        private Quaternion PlayerDirection()
        {
            Vector2 dir = PlayerController.instance.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
            return direction;
        }
    }
}
