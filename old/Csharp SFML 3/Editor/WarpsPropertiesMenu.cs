using Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor
{
    public partial class WarpsPropertiesMenu : Form
    {
        Level level;

        public WarpsPropertiesMenu(Level _level)
        {
            InitializeComponent();

            level = _level;

            var listIDRed = new List<int>();
            foreach (var warp in level.warps)
            {
                var row = new DataGridViewRow();
                row.CreateCells(DGV, warp.type, warp.TilePosition, warp.Enter_WarpID, warp.Exit_LevelEnterID, warp.Exit_LevelName, warp);
                DGV.Rows.Add(row);
                DGV.Rows[row.Index].Cells["ID"].ReadOnly = warp.type != Warp.WarpType.Enter;
                DGV.Rows[row.Index].Cells["ID"].Style.BackColor = (warp.type != Warp.WarpType.Enter ? Color.Gainsboro : (int.Parse(DGV.Rows[row.Index].Cells["ID"].Value.ToString()) == 0 ? Color.Red : Color.LightSkyBlue));
                DGV.Rows[row.Index].Cells["Exit_LevelEnterID"].ReadOnly = warp.type != Warp.WarpType.Exit;
                DGV.Rows[row.Index].Cells["Exit_LevelEnterID"].Style.BackColor = (warp.type != Warp.WarpType.Exit ? Color.Gainsboro : (int.Parse(DGV.Rows[row.Index].Cells["ID"].Value.ToString()) == 0 ? Color.Red : Color.LightSkyBlue));
                DGV.Rows[row.Index].Cells["LevelName"].ReadOnly = warp.type != Warp.WarpType.Exit;
                DGV.Rows[row.Index].Cells["LevelName"].Style.BackColor = (warp.type != Warp.WarpType.Exit ? Color.Gainsboro : (string.IsNullOrWhiteSpace(DGV.Rows[row.Index].Cells["LevelName"].Value?.ToString()) ? Color.Red : Color.LightSkyBlue));

                if (!listIDRed.Contains(int.Parse(DGV.Rows[row.Index].Cells["ID"].Value.ToString())))
                {
                    listIDRed.Add(int.Parse(DGV.Rows[row.Index].Cells["ID"].Value.ToString()));
                    DGV_CellValueChanged(null, new DataGridViewCellEventArgs(DGV.Columns["ID"].Index, row.Index));
                }
            }

            DGV.SelectionChanged += DGV_SelectionChanged;
            FormClosed += WarpsPropertiesMenu_FormClosed;
        }
        public WarpsPropertiesMenu(Level _level, Warp WarpToEdit)
        {
            InitializeComponent();

            level = _level;
            var row = new DataGridViewRow();
            row.CreateCells(DGV, WarpToEdit.type, WarpToEdit.TilePosition, WarpToEdit.Enter_WarpID, WarpToEdit.Exit_LevelEnterID, WarpToEdit.Exit_LevelName, WarpToEdit);
            DGV.Rows.Add(row);
            DGV.Rows[row.Index].Cells["ID"].ReadOnly = WarpToEdit.type != Warp.WarpType.Enter;
            DGV.Rows[row.Index].Cells["ID"].Style.BackColor = WarpToEdit.type != Warp.WarpType.Enter ? Color.Gainsboro : Color.LightSkyBlue;
            DGV.Rows[row.Index].Cells["Exit_LevelEnterID"].ReadOnly = WarpToEdit.type != Warp.WarpType.Exit;
            DGV.Rows[row.Index].Cells["Exit_LevelEnterID"].Style.BackColor = WarpToEdit.type != Warp.WarpType.Exit ? Color.Gainsboro : Color.LightSkyBlue;
            DGV.Rows[row.Index].Cells["LevelName"].ReadOnly = WarpToEdit.type != Warp.WarpType.Exit;
            DGV.Rows[row.Index].Cells["LevelName"].Style.BackColor = WarpToEdit.type != Warp.WarpType.Exit ? Color.Gainsboro : Color.LightSkyBlue;

            DGV.SelectionChanged += DGV_SelectionChanged;
            FormClosed += WarpsPropertiesMenu_FormClosed;
        }
        private void WarpsPropertiesMenu_FormClosed(object sender, EventArgs e)
        {
            level.WarpSelection = 0;
        }

        private void DGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= DGV.RowCount)
                return;

            var warp = (DGV.Rows[e.RowIndex].Cells["WarpInfo"].Value as Warp);

            if (DGV.Columns[e.ColumnIndex].Name == "ID")
            {
                int id = -1;
                int.TryParse(DGV.Rows[e.RowIndex].Cells["ID"].Value.ToString(), out id);
                if (id != -1)
                {
                    if (id < 0) id = 0;
                    warp.Enter_WarpID = id;
                    DGV.Rows[e.RowIndex].Cells["ID"].Style.BackColor = (id == 0 ? Color.Red : (level.WarpEnterIDAlreadyExists(warp) ? Color.OrangeRed : Color.LightSkyBlue));
                }
                else
                {
                    DGV.Rows[e.RowIndex].Cells["ID"].Value = "0";
                    DGV.Rows[e.RowIndex].Cells["ID"].Style.BackColor = Color.Red;
                }
            }
            else
            if (DGV.Columns[e.ColumnIndex].Name == "Exit_LevelEnterID")
            {
                int id = -1;
                int.TryParse(DGV.Rows[e.RowIndex].Cells["Exit_LevelEnterID"].Value.ToString(), out id);
                if (id != -1)
                {
                    if (id < 0) id = 0;
                    warp.Exit_LevelEnterID = id;
                    DGV.Rows[e.RowIndex].Cells["Exit_LevelEnterID"].Style.BackColor = (id == 0 ? Color.Red : Color.LightSkyBlue);
                }
                else
                {
                    DGV.Rows[e.RowIndex].Cells["Exit_LevelEnterID"].Value = "0";
                    DGV.Rows[e.RowIndex].Cells["Exit_LevelEnterID"].Style.BackColor = Color.Red;
                }
            }
            else
            if (DGV.Columns[e.ColumnIndex].Name == "LevelName")
            {
                warp.Exit_LevelName = DGV.Rows[e.RowIndex].Cells["LevelName"].Value.ToString();
                DGV.Rows[e.RowIndex].Cells["LevelName"].Style.BackColor = (string.IsNullOrWhiteSpace(DGV.Rows[e.RowIndex].Cells["LevelName"].Value?.ToString()) ? Color.Red : Color.LightSkyBlue);
            }
        }

        private void DGV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (DGV.Columns[e.ColumnIndex].Name == "ID" && e.Button == MouseButtons.Right)
            {
                var warp = (DGV.Rows[e.RowIndex].Cells["WarpInfo"].Value as Warp);
                if (level.WarpEnterIDAlreadyExists(warp))
                {
                    warp.Enter_WarpID = level.WarpGetAvailableID();
                    DGV.Rows[e.RowIndex].Cells["ID"].Value = warp.Enter_WarpID;
                    DGV.Rows[e.RowIndex].Cells["ID"].Style.BackColor = Color.LightSkyBlue;
                }
            }
        }
        private void DGV_SelectionChanged(object sender, EventArgs e)
        {
            if (DGV.SelectedRows.Count == 0)
                return;

            var warpCell = (DGV.SelectedRows[0].Cells["WarpInfo"].Value as Warp);

            level.WarpSelection = warpCell.Enter_WarpID;
            var warp = level.GetWarp(level.WarpSelection);
            level.EnsureTileVisible(warp.TilePosition.X, warp.TilePosition.Y);
        }
    }
}
