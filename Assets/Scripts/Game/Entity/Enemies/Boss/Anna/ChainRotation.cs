using Game.Entity.Enemies.Boss;
using Game.Puzzle.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Anna
{
    class ChainRotation : SequentialLoopPhase
    {
        [SerializeField] private GameObject chain;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Transform teleportationPoint;
        [SerializeField] private new MeshLight light;

        private Animator animator;
        private SpawnedEnemyManager enemyManager;

        protected override void Init()
        {
            animator = GetComponent<Animator>();
            enemyManager = GetComponent<SpawnedEnemyManager>();
        }

        public override bool CanEnter()
        {
            return true;
        }

        public override void Enter()
        {
            base.Enter();

            animator.SetTrigger(Values.AnimationParameters.Anna.Special);

            chain.SetActive(true);

            light.Close();

            transform.position = teleportationPoint.transform.position;
        }

        public override void Finish()
        {
            base.Finish();
            enemyManager.Clear();
        }

        protected override void Idle()
        {
            base.Idle();

            chain.transform.Rotate(-Vector3.forward, rotationSpeed);
        }
    }
}