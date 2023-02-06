using Autotile.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autotile
{
    public partial class Form1 : Form
    {
        bool MouseHeld = false;
        Bitmap Layer1, Layer2;
        Graphics g1, g2;
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        byte[,] PaletteIndexes;
        byte CurrentPaletteIndex = 0;
        Palette Palette = new Palette();
        bool ControlKeyHelp = false;
        Point TileToManuallySet = Point.Empty;
        Point TileToShowAllTiles => new Point(TileToManuallySet.X + (TileToManuallySet.X + 3 > Render.Width / Data.TileSize ?  - 3 : (TileToManuallySet.X - 2 < 0 ? 1 : -2)), TileToManuallySet.Y + (TileToManuallySet.Y - 5 > 1 ? -5 : 3));
        bool DisplayAllTiles = false;
        Point MouseLocation;
        Color DefaultBGColor = Color.Black;

        public Form1()
        {
            InitializeComponent();

            LoadPalette();
            PaletteIndexes = new byte[Render.Width / Data.TileSize + 1, Render.Height / Data.TileSize + 1];

            Layer1 = new Bitmap(Render.Width, Render.Height);
            g1 = Graphics.FromImage(Layer1);
            g1.Clear(DefaultBGColor);
            Layer2 = new Bitmap(Render.Width, Render.Height);
            g2 = Graphics.FromImage(Layer2);

            TimerDraw.Tick += Draw;
        }

        private void LoadPalette()
        {
            Palette.Tiles.Add(new Tile(Resources.black));
            Palette.Tiles.Add(new Autotile(Resources.auto_ground_pavet, 0));
            Palette.Tiles.Add(new Autotile(Resources.auto_ground_stone, 0));
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            ControlKeyHelp = e.Control;

            var list = new List<Keys>() { Keys.D1, Keys.D2, Keys.D3 };
            if (list.Contains(e.KeyCode))
                CurrentPaletteIndex = (byte)list.IndexOf(e.KeyCode);
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            ControlKeyHelp = false;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if (ControlKeyHelp)
            {
                if (DisplayAllTiles)
                {
                    TileToManuallySet = Point.Empty;
                    DisplayAllTiles = false;
                }
                else
                {
                    var at = Palette.Tiles[CurrentPaletteIndex] as Autotile;
                    if (at != null)
                    {
                        var x = e.X / Data.TileSize;
                        var y = e.Y / Data.TileSize;
                        TileToManuallySet = new Point(x, y);
                        DisplayAllTiles = true;
                    }
                }
            }
            else
            {
                if (DisplayAllTiles)
                {
                    var at = Palette.Tiles[CurrentPaletteIndex] as Autotile;
                    if (at != null)
                    {
                        int x = e.X / Data.TileSize - TileToShowAllTiles.X;
                        int y = e.Y / Data.TileSize - TileToShowAllTiles.Y;
                        var tiles = at.AllTiles();
                        if (!(y < 0 || y > 3 || x < 0 || x > 2))
                            g1.DrawImage(tiles[y * 3 + x], TileToManuallySet.X * Data.TileSize, TileToManuallySet.Y * Data.TileSize);
                    }
                    TileToManuallySet = Point.Empty;
                    DisplayAllTiles = false;
                    ResetLayer2();
                }
                else
                {
                    MouseHeld = true;
                    Render_MouseMove(true, e);
                }
            }
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            MouseHeld = false;
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            MouseHeld = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            MouseLocation = e.Location;

            if (!MouseHeld) return;

            var x = e.X / Data.TileSize;
            var y = e.Y / Data.TileSize;

            if (ControlKeyHelp)
            {
                //if (PaletteIndexes[x, y] != 0)
                //{
                //    var at = Palette.Tiles[PaletteIndexes[x, y]] as Autotile;
                //    if (at != null)
                //        g.DrawImage(at.GetNext(), x * Data.TileSize, y * Data.TileSize);
                //}
            }
            else
            {
                 if(e.Button == MouseButtons.Right)
                {
                    CurrentPaletteIndex = PaletteIndexes[x, y];
                    return;
                }

                PaletteIndexes[x, y] = CurrentPaletteIndex;

                var at = Palette.Tiles[PaletteIndexes[x, y]] as Autotile;
                var around = GetAround(x, y);
                if (at != null)
                    g1.DrawImage(at.CalculateAndGet(Palette, around), x * Data.TileSize, y * Data.TileSize);
                else
                    g1.FillRectangle(new SolidBrush(DefaultBGColor), x * Data.TileSize, y * Data.TileSize, Data.TileSize, Data.TileSize);

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        at = Palette.Tiles[GetTile(x + i, y + j)] as Autotile;
                        if (at != null)
                            g1.DrawImage(at.CalculateAndGet(Palette, GetAround(x + i, y + j)), (x + i) * Data.TileSize, (y + j) * Data.TileSize);
                    }
                }
            }
        }

        byte GetTile(int _x, int _y) => (_x < 0 || _y < 0 || _x >= Render.Width / Data.TileSize || _y >= Render.Height / Data.TileSize) ? (byte)0 : PaletteIndexes[_x, _y];
        private byte[,] GetAround(int x, int y)
        {
            byte Get(int _x, int _y) => GetTile(_x, _y);

            //return new byte[,]
            //{
            //    { Get(x-1, y-1), Get(x-1, y), Get(x-1, y+1) },
            //    { Get(x, y-1), 0, Get(x, y) },
            //    { Get(x+1, y-1), Get(x+1, y), Get(x+1, y+1)}
            //};

            var result=new byte[,]
            {
                { Get(x-1, y-1), Get(x-1, y), Get(x-1, y+1) },
                { Get(x, y-1), 0, Get(x, y+1) },
                { Get(x+1, y-1), Get(x+1, y), Get(x+1, y+1)}
            };
            return result;
        }

        private void Draw(object sender, EventArgs e)
        {
            if(DisplayAllTiles)
            {
                var at = Palette.Tiles[CurrentPaletteIndex] as Autotile;
                if (at != null)
                {
                    int x = 0, y = 0;
                    ResetLayer2();
                    var tiles = at.AllTiles();
                    g2.FillRectangle(Brushes.Black, TileToShowAllTiles.X * Data.TileSize, TileToShowAllTiles.Y * Data.TileSize, 3 * Data.TileSize - 1, 3 * Data.TileSize - 1);
                    g2.DrawRectangle(Pens.White, TileToShowAllTiles.X * Data.TileSize - 1, TileToShowAllTiles.Y * Data.TileSize - 1, 3 * Data.TileSize + 1, 3 * Data.TileSize + 1);
                    foreach (Bitmap tile in tiles)
                    {
                        g2.DrawImage(tile, (TileToShowAllTiles.X + x) * Data.TileSize, (TileToShowAllTiles.Y + y) * Data.TileSize);
                        x++;
                        if (x > 2)
                        {
                            x = 0;
                            y++;
                        }
                    }
                }
            }

            Bitmap img = new Bitmap(Render.Width, Render.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(Layer1, 0, 0);
                g.DrawImage(Layer2, 0, 0);
            }
            Render.Image = img;
        }

        private void ResetLayer2()
        {
            Layer2 = new Bitmap(Layer2.Width, Layer2.Height);
            g2 = Graphics.FromImage(Layer2);
        }

        private int Snap(int coord) => (coord / Data.TileSize) * Data.TileSize;
    }
}
