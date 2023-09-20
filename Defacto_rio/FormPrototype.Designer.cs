namespace Defacto_rio
{
    partial class FormPrototype<T>
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
            this.listItems = new System.Windows.Forms.ListBox();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // listItems
            // 
            this.listItems.Dock = System.Windows.Forms.DockStyle.Left;
            this.listItems.FormattingEnabled = true;
            this.listItems.ItemHeight = 25;
            this.listItems.Location = new System.Drawing.Point(0, 0);
            this.listItems.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.listItems.Name = "listItems";
            this.listItems.Size = new System.Drawing.Size(217, 639);
            this.listItems.TabIndex = 0;
            this.listItems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listItems_MouseDown);
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Key,
            this.Value});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(217, 0);
            this.dgv.Name = "dgv";
            this.dgv.Size = new System.Drawing.Size(897, 639);
            this.dgv.TabIndex = 1;
            this.dgv.Visible = false;
            this.dgv.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_CellMouseClick);
            // 
            // Key
            // 
            this.Key.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Key.HeaderText = "Key";
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            this.Key.Width = 256;
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // FormRecipes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 639);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.listItems);
            this.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "FormPrototype";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormDefaco.rio - Prototypes";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listItems;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
    }
}