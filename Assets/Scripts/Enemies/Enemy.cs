using System.Security.AccessControl;
using Harmony;
using UnityEngine;

namespace Playmode.EnnemyRework
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] private int scoreValue = 0;
        
        protected Health health;
        protected Animator animator;
        public int ScoreValue => scoreValue;

        protected void Awake()
        {
            health = GetComponent<Health>();
            GetComponentInChildren<HitSensor>().OnHit  += OnHit;
            health.OnDeath += OnDeath;
            animator = GetComponent<Animator>();
            Init();
        }

        protected abstract void Init();

        protected virtual void OnHit(HitStimulus other)
        {
            if (other.DamageSource == HitStimulus.DamageSourceType.Reaper || other.DamageSource == HitStimulus.DamageSourceType.William)
                health.Hit();
        }

        private void OnDeath(GameObject gameObject)
        {
            Destroy(this.gameObject);
        }
    }
}