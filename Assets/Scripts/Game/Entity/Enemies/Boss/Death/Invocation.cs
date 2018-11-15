using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    public class Invocation : Capacity
    {
        [SerializeField] private GameObject enemyToInvoque;
        [SerializeField] private float cooldown;
        [SerializeField] private Transform[] invocationPoints;
        [SerializeField] private bool UsableAtStart;

        [SerializeField] private SpawnedEnemyManager enemyManager;
        private float lastTimeUsed;

        private void Awake()
        {
            if (UsableAtStart)
                lastTimeUsed = int.MinValue;
        }

        public override void Act()
        {

        }

        public override bool CanEnter()
        {
            if (Time.time - lastTimeUsed > cooldown)
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();

            foreach(Transform spawnPoint in invocationPoints)
            {
                enemyManager.SpawnEnemy(enemyToInvoque, spawnPoint.position, Quaternion.identity);
            }

            lastTimeUsed = Time.time;

            Finish();
        }
    }
}
