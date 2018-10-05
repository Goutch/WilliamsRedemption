using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edgar
{
    class IdlePhase2 : State
    {
        private EdgarController edgarController;

        public void Act()
        {
            if (edgarController.CanJump())
                edgarController.TransitionToJumpState();
        }

        public bool Init(EdgarController edgarController)
        {
            this.edgarController = edgarController;
            return true;
        }
    }
}