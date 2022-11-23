using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsFormsApp8
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap Image, ImageUI;

        public Form1()
        {
            InitializeComponent();

            Core.RW = Render.Width;
            Core.RH = Render.Height;
            Image = new Bitmap(Core.RW, Core.RH);
            Core.g = Graphics.FromImage(Image);
            ImageUI = new Bitmap(Core.RW, Core.RH);
            ImageUI.MakeTransparent(Color.White);
            Core.ListTiles = listTiles;

            RenderClass.Initialize();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            listTiles.ForeColor = Color.White;
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Core.IsMouseDown = true;
            Core.IsRightMouseDown = e.Button == MouseButtons.Right;
            RenderClass.MouseDown();
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
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            RenderClass.MouseMove();
            Core.MousePosition = e.Location;
        }

        private void Update(object sender, EventArgs e)
        {
            RenderClass.Update();
        }

        private void Draw(object sender, EventArgs e)
        {
            Bitmap renderImage = new Bitmap(Core.RW, Core.RH);
            using (Graphics g = Graphics.FromImage(renderImage))
            {
                using (Core.gui = Graphics.FromImage(ImageUI))
                {
                    RenderClass.Draw();

                    g.DrawImage(Image, 0, 0);
                    g.DrawImage(ImageUI, 0, 0);

                    Core.gui.Clear(Color.White);
                }
                ImageUI.MakeTransparent(Color.White);
            }

            Render.Image = renderImage;
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

                }
                else
                {
                    // Click away from items
                    menu.Items.Add("Import Tile").Click += (_s, _e) => RenderClass.ImportTile();
                    menu.Items.Add("Import Palette").Click += (_s, _e) => RenderClass.ImportPalette();
                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add("Clear").Click += (_s, _e) => { if (MessageBox.Show(this, "Are you sure you want to clear the item list ?", "Clear", MessageBoxButtons.YesNo) == DialogResult.Yes) listTiles.Items.Clear(); };

                }

                menu.Show(listTiles, e.Location);
            }
        }

        private void Form_KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.L)
                RenderClass.LoadMap();

            if (e.KeyCode == Keys.S)
                RenderClass.SaveMap();
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
    }
}
