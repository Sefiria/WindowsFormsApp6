using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolActivityMaker
{
    public partial class UCTime : UserControl
    {
        public UCTime(TimeSpan time)
        {
            InitializeComponent();

            Hours.Controls[0].Visible = false;
            Minutes.Controls[0].Visible = false;

            Hours.Value = time.Hours;
            Minutes.Value = time.Minutes;
        }
    }
}
