using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public delegate void OnStateFinish(State state, State nextState);

    [RequireComponent(typeof(BossController))]
    public abstract class State : MonoBehaviour
    {
        public event OnStateFinish OnStateFinish;
        protected BossController bossController;

        public abstract void Act();
        public abstract bool CanEnter();

        public virtual void Enter()
        {
            Initialise();
            Debug.Log(this);
        }

        public virtual void Finish(State nextState)
        {
            OnStateFinish?.Invoke(this, nextState);
        }

        public virtual void Finish()
        {
            OnStateFinish?.Invoke(this, null);
        }

        protected abstract void Initialise();

        protected void Awake()
        {
            bossController = GetComponent<BossController>();
        }
    }
}


