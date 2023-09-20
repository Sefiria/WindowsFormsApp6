namespace Defacto_rio
{
    partial class FormProperties
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
            this.dgv = new System.Windows.Forms.DataGridView();
            this.btSave = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.btColumnsAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btColumnsRemove = new System.Windows.Forms.Button();
            this.cbbColumnsSelected = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbColumnName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToOrderColumns = true;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Location = new System.Drawing.Point(0, 64);
            this.dgv.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(891, 392);
            this.dgv.TabIndex = 0;
            this.dgv.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_CellMouseClick);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSave.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btSave.Location = new System.Drawing.Point(0, 456);
            this.btSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(430, 60);
            this.btSave.TabIndex = 1;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btCancel.Location = new System.Drawing.Point(482, 456);
            this.btCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(407, 60);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btColumnsAdd
            // 
            this.btColumnsAdd.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btColumnsAdd.Location = new System.Drawing.Point(98, 14);
            this.btColumnsAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btColumnsAdd.Name = "btColumnsAdd";
            this.btColumnsAdd.Size = new System.Drawing.Size(106, 40);
            this.btColumnsAdd.TabIndex = 1;
            this.btColumnsAdd.Text = "Add";
            this.btColumnsAdd.UseVisualStyleBackColor = true;
            this.btColumnsAdd.Click += new System.EventHandler(this.btColumnsAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Columns :";
            // 
            // btColumnsRemove
            // 
            this.btColumnsRemove.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btColumnsRemove.Location = new System.Drawing.Point(768, 14);
            this.btColumnsRemove.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btColumnsRemove.Name = "btColumnsRemove";
            this.btColumnsRemove.Size = new System.Drawing.Size(106, 40);
            this.btColumnsRemove.TabIndex = 1;
            this.btColumnsRemove.Text = "Remove";
            this.btColumnsRemove.UseVisualStyleBackColor = true;
            this.btColumnsRemove.Click += new System.EventHandler(this.btColumnsRemove_Click);
            // 
            // cbbColumnsSelected
            // 
            this.cbbColumnsSelected.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbColumnsSelected.FormattingEnabled = true;
            this.cbbColumnsSelected.Location = new System.Drawing.Point(296, 22);
            this.cbbColumnsSelected.Name = "cbbColumnsSelected";
            this.cbbColumnsSelected.Size = new System.Drawing.Size(151, 29);
            this.cbbColumnsSelected.TabIndex = 4;
            this.cbbColumnsSelected.SelectedIndexChanged += new System.EventHandler(this.cbbColumnsSelected_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Selected :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(453, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "Name :";
            // 
            // tbColumnName
            // 
            this.tbColumnName.Location = new System.Drawing.Point(518, 21);
            this.tbColumnName.Name = "tbColumnName";
            this.tbColumnName.Size = new System.Drawing.Size(243, 29);
            this.tbColumnName.TabIndex = 5;
            this.tbColumnName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbColumnName_KeyDown);
            // 
            // FormProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 517);
            this.Controls.Add(this.tbColumnName);
            this.Controls.Add(this.cbbColumnsSelected);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btColumnsRemove);
            this.Controls.Add(this.btColumnsAdd);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.dgv);
            this.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormProperties";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btColumnsAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btColumnsRemove;
        private System.Windows.Forms.ComboBox cbbColumnsSelected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbColumnName;
    }
}