namespace Editor
{
    partial class DoorsPropertiesMenu
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
            this.DoorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Locked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Layer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Exit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DoorInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DoorName,
            this.Locked,
            this.Layer,
            this.ID,
            this.X,
            this.Y,
            this.Exit,
            this.DoorInfo});
            this.DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGV.Location = new System.Drawing.Point(0, 0);
            this.DGV.Name = "DGV";
            this.DGV.RowHeadersVisible = false;
            this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV.Size = new System.Drawing.Size(493, 183);
            this.DGV.TabIndex = 2;
            this.DGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellContentClick);
            this.DGV.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellValueChanged);
            // 
            // DoorName
            // 
            this.DoorName.HeaderText = "Name";
            this.DoorName.Name = "DoorName";
            this.DoorName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DoorName.Width = 150;
            // 
            // Locked
            // 
            this.Locked.FalseValue = "false";
            this.Locked.HeaderText = "Locked";
            this.Locked.IndeterminateValue = "null";
            this.Locked.Name = "Locked";
            this.Locked.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Locked.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Locked.TrueValue = "true";
            this.Locked.Width = 50;
            // 
            // Layer
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            this.Layer.DefaultCellStyle = dataGridViewCellStyle1;
            this.Layer.HeaderText = "Layer";
            this.Layer.Name = "Layer";
            this.Layer.ReadOnly = true;
            this.Layer.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Layer.Width = 50;
            // 
            // ID
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            this.ID.DefaultCellStyle = dataGridViewCellStyle2;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.Width = 30;
            // 
            // X
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro;
            this.X.DefaultCellStyle = dataGridViewCellStyle3;
            this.X.HeaderText = "X";
            this.X.Name = "X";
            this.X.ReadOnly = true;
            this.X.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.X.Width = 30;
            // 
            // Y
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro;
            this.Y.DefaultCellStyle = dataGridViewCellStyle4;
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            this.Y.ReadOnly = true;
            this.Y.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Y.Width = 30;
            // 
            // Exit
            // 
            this.Exit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Exit.HeaderText = "Exit";
            this.Exit.Name = "Exit";
            this.Exit.ReadOnly = true;
            this.Exit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Exit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // DoorInfo
            // 
            this.DoorInfo.HeaderText = "DoorInfo";
            this.DoorInfo.Name = "DoorInfo";
            this.DoorInfo.ReadOnly = true;
            this.DoorInfo.Visible = false;
            // 
            // DoorsPropertiesMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 183);
            this.Controls.Add(this.DGV);
            this.Name = "DoorsPropertiesMenu";
            this.Text = "DoorsPropertiesMenu";
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn DoorName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Locked;
        private System.Windows.Forms.DataGridViewTextBoxColumn Layer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.DataGridViewButtonColumn Exit;
        private System.Windows.Forms.DataGridViewTextBoxColumn DoorInfo;
    }
}