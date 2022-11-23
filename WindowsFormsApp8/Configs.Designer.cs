namespace WindowsFormsApp8
{
    partial class Configs
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
            this.numZoom = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.lbHeight = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.lbWidth = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // numZoom
            // 
            this.numZoom.Location = new System.Drawing.Point(120, 138);
            this.numZoom.Margin = new System.Windows.Forms.Padding(6);
            this.numZoom.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numZoom.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numZoom.Name = "numZoom";
            this.numZoom.Size = new System.Drawing.Size(220, 36);
            this.numZoom.TabIndex = 17;
            this.numZoom.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 140);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 30);
            this.label1.TabIndex = 16;
            this.label1.Text = "Zoom";
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(195, 200);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(181, 35);
            this.btCancel.TabIndex = 14;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(13, 200);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(176, 35);
            this.btOK.TabIndex = 15;
            this.btOK.Text = "Save";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(120, 76);
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
            this.numHeight.Size = new System.Drawing.Size(220, 36);
            this.numHeight.TabIndex = 13;
            this.numHeight.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // lbHeight
            // 
            this.lbHeight.AutoSize = true;
            this.lbHeight.Location = new System.Drawing.Point(46, 78);
            this.lbHeight.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbHeight.Name = "lbHeight";
            this.lbHeight.Size = new System.Drawing.Size(78, 30);
            this.lbHeight.TabIndex = 12;
            this.lbHeight.Text = "Height";
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(120, 15);
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
            this.numWidth.Size = new System.Drawing.Size(220, 36);
            this.numWidth.TabIndex = 11;
            this.numWidth.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // lbWidth
            // 
            this.lbWidth.AutoSize = true;
            this.lbWidth.Location = new System.Drawing.Point(45, 17);
            this.lbWidth.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbWidth.Name = "lbWidth";
            this.lbWidth.Size = new System.Drawing.Size(71, 30);
            this.lbWidth.TabIndex = 10;
            this.lbWidth.Text = "Width";
            // 
            // Configs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 251);
            this.Controls.Add(this.numZoom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.lbHeight);
            this.Controls.Add(this.numWidth);
            this.Controls.Add(this.lbWidth);
            this.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "Configs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configs";
            ((System.ComponentModel.ISupportInitialize)(this.numZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NumericUpDown numZoom;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btCancel;
        public System.Windows.Forms.Button btOK;
        public System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.Label lbHeight;
        public System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Label lbWidth;
    }
}