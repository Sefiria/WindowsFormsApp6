using PishConverter.Common.MapSpec;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PishConverter
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Bitmap img;

        public Form1()
        {
            InitializeComponent();

            Initialize();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Initialize()
        {
            int w = Render.Width;
            int h = Render.Height;
            Global.W = w;
            Global.H = h;
            draw_reinit();

            Map.Instance.Inisitalize();
        }

        private void Update(object sender, EventArgs e)
        {
            Map.Instance.Update();
        }

        private void draw_reinit()
        {
            img = new Bitmap(Global.W, Global.H);
            Global.g = Graphics.FromImage(img);
            Global.g.Clear(Color.Black);
        }
        private void Draw(object sender, EventArgs e)
        {
            Map.Instance.Draw();

            Render.Image = img;
            draw_reinit();
        }
    }
}
