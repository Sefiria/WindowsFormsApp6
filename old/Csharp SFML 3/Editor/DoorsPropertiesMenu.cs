using Framework;
using Framework.Entities._Entity._Material;
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
    public partial class DoorsPropertiesMenu : Form
    {
        Level level;

        public DoorsPropertiesMenu(Level _level)
        {
            InitializeComponent();

            level = _level;
            foreach (var door in level.doors)
            {
                var row = new DataGridViewRow();
                row.CreateCells(DGV, door.Name, door.Locked, door.Layer, door.ID, door.position.X, door.position.Y, door.Exit, door);
                DGV.Rows.Add(row);
            }

            DGV.SelectionChanged += DGV_SelectionChanged;
            FormClosed += DoorsPropertiesMenu_FormClosed;
        }
        public DoorsPropertiesMenu(Level _level, Door DoorToEdit)
        {
            InitializeComponent();

            level = _level;
            var row = new DataGridViewRow();
            row.CreateCells(DGV, DoorToEdit.Name, DoorToEdit.Locked, DoorToEdit.Layer, DoorToEdit.ID, DoorToEdit.position.X, DoorToEdit.position.Y, DoorToEdit.Exit, DoorToEdit);
            DGV.Rows.Add(row);

            DGV.SelectionChanged += DGV_SelectionChanged;
            FormClosed += DoorsPropertiesMenu_FormClosed;
        }
        private void DoorsPropertiesMenu_FormClosed(object sender, EventArgs e)
        {
            level.DoorSelection = (0, -1, -1);
        }

        private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Columns[e.ColumnIndex].Name == "Exit")
            {
                var form = new Form();
                var cbb = new ComboBox();
                cbb.Parent = form;
                cbb.Dock = DockStyle.Fill;
                cbb.DropDownStyle = ComboBoxStyle.DropDownList;
                foreach (var doorIT in level.doors)
                {
                    if (doorIT.Name == DGV.Rows[e.RowIndex].Cells["DoorName"].Value.ToString())
                        continue;

                    var doorFound = Door.FirstOrDefault(level.doors, doorIT.Layer, doorIT.GetPositionAsPoint().X, doorIT.GetPositionAsPoint().Y, doorIT.ID);
                    if(doorFound != null)
                        cbb.Items.Add(doorFound);
                }
                cbb.SelectedIndexChanged += delegate { DGV.Rows[e.RowIndex].Cells["Exit"].Value = cbb.SelectedItem; (DGV.Rows[e.RowIndex].Cells["DoorInfo"].Value as Door).Exit = cbb.SelectedItem as Door; };
                form.ClientSize = cbb.Size;
                form.ShowDialog();
            }
        }

        private void DGV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (DGV.Columns[e.ColumnIndex].Name == "DoorName")
                (DGV.Rows[e.RowIndex].Cells["DoorInfo"].Value as Door).Name = DGV.Rows[e.RowIndex].Cells["DoorName"].Value.ToString();
        }
        private void DGV_SelectionChanged(object sender, EventArgs e)
        {
            if (DGV.SelectedRows.Count == 0)
                return;

            var doorCell = (DGV.SelectedRows[0].Cells["DoorInfo"].Value as Door);

            level.DoorSelection = (doorCell.ID, (int)doorCell.position.X, (int)doorCell.position.Y);
            var door = level.GetWarp(level.DoorSelection.X, level.DoorSelection.Y);
            level.EnsureTileVisible(door.TilePosition.X, door.TilePosition.Y);
        }
    }
}
