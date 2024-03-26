using System;
using System.Windows.Forms;
using System.Device.Location;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Core;
using System.IO;
using Core.Utils;
using System.Windows.Forms.Design;

namespace Test
{
    public partial class FormTest : Form
    {
        Timer TimerUpdate = new Timer() { Interval = 50 };
        Timer TimerDraw = new Timer() { Interval = 50 };
        TileMap tileMap = null;
        float zoom = 1F;
        bool ClearRender = false;
        TrafficMgr trafficMgr = null;
        TrackBar tb;
        Bitmap MapImage = null;

        public FormTest()
        {
            InitializeComponent();

            tb = new TrackBar();
            tb.Name = "TrafficFlow";
            tb.Minimum = 0;
            tb.Maximum = 10;
            tb.Value = 1;
            tb.SmallChange = 1;
            tb.LargeChange = 5;
            ToolStripControlHost tsch_tb = new ToolStripControlHost(tb, tb.Name);
            tsch_tb.Text = tsch_tb.Name;
            tsbtTrafficFlow.DropDownItems.Add(tsch_tb);

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
            TimerUpdate.Enabled = TimerDraw.Enabled = true;
            MouseWheel += form_MouseWheel;
        }

        protected override void OnLoad(EventArgs e)
        {
            TileResMngr.Instance.Initialize(Directory.GetCurrentDirectory());
            MapImage = new Bitmap(Render.Width, Render.Height);
        }

        private void Update(object sender, EventArgs e)
        {
            trafficMgr?.Update(tb.Value);
        }

        private void Map_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {

            }
        }
        private void Map_MouseDown(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
            }
        }
        private void form_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Render.Image == null)
                return;

            zoom += Math.Sign(e.Delta) * 0.2F;
            if (zoom < 1F) zoom = 1F;
            if (zoom > 2F) zoom = 2F;
            ClearRender = true;
            tileMap?.Invalidate();
        }

        private void Draw(object sender, EventArgs e)
        {
            if (tileMap == null)
                return;
                        
            using (Graphics g = Graphics.FromImage(MapImage))
            {
                if (ClearRender)
                    g.Clear(SystemColors.Control);
                if(tileMap != null)
                    tileMap.Render(g, zoom, false);
            }

            ClearRender = false;

            Bitmap img = new Bitmap(MapImage);
            using (Graphics g = Graphics.FromImage(img))
            {
                trafficMgr?.Draw(g, zoom);
                //TextRenderer.DrawText(g, Map.Zoom.ToString(), DefaultFont, new Point(5, 20), Color.Lime);
            }

            Render.Image = img;
        }

        private void MenuLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dial = new OpenFileDialog();
            dial.InitialDirectory = Directory.GetCurrentDirectory();
            dial.Filter = "TILEMAP files (*.tilemap)|*.tilemap";
            if (dial.ShowDialog() == DialogResult.OK)
            {
                TileMap tileMap = Tools.DeserializeJSONFromFile<TileMap>(dial.FileName);
                if (tileMap != null)
                {
                    this.tileMap = tileMap;
                    tileMap.Invalidate();
                    trafficMgr = new TrafficMgr(tileMap);
                }
            }
        }
        private void tsbtReset_Click(object sender, EventArgs e)
        {
            trafficMgr?.Cars.Clear();
        }
    }
}
