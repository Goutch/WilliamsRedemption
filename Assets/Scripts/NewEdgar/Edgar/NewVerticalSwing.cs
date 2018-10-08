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
    class NewVerticalSwing : Capacity
    {
        [Header("Config")]
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.VerticalSwing + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;

        [Header("Projectile")]
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject projectileSpawnPoint;

        private float lastTimeUsed;

        private new void Awake()
        {
            base.Awake();
            if (capacityUsableAtStart)
                lastTimeUsed = -cooldown;
        }

        public override void Act()
        {
            
        }

        public override bool CanTransit()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        public override void Transite()
        {
            base.Transite();
            animator.SetTrigger(R.S.AnimatorParameter.VerticalSwing);
            lastTimeUsed = Time.time;
        }

        public void OnVerticalSwingFinish()
        {
            ShootProjectileAfterVerticalSwing();
            base.Finish();
        }

        public void ShootProjectileAfterVerticalSwing()
        {
            GameObject projectileObject = Instantiate(projectile, projectileSpawnPoint.transform.transform.position, transform.rotation);
        }
    }
}
