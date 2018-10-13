using System.Security.AccessControl;
using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private int scoreValue = 0;
        protected Health health;

        protected void Awake()
        {
            health = GetComponent<Health>();
            GetComponentInChildren<HitSensor>().OnHit  += HandleCollision;
            health.OnDeath += Health_OnDeath;
            Init();
        }

        private void Health_OnDeath(GameObject gameObject)
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