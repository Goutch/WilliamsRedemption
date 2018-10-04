using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public abstract class Phase : State
    {
        [SerializeField] protected Capacity[] subStates;

        private bool IsIdling = true;

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
                currentState.Transit();
            }
        }

        public override void Transit()
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
                    Transit();
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
            foreach (Capacity subState in subStates)
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


