using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp12
{
    public abstract class ChangeAction
    {
        public Boundary Boundary;
        protected int CurrentTime = 0, TimeToDo, OldX, OldY, NewX, NewY;
        public ChangeAction(Boundary boundary)
        {
            Boundary = boundary;
        }
        public abstract void Do();
        public abstract void Terminate();
    }
}
