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
    public partial class FormSubGroups : Form
    {
        int last_selected_id = -1;
        public FormSubGroups()
        {
            InitializeComponent();

            Data.SubGroups.ForEach(item => listItems.Items.Add(item));
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
                    listItems.SelectedIndex = listItems.Items.Add("new-subgroup");
                    dgv.Rows.Clear();
                    dgv.Create(typeof(SubGroup));
                    dgv.Rows[0].Cells[1].Value = "new-subgroup";
                });
                var edit = pop.Items.Add("Edit", null, (_s, _e) =>
                {
                    dgv.Visible = btSaveItem.Enabled = true;
                    dgv.Rows.Clear();
                    dgv.Edit(typeof(SubGroup), Data.SubGroups[listItems.SelectedIndex]);
                });
                edit.Enabled = idselected;
                var remove = pop.Items.Add("Remove", null, (_s, _e) =>
                {
                    var id = listItems.SelectedIndex;
                    listItems.Items.RemoveAt(id);
                    Data.SubGroups.RemoveAt(id);
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
                MessageBox.Show(this, "No subgroup selected in the list !", "Defacto.rio - SubGroups - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Data.SubGroups[listItems.SelectedIndex].name = dgv.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells[0].Value as string == "name")?.Cells[1].Value as string;
            if (Data.SubGroups.Count <= listItems.SelectedIndex)
                Data.SubGroups.Add(new SubGroup());
            dgv.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => typeof(SubGroup).GetFields().FirstOrDefault(field => field.Name == row.Cells[0].Value as string)?.SetValue(Data.SubGroups[listItems.SelectedIndex], row.Cells[1].Value as string));
            listItems.Items[listItems.SelectedIndex] = dgv[1, 0].Value;
        }
    }
}
