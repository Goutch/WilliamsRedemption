﻿using System.Collections;
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
                if(value != null)
                {
                    currentState.OnStateFinish += CurrentState_OnStateFinish;
                    currentState.Enter();
                }
            }
        }

        public override void Finish()
        {
            currentState = null;
            base.Finish();
        }

        protected virtual void CurrentState_OnStateFinish(State state, State nextState)
        {
            state.OnStateFinish -= CurrentState_OnStateFinish;

            if (nextState != null)
                CurrentState = nextState;
            else
                currentState = null;
        }

        protected abstract void Idle();

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
                if (!IsIdling)
                {
                    EnterIdle();
                    IsIdling = true;
                }

                Idle();
            }
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

