using Harmony;
using System.Collections;
using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    class Vulnerable : State
    {
        [Tooltip("Use Trigger '" + Values.AnimationParameters.Edgar.Vulnerable + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float duration;
        [SerializeField] private bool disableDamage = false;
        [SerializeField] private float delayBeforeDoingDamageToPlayerAfterLeavingState;
        private HitStimulus[] hitStimuli;

        private float timeEntered;

        private void Start()
        {
            hitStimuli = this.GetComponentsInObject<HitStimulus>();
            Debug.Log(hitStimuli.Length);
        }
        public override void Finish()
        {
            base.Finish();

            if(disableDamage)
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

            if(disableDamage)
            {
                foreach (HitStimulus hitStimulus in hitStimuli)
                    hitStimulus.Enabled = false;
            }

            timeEntered = Time.time;
            animator.SetTrigger(Values.AnimationParameters.Edgar.Vulnerable);
        }

        private IEnumerator enableStimulus()
        {
            yield return new WaitForSeconds(delayBeforeDoingDamageToPlayerAfterLeavingState);

            foreach (HitStimulus hitStimulus in hitStimuli)
                hitStimulus.Enabled = true;
        }
    }
}
