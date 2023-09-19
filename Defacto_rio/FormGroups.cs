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
    public partial class FormGroups : Form
    {
        int last_selected_id = -1;
        public FormGroups()
        {
            InitializeComponent();

            Data.Groups.ForEach(item => listItems.Items.Add(item));
        }

        private void listItems_MouseDown(object sender, MouseEventArgs e)
        {
            bool idselected = listItems.SelectedIndex > -1 && listItems.SelectedIndex < listItems.Items.Count;
            if (e.Button == MouseButtons.Right)
            {
                ToolStripDropDown pop = new ToolStripDropDown();
                pop.Items.Add("Create", null, (_s, _e) =>
                {
                    dgv.Visible = btSaveItem.Enabled = true;
                    listItems.SelectedIndex = listItems.Items.Add("new-group");
                    dgv.Rows.Clear();
                    typeof(Group).GetFields().ToList().ForEach(field => dgv.Rows.Add(field.Name, ""));
                    dgv.Create(typeof(Group));
                    dgv.Rows[0].Cells[1].Value = "new-group";
                });
                var edit = pop.Items.Add("Edit", null, (_s, _e) =>
                {
                    dgv.Visible = btSaveItem.Enabled = true;
                    dgv.Rows.Clear();
                    dgv.Edit(typeof(Group), Data.Groups[listItems.SelectedIndex]);
                });
                edit.Enabled = idselected;
                var remove = pop.Items.Add("Remove", null, (_s, _e) =>
                {
                    var id = listItems.SelectedIndex;
                    listItems.Items.RemoveAt(id);
                    Data.Groups.RemoveAt(id);
                    dgv.Visible = btSaveItem.Enabled = false;
                    dgv.Rows.Clear();
                });
                remove.Enabled = idselected;
                pop.Show(this, e.Location);
            }

            if (listItems.SelectedItem == null)
            {
                dgv.Visible = btSaveItem.Enabled = false;
                dgv.Rows.Clear();
            }
            else
            {
                last_selected_id = listItems.SelectedIndex;
            }
        }

        private void btSaveItem_Click(object sender, EventArgs e)
        {
            if (listItems.SelectedItem == null)
            {
                MessageBox.Show(this, "No group selected in the list !", "Defacto.rio - Groups - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Data.Groups[listItems.SelectedIndex].name = dgv.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells[0].Value as string == "name")?.Cells[1].Value as string;
            if (Data.Groups.Count <= listItems.SelectedIndex)
                Data.Groups.Add(new Group());
            dgv.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => typeof(Group).GetFields().FirstOrDefault(field => field.Name == row.Cells[0].Value as string)?.SetValue(Data.Groups[listItems.SelectedIndex], row.Cells[1].Value as string));
            listItems.Items[listItems.SelectedIndex] = dgv[1, 0].Value;
        }
    }
}
