using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Defacto_rio
{
    public partial class FormPrototype<T> : Form where T : Prototype
    {
        List<T> DataType;
        int LinkedDgvListItemId = -1;

        T CreateTInstance(string name)
        {
            var result = (T)Activator.CreateInstance(typeof(T));
            result.name = name;
            return result;
        }

        public FormPrototype(List<T> DataType)
        {
            InitializeComponent();

            this.DataType = DataType;

            DataType.ForEach(item => listItems.Items.Add(item));
        }

        bool IsIdSelected => listItems.SelectedIndex > -1 && listItems.SelectedIndex < listItems.Items.Count;
        void SaveCurrentListItem()
        {
            if (LinkedDgvListItemId > -1 && LinkedDgvListItemId < listItems.Items.Count)
            {
                listItems.Items[LinkedDgvListItemId] = dgv.ToPrototype<T>();
            }
        }
        private void listItems_MouseDown(object sender, MouseEventArgs e)
        {
            void Edit()
            {
                SaveCurrentListItem();
                if (IsIdSelected)
                {
                    dgv.Visible = true;
                    dgv.Rows.Clear();
                    dgv.Edit(typeof(T), listItems.SelectedItem);
                    LinkedDgvListItemId = IsIdSelected ? listItems.SelectedIndex : -1;
                }
            }
            void Create()
            {
                SaveCurrentListItem();
                dgv.Visible = true;
                listItems.SelectedIndex = listItems.Items.Add(CreateTInstance($"new-{typeof(T).Name.ToLower()}"));
                Edit();
                Save();
            }
            void Remove()
            {
                SaveCurrentListItem();
                var id = listItems.SelectedIndex;
                listItems.Items.RemoveAt(id);
                dgv.Visible = false;
                dgv.Rows.Clear();
                Save();
            }


            Edit();
            if (e.Button == MouseButtons.Right)
            {
                ToolStripDropDown pop = new ToolStripDropDown();
                pop.Items.Add("Create New", null, (_s, _e) => Create());
                var remove = pop.Items.Add("Remove Selected", null, (_s, _e) => Remove()).Enabled = IsIdSelected;
                pop.Show(this, e.Location);
            }

            if (listItems.SelectedItem == null)
            {
                dgv.Visible = false;
                dgv.Rows.Clear();
            }
        }

        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
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

            if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex == 1)
            {
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

        void Save()
        {
            SaveCurrentListItem();
            DataType.Clear();
            listItems.Items.Cast<T>().ToList().ForEach(item => DataType.Add(item));
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Save();
            base.OnFormClosing(e);
        }
    }
}
