using Playmode.EnnemyRework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace Boss
{
    public class BossController : Enemy
    {
        [SerializeField] private Phase[] phases;
        private int currentPhaseIndex;

        private void ValidateSerilizeFiled()
        {
            if (phases.Length <= 0)
                throw new System.NullReferenceException("No phase attributed");

            phases[currentPhaseIndex].OnStateFinish += BossController_OnStateFinish;
        }

        private void BossController_OnStateFinish(State state, State nextState)
        {
            phases[currentPhaseIndex].OnStateFinish -= BossController_OnStateFinish;
            
            ++currentPhaseIndex;
            phases[currentPhaseIndex].Enter();
        }

        public void Update()
        {
            phases[currentPhaseIndex].Act();
        }

        protected override void Init()
        {
            ValidateSerilizeFiled();

            currentPhaseIndex = 0;
            phases[currentPhaseIndex].Enter();
        }
    }
}


