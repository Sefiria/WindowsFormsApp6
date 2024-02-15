using System;
using System.Drawing;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp17
{
    public partial class FormConfig : Form
    {
        Bitmap img;

        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        UserUI user = null;
        MouseEventArgs LastMouseEventArgs = null;

        public FormConfig()
        {
            InitializeComponent();

            Core.rw = Render.Width;
            Core.rh = Render.Height;
            img = new Bitmap(Core.rw, Core.rh);
            Core.g = Graphics.FromImage(img);
            Core.res = 1F;

            Data.Instance.Init();
            KB.Init();
            Core.Init();

            user = new UserUI();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object _, EventArgs e)
        {
            MouseStatesV1.Position = Render.PointToClient(MousePosition);

            //new List<Structure>(Data.Instance.Structures.Values).ForEach(s => s.Update());
            //new List<Item>(Data.Instance.Items).ForEach(o => o.Update());

            if (LastMouseEventArgs != null)
                Render_MouseMove(null, LastMouseEventArgs);

            user.Update();
            Data.Instance.map.FluidUpdate();

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

        #region DrawMap internals
        Color[] colors_waters = new Color[3]{
                Color.Cyan,
                Color.DodgerBlue,
                Color.MidnightBlue
            };
        int[] water_level = new int[] { 15, 30, 50 };
        Brush calc_water_brush(int __y)
        {
            Color _c;
            if (__y < water_level[0]) _c = colors_waters[0];
            else if (__y < water_level[1]) { _c = colors_waters[0].ShadeWith(colors_waters[1], (__y - water_level[0]) / (water_level[1] - water_level[0] - 1F)); }
            else if (__y < water_level[2]) { _c = colors_waters[1].ShadeWith(colors_waters[2], (__y - water_level[1]) / (water_level[2] - water_level[1] - 1F)); }
            else _c = colors_waters[2];
            return new SolidBrush(_c);
        }
        #endregion
        private void DrawMap()
        {
            //vecf start = Data.Instance.cam - new vecf(Core.rw / 2F, Core.rh / 2F) - Core.TSZ;

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
                        if (Data.Instance.map.check(_x, _y))
                        {
                            Map.fluid fluid = Data.Instance.map.fluids[_x, _y];
                            float q = Math.Min(1F, fluid.quantity);
                            if (q > 0F) Core.g.FillRectangle(calc_water_brush(_y), (hrtw + x) * tsz - cam.x % tsz, (hrth + y) * tsz - cam.y % tsz + tsz * (1F - q), tsz, tsz * q);
                        }
                    }
                    else
                    {
                        tile = ResMgr.GetTile(id);
                        Core.g.DrawImage(tile, (hrtw + x) * tsz - cam.x % tsz, (hrth + y) * tsz - cam.y % tsz);
                    }
                }
            }

            //Console.WriteLine("-------------------");
            //Console.WriteLine((Data.Instance.cam - new vecf(Core.rw / 2F, Core.rh / 2F)).tile(Core.TSZ));
            //Console.WriteLine("+ " + Core.MouseTile);
            //Console.WriteLine("+ " + 1);
            //Console.WriteLine("= " + Core.MouseCamTile);

            //Console.WriteLine($"tile [{Core.MouseCamTile}]");

            float i = Core.MouseCamTile.x;
            float j = Core.MouseCamTile.y;
            Core.g.DrawRectangle(Pens.Cyan, (hrtw + i) * tsz - cam.x, (hrth + j) * tsz - cam.y, tsz, tsz);
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStatesV1.ButtonDown = e.Button;
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
