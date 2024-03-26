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
    public partial class TextboxDialog : Form
    {
        public TextboxDialog()
        {
            InitializeComponent();
        }

        private void tbValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        public static (TextboxDialog Instance, DialogResult Result) ShowDialog(string Description, string DefaultValue = "", int MaxLength = 64, bool OnlyNumbers = false)
        {
            TextboxDialog Instance = new TextboxDialog();
            Instance.Text = Description;
            Instance.tbValue.Text = DefaultValue;
            Instance.tbValue.MaxLength = MaxLength;
            if (OnlyNumbers)
                Instance.tbValue.KeyPress += Instance.tbValue_KeyPress;
            return (Instance, Instance.ShowDialog());
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
