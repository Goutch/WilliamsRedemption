using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    class SequentialPhase : State
    {
        [SerializeField] private State[] subState;
        private int currentIndex;

        public override void Act()
        {
            subState[currentIndex].Act();
        }

        public override bool CanEnter()
        {
            if (subState[0].CanEnter())
                return true;
            else
                return false;
        }

        public override void Enter()
        {
            base.Enter();
            currentIndex = 0;

            subState[currentIndex].Enter();
            subState[currentIndex].OnStateFinish += TransiteToNextState;
        }

        private void TransiteToNextState(State stateFinish)
        {
            subState[currentIndex].OnStateFinish -= TransiteToNextState;

            currentIndex++;

            if (currentIndex == subState.Length)
            {
                Finish();
            }
            else
            {
                subState[currentIndex].Enter();
                subState[currentIndex].OnStateFinish += TransiteToNextState;
            }
        }
    }
}
