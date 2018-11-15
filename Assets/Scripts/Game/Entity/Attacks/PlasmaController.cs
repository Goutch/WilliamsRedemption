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

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            HitStimulus hitStimulus;
            if (CanBeReturned && (hitStimulus = collision.GetComponent<HitStimulus>()) && hitStimulus.Range == HitStimulus.AttackRange.Melee)
            {
                transform.localRotation *= Quaternion.Euler(0, 0, 180);

                hitStimulus.Type = HitStimulus.DamageType.Darkness;
            }
        }
    }
}

