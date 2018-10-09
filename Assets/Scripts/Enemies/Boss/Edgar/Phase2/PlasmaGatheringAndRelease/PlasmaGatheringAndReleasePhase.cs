using Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Edgar
{
    class PlasmaGatheringAndReleasePhase : Phase
    {
        public override bool CanEnter()
        {
            return subStates[0].CanEnter();
        }

        protected override void EnterIdle()
        {

        }

        protected override void Idle()
        {

        }

        protected override void Initialise()
        {
            subStates[1].OnStateFinish += PlasmaRelease_OnStateFinish;
        }

        private void PlasmaRelease_OnStateFinish(Boss.State state)
        {
            subStates[1].OnStateFinish -= PlasmaRelease_OnStateFinish;
            Finish();
        }
    }
}
