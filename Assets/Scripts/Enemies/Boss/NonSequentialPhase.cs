using UnityEngine;

namespace Playmode.EnnemyRework.Boss
{
    public abstract class NonSequentialPhase : State
    {
        [SerializeField] protected State[] subStates;

        private bool IsIdling = false;

        private State currentState;
        protected State CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                currentState = value;
                if(value != null)
                {
                    currentState.OnStateFinish += CurrentState_OnStateFinish;
                    currentState.Enter();
                }
            }
        }

        protected virtual void Idle()
        {
            if (!IsIdling)
            {
                EnterIdle();
                IsIdling = true;
            }
        }
        protected abstract void EnterIdle();

        public override void Act()
        {
            State nextState;
            if (currentState != null)
            {
                currentState.Act();
            }
            else if(nextState = GetAvailableState())
            {
                CurrentState = nextState;
                IsIdling = false;
            }
            else
            {
                Idle();
            }
        }

        public override void Finish()
        {
            currentState = null;
            base.Finish();
        }

        protected virtual void CurrentState_OnStateFinish(State state)
        {
            state.OnStateFinish -= CurrentState_OnStateFinish;

            currentState = null;
        }

        private State GetAvailableState()
        {
            foreach (State subState in subStates)
            {
                if (subState.CanEnter())
                    return subState;
            }

            return null;
        }
    }
}


