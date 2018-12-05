using Game.Entity.Enemies.Attack;
using Game.Entity.Player;
using Harmony;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Zombie : WalkTowardPlayerEnnemy
    {
        [SerializeField] private Vector2 bulletKnockBackForce;
        [SerializeField] private Vector2 DarknessKnockBackForce;
        [SerializeField] private Vector2 playerKnockBackForce;

        [Header("Sound")] [SerializeField] private AudioClip zombieSound;
        [SerializeField] private float timerBetweenZombieMoans;
        [SerializeField] private GameObject soundToPlayPrefab;
        [SerializeField] private float maximumDistanceSoundX;
        [SerializeField] private float maximumDistanceSoundY;
        
        private float timeSinceLastMoan;
        private const float PUSH_BACK_TIME = 0.2f;

        private new Rigidbody2D rigidbody;
        private HitStimulus[] hitStimuli;
        private bool knocked = false;

        protected override void Init()
        {
            base.Init();
            rigidbody = GetComponent<Rigidbody2D>();
            hitStimuli = GetComponentsInChildren<HitStimulus>();
            timeSinceLastMoan = Time.time;
        }

        private void OnEnable()
        {
            foreach (HitStimulus hitStimulus in hitStimuli)
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
        }

        private void Update()
        {
            ZombieMoans();
        }

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            
            if (hitStimulus.Type == HitStimulus.DamageType.Darkness)
            {
                base.OnHit(hitStimulus);
                int direction = (transform.rotation.y == -1 ? -1 : 1);

                rigidbody.AddForce(new Vector2(DarknessKnockBackForce.x * -direction, DarknessKnockBackForce.y),
                    ForceMode2D.Impulse);
                StartCoroutine(KnockBack());
                return true;
            }
            else if (hitStimulus.Type == HitStimulus.DamageType.Physical)
            {
                int direction = (transform.rotation.y == -1 ? -1 : 1);

                rigidbody.AddForce(new Vector2(bulletKnockBackForce.x * -direction, bulletKnockBackForce.y),
                    ForceMode2D.Impulse);
                StartCoroutine(KnockBack());
                return true;
            }

            return false;
        }

        private IEnumerator KnockBack()
        {
            knocked = true;
            yield return new WaitForSeconds(PUSH_BACK_TIME);
            knocked = false;
        }

        private void ZombieMoans()
        {
            Vector2 distancePlayerAndProjectile = transform.position - GameObject.FindGameObjectWithTag(Values.Tags.Player).transform.position;
            if (Time.time - timeSinceLastMoan > timerBetweenZombieMoans &&
                Mathf.Abs(distancePlayerAndProjectile.x) < maximumDistanceSoundX &&
                Mathf.Abs(distancePlayerAndProjectile.y) < maximumDistanceSoundY)
            {
                SoundCaller.CallSound(zombieSound, soundToPlayPrefab, gameObject, true);
                timeSinceLastMoan = Time.time;
            }
        }
    }
}