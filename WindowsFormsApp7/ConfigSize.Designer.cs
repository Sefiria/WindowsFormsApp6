namespace WindowsFormsApp7
{
    partial class FormConfigSize
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
            this.lbWidth = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.lbHeight = new System.Windows.Forms.Label();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.numTileSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lbResultsize = new System.Windows.Forms.Label();
            this.lbResultsizeValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTileSize)).BeginInit();
            this.SuspendLayout();
            // 
            // lbWidth
            // 
            this.lbWidth.AutoSize = true;
            this.lbWidth.Location = new System.Drawing.Point(45, 111);
            this.lbWidth.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbWidth.Name = "lbWidth";
            this.lbWidth.Size = new System.Drawing.Size(63, 25);
            this.lbWidth.TabIndex = 0;
            this.lbWidth.Text = "Width";
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(130, 109);
            this.numWidth.Margin = new System.Windows.Forms.Padding(6);
            this.numWidth.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numWidth.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(220, 32);
            this.numWidth.TabIndex = 1;
            this.numWidth.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numWidth.ValueChanged += new System.EventHandler(this.ResetResultSize);
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(130, 170);
            this.numHeight.Margin = new System.Windows.Forms.Padding(6);
            this.numHeight.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numHeight.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(220, 32);
            this.numHeight.TabIndex = 3;
            this.numHeight.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numHeight.ValueChanged += new System.EventHandler(this.ResetResultSize);
            // 
            // lbHeight
            // 
            this.lbHeight.AutoSize = true;
            this.lbHeight.Location = new System.Drawing.Point(45, 172);
            this.lbHeight.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbHeight.Name = "lbHeight";
            this.lbHeight.Size = new System.Drawing.Size(68, 25);
            this.lbHeight.TabIndex = 2;
            this.lbHeight.Text = "Height";
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(12, 294);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(176, 35);
            this.btOK.TabIndex = 4;
            this.btOK.Text = "Save";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(194, 294);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(181, 35);
            this.btCancel.TabIndex = 4;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // numTileSize
            // 
            this.numTileSize.Location = new System.Drawing.Point(130, 232);
            this.numTileSize.Margin = new System.Windows.Forms.Padding(6);
            this.numTileSize.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numTileSize.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numTileSize.Name = "numTileSize";
            this.numTileSize.Size = new System.Drawing.Size(220, 32);
            this.numTileSize.TabIndex = 6;
            this.numTileSize.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numTileSize.ValueChanged += new System.EventHandler(this.ResetResultSize);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 234);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tile size";
            // 
            // lbResultsize
            // 
            this.lbResultsize.AutoSize = true;
            this.lbResultsize.Location = new System.Drawing.Point(15, 28);
            this.lbResultsize.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbResultsize.Name = "lbResultsize";
            this.lbResultsize.Size = new System.Drawing.Size(99, 25);
            this.lbResultsize.TabIndex = 7;
            this.lbResultsize.Text = "Result size";
            // 
            // lbResultsizeValue
            // 
            this.lbResultsizeValue.AutoSize = true;
            this.lbResultsizeValue.Location = new System.Drawing.Point(126, 28);
            this.lbResultsizeValue.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbResultsizeValue.Name = "lbResultsizeValue";
            this.lbResultsizeValue.Size = new System.Drawing.Size(51, 25);
            this.lbResultsizeValue.TabIndex = 8;
            this.lbResultsizeValue.Text = "0 x 0";
            // 
            // FormConfigSize
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(387, 341);
            this.Controls.Add(this.lbResultsizeValue);
            this.Controls.Add(this.lbResultsize);
            this.Controls.Add(this.numTileSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.lbHeight);
            this.Controls.Add(this.numWidth);
            this.Controls.Add(this.lbWidth);
            this.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormConfigSize";
            this.Text = "Config Size";
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTileSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbWidth;
        public System.Windows.Forms.NumericUpDown numWidth;
        public System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.Label lbHeight;
        public System.Windows.Forms.Button btOK;
        public System.Windows.Forms.Button btCancel;
        public System.Windows.Forms.NumericUpDown numTileSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbResultsize;
        private System.Windows.Forms.Label lbResultsizeValue;
    }
}