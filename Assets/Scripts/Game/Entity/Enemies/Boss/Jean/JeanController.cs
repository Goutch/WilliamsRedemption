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

        protected override void OnHit(HitStimulus other)
        {
            if (!shieldManager.IsShieldActive)
            {
                base.OnHit(other);
            }
        }
    }
}
