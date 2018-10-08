using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public abstract class Phase : State
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
                currentState?.Transite();
            }
        }

        public virtual void OnLeftSubState()
        {
            IsIdling = true;
        }

        protected abstract void Idle();

        public override void Act()
        {
            if(currentState != null && currentState.IsFinish())
            {
                if(!IsIdling)
                {
                    OnLeftSubState();
                }

                ChangeStateIfAvailable();
            }
            else if(currentState == null)
            {
                ChangeStateIfAvailable();
            }

            if (currentState != null && !currentState.IsFinish())
                currentState.Act();
            else
                Idle();

        }

        private void ChangeStateIfAvailable()
        {
            foreach (State subState in subStates)
            {
                if (subState.CanTransit())
                {
                    CurrentState = subState;
                    IsIdling = false;
                    return;
                }
            }
            IsIdling = true;
        }
    }
}


