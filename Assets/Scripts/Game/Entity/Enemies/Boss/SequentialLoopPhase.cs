using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    public class SequentialLoopPhase : Phase
    {
        [SerializeField] protected State[] subStates;
        [SerializeField] protected State[] passiveCapacities;
        protected int currentIndex;

        protected bool IsIdling = false;

        protected State currentState;

        public override void Act()
        {
            UsePassiveCapacity();

            if (currentState == null)
                TransiteToNextStateIfReady();

            if (currentState != null)
                currentState.Act();
            else
                Idle();
        }

        protected virtual void TransiteToNextStateIfReady()
        {
            if (subStates.Length != 0 && subStates[currentIndex].CanEnter())
            {
                if (IsIdling)
                    ExitIdle();

                currentState = subStates[currentIndex];

                currentState.OnStateFinish += CurrentState_OnStateFinish;
                currentState.Enter();
            }
        }

        protected virtual void Idle()
        {
            if (!IsIdling)
            {
                EnterIdle();
            }
        }

        public override bool CanEnter()
        {
            if (subStates[0].CanEnter())
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            currentIndex = 0;

            currentState = null;
            base.Enter();
        }

        protected virtual void EnterIdle()
        {
            IsIdling = true;
            Debug.Log("Enter Idle: " + this);
        }

        protected virtual void ExitIdle()
        {
            IsIdling = false;
            Debug.Log("Exit Idle: " + this);
        }

        protected virtual void CurrentState_OnStateFinish(State state)
        {
            state.OnStateFinish -= CurrentState_OnStateFinish;

            currentIndex++;

            if (currentIndex == subStates.Length)
            {
                currentIndex = 0;
            }

            currentState = null;
        }

        private void UsePassiveCapacity()
        {
            foreach (State passiveCapacity in passiveCapacities)
            {
                if (passiveCapacity.CanEnter())
                    passiveCapacity.Enter();
            }
        }

        public override void Finish()
        {
            currentState?.Finish();
            currentState = null;
            base.Finish();
        }

        public override State GetCurrentState()
        {
            return currentState?.GetCurrentState() ?? this;
        }
    }
}
