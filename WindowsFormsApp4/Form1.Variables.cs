using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 1 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 1 };
        Bitmap RenderImage;
    }
}
