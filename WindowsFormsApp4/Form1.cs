using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Data.MainForm = this;
            Data.RenderW = Render.Width;
            Data.RenderH = Render.Height;
            RenderImage = new Bitmap(Render.Width, Render.Height);
            Data.g = Graphics.FromImage(RenderImage);

            Data.Carte = Carte.Default();
            Data.Pions.Add(new Pion(Data.Carte.StartCard.Coord));

            TimerUpdate.Tick += TimerUpdate_Tick;
            TimerDraw.Tick += TimerDraw_Tick;
        }
    }
}
