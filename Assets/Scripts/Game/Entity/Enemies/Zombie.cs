using Game.Entity.Enemies.Attack;
using Game.Entity.Player;
using Harmony;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Zombie : WalkTowardPlayerEnnemy
    {
        [SerializeField] private Vector2 bulletKnockBackForce;
        [SerializeField] private Vector2 playerKnockBackForce;

        private new Rigidbody2D rigidbody;
        private HitStimulus[] hitStimuli;
        private bool knocked = false;

        protected override void Init()
        {
            base.Init();
            rigidbody = GetComponent<Rigidbody2D>();
            hitStimuli = GetComponentsInChildren<HitStimulus>();
        }

        private void OnEnable()
        {
            foreach(HitStimulus hitStimulus in hitStimuli)
                hitStimulus.OnHitStimulusSensed += HitStimulus_OnHitStimulusSensed;
        }

        private void OnDisable()
        {
            foreach (HitStimulus hitStimulus in hitStimuli)
                hitStimulus.OnHitStimulusSensed -= HitStimulus_OnHitStimulusSensed;
        }

        private void HitStimulus_OnHitStimulusSensed(HitSensor hitSensor)
        {
            if (hitSensor.CompareTag(Values.Tags.Player))
            {
                int direction = (transform.root.rotation.y == -1 ? -1 : 1);

                hitSensor.Root().GetComponent<KinematicRigidbody2D>().AddForce(
                    new Vector2(playerKnockBackForce.x * direction, playerKnockBackForce.y));
            }
        }

        private new void FixedUpdate()
        {
            if (!knocked)
                base.FixedUpdate();
            if (knocked && rigidbody.velocity.y == 0)
                    knocked = false;
        }

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if(hitStimulus.Type == HitStimulus.DamageType.Darkness)
            {
                base.OnHit(hitStimulus);

                return true;
            }
            else if(hitStimulus.Type == HitStimulus.DamageType.Physical)
            {
                int direction = (transform.rotation.y == -1 ? -1 : 1);

                rigidbody.AddForce(new Vector2(bulletKnockBackForce.x * -direction, bulletKnockBackForce.y),
                    ForceMode2D.Impulse);
                knocked = true;

                return true;
            }

            return false;
        }
    }
}