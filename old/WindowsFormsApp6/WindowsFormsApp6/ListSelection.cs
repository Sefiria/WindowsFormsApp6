using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class ListSelection : Form
    {
        static public string UserNameSelected = null;
        private ListSelection(string[] users)
        {
            InitializeComponent();
            listOnlineUsers.Items.AddRange(users);
        }

        private void listOnlineUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btSpy.Enabled = listOnlineUsers.SelectedIndex > -1;
            if (listOnlineUsers.SelectedIndex > -1)
                UserNameSelected = listOnlineUsers.SelectedItem.ToString();
        }

        private void btSpy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        static public DialogResult ShowDialog(string[] users)
        {
            UserNameSelected = null;
            return new ListSelection(users).ShowDialog();
        }
    }
}
