using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowsFormsApp12
{
    public enum BoundaryTypes
    {
        Circle,
        Rectangle,
        Path
    }

    public class Boundary
    {
        public string Tag = "";
        public BoundaryTypes Type { get; private set; } = BoundaryTypes.Circle;
        public int X, Y, W, H, R;
        public (Point Point, PathPointType Type)[] Path { get; private set; }
        public bool MouseOnArea = true;

        public Point[] GetPathPoints() => Path.ToList().Select(x => x.Point).ToArray();
        public PathPointType[] GetPathTypes() => Path.ToList().Select(x => x.Type).ToArray();
        public byte[] GetPathTypesAsBytes() => Path.ToList().Select(x => (byte)x.Type).ToArray();
        public int DrawX, DrawY, DrawW, DrawH;
        public float CumulOut = 0F, CumulOutMax = 17, SpeedCumulOut = 0.2F;
        public ChangeAction ChangeAction = null;
        public float Opacity = 1F;

        public Boundary()
        {
            Type = BoundaryTypes.Circle;
            X = 0;
            Y = 0;
            R = 100;
        }
        public Boundary(Boundary copy)
        {
            Type = copy.Type;
            X = copy.X;
            Y = copy.Y;
            R= copy.R;
            W= copy.W;
            H= copy.H;
            Path = copy.Path;
        }

        public void Move(int x, int y)
        {
            X += x;
            if (X < -Core.X + R) X = -Core.X + R;
            if (X > Core.X - R) X = Core.X - R;
            Y += y;
            if (Y < -Core.Y + R) Y = -Core.Y + R;
            if (Y > Core.Y - R) Y = Core.Y - R;
        }
        public void ReSize(int amount)
        {
            R += amount;
            if (R < 50) R = 50;
            if (R > 400) R = 400;
            W += amount;
            if (W < 100) W = 50;
            if (W > 800) W = 800;
            H += amount;
            if (H < 100) H = 50;
            if (H > 400) H = 400;
        }
        public void Fake(int time, int fakeDirection, int speedMove)
        {
            if (ChangeAction != null)
                ChangeAction.Terminate();
            ChangeAction = new FakeAction(this, time, fakeDirection, speedMove);
        }
        public void Hide(int time)
        {
            if (ChangeAction != null)
                ChangeAction.Terminate();
            ChangeAction = new HideAction(this, time);
        }
        public void NewCircle(int time, int r, int? x = null, int? y = null)
        {
            //if (ChangeAction != null)
            //    ChangeAction.Terminate();
            //ChangeAction = new MoveAction(this, time, X, Y, X + x.Value, Y + y.Value);
        }
        public void NewRectangle(int time, int w, int h, int? x = null, int? y = null)
        {
        }
        public void NewPath(int time, (Point, PathPointType)[] path)
        {
        }


        public void Update()
        {
            MouseOnArea = Contains(ManageMouse.MousePos.Renderize());
            if (!MouseOnArea)
                CumulOut += SpeedCumulOut;
            else
                CumulOut = 0F;
            if (CumulOut > CumulOutMax)
            {
                if (Tag == "Fake")
                    CumulOut = CumulOutMax;
                else
                {
                    //CumulOut = 0F;
                    //if(Keyboard.IsKeyDown(Key.T))
                    //    Data.End();
                    Data.End();
                }
            }

            if (ChangeAction != null)
            {
                ChangeAction.Do();
            }
        }

        public void Draw()
        {
            switch(Type)
            {
                case BoundaryTypes.Circle: DrawCircle(); break;
                case BoundaryTypes.Rectangle: DrawRectangle(); break;
                case BoundaryTypes.Path: DrawPath(); break;
            }
        }
        private void DrawCircle()
        {
            DrawX = Core.X + X - R;
            DrawY = Core.Y + Y - R;
            DrawW = R * 2 - 3;
            DrawH = R * 2 - 3;
            if(!MouseOnArea)
                Core.g.FillEllipse(new SolidBrush(Color.FromArgb((byte)(175F * Opacity), Color.FromArgb((int)CumulOut * 15, 0, 0))), DrawX, DrawY, DrawW, DrawH);
            Core.g.DrawEllipse(new Pen(Color.FromArgb((byte)(255F * Opacity), Color.White)), DrawX, DrawY, DrawW, DrawH);
        }
        private void DrawRectangle()
        {
            DrawX = Core.X + X - W / 2;
            DrawY = Core.Y + Y - H / 2;
            DrawW = W;
            DrawH = H;
            Core.g.DrawRectangle(new Pen(Color.FromArgb((byte)(255F * Opacity), Color.White)), DrawX, DrawY, DrawW, DrawH);
        }
        private void DrawPath()
        {
            Core.g.DrawPath(new Pen(Color.FromArgb((byte)(255F * Opacity), Color.White)), new GraphicsPath(GetPathPoints(), GetPathTypesAsBytes()));
        }

        public bool Contains(Point pos)
        {
            switch (Type)
            {
                case BoundaryTypes.Circle: return ContainsCircle(pos);
                case BoundaryTypes.Rectangle: return ContainsRectangle(pos);
                case BoundaryTypes.Path: return ContainsPath(pos);
            }
            return false;
        }
        private bool ContainsCircle(Point pos)
        {
            return Maths.Distance(pos.X, pos.Y, X, Y) <= R;
        }
        private bool ContainsRectangle(Point pos)
        {
            return false;
        }
        private bool ContainsPath(Point pos)
        {
            return false;
        }
    }
}
