using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boss;
using UnityEngine;

namespace Edgar
{
    class NewHorizontalSwing : Capacity
    {
        private const string ANIMATOR_TRIGGER = "HorizontalSwing";

        [Tooltip("Use Trigger '" + ANIMATOR_TRIGGER + "' ")]
        [SerializeField] private Animator animator;
        [SerializeField] private float range;
        [SerializeField] private float cd;
        [SerializeField] private bool capacityUsableAtStart;
        private float lastTimeUsed;

        private new void Awake()
        {
            base.Awake();
            if (capacityUsableAtStart)
                lastTimeUsed = -cd;
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
            if (Time.time - lastTimeUsed > cd && Vector2.Distance(PlayerController.instance.transform.position, transform.position) < range)
                return true;
            else
                return false;
        }

        public override void Transit()
        {
            base.Transit();
            animator.SetTrigger(ANIMATOR_TRIGGER);
            lastTimeUsed = Time.time;
        }
    }
}
