using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Defacto_rio
{
    public partial class FormProperties : Form
    {
        DataGridViewCell LinkedCell;

        /// <summary>
        /// Needs a PropertyArray<Property> typed object
        /// </summary>
        /// <param name="props"></param>
        public FormProperties(DataGridViewCell linkedCell)
        {
            InitializeComponent();

            LinkedCell = linkedCell;
            var data = (LinkedCell.Value as string).ParseJson();
            if (data?.Count > 0)
            {
                data[0].Keys.ToList().ForEach(column => dgv.Columns[dgv.Columns.Add(column, column)].ReadOnly = false);
                foreach (var row in data)
                {
                    int id = dgv.Rows.Add();
                    foreach (var kv in row)
                    {
                        dgv.Rows[id].Cells[kv.Key].Value = kv.Value;
                    }
                }
            }

            RefreshColumnsList();
            if(cbbColumnsSelected.Items.Count > 0)
                cbbColumnsSelected.SelectedIndex = 0;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            dgv.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => { var r = new Dictionary<string, string>(); row.Cells.Cast<DataGridViewTextBoxCell>().ToList().ForEach(c => r[c.OwningColumn.HeaderText] = c.Value as string); data.Add(r); });
            var json = data.ParseJson();
            LinkedCell.Value = json != "[]" ? data.ParseJson() : "";

            var col = btSave.BackColor;
            var t = btSave.Text;
            btSave.BackColor = Color.Yellow;
            btSave.Text = "SAVED ! OK";
            Application.DoEvents();
            Thread.Sleep(100);
            btSave.BackColor = col;
            btSave.Text = t;
        }
        private void btCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btColumnsAdd_Click(object sender, EventArgs e)
        {
            var name = $"new-column-{Common.Rnd.Next(10)}{Common.Rnd.Next(10)}";
            dgv.Columns.Add(name, name);
            RefreshColumnsList();
        }
        private void btColumnsRemove_Click(object sender, EventArgs e)
        {
            if (cbbColumnsSelected.SelectedIndex == -1) return;
            dgv.Columns.Remove(cbbColumnsSelected.SelectedItem as string);
            RefreshColumnsList();
        }

        void RefreshColumnsList()
        {
            var id = cbbColumnsSelected.SelectedIndex;
            cbbColumnsSelected.Items.Clear();
            dgv.Columns.Cast<DataGridViewColumn>().ToList().ForEach(c => cbbColumnsSelected.Items.Add(c.Name));
            if(cbbColumnsSelected.Items.Count > 0)
                cbbColumnsSelected.SelectedIndex = id;
        }
        private void cbbColumnsSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbColumnName.Text= cbbColumnsSelected.SelectedItem as string;
        }
        private void tbColumnName_KeyDown(object sender, KeyEventArgs e)
        {
            Application.DoEvents();
            var szid = cbbColumnsSelected.SelectedItem as string;
            if (szid != null && dgv.Columns[szid] != null)
            {
                dgv.Columns[szid].Name = tbColumnName.Text;
                dgv.Columns[tbColumnName.Text].HeaderText = tbColumnName.Text;
                cbbColumnsSelected.Items[cbbColumnsSelected.SelectedIndex] = tbColumnName.Text;
            }
        }

        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex == -1)
                {
                    cbbColumnsSelected.SelectedIndex = e.ColumnIndex;
                }
                else if (e.ColumnIndex > -1)
                {
                    void Edit()
                    {
                        new FormProperties(dgv[e.ColumnIndex, e.RowIndex]).ShowDialog(this);
                    }
                    void Create(string template)
                    {
                        dgv[e.ColumnIndex, e.RowIndex].Value = Common.GetTemplateJson(template);
                        Edit();
                    }
                    ToolStripDropDown pop = new ToolStripDropDown();
                    pop.Items.Add("Edit Properties", null, (_s, _e) => Edit());
                    pop.Items.Add(new ToolStripSeparator());
                    pop.Items.Add("Create Template : Results", null, (_s, _e) => Create("Result"));
                    pop.Items.Add("Create Template : Ingredients", null, (_s, _e) => Create("Ingredient"));
                    pop.Items.Add("Create Template : Units", null, (_s, _e) => Create("Unit"));
                    pop.Items.Add("Create Template : Effects", null, (_s, _e) => Create("Effect"));
                    pop.Items.Add(new ToolStripSeparator());
                    pop.Items.Add("Empty Content", null, (_s, _e) => dgv[e.ColumnIndex, e.RowIndex].Value = "");
                    Rectangle loc = dgv.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    pop.Font = Font;
                    pop.Show(this, new Point(dgv.Location.X + loc.X + e.X, dgv.Location.Y + loc.Y + e.Y));
                }
            }
        }
    }
}
