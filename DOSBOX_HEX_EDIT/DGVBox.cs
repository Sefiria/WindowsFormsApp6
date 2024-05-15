using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DOSBOX_HEX_EDIT
{
    public partial class DGVBox : Form
    {
        public DGVBox(DataTable db)
        {
            InitializeComponent();
            DGV.DataSource = db;
            CorrectWindowSize();
        }
        public void CorrectWindowSize()
        {
            int width = WinObjFunctions.CountGridWidth(DGV);
            ClientSize = new Size(width, ClientSize.Height);
        }
        public static class WinObjFunctions
        {
            public static int CountGridWidth(DataGridView dgv)
            {
                int width = 0;
                foreach (DataGridViewColumn column in dgv.Columns)
                    if (column.Visible == true)
                        width += column.Width;
                return width += 20;
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            this.Validate();
            DGV.BindingContext[DGV.DataSource].EndCurrentEdit();
            base.OnClosed(e);
        }
    }
}
