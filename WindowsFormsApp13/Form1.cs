using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp13
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };

        public Form1()
        {
            InitializeComponent();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
        }
        private void Update(object _, EventArgs e)
        {
        }
        private void Draw(object _, EventArgs e)
        {
        }
    }
}
