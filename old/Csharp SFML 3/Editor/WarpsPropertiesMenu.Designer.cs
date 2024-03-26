namespace Editor
{
    partial class WarpsPropertiesMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Exit_LevelEnterID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WarpInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.Position,
            this.ID,
            this.Exit_LevelEnterID,
            this.LevelName,
            this.WarpInfo});
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.Location = new System.Drawing.Point(0, 0);
            this.DGV.Name = "DGV";
            this.DGV.RowHeadersVisible = false;
            this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV.Size = new System.Drawing.Size(493, 183);
            this.DGV.TabIndex = 2;
            this.DGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_CellMouseClick);
            this.DGV.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellValueChanged);
            // 
            // Type
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            this.Type.DefaultCellStyle = dataGridViewCellStyle1;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Position
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            this.Position.DefaultCellStyle = dataGridViewCellStyle2;
            this.Position.HeaderText = "Position";
            this.Position.Name = "Position";
            this.Position.ReadOnly = true;
            this.Position.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ID
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro;
            this.ID.DefaultCellStyle = dataGridViewCellStyle3;
            this.ID.HeaderText = "Warp ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.Width = 70;
            // 
            // Exit_LevelEnterID
            // 
            this.Exit_LevelEnterID.HeaderText = "Exit ID";
            this.Exit_LevelEnterID.Name = "Exit_LevelEnterID";
            this.Exit_LevelEnterID.ReadOnly = true;
            this.Exit_LevelEnterID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Exit_LevelEnterID.Width = 70;
            // 
            // LevelName
            // 
            this.LevelName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro;
            this.LevelName.DefaultCellStyle = dataGridViewCellStyle4;
            this.LevelName.HeaderText = "Exit Level Name";
            this.LevelName.Name = "LevelName";
            this.LevelName.ReadOnly = true;
            this.LevelName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // WarpInfo
            // 
            this.WarpInfo.HeaderText = "WarpInfo";
            this.WarpInfo.Name = "WarpInfo";
            this.WarpInfo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.WarpInfo.Visible = false;
            // 
            // WarpsPropertiesMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 183);
            this.Controls.Add(this.DGV);
            this.Name = "WarpsPropertiesMenu";
            this.Text = "WarpsPropertiesMenu";
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Exit_LevelEnterID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn WarpInfo;
    }
}