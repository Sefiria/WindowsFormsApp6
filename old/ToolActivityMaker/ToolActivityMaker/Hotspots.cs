using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolActivityMaker
{
    public static class Hotspots
    {
        public static int BeginY => 101;
        public static int OffsetY => 124;

        public static class HSDay
        {
            public static int OffsetX => 6;
            public static Rectangle Bounds => new Rectangle(OffsetX, 11, 1243, 101);
            public static Point Day1 => new Point(OffsetX, BeginY);
        }
        public static class HSTwoDays
        {
            public static int OffsetX => 6;
            public static Rectangle GetBounds(int day)
            {
                return day == 1 ? new Rectangle(6, 11, 1243, 101) : new Rectangle(6, 135, 1243, 225);
            }
            public static Point Day1 => new Point(OffsetX, BeginY);
            public static Point Day2 => new Point(OffsetX, Day1.Y + OffsetY);
        }
        public static class HSWeek
        {
            public static int OffsetX => 26;
            public static Rectangle GetBounds(int day) => new Rectangle(OffsetX, GetYFromDay(day), 1243 - OffsetX, OffsetY);
            public static Point Day1 => new Point(OffsetX, BeginY);
            public static Point Day2 => new Point(OffsetX, Day1.Y + OffsetY);
            public static Point Day3 => new Point(OffsetX, Day2.Y + OffsetY);
            public static Point Day4 => new Point(OffsetX, Day3.Y + OffsetY);
            public static Point Day5 => new Point(OffsetX, Day4.Y + OffsetY);
            public static Point Day6 => new Point(OffsetX, Day5.Y + OffsetY);
            public static Point Day7 => new Point(OffsetX, Day6.Y + OffsetY);

            public static Point Day(int day) => new Point(OffsetX, BeginY + OffsetY * (day - 1));
        }
        public static class HSCustom
        {
            public static int BeginY => 92;
            public static int OffsetX => 6;
            public static Rectangle GetBounds(int day) => new Rectangle(OffsetX, GetYFromDay(day), 1243 - OffsetX, OffsetY);
            public static Point Day(int day) => new Point(OffsetX, BeginY + OffsetY * (day - 1));
        }

        public static int GetYFromDay(int day)
        {
            if (day < 1) day = 1;
            return (MainForm.Instance.TType == MainForm.TemplateType.Custom ? HSCustom.BeginY : BeginY) + OffsetY * (day - 1);
        }
        public static int GetDayFromPixels(int pixelsY)
        {
            return pixelsY / OffsetY + 1;
        }
    }
}
