using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp12
{
    public class HideAction : ChangeAction
    {
        public HideAction(Boundary boundary, int time) : base(boundary)
        {
            TimeToDo = time;
        }
        public override void Do()
        {
            if (CurrentTime == TimeToDo)
            {
                Terminate();
            }
            else
            {
                Boundary.Opacity = 0F;
                CurrentTime++;
            }
        }
        public override void Terminate()
        {
            Boundary.Opacity = 1F;
            Boundary.ChangeAction = null;
        }
    }
}
