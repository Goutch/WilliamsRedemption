using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    public class BossController : MonoBehaviour
    {
        [SerializeField] private Phase[] phases;
        private int currentPhaseIndex;

        private void ValidateSerilizeFiled()
        {
            if (phases.Length <= 0)
                throw new System.NullReferenceException("No phase attributed");
        }

        private void Awake()
        {
            ValidateSerilizeFiled();

            currentPhaseIndex = 0;
            phases[currentPhaseIndex].Transit();
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
    }
}


