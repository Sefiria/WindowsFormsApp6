using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp12
{
    public class FakeAction : ChangeAction
    {
        int FakeDirection = -1;
        Boundary BoundaryFake;
        int SpeedMove;

        public FakeAction(Boundary boundary, int time, int fakeDirection, int speedMove) : base(boundary)
        {
            TimeToDo = time;
            FakeDirection = fakeDirection;
            SpeedMove = speedMove;
            if (FakeDirection < 0) FakeDirection = 0;
            if (FakeDirection > 3) FakeDirection = 3;
            BoundaryFake = new Boundary(boundary);
            BoundaryFake.Tag = "Fake";
            ManageEntities.Boundaries.Add(BoundaryFake);
        }
        public override void Do()
        {
            if (CurrentTime == TimeToDo)
            {
                Terminate();
            }
            else
            {
                if (CurrentTime < TimeToDo / 2)
                {
                    BoundaryFake.X += FakeDirection == 1 ? -SpeedMove : (FakeDirection == 3 ? SpeedMove : 0);
                    BoundaryFake.Y += FakeDirection == 0 ? -SpeedMove : (FakeDirection == 2 ? SpeedMove : 0);
                }
                else
                {
                    BoundaryFake.Opacity = 1F - (CurrentTime / (float)TimeToDo);
                    if (CurrentTime < TimeToDo * 0.75)
                    {
                        BoundaryFake.X += FakeDirection == 2 ? -SpeedMove : (FakeDirection == 0 ? SpeedMove : 0);
                        BoundaryFake.Y += FakeDirection == 1 ? -SpeedMove : (FakeDirection == 3 ? SpeedMove : 0);
                    }
                }
                BoundaryFake.R = Boundary.R;
                BoundaryFake.W = Boundary.W;
                BoundaryFake.H = Boundary.H;

                CurrentTime++;
            }
        }
        public override void Terminate()
        {
            Boundary.ChangeAction = null;
            ManageEntities.Boundaries.Remove(BoundaryFake);
            BoundaryFake = null;
        }
    }
}
