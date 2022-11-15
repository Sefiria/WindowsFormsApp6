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
        public int ResultW, ResultH;

        public FormConfigSize()
        {
            InitializeComponent();

            numWidth.Minimum = numHeight.Minimum = 4;
            numTileSize.Minimum = 1;
            numWidth.Maximum = 256;
            numHeight.Maximum = 256;
            numTileSize.Maximum = 256;

            numWidth.Value = Core.RW < numWidth.Minimum ? numWidth.Minimum : (Core.RW > numWidth.Maximum ? numWidth.Maximum : Core.RW);
            numHeight.Value = Core.RH < numHeight.Minimum ? numHeight.Minimum : (Core.RH > numHeight.Maximum ? numHeight.Maximum : Core.RH);
            numTileSize.Value = Core.TileSz < numTileSize.Minimum ? numTileSize.Minimum : (Core.TileSz > numTileSize.Maximum ? numTileSize.Maximum : Core.TileSz);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ResetResultSize(object sender, EventArgs e)
        {
            ResultW = (int)(numWidth.Value / numTileSize.Value);
            ResultH = (int)(numHeight.Value / numTileSize.Value);
            lbResultsizeValue.Text = $"{ResultW} x {ResultH}";
        }
    }
}
