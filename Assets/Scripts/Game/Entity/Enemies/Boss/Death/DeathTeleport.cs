using Game.Entity.Enemies.Boss.Jacob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    class DeathTeleport : Capacity
    {
        [SerializeField] private Transform[] spawnTeleport;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;

        private RootMover mover;
        private FloorManager floorManager;

        private int nextTeleport = 0;
        private float lastTimeUsed;

        protected override void Init()
        {
            mover = GetComponent<RootMover>();
            floorManager = GetComponent<FloorManager>();

            if (capacityUsableAtStart)
                lastTimeUsed = float.MinValue;
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

            Teleport();

            floorManager.ShiftFloors();

            mover.LookAtPlayer();

            Finish();

            lastTimeUsed = Time.time;
        }

        private void Teleport()
        {
            transform.position = spawnTeleport[nextTeleport].position;

            nextTeleport = ++nextTeleport % spawnTeleport.Length;
        }
    }
}