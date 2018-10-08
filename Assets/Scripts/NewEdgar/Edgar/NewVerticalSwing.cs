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

        private SpawnedTilesManager spawnedTilesManager;

        private new void Awake()
        {
            base.Awake();
            if (capacityUsableAtStart)
                lastTimeUsed = -cooldown;

            spawnedTilesManager = GetComponent<SpawnedTilesManager>();
        }

        public override void Act() { }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        public void OnVerticalSwingFinish()
        {
            ShootProjectileAfterVerticalSwing();
            base.Finish();
        }

        public void ShootProjectileAfterVerticalSwing()
        {
            GameObject projectileObject = Instantiate(projectile, projectileSpawnPoint.transform.transform.position, transform.rotation);

            projectileObject.GetComponent<NewPlasmaGroundController>()?.Init(spawnedTilesManager);
        }

        protected override void Initialise()
        {
            animator.SetTrigger(R.S.AnimatorParameter.VerticalSwing);
            lastTimeUsed = Time.time;
        }
    }
}
