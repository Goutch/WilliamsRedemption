using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    public class PlasmaController : ProjectileController
    {
        [SerializeField] private bool canBeReturned;

        public bool CanBeReturned
        {
            get { return canBeReturned; }
            private set { canBeReturned = value; }
        }

        private HitStimulus hitStimulus;
        private HitSensor hitSensor;

        private void Awake()
        {
            base.Awake();
            hitStimulus = GetComponent<HitStimulus>();
            hitSensor = GetComponent<HitSensor>();
            hitSensor.OnHit += HitSensor_OnHit;
        }

        private void OnEnable()
        {
        }

        private bool HitSensor_OnHit(HitStimulus hitStimulus)
        {
            if (CanBeReturned
                && hitStimulus.Type == HitStimulus.DamageType.Darkness
                && hitStimulus.Range == HitStimulus.AttackRange.Melee)
            {
                transform.localRotation *= Quaternion.Euler(0, 0, 180);
                //TODO ARCHIEVEMENT
                this.hitStimulus.Type = HitStimulus.DamageType.Darkness;

                return false;
            }

            return false;
        }

        private void OnDisable()
        {
            hitSensor.OnHit -= HitSensor_OnHit;
        }
    }
}