using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Configs : Form
    {
        public Configs()
        {
            InitializeComponent();

            numWidth.Minimum = numHeight.Minimum = 4;
            numWidth.Maximum = numHeight.Maximum = 128;
            numZoom.Minimum = 1;
            numZoom.Maximum = 64;

            numWidth.Value = Core.WT < numWidth.Minimum ? numWidth.Minimum : (Core.WT > numWidth.Maximum ? numWidth.Maximum : Core.WT);
            numHeight.Value = Core.HT < numHeight.Minimum ? numHeight.Minimum : (Core.HT > numHeight.Maximum ? numHeight.Maximum : Core.HT);
            //numZoom.Value = Core.Zoom < numZoom.Minimum ? numZoom.Minimum : (Core.Zoom > numZoom.Maximum ? numZoom.Maximum : Core.Zoom);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            //Core.Zoom = (int)numZoom.Value;

            if(numWidth.Value != Core.WT || numHeight.Value != Core.HT)
            {
                Core.WT = (int)numWidth.Value;
                Core.HT = (int)numHeight.Value;
                RenderClass.ChangeTilesArraySize();
            }

            DialogResult = DialogResult.OK;
        }
    }
}
