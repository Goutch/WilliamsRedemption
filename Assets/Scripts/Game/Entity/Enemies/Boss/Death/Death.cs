﻿using Game.Entity.Enemies.Attack;
using Game.Puzzle.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Death
{
    class Death : BossController
    {
        private FloorManager floorManager;
        private ProjectileManager projectileManager;

        protected override void Init()
        {
            base.Init();
            health.OnDeath += Health_OnDeath;
            floorManager = GetComponent<FloorManager>();
            projectileManager = GetComponent<ProjectileManager>();
        }

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (hitStimulus.Type == HitStimulus.DamageType.Darkness)
                return base.OnHit(hitStimulus);

            return false;
        }

        private void Health_OnDeath(GameObject receiver, GameObject attacker)
        {
            health.OnDeath -= Health_OnDeath;
            floorManager.MoveAllFloorsUp();

            projectileManager.Clear();
        }
    }
}