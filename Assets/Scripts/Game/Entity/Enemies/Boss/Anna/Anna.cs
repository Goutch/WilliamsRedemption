using Game.Entity.Enemies.Attack;
using Game.Entity.Enemies.Boss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Anna
{
    public class Anna : BossController
    {
        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (hitStimulus.Type == HitStimulus.DamageType.Enemy)
            {
                IsInvulnerable = false;
                animator.SetBool(Values.AnimationParameters.Anna.Invulnerable, false);
                health.Hit(hitStimulus.gameObject);

                return true;
            }
            else if (!IsInvulnerable && (hitStimulus.Type == HitStimulus.DamageType.Darkness ||
                                         hitStimulus.Type == HitStimulus.DamageType.Physical))
            {
                health.Hit(hitStimulus.gameObject);
                return true;
            }

            return true;
        }
    }
}