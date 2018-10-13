using Playmode.EnnemyRework;
using UnityEngine;
 
namespace Boss
{
    public class BossController : Enemy
    {
        [SerializeField] private Phase[] phases;
        private int currentPhaseIndex;

        protected override void Init()
        {
            phases[currentPhaseIndex].OnStateFinish += BossController_OnStateFinish;

            currentPhaseIndex = 0;
            phases[currentPhaseIndex].Enter();
        }

        public void Update()
        {
            phases[currentPhaseIndex].Act();
        }

        private void BossController_OnStateFinish(State state, State nextState)
        {
            phases[currentPhaseIndex].OnStateFinish -= BossController_OnStateFinish;

            ++currentPhaseIndex;
            phases[currentPhaseIndex].Enter();
        }
    }
}


