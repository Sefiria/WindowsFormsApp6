using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp8.Properties;
using static System.Net.Mime.MediaTypeNames;

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
        public static Point TileToShowAllTiles;
        public static Bitmap Layer2;
        public static Graphics g2;

        private static byte GetTile(int _x, int _y) => (_x < 0 || _y < 0 || _x >= Core.WT || _y >= Core.HT) ? (byte)0 : Tiles[_x, _y];
        private static byte[,] GetAround(int x, int y)
        {
            byte Get(int _x, int _y) => GetTile(_x, _y);

            var result = new byte[,]
            {
                { Get(x-1, y-1), Get(x-1, y), Get(x-1, y+1) },
                { Get(x, y-1), 0, Get(x, y+1) },
                { Get(x+1, y-1), Get(x+1, y), Get(x+1, y+1)}
            };
            return result;
        }

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
            Layer2 = new Bitmap(Core.RW, Core.RH);
            g2 = Graphics.FromImage(Layer2);
            RefreshTiles();
        }

        public static void MouseDown(MouseEventArgs e)
        {
            if (Core.ControlKeyHelp)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (!Core.DisplayAllTiles)
                    {
                        var at = Core.ListTiles.SelectedItem as Autotile.Autotile;
                        if (at != null)
                        {
                            Core.TileToManuallySet = Core.MouseTile;
                            TileToShowAllTiles = Core.MousePosition;
                            Core.DisplayAllTiles = true;
                        }
                    }
                }
            }
            else
            {
                if (Core.DisplayAllTiles)
                {
                    var at = Core.ListTiles.SelectedItem as Autotile.Autotile;
                    if (at != null)
                    {
                        int x = (Core.MousePosition.X - TileToShowAllTiles.X) / Core.TileSz;
                        int y = (Core.MousePosition.Y - TileToShowAllTiles.Y) / Core.TileSz;
                        if (!(y < 0 || y > 3 || x < 0 || x > 2))
                        {
                            Tiles[Core.TileToManuallySet.X, Core.TileToManuallySet.Y] = (byte) Core.ListTiles.SelectedIndex;
                            ModifiedTiles.Add(Core.TileToManuallySet);
                            at.SetCurID(y * 3 + x);
                            //g2.DrawImage(at.AllTiles()[y * 3 + x], Core.TileToManuallySet.X * Core.TileSz - Core.Cam.X, Core.TileToManuallySet.Y * Core.TileSz - Core.Cam.Y);
                        }
                    }
                    Core.TileToManuallySet = Point.Empty;
                    Core.DisplayAllTiles = false;
                    //Core.g.DrawImage(Layer2, 0, 0);
                    ResetLayer2();
                    return;
                }
            }

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
        private static void ResetLayer2()
        {
            Layer2 = new Bitmap(Layer2.Width, Layer2.Height);
            g2 = Graphics.FromImage(Layer2);
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

            var ms = Core.MouseTile;
            if (ms.X < 0 || ms.Y < 0 || ms.X >= Core.WT || ms.Y >= Core.HT)
                return;

            if (!Core.IsMouseDown || Core.ControlKeyHelp)
                return;

            if (Core.IsRightMouseDown && Tiles[ms.X, ms.Y] < Core.ListTiles.Items.Count)
            {
                Core.ListTiles.SelectedIndex = Tiles[ms.X, ms.Y];
                return;
            }

            if (Core.ListTiles.SelectedIndex == -1)
                return;

            if (Tool == Tools.Pen)
            {
                Tiles[ms.X, ms.Y] = (byte)Core.ListTiles.SelectedIndex;
                var at = Core.ListTilesAutotile(ms.X, ms.Y);
                if (at != null)
                    at.Calculate(Core.ListTiles.Items.Cast<Autotile.Tile>().ToList(), GetAround(ms.X, ms.Y));
                ModifiedTiles.Add(ms);
            }
        }

        public static void Update()
        {
            if (Core.Palette == null)
                return;

            foreach (var obj in Core.ListTiles.Items)
            {
                Tile tile = obj as Tile;
                if (tile == null)
                    continue;
                tile.Increase();
                tile.ModifiedPixels = tile.GetAllPixelsPoints();
                tile.SetImage();
            }
        }

        public static void Draw()
        {
            if (Core.Palette == null || Core.ListTiles.Items.Count == 0) return;
            Bitmap img;
            for (int x = 0; x < Core.WT; x++)
            {
                for (int y = 0; y < Core.HT; y++)
                {
                    if (x < Core.CamTile.X - 1 || y < Core.CamTile.Y - 1 || x >= Core.CamTile.X + Core.CamTile.Width + 1 || y >= Core.CamTile.Y + Core.CamTile.Height + 1)
                        continue;
                    if (Tiles[x, y] < Core.ListTiles.Items.Count)
                    {
                        img = Core.ListTilesTile(x, y)?.Image;
                        if (img != null)
                        {
                            Core.g.DrawImage(img, x * Core.TileSz - Core.Cam.X, y * Core.TileSz - Core.Cam.Y);
                        }
                        else
                        {
                            var at = Core.ListTilesAutotile(x, y);
                            if(at != null)
                                Core.g.DrawImage(at.Current, x * Core.TileSz - Core.Cam.X, y * Core.TileSz - Core.Cam.Y);
                        }
                    }
                }
            }

            DrawAnimatedSelection();

            DisplayAutotileAllTiles();
        }
        private static void DrawAnimatedSelection()
        {
            var ms = Core.MouseSnap;

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

            //Core.gui.DrawRectangle(Pens.Red, tms.X - 1, tms.Y - 1, Core.TileSz, Core.TileSz);
        }
        private static void DisplayAutotileAllTiles()
        {
            if (Core.DisplayAllTiles)
            {
                var at = Core.ListTiles.SelectedItem as Autotile.Autotile;
                if (at != null)
                {
                    int x = TileToShowAllTiles.X, y = TileToShowAllTiles.Y;
                    ResetLayer2();
                    var tiles = at.AllTiles();
                    g2.FillRectangle(Brushes.Black, x, y, 3 * Core.TileSz - 1, 3 * Core.TileSz - 1);
                    g2.DrawRectangle(Pens.White, x, y, 3 * Core.TileSz + 1, 3 * Core.TileSz + 1);
                    foreach (Bitmap tile in tiles)
                    {
                        g2.DrawImage(tile, x, y);
                        x += Core.TileSz;
                        if (x > TileToShowAllTiles.X + 2 * Core.TileSz)
                        {
                            x = TileToShowAllTiles.X;
                            y += Core.TileSz;
                        }
                    }
                }
            }
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
            if (lines[5].Length != wt * ht * 2) return;
            Core.WT = wt;
            Core.HT = ht;
            Core.TileSz = tsz;
            ImportPalette(fnpal);
            ClearTileList();
            fntiles = new List<string> { "grass.til", "dirtygrass.til", "dirtygrass.til", "dirtabitofgrass.til", "dirt.til", "auto_ground_pavet.png" };
            foreach (string fntile in fntiles)
            {
                if (fntile.StartsWith("auto"))
                    ImportAutotile(fntile);
                else
                    ImportTile(fntile);
            }
            Core.TileSz = tsz;
            Tiles = new byte[wt, ht];
            byte px;
            Autotile.Autotile at;
            for (int x = 0; x < wt; x++)
            {
                for (int y = 0; y < ht; y++)
                {
                    px = (byte)int.Parse("" + lines[5][x * 2 * ht + y * 2]);
                    Tiles[x, y] = px;
                    at = Core.ListTiles.Items[px] as Autotile.Autotile;
                    if (at != null)
                        at.SetCurID(int.Parse("" + lines[5][x * 2 * ht + y * 2 + 1]));
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
            foreach (dynamic tile in Core.ListTiles.Items)
            {
                fntiles += tile.FileName + ",";
            }
            content += fntiles.Remove(fntiles.Length - 1) + Environment.NewLine;
            for (int x = 0; x < Core.WT; x++)
            {
                for (int y = 0; y < Core.HT; y++)
                {
                    content += Tiles[x, y];
                    content += 0;
                }
            }
            File.WriteAllText(dial.FileName, content.Zip());
        }
        
        public static void ImportAutotile()
        {
            var dial = new OpenFileDialog();
            dial.Filter = "PNG Files | auto*.png|Bitmap Files | *.bmp|JPEG Files | *.jpg";
            if (dial.ShowDialog() == DialogResult.OK)
                ImportAutotile(dial.FileName);
        }
        public static void ImportAutotile(string fnAutotile)
        {
            var autotile = GetImportedAutotile(fnAutotile);
            if (autotile != null)
                Core.ListTiles.Items.Add(autotile);
        }
        public static Autotile.Autotile GetImportedAutotile()
        {
            Autotile.Autotile autotile = null;
            var dial = new OpenFileDialog();
            dial.Filter = "PNG Files | auto*.png|Bitmap Files | *.bmp|JPEG Files | *.jpg";
            if (dial.ShowDialog() == DialogResult.OK)
            {
                autotile = GetImportedAutotile(dial.FileName);
                autotile.FileName = Path.GetFileName(dial.FileName);
            }
            return autotile;
        }
        public static Autotile.Autotile GetImportedAutotile(string fnAutotile)
        {
            var at =  new Autotile.Autotile((Bitmap)Bitmap.FromFile(fnAutotile), 0);
            at.FileName = Path.GetFileName(fnAutotile);
            return at;
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
                    px = (byte)(lines[4][x * (rh / tsz) + y] - '0');
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
            foreach (var obj in Core.ListTiles.Items)
            {
                var tile = obj as Tile;
                if (tile != null)
                    tile.ModifiedPixels = tile.GetAllPixelsPoints(true);
            }
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
