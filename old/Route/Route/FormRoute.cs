using System;
using System.Windows.Forms;
using EditTile;
using EditMap;
using Test;
using EditSign;

namespace Route
{
    public partial class FormRoute : Form
    {
        public FormRoute()
        {
            InitializeComponent();
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            new FormTest().ShowDialog();
        }
        private void btEditMap_Click(object sender, EventArgs e)
        {
            new FormEditMap().ShowDialog();
        }
        private void btEditTile_Click(object sender, EventArgs e)
        {
            new FormEditTile().ShowDialog();
        }
        private void btEditSign_Click(object sender, EventArgs e)
        {
            new FormEditSign().ShowDialog();
        }
    }
}
