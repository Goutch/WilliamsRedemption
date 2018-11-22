using Harmony;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    class Vulnerable : Capacity
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.Vulnerable + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float duration;
        [SerializeField] private float delayBeforeDoingDamageToPlayerAfterLeavingState;
        [SerializeField] private Collider2D attackCollider;

        private float timeEntered;

        protected override void Init()
        {

        }

        public override void Finish()
        {
            base.Finish();

            if(delayBeforeDoingDamageToPlayerAfterLeavingState != 0)
                StartCoroutine(enableStimulus());
        }

        public override void Act()
        {
            if (Time.time - timeEntered > duration)
                Finish();
        }

        public override bool CanEnter()
        {
            return true;
        }
        public override void Enter()
        {
            base.Enter();

            if(delayBeforeDoingDamageToPlayerAfterLeavingState != 0 && attackCollider != null)
            {
                attackCollider.enabled = false;

                StopAllCoroutines();
            }

            timeEntered = Time.time;
            animator.SetTrigger(Values.AnimationParameters.Edgar.Vulnerable);
        }

        private IEnumerator enableStimulus()
        {
            yield return new WaitForSeconds(delayBeforeDoingDamageToPlayerAfterLeavingState);

            attackCollider.enabled = true;
        }
    }
}
