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
        Bitmap Image;
        Graphics g;
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        byte[,] PaletteIndexes;
        Palette Palette = new Palette();
        bool ControlKeyHelp = false;

        public Form1()
        {
            InitializeComponent();

            LoadPalette();
            PaletteIndexes = new byte[Render.Width / Data.TileSize, Render.Height / Data.TileSize];

            Image = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(Image);
            TimerDraw.Tick += Draw;
        }

        private void LoadPalette()
        {
            Palette.Tiles.Add(new Tile(Resources.black));
            Palette.Tiles.Add(new Autotile(Resources.auto_ground));
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            ControlKeyHelp = e.Control;
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            ControlKeyHelp = false;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseHeld = true;
            Render_MouseMove(true, e);
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
            if (!MouseHeld || !(sender is bool) || (bool)sender != true) return;

            var x = e.X / Data.TileSize;
            var y = e.Y / Data.TileSize;

            if (ControlKeyHelp)
            {
                if(PaletteIndexes[x, y] != 0)
                {
                    var at = Palette.Tiles[PaletteIndexes[x, y]] as Autotile;
                    if (at != null)
                        g.DrawImage(at.GetNext(), x * Data.TileSize, y * Data.TileSize);
                }
            }
            else
            {
                PaletteIndexes[x, y] = 1;

                var at = Palette.Tiles[PaletteIndexes[x, y]] as Autotile;
                if (at != null)
                {
                    var around = GetAround(x, y);
                    g.DrawImage(at.CalculateAndGet(Palette, around), x * Data.TileSize, y * Data.TileSize);
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (i == 0 && j == 0) continue;
                            at = Palette.Tiles[GetTile(x + i, y + j)] as Autotile;
                            if (at != null)
                                g.DrawImage(at.CalculateAndGet(Palette, GetAround(x + i, y + j)), (x + i) * Data.TileSize, (y + j) * Data.TileSize);
                        }
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
            Render.Image = Image;
        }
    }
}
