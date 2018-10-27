using System;
using UnityEngine;

namespace Playmode.EnnemyRework.Boss
{
    public abstract class SequentialPhaseWithWait : State
    {
        [SerializeField] private State[] subState;

        private bool currentStateFinish;
        private int currentIndex;

        private bool IsIdling = false;

        public override void Act()
        {
            if (!currentStateFinish)
                subState[currentIndex].Act();
            else
                WaitForNextState();
        }

        private void WaitForNextState()
        {
            if(subState[currentIndex + 1].CanEnter())
            {
                IsIdling = false;
                currentIndex++;

                EnterCurrentState();
            }
            else
            {
                Idle();
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

        public override bool CanEnter()
        {
            if (subState[0].CanEnter())
                return true;
            else
                return false;
        }

        private void EnterCurrentState()
        {
            currentStateFinish = false;

            subState[currentIndex].Enter();
            subState[currentIndex].OnStateFinish += CurrentState_OnStateFinish;
        }

        public override void Enter()
        {
            base.Enter();
            currentIndex = 0;

            EnterCurrentState();
        }

        private void CurrentState_OnStateFinish(State stateFinish)
        {
            subState[currentIndex].OnStateFinish -= CurrentState_OnStateFinish;

            currentStateFinish = true;

            if (currentIndex + 1 == subState.Length)
            {
                Finish();
            }
        }
    }
}
