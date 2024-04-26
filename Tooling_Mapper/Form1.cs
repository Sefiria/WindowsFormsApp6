using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;

namespace Tooling_Mapper
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };
        Bitmap RenderImage;
        Graphics g;

        public Form1()
        {
            InitializeComponent();

            MouseStates.Initialize(Render);
            KB.Init();

            RenderImage = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(RenderImage);

            Cursor.Hide();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }

        private void Update(object sender, EventArgs e)
        {
            KB.Update();
            MouseStates.Update();
        }
        private void Draw(object sender, EventArgs e)
        {
            g.Clear(Color.Black);

            Render.Image = RenderImage;
        }
    }
}
