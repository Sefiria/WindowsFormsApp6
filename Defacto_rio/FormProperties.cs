using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using static Defacto_rio.PropertyTypes;

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
            data[0].Keys.ToList().ForEach(column => dgv.Columns.Add(column, column));
            foreach(var row in data)
            {
                int id = dgv.Rows.Add();
                foreach (var kv in row)
                {
                    dgv.Rows[id].Cells[kv.Key].Value = kv.Value;
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            dgv.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => { var r = new Dictionary<string, string>(); row.Cells.Cast<DataGridViewTextBoxCell>().ToList().ForEach(c => r[c.OwningColumn.HeaderText] = c.Value as string); data.Add(r); });
            LinkedCell.Value = data.ParseJson();

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
    }
}
