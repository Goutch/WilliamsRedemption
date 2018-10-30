using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    public class BossController : Enemy
    {
        [SerializeField] private NonSequentialPhase[] phases;
        private int currentPhaseIndex;

        protected override void Init()
        {
            currentPhaseIndex = 0;

            phases[currentPhaseIndex].OnStateFinish += BossController_OnStateFinish;
            phases[currentPhaseIndex].Enter();
        }
        
        public void Update()
        {
            phases[currentPhaseIndex].Act();
        }

        private void BossController_OnStateFinish(State state)
        {
            phases[currentPhaseIndex].OnStateFinish -= BossController_OnStateFinish;

            ++currentPhaseIndex;
            phases[currentPhaseIndex].Enter();
            //TODO : Register to next state
        }
    }
}


