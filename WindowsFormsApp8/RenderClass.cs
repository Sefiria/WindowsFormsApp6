using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp8.Properties;

namespace WindowsFormsApp8
{
    public class RenderClass
    {
        public enum Tools
        {
            Pen,
            Bucket,
        }

        public static byte[,] Tiles = new byte[Core.WT, Core.HT];
        public static List<Point> ModifiedTiles = new List<Point>();
        public static int AnimSelTmr = 0, AnimSelId = 0;
        public static Point MousePositionAtMiddleFirstClick;
        public static Tools Tool = Tools.Pen;

        public static List<Point> GetAllTilesPoints()
        {
            List<Point> result = new List<Point>();
            for (int x = 0; x < Core.WT; x++)
            {
                for (int y = 0; y < Core.HT; y++)
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
            if (Tool == Tools.Bucket && !Core.IsRightMouseDown && !Core.IsMiddleMouseDown)
            {
                Point ms = new Point(Core.MouseTile.X + Core.CamTile.X, Core.MouseTile.Y + Core.CamTile.Y);
                FloodFillTiles(ms, (byte)Core.ListTiles.SelectedIndex);
            }
            else
            {
                MouseMove();
            }
        }
        public static void MouseMove()
        {
            if (Core.IsMiddleMouseDown)
            {
                Core.Cam.X += Core.MousePosition.X - MousePositionAtMiddleFirstClick.X;
                Core.Cam.Y += Core.MousePosition.Y - MousePositionAtMiddleFirstClick.Y;
                ClearRenderImage();
                return;
            }

            Point ms = new Point(Core.MouseTile.X + Core.CamTile.X, Core.MouseTile.Y + Core.CamTile.Y);
            if (ms.X < 0 || ms.Y < 0 || ms.X >= Core.WT || ms.Y >= Core.HT)
                return;

            if (!Core.IsMouseDown)
                return;

            if (Core.IsRightMouseDown && Tiles[Core.MouseTile.X, Core.MouseTile.Y] < Core.ListTiles.Items.Count)
            {
                Core.ListTiles.SelectedIndex = Tiles[Core.MouseTile.X, Core.MouseTile.Y];
                return;
            }

            if (Core.ListTiles.SelectedIndex == -1)
                return;

            if (Tool == Tools.Pen)
            {
                Tiles[Core.MouseTile.X + Core.CamTile.X, Core.MouseTile.Y + Core.CamTile.Y] = (byte)Core.ListTiles.SelectedIndex;
                ModifiedTiles.Add(Core.MouseTile);
            }
        }

        public static void Update()
        {
            if (Core.Palette == null)
                return;

            foreach (Tile tile in Core.ListTiles.Items)
            {
                tile.Increase();
                tile.ModifiedPixels = tile.GetAllPixelsPoints();
                tile.SetImage();
            }
        }

        public static void Draw()
        {
            if (Core.Palette == null || Core.ListTiles.Items.Count == 0) return;
            for (int x = 0; x < Core.WT; x++)
            {
                for (int y = 0; y < Core.HT; y++)
                {
                    if (x < Core.CamTile.X - 1 || y < Core.CamTile.Y - 1 || x >= Core.CamTile.X + Core.CamTile.Width + 1 || y >= Core.CamTile.Y + Core.CamTile.Height + 1)
                        continue;
                    if (Tiles[x, y] < Core.ListTiles.Items.Count)
                        Core.g.DrawImage((Core.ListTiles.Items[Tiles[x, y]] as Tile).Image, x * Core.TileSz - Core.Cam.X, y * Core.TileSz - Core.Cam.Y);
                }
            }

            DrawAnimatedSelection();
        }
        private static void DrawAnimatedSelection()
        {
            Point ms = new Point(Core.MouseSnap.X - (int)Core.Cam.X % Core.TileSz, Core.MouseSnap.Y - (int)Core.Cam.Y % Core.TileSz);
            if (ms.X + Core.Cam.X < 0 || ms.Y + Core.Cam.Y < 0 || ms.X + Core.Cam.X >= Core.WT * Core.TileSz || ms.Y + Core.Cam.Y >= Core.HT * Core.TileSz)
                return;
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

        public static void LoadMap()
        {
            var dial = new OpenFileDialog();
            dial.Filter = "Map files | *.map";
            if (dial.ShowDialog() != DialogResult.OK) return;
            string content = File.ReadAllText(dial.FileName).UnZip();
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length != 6) return;
            if (!int.TryParse(lines[0], out int wt)) return;
            if (!int.TryParse(lines[1], out int ht)) return;
            if (!int.TryParse(lines[2], out int tsz)) return;
            string fnpal = lines[3];
            List<string> fntiles = lines[4].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (lines[5].Length != wt * ht) return;
            Core.WT = wt;
            Core.HT = ht;
            Core.TileSz = tsz;
            ImportPalette(fnpal);
            ClearTileList();
            foreach (string fntile in fntiles)
                ImportTile(fntile);
            Core.TileSz = tsz;
            Tiles = new byte[wt, ht];
            byte px;
            for (int x = 0; x < wt; x++)
            {
                for (int y = 0; y < ht; y++)
                {
                    px = (byte)int.Parse("" + lines[5][x * ht + y]);
                    Tiles[x, y] = px;
                    ModifiedTiles.Add(new Point(x, y));
                }
            }
            Core.g.Clear(Color.Black);
        }
        public static void SaveMap()
        {
            if (Core.Palette == null)
            {
                MessageBox.Show("No palette set in the map !");
                return;
            }
            var dial = new SaveFileDialog();
            dial.Filter = "Map files | *.map";
            if (dial.ShowDialog() != DialogResult.OK) return;
            string content = "";
            content += Core.WT + Environment.NewLine;
            content += Core.HT + Environment.NewLine;
            content += Core.TileSz + Environment.NewLine;
            content += Core.Palette.FileName + Environment.NewLine;
            string fntiles = "";
            foreach (Tile tile in Core.ListTiles.Items)
                fntiles += tile.FileName + ",";
            content += fntiles.Remove(fntiles.Length - 1) + Environment.NewLine;
            for (int x = 0; x < Core.WT; x++)
            {
                for (int y = 0; y < Core.HT; y++)
                {
                    content += Tiles[x, y];
                }
            }
            File.WriteAllText(dial.FileName, content.Zip());
        }

        public static Tile GetImportedTile()
        {
            Tile tile = null;
            var dial = new OpenFileDialog();
            dial.Filter = "TIL files | *.til";
            if (dial.ShowDialog() == DialogResult.OK)
                tile = GetImportedTile(dial.FileName);
            return tile;
        }
        public static Tile GetImportedTile(string fnTile)
        {
            if (!File.Exists(fnTile))
            {
                MessageBox.Show($"[Tile] File Not Found :{Environment.NewLine}{fnTile}");
                ImportTile();
                return null;
            }
            string content = File.ReadAllText(fnTile).UnZip();
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length != 5) return null;
            if (!int.TryParse(lines[0], out int rw)) return null;
            if (!int.TryParse(lines[1], out int rh)) return null;
            if (!int.TryParse(lines[2], out int tsz)) return null;
            if (!int.TryParse(lines[3], out int iten)) return null;
            if (lines[4].Length != (rw / tsz) * (rh / tsz)) return null;
            Tile tile = new Tile(fnTile, Path.GetFileNameWithoutExtension(fnTile), rw, rh, iten);
            tile.Pixels = new byte[rw / tsz, rh / tsz];
            byte px;
            for (int x = 0; x < rw / tsz; x++)
            {
                for (int y = 0; y < rh / tsz; y++)
                {
                    px = (byte)int.Parse("" + lines[4][x * (rh / tsz) + y]);
                    tile.Pixels[x, y] = px;
                    tile.ModifiedPixels.Add(new Point(x, y));
                }
            }
            return tile;
        }
        public static void ImportTile()
        {
            var dial = new OpenFileDialog();
            dial.Filter = "TIL files | *.til";
            if (dial.ShowDialog() == DialogResult.OK)
                ImportTile(dial.FileName);
        }
        public static void ImportTile(string fnTile)
        {
            var tile = GetImportedTile(fnTile);
            if (tile != null)
                Core.ListTiles.Items.Add(tile);
        }
        public static void ImportPalette()
        {
            var dial = new OpenFileDialog();
            dial.Filter = "PAL files | *.pal";
            if (dial.ShowDialog() == DialogResult.OK)
                ImportPalette(dial.FileName);
        }
        public static void ImportPalette(string fnPal)
        {
            if (!File.Exists(fnPal))
            {
                MessageBox.Show($"[Palette] File Not Found :{Environment.NewLine}{fnPal}");
                ImportPalette();
                return;
            }
            string content = File.ReadAllText(fnPal).UnZip();
            string[] lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Core.Palette = new Palette(fnPal);
            Core.Palette.TickValue = int.Parse(lines[0]);
            lines = lines.Skip(1).ToArray();
            Pixel p;
            List<Color> colors;
            Core.Palette.Pixels.Clear();
            string line;
            foreach (string l in lines)
            {
                line = l;
                p = new Pixel();
                p.Gradient.Clear();
                p.IsLerp = int.Parse("" + line[0]) == 1;
                line = line.Remove(0, 1);
                colors = line.Split(',').Select(x => Color.FromArgb(int.Parse(x))).ToList();
                foreach (var c in colors)
                    p.Gradient.Add(c);
                Core.Palette.Pixels.Add(p);
            }
            foreach (Tile tile in Core.ListTiles.Items) tile.ModifiedPixels = tile.GetAllPixelsPoints();
        }

        public static void ClearTileList()
        {
            Core.ListTiles.Items.Clear();
            ClearRenderImage();
        }
        public static void ClearRenderImage()
        {
            Core.Image = new Bitmap(Core.Image.Width, Core.Image.Height);
            Core.g.Dispose();
            Core.g = Graphics.FromImage(Core.Image);
        }

        public static void ChangeTilesArraySize()
        {
            byte[,] tiles = new byte[Tiles.GetLength(0), Tiles.GetLength(1)];
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    tiles[x, y] = Tiles[x, y];
                }
            }
            Tiles = new byte[Core.WT, Core.HT];
            for (int x = 0; x < tiles.GetLength(0) && x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1) && y < Tiles.GetLength(1); y++)
                {
                    Tiles[x, y] = tiles[x, y];
                }
            }
            ClearRenderImage();
        }


        public static void FloodFillTiles(Point pt, byte replacementPx)
        {
            byte targetPx = Tiles[pt.X, pt.Y];
            if (targetPx == replacementPx)
                return;
            Queue<Point> q = new Queue<Point>();
            q.Enqueue(pt);
            while (q.Count > 0)
            {
                Point n = q.Dequeue();
                if (Tiles[n.X, n.Y] != targetPx)
                    continue;
                Point w = n, e = new Point(n.X + 1, n.Y);
                while ((w.X >= 0) && Tiles[w.X, w.Y] == targetPx)
                {
                    Tiles[w.X, w.Y] = replacementPx;
                    ModifiedTiles.Add(w);
                    if ((w.Y > 0) && Tiles[w.X, w.Y - 1] == targetPx)
                        q.Enqueue(new Point(w.X, w.Y - 1));
                    if ((w.Y < Core.HT - 1) && Tiles[w.X, w.Y + 1] == targetPx)
                        q.Enqueue(new Point(w.X, w.Y + 1));
                    w.X--;
                }
                while ((e.X <= Core.WT - 1) && Tiles[e.X, e.Y] == targetPx)
                {
                    Tiles[e.X, e.Y] = replacementPx;
                    ModifiedTiles.Add(e);
                    if ((e.Y > 0) && Tiles[e.X, e.Y - 1] == targetPx)
                        q.Enqueue(new Point(e.X, e.Y - 1));
                    if ((e.Y < Core.HT - 1) && Tiles[e.X, e.Y + 1] == targetPx)
                        q.Enqueue(new Point(e.X, e.Y + 1));
                    e.X++;
                }
            }
        }
    }
}
