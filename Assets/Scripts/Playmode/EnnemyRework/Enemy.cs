using System.Security.AccessControl;
using DefaultNamespace.Playmode;
using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public abstract class Enemy : EnemyData, IEntityData
    {
        protected Health health;

        private void Awake()
        {
            Init();
            health = GetComponent<Health>();
            GetComponent<HitSensor>().OnHit += HandleCollision;
        }

        protected abstract void Init();

        protected virtual void HandleCollision(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper || other.DamageSource == HitStimulus.DamageSourceType.William)
                health.Hit();
        }
    }
}