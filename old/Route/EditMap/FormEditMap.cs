using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Core;
using Core.Utils;
using static Core.Definitions;
using static Core.TileResMngr;
using static Core.TileMap;

namespace EditMap
{
    public partial class FormEditMap : Form
    {
        Timer TimerUpdate = new Timer() { Interval = 50 };
        Timer TimerDraw = new Timer() { Interval = 50 };
        string CurFileName;
        TileMap tileMap;
        PenType penType;
        TileIndex tileIndex = new TileIndex();
        Point MouseClient => Render.PointToClient(MousePosition);
        Point MouseTile => new Point(
                MouseClient.X / SharedData.TileSize,
                MouseClient.Y / SharedData.TileSize
                );

        public FormEditMap()
        {
            InitializeComponent();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
            TimerUpdate.Enabled = TimerDraw.Enabled = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            TileResMngr.Instance.Initialize(Directory.GetCurrentDirectory());

            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(SharedData.TileSize, SharedData.TileSize);
            listTiles.Clear();
            Dictionary<int, TileInfo> list = TileResMngr.Instance.ResourcesTileInfo;
            foreach (KeyValuePair<int, TileInfo> tile in list)
            {
                imgList.Images.Add(TileResMngr.Instance.ResourcesTileInfo[tile.Key].ImageWithSigns);
                listTiles.Items.Add(new ListViewItem("", tile.Key));
            }
            listTiles.LargeImageList = imgList;
            int lp = ((SharedData.TileSize + 4) << 16) + SharedData.TileSize + 4;
            SendMessage(listTiles.Handle, 0x1035, IntPtr.Zero, (IntPtr)lp);

            tileMap = new TileMap(8);
            penType = PenType.Pen;
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private void Update(object sender, EventArgs e)
        {
        }

        private void Render_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.R:
                    Tools.Rotate(ref tileIndex.Angle);
                    Bitmap img = new Bitmap(CurrentTileView.Image);
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    CurrentTileView.Image = img;
                    break;
            }
        }
        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseTile.X < 0 || MouseTile.X >= tileMap.Size ||
                MouseTile.Y < 0 || MouseTile.Y >= tileMap.Size)
                return;

            if (e.Button == MouseButtons.Right)
            {
                tileIndex = tileMap[MouseTile];
                CurrentTileView.Image = Tools.ApplyRotation((Bitmap)listTiles.LargeImageList.Images[tileIndex.Index], tileIndex.Angle);
            }

            if (e.Button == MouseButtons.Left)
            {
                tileMap[MouseTile] = tileIndex;
            }
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                Render_MouseDown(sender, e);
        }

        private void Draw(object sender, EventArgs e)
        {
            Bitmap img;
            if (Render.Image != null)
                img = new Bitmap(Render.Image);
            else
                img = new Bitmap(Render.Width, Render.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                tileMap.Render(g, 1F, cbShowDots.Checked);
                //TextRenderer.DrawText(g, Map.Zoom.ToString(), DefaultFont, new Point(5, 20), Color.Lime);
            }
            Render.Image = img;
        }

        private void MenuNew_Click(object sender, EventArgs e)
        {
            OnLoad(null);
        }
        private void MenuLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dial = new OpenFileDialog();
            dial.InitialDirectory = Directory.GetCurrentDirectory();
            dial.Filter = "TILEMAP files (*.tilemap)|*.tilemap";
            if (dial.ShowDialog() == DialogResult.OK)
            {
                CurFileName = dial.FileName;
                TileMap tileMap = Tools.DeserializeJSONFromFile<TileMap>(CurFileName);
                if (tileMap != null)
                {
                    this.tileMap = tileMap;
                    tileMap.Invalidate();
                }
            }
        }
        private void MenuSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurFileName))
            {
                toolStripButton4.PerformClick();
            }
            else
            {
                Tools.Serialize(tileMap, CurFileName);
            }
        }
        private void MenuSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dial = new SaveFileDialog();
            dial.InitialDirectory = Directory.GetCurrentDirectory();
            dial.Filter = "TILEMAP files (*.tilemap)|*.tilemap";
            if (dial.ShowDialog() == DialogResult.OK)
            {
                CurFileName = dial.FileName;
            }
            else
            {
                return;
            }
            Tools.Serialize(tileMap, CurFileName);
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            Render.Image = new Bitmap(Render.Width, Render.Height);
            tileMap = new TileMap(8);
        }
        private void btPen_Click(object sender, EventArgs e)
        {
            penType = PenType.Pen;

            btPen.FlatStyle = FlatStyle.Standard;
            btEraser.FlatStyle = FlatStyle.Popup;
            btFill.FlatStyle = FlatStyle.Popup;
        }
        private void btEraser_Click(object sender, EventArgs e)
        {
            penType = PenType.Eraser;

            btPen.FlatStyle = FlatStyle.Popup;
            btEraser.FlatStyle = FlatStyle.Standard;
            btFill.FlatStyle = FlatStyle.Popup;
        }
        private void btFill_Click(object sender, EventArgs e)
        {
            penType = PenType.Fill;

            btPen.FlatStyle = FlatStyle.Popup;
            btEraser.FlatStyle = FlatStyle.Popup;
            btFill.FlatStyle = FlatStyle.Standard;
        }

        private void listTiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listTiles.SelectedIndices.Count == 0 || listTiles.SelectedIndices[0] == -1)
                return;

            int index = listTiles.SelectedIndices[0];
            ListViewItem item = listTiles.Items[index];
            CurrentTileView.Image = listTiles.LargeImageList.Images[item.ImageIndex];
            tileIndex = new TileIndex(item.ImageIndex);
        }

        private void listTiles_MouseClick(object sender, MouseEventArgs e)
        {
            listTiles.SelectedIndices.Clear();
        }

        private void cbShowDots_CheckedChanged(object sender, EventArgs e)
        {
            tileMap.Invalidate();
        }
    }
}
