using System.Security.AccessControl;
using DefaultNamespace.Playmode;
using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public abstract class Enemy : EnemyData
    {
        [SerializeField] private int scoreValue=0;
        protected Health health;
        

        protected void Awake()
        {
            health = GetComponent<Health>();
            GetComponentInChildren<HitSensor>().OnHit  += HandleCollision;
            Init();
        }

        protected abstract void Init();

        protected virtual void HandleCollision(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper || other.DamageSource == HitStimulus.DamageSourceType.William)
                health.Hit();
        }
    }
}