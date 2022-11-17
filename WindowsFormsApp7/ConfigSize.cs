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
            numTileSize.Minimum = numIteN.Minimum = 1;
            numWidth.Maximum = 1024;
            numHeight.Maximum = 1024;
            numTileSize.Maximum = numIteN.Maximum = 64;

            numWidth.Value = Core.RWT < numWidth.Minimum ? numWidth.Minimum : (Core.RWT > numWidth.Maximum ? numWidth.Maximum : Core.RWT);
            numHeight.Value = Core.RHT < numHeight.Minimum ? numHeight.Minimum : (Core.RHT > numHeight.Maximum ? numHeight.Maximum : Core.RHT);
            numTileSize.Value = Core.TileSz < numTileSize.Minimum ? numTileSize.Minimum : (Core.TileSz > numTileSize.Maximum ? numTileSize.Maximum : Core.TileSz);
            numIteN.Value = Core.IterationsCount < numIteN.Minimum ? numIteN.Minimum : (Core.IterationsCount > numIteN.Maximum ? numIteN.Maximum : Core.IterationsCount);
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            Core.IterationsCount = (int)numIteN.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ResetResultSize(object sender, EventArgs e)
        {
            ResultW = (int)numWidth.Value;
            ResultH = (int)numHeight.Value;
        }
    }
}
