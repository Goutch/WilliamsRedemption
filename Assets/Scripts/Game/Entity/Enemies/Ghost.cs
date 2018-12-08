using Game.Entity.Enemies.Attack;
using Game.Entity.Player;
using Game.Puzzle.Light;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies
{
    public class Ghost : Enemy
    {
        [SerializeField] private float damageKnockBackForce = 1;
        [SerializeField] private float disapearTimeLimitBeforeDespawn = 3;
        [SerializeField] private float lifeDuration;

        private GameObject attack;
        private LightSensor playerLightSensor;

        private RootMover rootMover;
        private Rigidbody2D rigidBody;

        private bool isEnable = true;
        private float birth;

        private new void Awake()
        {
            base.Awake();
            rootMover = GetComponent<RootMover>();
            rigidBody = GetComponent<Rigidbody2D>();
            playerLightSensor = player.GetComponent<LightSensor>();

            attack = transform.GetChild(0).gameObject;

            if (playerLightSensor.InLight != false)
            {
                Disable();
            }

            birth = Time.time;
        }

        private void Update()
        {
            if (Time.time - birth > lifeDuration)
                health.Kill(gameObject);
        }

        protected override bool OnHit(HitStimulus hitStimulus)
        {
            if (hitStimulus.Type == HitStimulus.DamageType.Darkness)
            {
                base.OnHit(hitStimulus);

                Vector2 kockBackDir = (this.transform.position - hitStimulus.transform.root.position);
                rigidBody.AddForce(kockBackDir.normalized * damageKnockBackForce, ForceMode2D.Impulse);

                return true;
            }

            return false;
        }

        private void FixedUpdate()
        {
            if (playerLightSensor.InLight == false)
            {
                if (!isEnable)
                {
                    Enable();
                }

                rootMover.LookAtPlayer();
                rootMover.FlyToward(player.transform.position);
            }
            else
            {
                Disable();
            }
        }

        private void Enable()
        {
            spriteRenderer.enabled = true;
            attack.SetActive(true);

            isEnable = true;

            StopAllCoroutines();
        }

        private void Disable()
        {
            spriteRenderer.enabled = false;
            attack.SetActive(false);

            isEnable = false;

            StartCoroutine(DisapearRoutine());
        }

        protected override void Init()
        {
        }


        private IEnumerator DisapearRoutine()
        {
            yield return new WaitForSeconds(disapearTimeLimitBeforeDespawn);

            this.GetComponent<Health>().Kill(gameObject);
        }
    }
}