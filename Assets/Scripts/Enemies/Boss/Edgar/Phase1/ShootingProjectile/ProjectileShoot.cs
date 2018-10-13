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

        private void Awake()
        {
            if (capacityUsableAtStart)
                lastTimeCapacityUsed = -cooldown;
        }

        public override void Act()
        {
            Debug.Log(this);
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeCapacityUsed > cooldown)
                return true;
            else
                return false;
        }

        public void ShootProjectile()
        {
            Quaternion direction = PlayerDirection();
            GameObject projectile = Instantiate(bullet, transform.position, direction);
            projectile.GetComponent<HitStimulus>().SetDamageSource(HitStimulus.DamageSourceType.Enemy);

            Finish();
        }

        private Quaternion PlayerDirection()
        {
            Vector2 dir = PlayerController.instance.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion direction = Quaternion.AngleAxis(angle, Vector3.forward);
            return direction;
        }

        public override void Enter()
        {
            base.Enter();

            lastTimeCapacityUsed = Time.time;

            ShootProjectile();
        }
    }
}
