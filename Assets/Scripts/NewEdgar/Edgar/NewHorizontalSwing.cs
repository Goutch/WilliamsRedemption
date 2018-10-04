﻿using System;
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
            animator.SetTrigger(R.S.AnimatorParameter.HorizontalSwing);
            lastTimeUsed = Time.time;
        }
    }
}
