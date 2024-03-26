using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolActivityMaker
{
    public partial class DateTimeDialog : Form
    {
        public DateTimeDialog()
        {
            InitializeComponent();
        }

        public static (DateTimeDialog Instance, DialogResult Result) ShowDialog(string Description)
        {
            DateTimeDialog Instance = new DateTimeDialog();
            Instance.Text = Description;
            return (Instance, Instance.ShowDialog());
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
