using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class World
    {
        public int W, H;
        public Map[,] Maps;
        private Point CurrentCoord, StartCoord, EndCoord;
        private Bitmap MinimapImage;
        private int MinimapW = 128, MinimapH = 128;

        public Map Current => Maps[CurrentCoord.X, CurrentCoord.Y];
        public Map Left => CurrentCoord.X - 1 >= 0 ? Maps[CurrentCoord.X - 1, CurrentCoord.Y] : null;
        public Map Right => CurrentCoord.X + 1 < W ? Maps[CurrentCoord.X + 1, CurrentCoord.Y] : null;
        public Map Top => CurrentCoord.Y - 1 >= 0 ? Maps[CurrentCoord.X, CurrentCoord.Y - 1] : null;
        public Map Bottom => CurrentCoord.Y + 1 < W ? Maps[CurrentCoord.X, CurrentCoord.Y + 1] : null;
        public void GoLeft() { if (Left != null) { DrawMinimapRectCurrent(Brushes.Gray); CurrentCoord.X--; SetPlayerCoordOnDoorRight(); } }
        public void GoRight() { if (Right != null) { DrawMinimapRectCurrent(Brushes.Gray); CurrentCoord.X++; SetPlayerCoordOnDoorLeft(); } }
        public void GoTop() { if (Top != null) { DrawMinimapRectCurrent(Brushes.Gray); CurrentCoord.Y--; SetPlayerCoordOnDoorBottom(); } }
        public void GoBottom() { if (Bottom != null) { DrawMinimapRectCurrent(Brushes.Gray); CurrentCoord.Y++; SetPlayerCoordOnDoorTop(); } }


        private Map LeftOf(int x, int y) => x - 1 >= 0 ? Maps[x - 1, y] : null;
        private Map RightOf(int x, int y) => x + 1 < W ? Maps[x + 1, y] : null;
        private Map TopOf(int x, int y) => y - 1 >= 0 ? Maps[x, y - 1] : null;
        private Map BottomOf(int x, int y) => y + 1 < W ? Maps[x, y + 1] : null;
        private void SetPlayerCoordOnDoorLeft() => SharedData.Player.TCoords = new Point(Current.DoorLeft.X + 1, Current.DoorLeft.Y);
        private void SetPlayerCoordOnDoorRight() => SharedData.Player.TCoords = new Point(Current.DoorRight.X - 1, Current.DoorRight.Y);
        private void SetPlayerCoordOnDoorTop() => SharedData.Player.TCoords = new Point(Current.DoorTop.X, Current.DoorTop.Y + 1);
        private void SetPlayerCoordOnDoorBottom() => SharedData.Player.TCoords = new Point(Current.DoorBottom.X, Current.DoorBottom.Y - 1);

        public World(int w, int h)
        {
            W = w;
            H = h;
            Maps = new Map[W, H];
            List<(int x, int y, Map m)> list;
            do { list = Gen(); } while (list.Count == 0);
            foreach (var i in list)
            {
                Maps[i.x, i.y] = i.m;
                Maps[i.x, i.y].WorldCoord = new Point(i.x, i.y);
            }
            SetDoors();
            var rndmap = list[list.Count / 2];
            CurrentCoord = new Point(rndmap.x, rndmap.y);
            SetStartEndCoords();
            CreateMinimap();
        }

        private void SetStartEndCoords()
        {
            StartCoord = CurrentCoord;
            Point end;
            List<Point> treated = new List<Point>();
            List<Point> linked = new List<Point>() { CurrentCoord };

            List<Point> GetLinked(Point _pt)
            {
                List<Point> result = new List<Point>();
                if (LeftOf(_pt.X, _pt.Y) != null) result.Add(LeftOf(_pt.X, _pt.Y).WorldCoord);
                if (RightOf(_pt.X, _pt.Y) != null) result.Add(RightOf(_pt.X, _pt.Y).WorldCoord);
                if (TopOf(_pt.X, _pt.Y) != null) result.Add(TopOf(_pt.X, _pt.Y).WorldCoord);
                if (BottomOf(_pt.X, _pt.Y) != null) result.Add(BottomOf(_pt.X, _pt.Y).WorldCoord);
                return result;
            }

            while (linked.Where(x => !treated.Contains(x)).ToList().Count > 0)
            {
                var list = linked.Where(x => !treated.Contains(x)).ToList();
                foreach (var pt in list)
                {
                    treated.Add(pt);
                    var inputs = GetLinked(pt);
                    foreach (var input in inputs)
                        if(!linked.Contains(input))
                            linked.Add(input);
                }
            }

            EndCoord = treated[Tools.RND.Next(0, treated.Count)];
        }

        private void SetDoors()
        {
            for (int x = 0; x < W; x++)
            {
                for (int y = 0; y < H; y++)
                {
                    if (Maps[x, y] == null)
                        continue;

                    if (LeftOf(x, y) != null)
                    {
                        var _x = Maps[x, y].Boxes[0].X - 1;
                        var _y = Tools.RND.Next(Maps[x, y].Boxes[0].Y, Maps[x, y].Boxes[0].Y + Maps[x, y].Boxes[0].Height);
                        Maps[x, y].Tiles[_x, _y - 1] = 3;
                        Maps[x, y].Tiles[_x, _y] = 1;
                        Maps[x, y].DoorLeft = new Point(_x, _y);
                    }

                    if (RightOf(x, y) != null)
                    {
                        var _x = Maps[x, y].Boxes[0].X + Maps[x, y].Boxes[0].Width;
                        var _y = Tools.RND.Next(Maps[x, y].Boxes[0].Y, Maps[x, y].Boxes[0].Y + Maps[x, y].Boxes[0].Height);
                        Maps[x, y].Tiles[_x, _y - 1] = 3;
                        Maps[x, y].Tiles[_x, _y] = 1;
                        Maps[x, y].DoorRight = new Point(_x, _y);
                    }

                    if (TopOf(x, y) != null)
                    {
                        var _x = Tools.RND.Next(Maps[x, y].Boxes[0].X, Maps[x, y].Boxes[0].X + Maps[x, y].Boxes[0].Width);
                        var _y = Maps[x, y].Boxes[0].Y - 2;
                        Maps[x, y].Tiles[_x, _y] = 0;
                        Maps[x, y].Tiles[_x, _y + 1] = 1;
                        Maps[x, y].DoorTop = new Point(_x, _y + 1);
                    }

                    if (BottomOf(x, y) != null)
                    {
                        var _x = Tools.RND.Next(Maps[x, y].Boxes[0].X, Maps[x, y].Boxes[0].X + Maps[x, y].Boxes[0].Width);
                        var _y = Maps[x, y].Boxes[0].Y + Maps[x, y].Boxes[0].Height + 1;
                        Maps[x, y].Tiles[_x, _y - 1] = 1;
                        Maps[x, y].Tiles[_x, _y] = 1;
                        Maps[x, y].DoorBottom = new Point(_x, _y);
                    }
                }
            }
        }

        private List<(int x, int y, Map m)> Gen()
        {
            List<(int x, int y, Map m)> list = new List<(int x, int y, Map m)>();

            int t;
            for (int x = 0; x < W; x++)
            {
                for (int y = 0; y < H; y++)
                {
                    t = Tools.RND.Next(100);
                    if (t < 65)
                        list.Add((x, y, new Map().Generate1()));
                }
            }

            return list;
        }

        public void CreateMinimap()
        {
            MinimapImage = new Bitmap(MinimapW, MinimapH);
            Graphics g = Graphics.FromImage(MinimapImage);
            g.Clear(Color.Black);
            for (int x = 0; x < W; x++)
            {
                for (int y = 0; y < H; y++)
                {
                    if (Maps[x, y] != null)
                        DrawMinimapRect(g, Color.Gray.SetAlpha(100), x, y);
                }
            }
            g.Dispose();
        }
        public void DrawMinimap()
        {
            SharedCore.g.DrawImage(MinimapImage, SharedCore.RenderW - MinimapW - 8, 8);
            DrawMinimapRect(Color.Red.SetAlpha(50), StartCoord.X, StartCoord.Y);
            DrawMinimapRect(Color.Lime.SetAlpha(50), EndCoord.X, EndCoord.Y);
            DrawMinimapRectCurrent(Color.Orange.SetAlpha(50));
        }
        private void DrawMinimapRect(Graphics g, Brush color, int x, int y)
        {
            g.FillRectangle(color,
                            (int)((x / (float)W) * MinimapW),
                            (int)((y / (float)H) * MinimapH),
                            MinimapW / W,
                            MinimapH / H);
        }
        private void DrawMinimapRect(Brush color, int x, int y)
        {
            using (Graphics g = Graphics.FromImage(MinimapImage))
                DrawMinimapRect(g, color, x, y);
        }
        private void DrawMinimapRectCurrent(Brush color) => DrawMinimapRect(color, CurrentCoord.X, CurrentCoord.Y);
    }
}
