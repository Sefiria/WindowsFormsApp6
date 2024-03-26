using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Definitions
    {
        /*
            CA          Topleft     TopRight    CZ
            LeftTop     A           Z           RightTop
            LeftBottom  Q           S           RightBottom
            CQ          BottomLeft  BottomRight CS

               12   0   1  13
                3   8   9   6
                2  10  11   7
               14   5   4   15

            Flow direction Right = even : 0, 2, 4, 6
            Flow direction Left = odd : 1, 3, 5, 7
        */
        public enum PathDots
        {
            TopLeft = 0,
            TopRight = 1,
            BottomLeft = 5,
            BottomRight = 4,
            LeftTop = 3,
            LeftBottom = 2,
            RightTop = 6,
            RightBottom = 7,
            A = 8,
            Z = 9,
            Q = 10,
            S = 11,
            CA = 12,
            CZ = 13,
            CQ = 14,
            CS = 15
        }
        public enum PenType { Pen, Eraser, Fill }
        public enum PathDotType { None, In, Out, Way }
        public static readonly Dictionary<PathDots, Rectangle> PathDotsRect = new Dictionary<PathDots, Rectangle>()
        {
            [PathDots.TopLeft] = new Rectangle((int)(128 / 6F * 2 - 5), 2, 10, 10),
            [PathDots.TopRight] = new Rectangle((int)(128 / 6F * 4 - 5), 2, 10, 10),
            [PathDots.BottomLeft] = new Rectangle((int)(128 / 6F * 2 - 5), 128 - 10 - 2, 10, 10),
            [PathDots.BottomRight] = new Rectangle((int)(128 / 6F * 4 - 5), 128 - 10 - 2, 10, 10),
            [PathDots.LeftTop] = new Rectangle(2, (int)(128 / 6F * 2 - 5), 10, 10),
            [PathDots.LeftBottom] = new Rectangle(2, (int)(128 / 6F * 4 - 5), 10, 10),
            [PathDots.RightTop] = new Rectangle(128 - 10 - 2, (int)(128 / 6F * 2 - 5), 10, 10),
            [PathDots.RightBottom] = new Rectangle(128 - 10 - 2, (int)(128 / 6F * 4 - 5), 10, 10),
            [PathDots.A] = new Rectangle((int)(128 / 6F * 2 - 5), (int)(128 / 6F * 2 - 5), 10, 10),
            [PathDots.Z] = new Rectangle((int)(128 / 6F * 4 - 5), (int)(128 / 6F * 2 - 5), 10, 10),
            [PathDots.Q] = new Rectangle((int)(128 / 6F * 2 - 5), (int)(128 / 6F * 4 - 5), 10, 10),
            [PathDots.S] = new Rectangle((int)(128 / 6F * 4 - 5), (int)(128 / 6F * 4 - 5), 10, 10),
            [PathDots.CA] = new Rectangle(2, 2, 10, 10),
            [PathDots.CZ] = new Rectangle(128 - 10 - 2, 2, 10, 10),
            [PathDots.CQ] = new Rectangle(2, 128 - 10 - 2, 10, 10),
            [PathDots.CS] = new Rectangle(128 - 10 - 2, 128 - 10 - 2, 10, 10)
        };
        public static Point? PathDotsLoc(PathDots? dot)
        {
            if (dot == null)
                return null;

            return new Point((PathDotsRect[dot.Value].X + PathDotsRect[dot.Value].Width / 2) / 4, (PathDotsRect[dot.Value].Y + PathDotsRect[dot.Value].Height / 2) / 4);
        }
        public static readonly Dictionary<PathDots, (int X, int Y, PathDots D)> PathDotsLinkedCell = new Dictionary<PathDots, (int X, int Y, PathDots D)>()
        {
            [PathDots.TopLeft] = (0, -1, PathDots.BottomLeft),
            [PathDots.TopRight] = (0, -1, PathDots.BottomRight),
            [PathDots.BottomLeft] = (0, 1, PathDots.TopLeft),
            [PathDots.BottomRight] = (0, 1, PathDots.TopRight),
            [PathDots.LeftTop] = (-1, 0, PathDots.RightTop),
            [PathDots.LeftBottom] = (-1, 0, PathDots.RightBottom),
            [PathDots.RightTop] = (1, 0, PathDots.LeftTop),
            [PathDots.RightBottom] = (1, 0, PathDots.LeftBottom),
            [PathDots.A] = (0, 0, PathDots.A),
            [PathDots.Z] = (0, 0, PathDots.Z),
            [PathDots.Q] = (0, 0, PathDots.Q),
            [PathDots.S] = (0, 0, PathDots.S),
            [PathDots.CA] = (0, 0, PathDots.CA),
            [PathDots.CZ] = (0, 0, PathDots.CZ),
            [PathDots.CQ] = (0, 0, PathDots.CQ),
            [PathDots.CS] = (0, 0, PathDots.CS)
        };
    }
}
