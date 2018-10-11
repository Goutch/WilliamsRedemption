using Boss;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Jacob
{
    class SpawnZombie : Capacity
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.IdlePhase1 + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private int numberOfZombiesToSpawn;
        [SerializeField] private GameObject zombiePrefab;
        [SerializeField] private float cooldown;
        [SerializeField] private State stunState;

        private float lastTimeUsed;
        private int zombieSpawned = 0;
        private int zombieAlive = 0;
        private bool isAllZombieSpawned = false;

        public override void Act()
        {
            
        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown && !isAllZombieSpawned)
                return true;
            else
                return false;
        }

        protected override void Initialise()
        {
            lastTimeUsed = Time.time;
            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase1);

            SpawnAZombie();
        }

        private void SpawnAZombie()
        {
            GameObject zombie = Instantiate(zombiePrefab, transform.position, Quaternion.identity);
            zombie.GetComponent<Health>().OnDeath += SpawnZombie_OnDeath;

            ++zombieSpawned;
            ++zombieAlive;

            if (zombieSpawned == numberOfZombiesToSpawn)
                isAllZombieSpawned = true;
        }

        private void SpawnZombie_OnDeath(GameObject gameObject)
        {
            gameObject.GetComponent<Health>().OnDeath -= SpawnZombie_OnDeath;

            --zombieAlive;

            if(isAllZombieSpawned && zombieAlive == 0)
            {
                Finish(stunState);
                isAllZombieSpawned = false;
                zombieSpawned = 0;
                zombieAlive = 0;
            }
        }
    }
}
