using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boss;
using Harmony;
using UnityEngine;

namespace Edgar
{
    class NewHorizontalSwing : Capacity
    {
        [Tooltip("Use Trigger '" + R.S.AnimatorParameter.HorizontalSwing + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float range;
        [SerializeField] private float cooldown;
        [SerializeField] private bool capacityUsableAtStart;
        private float lastTimeUsed;

        private new void Awake()
        {
            base.Awake();
            if (capacityUsableAtStart)
                lastTimeUsed = -cooldown;
        }

        public void HorizontalSwingFinish()
        {
            Finish();
        }

        public override void Act()
        {

        }

        public override bool CanTransit()
        {
            if (Time.time - lastTimeUsed > cooldown && Vector2.Distance(PlayerController.instance.transform.position, transform.position) < range)
                return true;
            else
                return false;
        }

        public override void Transite()
        {
            base.Transite();
            animator.SetTrigger(R.S.AnimatorParameter.HorizontalSwing);
            lastTimeUsed = Time.time;
        }
    }
}
