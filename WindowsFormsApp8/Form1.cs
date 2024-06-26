﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap ImageUI;

        public Form1()
        {
            InitializeComponent();

            Autotile.Data.TileSize = Core.TileSz;

            Core.RW = Render.Width;
            Core.RH = Render.Height;
            Core.Cam = new Point(-Core.RW / 2 + (Core.WT * Core.TileSz) / 2, -Core.RH / 2 + (Core.HT * Core.TileSz) / 2);
            Core.Image = new Bitmap(Core.RW, Core.RH);
            Core.g = Graphics.FromImage(Core.Image);
            ImageUI = new Bitmap(Core.RW, Core.RH);
            ImageUI.MakeTransparent(Color.White);
            Core.ListTiles = listTiles;

            RenderClass.Initialize();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            listTiles.ForeColor = Color.White;

            btPen_Click(null, null);
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Core.IsMouseDown = true;
            Core.IsRightMouseDown = e.Button == MouseButtons.Right;
            if(e.Button == MouseButtons.Middle)
            {
                Core.IsMiddleMouseDown = true;
                RenderClass.MousePositionAtMiddleFirstClick = e.Location;
            }
            RenderClass.MouseDown(e);
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            Core.IsMouseDown = false;
            Core.IsRightMouseDown = false;
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            Core.IsMouseDown = false;
            Core.IsRightMouseDown = false;
            Core.IsMiddleMouseDown = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
                RenderClass.MousePositionAtMiddleFirstClick = e.Location;
            RenderClass.MouseMove();
            Core.MousePosition = e.Location;
        }

        private void Update(object sender, EventArgs e)
        {
            RenderClass.Update();
        }

        private void Draw(object sender, EventArgs e)
        {
            //Stopwatch toto = new Stopwatch();
            //toto.Start();

            if (Render.Width == 0 || Render.Height == 0) return;
            Bitmap renderImage = new Bitmap(Render.Width, Render.Height);
            using (Graphics g = Graphics.FromImage(renderImage))
            {
                using (Core.gui = Graphics.FromImage(ImageUI))
                {
                    RenderClass.Draw();

                    g.DrawImage(Core.Image, 0, 0);
                    g.DrawImage(ImageUI, 0, 0);
                    g.DrawImage(RenderClass.Layer2, 0, 0);

                    g.DrawRectangle(Pens.Red, 0, 0, Render.Width - 1, Render.Height - 1);

                    Core.gui.Clear(Color.White);
                }
                ImageUI.MakeTransparent(Color.White);
            }

            Render.Image = renderImage;

            //toto.Stop();
            //Console.WriteLine(toto.ElapsedMilliseconds);
        }



        private void listTiles_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                ToolStripDropDown menu = new ToolStripDropDown();
                menu.BackColor = Color.FromArgb(22, 22, 22);
                menu.ForeColor = Color.FromArgb(200, 200, 200);
                menu.Font = new Font("Segoe UI", 12F);

                int itemId = GetListTilesItemClicked(e);
                if (itemId != -1)
                {
                    // Click on a specific item
                    menu.Items.Add("Replace Autotile").Click += (_s, _e) => { var tile = RenderClass.GetImportedAutotile(); if (tile != null) listTiles.Items[itemId] = tile; };
                    menu.Items.Add("Replace Tile").Click += (_s, _e) => { var tile = RenderClass.GetImportedTile(); if (tile != null) listTiles.Items[itemId] = tile; };
                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add("Remove Tile").Click += (_s, _e) => listTiles.Items.RemoveAt(itemId);

                }
                else
                {
                    // Click away from items
                    menu.Items.Add("Import Autotile").Click += (_s, _e) => RenderClass.ImportAutotile();
                    menu.Items.Add("Import Tile").Click += (_s, _e) => RenderClass.ImportTile();
                    menu.Items.Add("Import Palette").Click += (_s, _e) => RenderClass.ImportPalette();
                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add("Clear").Click += (_s, _e) => { if (MessageBox.Show(this, "Are you sure you want to clear the item list ?", "Clear", MessageBoxButtons.YesNo) == DialogResult.Yes) RenderClass.ClearTileList(); };

                }

                menu.Show(listTiles, e.Location);
            }
        }

        int GetListTilesItemClicked(MouseEventArgs e)
        {
            for (int i = 0; i < listTiles.Items.Count; i++)
            {
                var rect = listTiles.GetItemRectangle(i);
                if (rect.Contains(e.Location))
                    return i;
            }
            return -1;
        }

        private void Configs()
        {
            new Configs().ShowDialog(this);
        }

        private void btPen_Click(object sender, EventArgs e)
        {
            RenderClass.Tool = RenderClass.Tools.Pen;
            imgUsedTool.Image = Resources.tool_pen;
        }
        private void btBucket_Click(object sender, EventArgs e)
        {
            RenderClass.Tool = RenderClass.Tools.Bucket;
            imgUsedTool.Image = Resources.tool_bucket;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Core.ControlKeyHelp = e.Control;

            if (e.KeyCode == Keys.L)
                RenderClass.LoadMap();

            if (e.KeyCode == Keys.S)
                RenderClass.SaveMap();

            if (listTiles.SelectedIndex > -1)
            {
                if (listTiles.SelectedIndex < listTiles.Items.Count - 1)
                {
                    if (e.KeyCode == Keys.D1)
                        listTiles.SelectedIndex++;
                    if (e.KeyCode == Keys.Down)
                    {
                        Tile tile = listTiles.SelectedItem as Tile;
                        listTiles.Items[listTiles.SelectedIndex] = listTiles.Items[listTiles.SelectedIndex + 1];
                        listTiles.Items[listTiles.SelectedIndex + 1] = tile;
                    }
                }
                if (listTiles.SelectedIndex > 0)
                {
                    if (e.Shift && e.KeyCode == Keys.D1)
                        listTiles.SelectedIndex--;
                    if (e.KeyCode == Keys.Up)
                    {
                        Tile tile = listTiles.SelectedItem as Tile;
                        listTiles.Items[listTiles.SelectedIndex] = listTiles.Items[listTiles.SelectedIndex - 1];
                        listTiles.Items[listTiles.SelectedIndex - 1] = tile;
                    }
                }
            }

            if (e.KeyCode == Keys.C)
                Configs();

            if (e.KeyCode == Keys.OemQuotes)
            {
                if (RenderClass.Tool == RenderClass.Tools.Pen)
                    btBucket_Click(null, null);
                else
                    btPen_Click(null, null);
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Core.ControlKeyHelp = e.Control;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Core.RW = Render.Width;
            Core.RH = Render.Height;
            Core.Image = new Bitmap(Core.RW, Core.RH);
            Core.g = Graphics.FromImage(Core.Image);
            ImageUI = new Bitmap(Core.RW, Core.RH);
            ImageUI.MakeTransparent(Color.White);
        }
    }
}
