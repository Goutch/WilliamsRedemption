using Game.Entity.Enemies.Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Anna
{
    public class Anna : BossController
    {
        protected override void OnHit(HitStimulus other)
        {
            base.OnHit(other);

            if (!IsInvulnerable && other.DamageSource == HitStimulus.DamageSourceType.Enemy)
            {
                health.Hit();
            }

            if(other.CompareTag(Values.Tags.ProjectileEnemy) && other.DamageSource == HitStimulus.DamageSourceType.Enemy)
                Destroy(other.gameObject);
        }
    }
}
