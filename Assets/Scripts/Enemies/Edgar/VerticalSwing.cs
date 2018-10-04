using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edgar
{
    class VerticalSwing : State
    {
        private EdgarController edgarController;

        public void Act()
        {
        }

        public bool Init(EdgarController edgarController)
        {
            this.edgarController = edgarController;
            return true;
        }
    }
}
