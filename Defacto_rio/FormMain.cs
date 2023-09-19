using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Defacto_rio
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btCreate_Click(object sender, EventArgs e)
        {
            var form = new FormCreate();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                lbProjectName.Text = form.tbName.Text;
            }
        }
        private void btOpen_Click(object sender, EventArgs e)
        {

        }
        private void btSave_Click(object sender, EventArgs e)
        {

        }

        private void btGroups_Click(object sender, EventArgs e)
        {
            new FormGroups().ShowDialog(this);
        }
        private void btSubGroups_Click(object sender, EventArgs e)
        {
            new FormSubGroups().ShowDialog(this);
        }
        private void btItems_Click(object sender, EventArgs e)
        {
            new FormItems().ShowDialog(this);
        }
        private void btRecipes_Click(object sender, EventArgs e)
        {
            new FormRecipes().ShowDialog(this);
        }
    }
}
