namespace Route
{
    partial class FormRoute
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
            this.btTest = new System.Windows.Forms.Button();
            this.btEditMap = new System.Windows.Forms.Button();
            this.btEditTile = new System.Windows.Forms.Button();
            this.btEditSign = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(12, 12);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(75, 29);
            this.btTest.TabIndex = 0;
            this.btTest.Text = "Test";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btEditMap
            // 
            this.btEditMap.Location = new System.Drawing.Point(93, 12);
            this.btEditMap.Name = "btEditMap";
            this.btEditMap.Size = new System.Drawing.Size(75, 29);
            this.btEditMap.TabIndex = 1;
            this.btEditMap.Text = "EditMap";
            this.btEditMap.UseVisualStyleBackColor = true;
            this.btEditMap.Click += new System.EventHandler(this.btEditMap_Click);
            // 
            // btEditTile
            // 
            this.btEditTile.Location = new System.Drawing.Point(174, 12);
            this.btEditTile.Name = "btEditTile";
            this.btEditTile.Size = new System.Drawing.Size(75, 29);
            this.btEditTile.TabIndex = 2;
            this.btEditTile.Text = "EditTile";
            this.btEditTile.UseVisualStyleBackColor = true;
            this.btEditTile.Click += new System.EventHandler(this.btEditTile_Click);
            // 
            // btEditSign
            // 
            this.btEditSign.Location = new System.Drawing.Point(255, 11);
            this.btEditSign.Name = "btEditSign";
            this.btEditSign.Size = new System.Drawing.Size(75, 29);
            this.btEditSign.TabIndex = 3;
            this.btEditSign.Text = "EditSign";
            this.btEditSign.UseVisualStyleBackColor = true;
            this.btEditSign.Click += new System.EventHandler(this.btEditSign_Click);
            // 
            // FormRoute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 52);
            this.Controls.Add(this.btEditSign);
            this.Controls.Add(this.btEditTile);
            this.Controls.Add(this.btEditMap);
            this.Controls.Add(this.btTest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormRoute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btTest;
        private System.Windows.Forms.Button btEditMap;
        private System.Windows.Forms.Button btEditTile;
        private System.Windows.Forms.Button btEditSign;
    }
}

