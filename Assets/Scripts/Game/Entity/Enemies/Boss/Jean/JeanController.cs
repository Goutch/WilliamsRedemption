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
    }
}
