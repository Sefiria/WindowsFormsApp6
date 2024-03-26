namespace ToolActivityMaker
{
    partial class UCTime
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Hours = new System.Windows.Forms.NumericUpDown();
            this.lbSeparator = new System.Windows.Forms.Label();
            this.Minutes = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.Hours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Minutes)).BeginInit();
            this.SuspendLayout();
            // 
            // Hours
            // 
            this.Hours.Location = new System.Drawing.Point(3, 3);
            this.Hours.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.Hours.Name = "Hours";
            this.Hours.Size = new System.Drawing.Size(30, 20);
            this.Hours.TabIndex = 0;
            // 
            // lbSeparator
            // 
            this.lbSeparator.AutoSize = true;
            this.lbSeparator.Location = new System.Drawing.Point(39, 5);
            this.lbSeparator.Name = "lbSeparator";
            this.lbSeparator.Size = new System.Drawing.Size(10, 13);
            this.lbSeparator.TabIndex = 1;
            this.lbSeparator.Text = ":";
            // 
            // Minutes
            // 
            this.Minutes.Location = new System.Drawing.Point(55, 3);
            this.Minutes.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.Minutes.Name = "Minutes";
            this.Minutes.Size = new System.Drawing.Size(30, 20);
            this.Minutes.TabIndex = 0;
            // 
            // UCTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbSeparator);
            this.Controls.Add(this.Minutes);
            this.Controls.Add(this.Hours);
            this.Name = "UCTime";
            this.Size = new System.Drawing.Size(91, 25);
            ((System.ComponentModel.ISupportInitialize)(this.Hours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Minutes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown Hours;
        private System.Windows.Forms.Label lbSeparator;
        private System.Windows.Forms.NumericUpDown Minutes;
    }
}
