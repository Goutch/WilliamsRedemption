using Game.Entity.Enemies;
using Game.Entity.Enemies.Attack;
using Harmony;
using UnityEngine;

namespace Game.Entity
{
    public delegate void HitEventHandler(HitSensor other);
    public class HitStimulus : MonoBehaviour
    {
        private new bool enabled = true;
        public event HitEventHandler OnHitOther;
        public enum DamageSourceType
        {
            Reaper,
            William,
            Enemy,
            Obstacle,
            None,
        }

        [SerializeField] private DamageSourceType damageSource;

        public DamageSourceType DamageSource => damageSource;

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.GetComponent<IgnoreStimulus>() && enabled)
            {
                HitSensor hitSensor = other.collider.Root().GetComponent<HitSensor>();

                if (hitSensor != null)
                {
                    OnHitOther?.Invoke(hitSensor);
                    hitSensor.Hit(this);

                }
            }
        }

        private void Awake()
        {
            if (GetComponent<Enemy>())
            {
                damageSource = DamageSourceType.Enemy;
            }
        }

        public void SetDamageSource(DamageSourceType newType)
        {
            damageSource = newType;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<IgnoreStimulus>() && enabled)
            {
                HitSensor hitSensor = other.Root().GetComponent<HitSensor>();

                if (hitSensor != null)
                {
                    OnHitOther?.Invoke(hitSensor);
                    hitSensor.Hit(this);
                }
                
            }
        }
    }


}

