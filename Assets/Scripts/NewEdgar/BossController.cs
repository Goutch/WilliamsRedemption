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
        }

        public void Update()
        {
            if(phases[currentPhaseIndex].IsFinish())
            {
                Debug.Log(currentPhaseIndex);
                ++currentPhaseIndex;
            }

            phases[currentPhaseIndex].Act();
        }

        protected override void Init()
        {
            ValidateSerilizeFiled();

            currentPhaseIndex = 0;
            phases[currentPhaseIndex].Transit();
        }
    }
}


