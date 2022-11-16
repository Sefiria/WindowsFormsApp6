using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp8.Properties;

namespace WindowsFormsApp8
{
    public class RenderClass
    {
        public static byte[,] Tiles = new byte[Core.WT, Core.HT];
        public static List<Point> ModifiedTiles = new List<Point>();
        public static int AnimSelTmr = 0, AnimSelId = 0;

        public static List<Point> GetAllTilesPoints()
        {
            List<Point> result = new List<Point>();
            for(int x=0; x<Core.WT; x++)
            {
                for(int y=0; y<Core.HT; y++)
                {
                    result.Add(new Point(x, y));
                }
            }
            return result;
        }
        public static void RefreshTiles()
        {
            ModifiedTiles = GetAllTilesPoints();
        }


        public static void Initialize()
        {
            RefreshTiles();
        }

        public static void MouseDown()
        {
        }
        public static void MouseMove()
        {
            if (!Core.IsMouseDown) return;

        }

        public static void Update()
        {
        }

        public static void Draw()
        {
            var list = new List<Point>(ModifiedTiles);
            foreach (Point pt in list)
            {
                Core.g.DrawImage(Resources.grass, pt.X * Core.TileSz, pt.Y * Core.TileSz);
                ModifiedTiles.Remove(pt);
            }

            DrawAnimatedSelection();
        }
        private static void DrawAnimatedSelection()
        {
            Point ms = Core.MouseSnap;
            if (AnimSelTmr == 2)
            {
                AnimSelId++;
                if (AnimSelId == 8)
                    AnimSelId = 0;
                AnimSelTmr = 0;
            }
            else AnimSelTmr++;

            Pen pa = new Pen(Color.FromArgb(200, 200, 255));
            Pen pb = new Pen(Color.FromArgb(170, 170, 255));
            Pen pc = new Pen(Color.FromArgb(140, 140, 255));
            Core.gui.DrawLine(AnimSelId == 0 ? pa : (new[] { 7, 1 }.Contains(AnimSelId) ? pb : pc), ms.X - 1, ms.Y - 1, ms.X + Core.TileSz / 2, ms.Y - 1);
            Core.gui.DrawLine(AnimSelId == 1 ? pa : (new[] { 0, 2 }.Contains(AnimSelId) ? pb : pc), ms.X + Core.TileSz / 2, ms.Y - 1, ms.X + Core.TileSz, ms.Y - 1);
            Core.gui.DrawLine(AnimSelId == 2 ? pa : (new[] { 1, 3 }.Contains(AnimSelId) ? pb : pc), ms.X + Core.TileSz, ms.Y - 1, ms.X + Core.TileSz, ms.Y + Core.TileSz / 2);
            Core.gui.DrawLine(AnimSelId == 3 ? pa : (new[] { 2, 4 }.Contains(AnimSelId) ? pb : pc), ms.X + Core.TileSz, ms.Y + Core.TileSz / 2, ms.X + Core.TileSz, ms.Y + Core.TileSz);
            Core.gui.DrawLine(AnimSelId == 4 ? pa : (new[] { 3, 5 }.Contains(AnimSelId) ? pb : pc), ms.X + Core.TileSz, ms.Y + Core.TileSz, ms.X + Core.TileSz / 2, ms.Y + Core.TileSz);
            Core.gui.DrawLine(AnimSelId == 5 ? pa : (new[] { 4, 6 }.Contains(AnimSelId) ? pb : pc), ms.X + Core.TileSz / 2, ms.Y + Core.TileSz, ms.X - 1, ms.Y + Core.TileSz);
            Core.gui.DrawLine(AnimSelId == 6 ? pa : (new[] { 5, 7 }.Contains(AnimSelId) ? pb : pc), ms.X - 1, ms.Y + Core.TileSz, ms.X - 1, ms.Y + Core.TileSz / 2);
            Core.gui.DrawLine(AnimSelId == 7 ? pa : (new[] { 6, 0 }.Contains(AnimSelId) ? pb : pc), ms.X - 1, ms.Y + Core.TileSz / 2, ms.X - 1, ms.Y - 1);
        }
    }
}
