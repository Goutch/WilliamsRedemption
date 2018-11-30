using UnityEngine;

namespace Game.Entity.Enemies.Attack
{
    public class PlasmaController : ProjectileController
    {
        [SerializeField] private bool canBeReturned;
        private HitSensor hitSensor;

        //BEN_REVIEW : Peut être carrément supprimé. Non utilisé à l'extérieur.
        public bool CanBeReturned
        {
            get { return canBeReturned; }
            private set { canBeReturned = value; }
        }

        private new void Awake()
        {
            base.Awake();
            hitSensor = GetComponent<HitSensor>();
        }

        private void OnEnable()
        {
            hitSensor.OnHit += HitSensor_OnHit;
        }

        private bool HitSensor_OnHit(HitStimulus hitStimulus)
        {
            if (CanBeReturned
                && hitStimulus.Type == HitStimulus.DamageType.Darkness
                && hitStimulus.Range == HitStimulus.AttackRange.Melee)
            {
                transform.localRotation *= Quaternion.Euler(0, 0, 180);
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