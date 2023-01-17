using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp12
{
    public class MoveAction : ChangeAction
    {
        public MoveAction(Boundary boundary, int time, int oldX, int oldY, int newX, int newY) : base(boundary)
        {
            TimeToDo = time;
            OldX = oldX;
            OldY = oldY;
            NewX= newX;
            NewY= newY;
        }
        public override void Do()
        {
            if (CurrentTime == TimeToDo)
            {
                Terminate();
            }
            else
            {
                Boundary.X = (int)Maths.Lerp(OldX, NewX, CurrentTime / (float)TimeToDo);
                Boundary.Y = (int)Maths.Lerp(OldY, NewY, CurrentTime / (float)TimeToDo);
                CurrentTime++;
            }
        }
        public override void Terminate()
        {
            Boundary.X = NewX;
            Boundary.Y = NewY;
            Boundary.ChangeAction = null;
        }
    }
}
