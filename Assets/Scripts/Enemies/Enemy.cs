using System.Security.AccessControl;
using DefaultNamespace.Playmode;
using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public abstract class Enemy : EnemyData
    {
        protected Health health;

        protected void Awake()
        {
            health = GetComponent<Health>();
            GetComponent<HitSensor>().OnHit += HandleCollision;
            health.OnDeath += Health_OnDeath;
            Init();
        }

        private void Health_OnDeath()
        {
            Destroy(this.gameObject);
        }

        protected abstract void Init();

        protected virtual void HandleCollision(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper || other.DamageSource == HitStimulus.DamageSourceType.William)
                health.Hit();
        }
    }
}