using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp10
{
    public partial class Form1 : Form
    {
        int W, H;
        Bitmap Image;
        Graphics g;
        Timer timerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer timerDraw = new Timer() { Enabled = true, Interval = 10 };
        Peix Px;

        public Form1()
        {
            InitializeComponent();

            W = Render.Width;
            H = Render.Height;
            Image = new Bitmap(W, H);
            g = Graphics.FromImage(Image);

            timerUpdate.Tick += Update;
            timerDraw.Tick += Draw;

            Px = new Peix(W / 2, H / 2);
        }

        void Update(object sender, EventArgs e)
        {
            Px.Update();
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if (Data.Money < 1) return;
            else Data.Money--;
            var sz = Tools.GetRnd(1, 16);
            if (sz <= 10) sz = 1;
            else sz -= 10;
            Data.Plants.Add(new Plant(e.X, e.Y, sz));
        }

        void Draw(object sender, EventArgs e)
        {
            g.Clear(Color.Black);

            var list = new List<Plant>(Data.Plants);
            for (int i = 0; i < list.Count; i++)
                g.FillEllipse(Brushes.Lime, list[i].X, list[i].Y, 1F + list[i].Size, 1F + list[i].Size);

            g.DrawEllipse(Pens.White, Px.X - Px.Diameter / 2F, Px.Y - Px.Diameter / 2F, Px.Diameter, Px.Diameter);
            g.FillEllipse(Brushes.White, Px.X - Px.Radius / 4F, Px.Y - Px.Radius / 4F, 1.5F, 1.5F);
            g.FillEllipse(Brushes.White, Px.X + Px.Radius / 4F, Px.Y - Px.Radius / 4F, 1.5F, 1.5F);
            g.DrawLine(Pens.White, Px.X - Px.Radius / 6F, Px.Y + Px.Radius / 4F, Px.X + Px.Radius / 6F, Px.Y + Px.Radius / 4F);
            g.DrawLine(Pens.White, Px.X - Px.Radius / 6F, Px.Y + Px.Radius / 4F, Px.X - Px.Radius / 4F, Px.Y + Px.Radius / 6F);
            g.DrawLine(Pens.White, Px.X + Px.Radius / 6F, Px.Y + Px.Radius / 4F, Px.X + Px.Radius / 4F, Px.Y + Px.Radius / 6F);

            g.DrawString(Data.Money.ToString(), Data.Font, Brushes.White, 5, 5);

            Render.Image = Image;
        }
    }
}
