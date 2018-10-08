using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    [RequireComponent(typeof(BossController))]
    public abstract class State : MonoBehaviour
    {
        private bool isFinish;
        protected BossController bossController;

        public abstract void Act();
        public abstract bool CanTransit();

        public virtual void Transite()
        {
            isFinish = false;
        }

        public virtual void Finish()
        {
            isFinish = true;
        }

        public virtual bool IsFinish()
        {
            return isFinish;
        }

        protected void Awake()
        {
            bossController = GetComponent<BossController>();
        }
    }
}


