using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCardsApp15.utilities
{
    internal class RelativeCoords
    {
        public static int L => Left();
        public static int M => Middle();
        public static int R => Right();
        public static int T => Top();
        public static int C => Center();
        public static int B => Bottom();

        public static int XLeft(int margin = 0) => margin;
        public static int XMiddle() => Middle();
        public static int XRight(int margin = 0) => Right(margin);
        public static int Left(int margin = 0) => XLeft(margin);
        public static int YTop(int margin = 0) => Top(margin);
        public static int YCenter() => Center();
        public static int YBottom(int margin = 0) => Bottom(margin);

        public static int Middle() => Core.W / 2;
        public static int Right(int margin = 0) => Core.W - 1 - margin;
        public static int Top(int margin = 0) => margin;
        public static int Center() => Core.H / 2;
        public static int Bottom(int margin = 0) => Core.H - 1 - margin;
    }
}
