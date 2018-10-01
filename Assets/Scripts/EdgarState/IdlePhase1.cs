using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Edgar
{
    class IdlePhase1 : State
    {
        private EdgarController edgarController;

        public void Act()
        {
            if (edgarController.CanVerticalSwing())
                edgarController.SwingVertical();
            else if (PlayerController.instance.CurrentController.Collider.IsTouching(edgarController.Range))
                edgarController.OnPlayerInRange();
            else if (edgarController.CanShootPlasma())
                edgarController.TransitionToPlasmaShoot();
        }

        public bool Init(EdgarController edgarController)
        {
            this.edgarController = edgarController;
            return true;
        }
    }
}


