using UnityEngine;

namespace Game.Entity.Enemies.Boss
{
    public class BossController : Enemy
    {
        [SerializeField] private State[] phases;
        private int currentPhaseIndex;

        public State GetCurrentState()
        {
            return phases[currentPhaseIndex].GetCurrentState();
        }

        protected override void Init()
        {
            currentPhaseIndex = 0;

            phases[currentPhaseIndex].OnStateFinish += BossController_OnStateFinish;
            phases[currentPhaseIndex].Enter();

            health.OnDeath += Health_OnDeath;
        }

        private void Health_OnDeath(GameObject receiver, GameObject attacker)
        {
            health.OnDeath -= Health_OnDeath;

            phases[currentPhaseIndex].OnStateFinish -= BossController_OnStateFinish;
            phases[currentPhaseIndex].Finish();
        }

        public void Update()
        {
            phases[currentPhaseIndex].Act();
        }

        private void BossController_OnStateFinish(State state)
        {
            phases[currentPhaseIndex].OnStateFinish -= BossController_OnStateFinish;

            if(currentPhaseIndex == phases.Length - 1)
            {
                Destroy(this.gameObject);
            }
            else
            {
                ++currentPhaseIndex;
                phases[currentPhaseIndex].Enter();
                phases[currentPhaseIndex].OnStateFinish += BossController_OnStateFinish;
            }
        }
    }
}


