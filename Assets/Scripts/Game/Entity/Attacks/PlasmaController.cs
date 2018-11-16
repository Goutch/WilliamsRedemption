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

        private new void Awake()
        {
            base.Awake();
            hitStimulus = GetComponent<HitStimulus>();
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            HitStimulus otherHitStimulus;
            if (CanBeReturned && (otherHitStimulus = collision.GetComponent<HitStimulus>()) && otherHitStimulus.Range == HitStimulus.AttackRange.Melee)
            {
                transform.localRotation *= Quaternion.Euler(0, 0, 180);
                //TODO ARCHIEVEMENT
                this.hitStimulus.Type = HitStimulus.DamageType.Darkness;
            }
        }
    }
}

