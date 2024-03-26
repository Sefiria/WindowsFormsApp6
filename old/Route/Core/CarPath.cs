using Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Definitions;

namespace Core
{
    public class CarPath
    {
        public int TX, TY;
        public PathDots DS, DFW, DE;
        public PathDots? DSW;// Second way is optional
        public int CurrentDot;
        public float Speed = 0.1F;
        public float t = 0F;

        public Point Start => PathDotsLoc(DS) ?? default(Point);
        public Point End => PathDotsLoc(DE) ?? default(Point);
        public Point FirstWay => PathDotsLoc(DFW) ?? default(Point);
        public Point SecondWay => DSW == null ? default(Point) : PathDotsLoc(DSW.Value) ?? default(Point);
        public PathDots? DotEnum(int dotValue)
        {
            switch(dotValue)
            {
                case 0: return DS;
                case 1: return DFW;
                case 2: return DSW ?? DE;
                case 3: return DE;
                default: return DE;
            }
        }
        public PathDots? CurrentDotEnum => DotEnum(CurrentDot);
        public bool Ended => CurrentDotEnum == DE;

        public CarPath(int TX, int TY, PathDots DS, PathDots DFW, PathDots? DSW, PathDots DE)
        {
            this.TX = TX;
            this.TY = TY;
            this.DS = DS;
            this.DFW = DFW;
            this.DSW = DSW;
            this.DE = DE;

            CurrentDot = 0;
        }

        public void Next(ref int X, ref int Y, ref int Angle)
        {
            Point? nA = PathDotsLoc(DotEnum(CurrentDot));
            Point? nB = PathDotsLoc(DotEnum(CurrentDot + 1));
            if (nA == null || nB == null)
                return;

            Point A = nA.Value;
            Point B = nB.Value;
            int prevX = X, prevY = Y;
            X = Maths.Spline(t, A.X, B.X);
            Y = Maths.Spline(t, A.Y, B.Y);
            if (prevX != X && prevY != Y)
            {
                int aA = (Angle / 90) * 90;
                int aB = aA + (B.X - A.X > B.Y - A.Y ? 90 : -90);
                Angle = Maths.Spline(t, aA, aB);
            }
            t += Speed;
            
            if(t > 1F)
            {
                t = 0F;
                CurrentDot++;
            }
        }
    }
}
