using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Tooling;

namespace WindowsFormsApp29
{
    public partial class MainForm : Form
    {
        public static MainForm Ref;
        Timer TmUpdate = new Timer() { Enabled = true, Interval = 1 };
        Timer TmDraw = new Timer() { Enabled = true, Interval = 1 };
        Bitmap Image;
        Graphics g;
        VM_main VM_Main;
        public MainForm()
        {
            Ref = this;
            VM_Main = new VM_main();

            InitializeComponent();

            KB.Init();
            MouseStates.Initialize(Render);

            TmUpdate.Tick += Update;
            TmDraw.Tick += Draw;
        }
        void Update(object sender, EventArgs e)
        {
            VM_Main.Update();
            KB.Update();
            MouseStates.Update();
        }
        void Draw(object sender, EventArgs e)
        {
            Image = new Bitmap(Size.Width, Size.Height);
            g = Graphics.FromImage(Image);
            VM_Main.Draw(g);
            Render.Image = Image;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            VM_Main.Dispose();
            base.OnClosing(e);
        }
    }
}
