namespace WindowsFormsApp9
{
    partial class Form1
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
            this.btTile = new System.Windows.Forms.Button();
            this.btMap = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btTile
            // 
            this.btTile.Location = new System.Drawing.Point(18, 18);
            this.btTile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btTile.Name = "btTile";
            this.btTile.Size = new System.Drawing.Size(112, 35);
            this.btTile.TabIndex = 0;
            this.btTile.Text = "Tile";
            this.btTile.UseVisualStyleBackColor = true;
            this.btTile.Click += new System.EventHandler(this.btTile_Click);
            // 
            // btMap
            // 
            this.btMap.Location = new System.Drawing.Point(18, 63);
            this.btMap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btMap.Name = "btMap";
            this.btMap.Size = new System.Drawing.Size(112, 35);
            this.btMap.TabIndex = 0;
            this.btMap.Text = "Map";
            this.btMap.UseVisualStyleBackColor = true;
            this.btMap.Click += new System.EventHandler(this.btMap_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(146, 112);
            this.Controls.Add(this.btMap);
            this.Controls.Add(this.btTile);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btTile;
        private System.Windows.Forms.Button btMap;
    }
}

