using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public class ManageMouse
    {
        public static Point MousePos;
        public static int AntiCheatPoints;
        public static bool FirstMouseLocSet = true;
        public static int DisplayUIMode = 0;

        public static void Restart()
        {
            AntiCheatPoints = 1000;
            MousePos = new Point(Core.X, Core.Y);
            FirstMouseLocSet = true;
        }

        public static void Update()
        {
            AntiCheatPoints += 10;
            if (AntiCheatPoints > 1000)
                AntiCheatPoints = 1000;
        }

        public static void Draw()
        {
        }
        public static void DrawUI()
        {
            switch (DisplayUIMode)
            {
                default:
                case 0: DrawHorizontalTop(); break;
                case 1: DrawHorizontalBottom(); break;
                case 2: DrawVerticalLeftSmall(); break;
                case 3: DrawVerticalRightSmall(); break;
                case 4: DrawVerticalLeft(); break;
                case 5: DrawVerticalRight(); break;
                case 6: break;
            }
        }
        private static void DrawHorizontalTop()
        {
            int bx = 10, by = 10, bw = Core.W - 20, bh = 20;
            int v = (int)(AntiCheatPoints / 1000F * bw);
            int x = bx;
            int y = by;
            Core.g.FillRectangle(Brushes.Blue, x, y, v, bh);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);


            by = 30;
            y = by;
            v = (int) ((1F - ManageKeyboard.Cooldown / (float)ManageKeyboard.CooldownMax) * bw);
            Core.g.FillRectangle(Brushes.Red, x, y, v, bh);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);
        }
        private static void DrawHorizontalBottom()
        {
            int bx = 10, by = Core.H - 50, bw = Core.W - 20, bh = 20;
            int v = (int)(AntiCheatPoints / 1000F * bw);
            int x = bx;
            int y = by;
            Core.g.FillRectangle(Brushes.Blue, x, y, v, bh);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);


            by = Core.H - 30;
            v = (int) ((1F - ManageKeyboard.Cooldown / (float)ManageKeyboard.CooldownMax) * bw);
            x = bx;
            y = by;
            Core.g.FillRectangle(Brushes.Red, x, y, v, bh);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);
        }
        private static void DrawVerticalLeftSmall()
        {
            int bx = 10, by = 10, bw = 20, bh = 100;
            int v = (int)(AntiCheatPoints / 1000F * bh);
            int x = bx;
            int y = by + bh - v;
            Core.g.FillRectangle(Brushes.Blue, x, y, bw, v);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);


            bx = 30;
            v = (int) ((1F - ManageKeyboard.Cooldown / (float)ManageKeyboard.CooldownMax) * bh);
            x = bx;
            y = by + bh - v;
            Core.g.FillRectangle(Brushes.Red, x, y, bw, v);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);
        }
        private static void DrawVerticalRightSmall()
        {
            int bx = Core.W - 30, by = 10, bw = 20, bh = 100;
            int v = (int) ((1F - ManageKeyboard.Cooldown / (float)ManageKeyboard.CooldownMax) * bh);
            int x = bx;
            int y = by + bh - v;
            Core.g.FillRectangle(Brushes.Red, x, y, bw, v);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);


            bx = Core.W - 50;
            v = (int)(AntiCheatPoints / 1000F * bh);
            x = bx;
            y = by + bh - v;
            Core.g.FillRectangle(Brushes.Blue, x, y, bw, v);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);
        }
        private static void DrawVerticalLeft()
        {
            int bx = 10, by = 10, bw = 20, bh = Core.H - 20;
            int v = (int)(AntiCheatPoints / 1000F * bh);
            int x = bx;
            int y = by + bh - v;
            Core.g.FillRectangle(Brushes.Blue, x, y, bw, v);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);


            bx = 30;
            v = (int)((1F - ManageKeyboard.Cooldown / (float)ManageKeyboard.CooldownMax) * bh);
            x = bx;
            y = by + bh - v;
            Core.g.FillRectangle(Brushes.Red, x, y, bw, v);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);
        }
        private static void DrawVerticalRight()
        {
            int bx = Core.W - 30, by = 10, bw = 20, bh = Core.H - 20;
            int v = (int) ((1F - ManageKeyboard.Cooldown / (float)ManageKeyboard.CooldownMax) * bh);
            int x = bx;
            int y = by + bh - v;
            Core.g.FillRectangle(Brushes.Red, x, y, bw, v);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);


            bx = Core.W - 50;
            v = (int)(AntiCheatPoints / 1000F * bh);
            x = bx;
            y = by + bh - v;
            Core.g.FillRectangle(Brushes.Blue, x, y, bw, v);
            Core.g.DrawRectangle(Pens.White, bx, by, bw, bh);
        }

        public static void MouseDown(MouseEventArgs e)
        {

        }

        public static void MouseUp(MouseEventArgs e)
        {

        }

        public static void MouseLeave(EventArgs e)
        {

        }

        public static void SetMouseCur()
        {
            Cursor.Position = new Point(Core.FormPosition.X + MousePos.X, Core.FormPosition.Y + MousePos.Y);
        }
        public static void MouseMove(MouseEventArgs e)
        {
            var pointsToRemove = (int)Maths.Distance(MousePos.X, MousePos.Y, e.X, e.Y);

            if(pointsToRemove <= 10)
            {
                MousePos = e.Location;
            }
            else
            if (pointsToRemove < AntiCheatPoints)
            {
                MousePos = e.Location;
                AntiCheatPoints -= pointsToRemove;
            }
            else
            {
                if (Core.FormFocused)
                {
                    //Console.WriteLine($"Cursor.Position {Cursor.Position}    new point {new Point(Core.FormPosition.X + MousePos.X, Core.FormPosition.Y + MousePos.Y)}    Core.FormPosition {Core.FormPosition}    MousePos {MousePos}");
                    Cursor.Position = new Point(Core.FormPosition.X + MousePos.X, Core.FormPosition.Y + MousePos.Y);
                }
            }
        }
    }
}
