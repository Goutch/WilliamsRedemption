using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Entity.Enemies.Boss.Anna
{
    [RequireComponent(typeof(BossController), typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator), typeof(HitSensor))]
    class Invulnerable : Capacity
    {
        [SerializeField] private float cooldown;
        [SerializeField] private float duration;

        private BossController bossController;
        private HitSensor hitSensor;
        private SpriteRenderer sprite;
        private Animator animator;

        private float lastTimeUsed;
        private bool isCapacityBeingUsed = false;

        private void Awake()
        {
            bossController = GetComponent<BossController>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            hitSensor = GetComponent<HitSensor>();
        }

        public override void Act()
        {
            
        }

        public override bool CanEnter()
        {
            return Time.time - lastTimeUsed > cooldown;
        }

        public override void Enter()
        {
            base.Enter();
            animator.SetTrigger(Values.AnimationParameters.Anna.Invulnerable);

            bossController.IsInvulnerable = true;

            sprite.color = Color.blue;

            isCapacityBeingUsed = true;
        }

        [UsedImplicitly]
        public void MakeInvulnerable()
        {
            Finish();
        }

        public override void Finish()
        {
            base.Finish();

            lastTimeUsed = Time.time;

            hitSensor.OnHit += HitSensor_OnHit;

            StopAllCoroutines();
            StartCoroutine(RemoveInvulnerablityCoroutine());

            isCapacityBeingUsed = false;
        }

        private void HitSensor_OnHit(HitStimulus otherStimulus)
        {
            if(otherStimulus.DamageSource == HitStimulus.DamageSourceType.Enemy)
            {
                hitSensor.OnHit -= HitSensor_OnHit;

                Destroy(otherStimulus.gameObject);

                if (isCapacityBeingUsed)
                    Finish();

                StopAllCoroutines();
                RemoveInvulnerablity();
            }
        }

        private IEnumerator RemoveInvulnerablityCoroutine()
        {
            yield return new WaitForSeconds(duration);
            RemoveInvulnerablity();
        }

        private void RemoveInvulnerablity()
        {
            bossController.IsInvulnerable = false;

            sprite.color = Color.white;
        }
    }
}
