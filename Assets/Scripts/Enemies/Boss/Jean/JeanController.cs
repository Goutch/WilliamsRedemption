using Boss;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss.Jean
{
    class JeanController : BossController
    {
        private ShieldManager shieldManager;

        private new void Awake()
        {
            shieldManager = GetComponentInChildren<ShieldManager>();
            base.Awake();
        }

        protected override void OnHit(HitStimulus other)
        {
            if(!shieldManager.IsShieldActive)
                base.OnHit(other);
        }
    }
}
