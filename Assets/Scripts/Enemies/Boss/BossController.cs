using UnityEngine;
 
namespace Playmode.EnnemyRework.Boss
{
    public class BossController : Enemy
    {
        [SerializeField] private State[] phases;
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


