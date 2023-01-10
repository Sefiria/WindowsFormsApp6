using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsFormsApp11.Travelers;

namespace WindowsFormsApp11
{
    public partial class Form1 : Form
    {
        Bitmap Image;
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerEvent = new Timer() { Enabled = true, Interval = 50 };
        bool CtrlDown = false;
        bool OpenCraftingTable = false;

        public Form1()
        {
            InitializeComponent();

            Var.W = Render.Width;
            Var.H = Render.Height;

            Image = new Bitmap(Var.W, Var.H);
            Var.g = Graphics.FromImage(Image);

            Var.Initialize();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
            TimerEvent.Tick += Event;
        }

        void Event(object sender, EventArgs e)
        {
            if(Var.Rnd.Next(50) == 25)
                TravelerFactory.CreateTiny();
        }

        void Update(object sender, EventArgs e)
        {
            if(Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (!CtrlDown)
                {
                    CtrlDown = true;
                    OpenCraftingTable = !OpenCraftingTable;
                    if (OpenCraftingTable)
                        CraftingTable.Open();
                    else
                        CraftingTable.Close();
                }
            }
            else
            {
                CtrlDown = false;
            }

            if (OpenCraftingTable)
            {
                CraftingTable.Update();
                return;
            }

            Var.Data.Update();
        }

        void Draw(object sender, EventArgs e)
        {
            Var.g.Clear(Color.Black);

            if (OpenCraftingTable)
            {
                CraftingTable.Draw();
            }
            else
            {
                Var.Data.Draw();

                UI.Draw();
            }

            Render.Image = Image;
        }

        private void Render_MouseLeave(object sender, EventArgs e)
        {
            if (OpenCraftingTable)
                CraftingTable.MouseLeave(sender, e);
        }
        private void Render_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (OpenCraftingTable)
                CraftingTable.MouseDown(sender, e);
        }
        private void Render_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (OpenCraftingTable)
                CraftingTable.MouseUp(sender, e);
        }
        private void Render_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (OpenCraftingTable)
                CraftingTable.MouseMove(sender, e);
        }
    }
}
