using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp17.Utilities;
using static WindowsFormsApp17.enums;

namespace WindowsFormsApp17
{
    public partial class Form1 : Form
    {
        Bitmap img;

        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        UserUI user = null;
        MouseEventArgs LastMouseEventArgs = null;

        public Form1()
        {
            InitializeComponent();

            Core.rw = Render.Width;
            Core.rh = Render.Height;
            img = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(img);

            ResMgr.Init();

            Data.Instance.Init();
            KB.Init();
            Core.Init();

            user = new UserUI();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            //new List<Structure>(Data.Instance.Structures.Values).ForEach(s => s.Update());
            //new List<Item>(Data.Instance.Items).ForEach(o => o.Update());

            if(LastMouseEventArgs != null)
                Render_MouseMove(null, LastMouseEventArgs);

            user.Update();

            KB.Update();
        }

        private void Draw(object _, EventArgs e)
        {
            img = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(img);
            Core.g.Clear(Color.Black);

            ////back
            //new List<Structure>(Data.Instance.Structures.Values).ForEach(s => s.Display());
            //new List<Item>(Data.Instance.Items).ForEach(s => s.Display());
            ////front
            //new List<Structure>(Data.Instance.Structures.Values).ForEach(s => s.FrontDisplay());

            DrawMap();

            user.Display();

            Render.Image = img;
        }

        private void DrawMap()
        {
            vecf start = Data.Instance.cam - new vecf(Core.rw / 2F, Core.rh / 2F) - Core.TSZ;

            int tsz = Core.TSZ;
            var cam = Data.Instance.cam;
            var tcam = Data.Instance.cam.tile(tsz);
            int hrtw = Core.rw / tsz / 2;
            int hrth = Core.rh / tsz / 2;

            int _x, _y, id;
            Bitmap tile;
            for (int x = -hrtw - 1; x < hrtw + 3; x++)
            {
                for (int y = -hrth - 1; y < hrth + 3; y++)
                {
                    _x = (int)(tcam.x + x);
                    _y = (int)(tcam.y + y);
                    id = Data.Instance.map[_x, _y];
                    if (id == 0)
                    {
                        // draw bg instead if exists
                        int bg_id = Data.Instance.map.GetBgTile(_x, _y);
                        if (bg_id > 0)
                            Core.g.DrawImage(ResMgr.GetBGTile(bg_id), (hrtw + x) * tsz - cam.x % tsz, (hrth + y) * tsz - cam.y % tsz);
                    }
                    else
                    {
                        tile = ResMgr.GetTile(id);
                        Core.g.DrawImage(tile, (hrtw + x) * tsz - cam.x % tsz, (hrth + y) * tsz - cam.y % tsz);
                    }
                }
            }
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            Core.MouseHolding = true;
            Render_MouseMove(null, e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            Core.MouseHolding = false;
            user.MouseInput(e, true);
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            Core.MouseHolding = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            LastMouseEventArgs = e;
            Core.MouseLocation = e.Location;

            if (Core.MouseHolding)
            {
                user.MouseInput(e);
            }
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            user.MouseWheel(e);
        }
    }
}
