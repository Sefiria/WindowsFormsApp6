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

            tbName.Text = Data.Project.name;
            var split = Data.Project.version.Split('.').Select(x => int.Parse(x)).ToList();
            numMajor.Value = split[0];
            numMinor.Value = split[1];
            numBuild.Value = split[2];
            tbTitle.Text = Data.Project.title;
            tbAuthor.Text = Data.Project.author;
            split = Data.Project.factorio_version.Split('.').Select(x => int.Parse(x)).ToList();
            numFMaj.Value = split[0];
            numFMin.Value = split[1];
            numFBuild.Value = split[2];
            rtbDescription.Text = Data.Project.description;

            TimerUpdate.Tick += Update;
        }

        private void Update(object sender, EventArgs e)
        {
            var input = tbName.Text;

            if (string.IsNullOrWhiteSpace(input) || Regex.IsMatch(input, "[^A-Za-z0-9]"))
            {
                ok = false;
                lbError.ForeColor = Color.Red;
                lbError.Text = "Mod name should contain only letters and digits (A-Za-z0-9).";
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
                Data.Project.name = tbName.Text;
                Data.Project.version = $"{numMajor.Value}.{numMinor.Value}.{numBuild.Value}";
                Data.Project.title = tbTitle.Text;
                Data.Project.author = tbAuthor.Text;
                Data.Project.factorio_version = $"{numFMaj.Value}.{numFMin.Value}.{numFBuild.Value}";
                Data.Project.description = rtbDescription.Text;

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
