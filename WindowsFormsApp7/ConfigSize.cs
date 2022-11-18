using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp7
{
    public partial class FormConfigSize : Form
    {
        public bool TilesHasChanged = false;
        public int ResultW, ResultH;
        private bool m_Init = true;

        public FormConfigSize()
        {
            InitializeComponent();

            numWidth.Minimum = numHeight.Minimum = 4;
            numZoom.Minimum = numIteN.Minimum = 1;
            numWidth.Maximum = 1024;
            numHeight.Maximum = 1024;
            numZoom.Maximum = numIteN.Maximum = 64;

            numWidth.Value = Core.RW < numWidth.Minimum ? numWidth.Minimum : (Core.RW > numWidth.Maximum ? numWidth.Maximum : Core.RW);
            numHeight.Value = Core.RH < numHeight.Minimum ? numHeight.Minimum : (Core.RH > numHeight.Maximum ? numHeight.Maximum : Core.RH);
            numZoom.Value = Core.Zoom < numZoom.Minimum ? numZoom.Minimum : (Core.Zoom > numZoom.Maximum ? numZoom.Maximum : Core.Zoom);
            numIteN.Value = Core.IterationsCount < numIteN.Minimum ? numIteN.Minimum : (Core.IterationsCount > numIteN.Maximum ? numIteN.Maximum : Core.IterationsCount);

            m_Init = false;
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            m_Init = true;
            Core.IterationsCount = (int)numIteN.Value;
            Core.Zoom = (int)numZoom.Value;

            if(!TilesHasChanged)
            {
                Core.g.Clear(Color.Black);
                RenderClass.ModifiedPixels = RenderClass.GetAllPixelsPoints();
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ResetResultSize(object sender, EventArgs e)
        {
            if (m_Init) return;
            ResultW = (int)numWidth.Value;
            ResultH = (int)numHeight.Value;
            TilesHasChanged = true;
        }
    }
}
