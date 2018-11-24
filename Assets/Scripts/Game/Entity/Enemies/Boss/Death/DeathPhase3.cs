using Game.Puzzle.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    class DeathPhase3 : NonSequentialPhase
    {
        [SerializeField] private MeshLight closeLight;
        [SerializeField] private MeshLight openLight;

        private Rigidbody2D rb;
        private SpawnedEnemyManager[] spawnedEnemyManagers;

        protected override void Init()
        {
            rb = GetComponent<Rigidbody2D>();
            spawnedEnemyManagers = GetComponents<SpawnedEnemyManager>();
        }

        public override void Finish()
        {
            base.Finish();

            foreach (SpawnedEnemyManager spawnedEnemyManager in spawnedEnemyManagers)
                spawnedEnemyManager.Clear();
        }

        public override bool CanEnter()
        {
            return true;
        }

        public override void Enter()
        {
            base.Enter();

            closeLight.Close();
            openLight.Open();

            rb.bodyType = RigidbodyType2D.Static;
        }
    }
}