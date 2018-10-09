using Boss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edgar
{
    class JumpPhase : Phase
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
            subStates[1].OnStateFinish += JumpPhase_OnStateFinish;
        }

        private void JumpPhase_OnStateFinish(Boss.State state)
        {
            subStates[1].OnStateFinish -= JumpPhase_OnStateFinish;
            Finish();
        }
    }
}
