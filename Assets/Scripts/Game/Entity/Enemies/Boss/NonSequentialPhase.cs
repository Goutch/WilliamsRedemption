using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    public abstract class NonSequentialPhase : Phase
    {
        [SerializeField] protected State[] subStates;
        [SerializeField] protected State[] passiveCapacities;
        protected bool IsIdling = false;

        protected State currentState;

        protected virtual void Idle()
        {
            if (!IsIdling)
            {
                EnterIdle();
            }
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

        protected virtual void CurrentState_OnStateFinish(State state)
        {
            state.OnStateFinish -= CurrentState_OnStateFinish;

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

        protected virtual void TransiteToNextStateIfReady()
        {
            foreach(State state in subStates)
            {
                if(state.CanEnter())
                {
                    if(IsIdling)
                        ExitIdle();

                    currentState = state;
                    currentState.OnStateFinish += CurrentState_OnStateFinish;
                    currentState.Enter();

                    break;
                }
            }
        }

        public override State GetCurrentState()
        {
            return currentState?.GetCurrentState() ?? this;
        }

        public override void Finish()
        {
            currentState?.Finish();
            base.Finish();
        }
    }
}


