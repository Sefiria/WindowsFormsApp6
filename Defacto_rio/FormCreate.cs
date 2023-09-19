using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Defacto_rio
{
    public partial class FormCreate : Form
    {
        bool ok = false;
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };

        public FormCreate()
        {
            InitializeComponent();

            TimerUpdate.Tick += Update;
        }

        private void Update(object sender, EventArgs e)
        {
            var input = tbName.Text;

            if (string.IsNullOrWhiteSpace(input) || Regex.IsMatch(input, "[^A-Za-z]"))
            {
                ok = false;
                lbError.ForeColor = Color.Red;
                lbError.Text = "Mod name should contain only letters (A-Za-z).";
            }
            else
            {
                ok = true;
                lbError.ForeColor = Color.Black;
                lbError.Text = "No error.";
            }
        }

        private void btValidate_Click(object sender, EventArgs e)
        {
            if(ok)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(this, lbError.Text, "Defacto.rio - CreateProject - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
