using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Defacto_rio.PropertyTypes;

namespace Defacto_rio
{
    public partial class FormRecipes : Form
    {
        int last_selected_id = -1;
        public FormRecipes()
        {
            InitializeComponent();

            Data.Recipes.ForEach(item => listItems.Items.Add(item));
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
                    listItems.SelectedIndex = listItems.Items.Add("new-recipe");
                    Data.Recipes.Insert(listItems.SelectedIndex, new RecipePrototype());
                    dgv.Rows.Clear();
                    dgv.Create(typeof(RecipePrototype));
                    dgv.Rows[0].Cells[1].Value = "new-recipe";
                });
                var edit = pop.Items.Add("Edit", null, (_s, _e) =>
                {
                    dgv.Visible = btSaveItem.Enabled = true;
                    dgv.Rows.Clear();
                    dgv.Edit(typeof(RecipePrototype), Data.Recipes[listItems.SelectedIndex]);
                });
                edit.Enabled = idselected;
                var remove = pop.Items.Add("Remove", null, (_s, _e) =>
                {
                    var id = listItems.SelectedIndex;
                    listItems.Items.RemoveAt(id);
                    Data.Recipes.RemoveAt(id);
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
                MessageBox.Show(this, "No recipe selected in the list !", "Defacto.rio - Recipes - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Data.Recipes[listItems.SelectedIndex].name = dgv.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => row.Cells[0].Value as string == "name")?.Cells[1].Value as string;
            if (Data.Recipes.Count <= listItems.SelectedIndex)
                Data.Recipes.Add(new RecipePrototype());
            dgv.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => typeof(RecipePrototype).GetFields().FirstOrDefault(field => field.Name == row.Cells[0].Value as string)?.SetValue(Data.Recipes[listItems.SelectedIndex], row.Cells[1].Value as string));
            listItems.Items[listItems.SelectedIndex] = dgv[1, 0].Value;
        }

        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ToolStripDropDown pop = new ToolStripDropDown();
                var edit = pop.Items.Add("Edit Properties", null, (_s, _e) =>
                {
                    new FormProperties(dgv[e.ColumnIndex, e.RowIndex]).ShowDialog(this);
                });
                Rectangle loc = dgv.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                pop.Show(this, new Point(dgv.Location.X + loc.X + e.X, dgv.Location.Y + loc.Y + e.Y));
            }
        }
    }
}
