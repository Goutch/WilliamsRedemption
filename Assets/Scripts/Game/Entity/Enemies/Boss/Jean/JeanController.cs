using Game.Entity.Enemies.Attack;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Jean
{
    class JeanController : BossController
    {
        private ShieldManager shieldManager;

        private new void Awake()
        {
            shieldManager = GetComponent<ShieldManager>();
            base.Awake();
        }

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (!shieldManager.IsShieldActive)
                return base.OnHit(hitStimulus);
            else
                return false;
        }
    }
}