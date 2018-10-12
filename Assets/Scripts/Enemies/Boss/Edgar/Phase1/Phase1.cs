using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boss;
using Harmony;
using UnityEngine;

namespace Edgar
{
    class Phase1 : Phase
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.IdlePhase1 + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float percentageHealthTransitionCondition;
        private Health health;

        private new void Awake()
        {
            health = GetComponent<Health>();
            health.OnHealthChange += Health_OnHealthChange;
        }

        private void Health_OnHealthChange(GameObject gameObject)
        {
            if (health.HealthPoints / (float)health.MaxHealth <= percentageHealthTransitionCondition)
                Finish();
        }

        public override bool CanEnter()
        {
            return true;
        }

        protected override void Idle()
        {
            float directionX = Mathf.Sign(PlayerController.instance.transform.position.x - transform.position.x);
            if (directionX > 0)
                transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            else
                transform.rotation = Quaternion.AngleAxis(0, Vector3.up);
        }

        protected override void Initialise() { }

        protected override void EnterIdle()
        {
            animator.SetTrigger(R.S.AnimatorParameter.IdlePhase1);
        }
    }
}
