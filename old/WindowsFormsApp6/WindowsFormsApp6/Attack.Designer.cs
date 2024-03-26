namespace WindowsFormsApp6
{
    partial class Attack
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
            this.Render = new System.Windows.Forms.PictureBox();
            this.lbTimeLeft = new System.Windows.Forms.Label();
            this.lbScoreUser1 = new System.Windows.Forms.Label();
            this.lbScoreUser2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Render)).BeginInit();
            this.SuspendLayout();
            // 
            // Render
            // 
            this.Render.BackColor = System.Drawing.Color.DimGray;
            this.Render.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Render.Location = new System.Drawing.Point(0, 0);
            this.Render.Name = "Render";
            this.Render.Size = new System.Drawing.Size(690, 370);
            this.Render.TabIndex = 0;
            this.Render.TabStop = false;
            this.Render.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Render_MouseDown);
            this.Render.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Render_MouseMove);
            // 
            // lbTimeLeft
            // 
            this.lbTimeLeft.AutoSize = true;
            this.lbTimeLeft.BackColor = System.Drawing.Color.DimGray;
            this.lbTimeLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTimeLeft.ForeColor = System.Drawing.Color.White;
            this.lbTimeLeft.Location = new System.Drawing.Point(13, 13);
            this.lbTimeLeft.Name = "lbTimeLeft";
            this.lbTimeLeft.Size = new System.Drawing.Size(71, 17);
            this.lbTimeLeft.TabIndex = 1;
            this.lbTimeLeft.Text = "TimeLeft";
            // 
            // lbScoreUser1
            // 
            this.lbScoreUser1.AutoSize = true;
            this.lbScoreUser1.BackColor = System.Drawing.Color.DimGray;
            this.lbScoreUser1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbScoreUser1.ForeColor = System.Drawing.Color.White;
            this.lbScoreUser1.Location = new System.Drawing.Point(13, 30);
            this.lbScoreUser1.Name = "lbScoreUser1";
            this.lbScoreUser1.Size = new System.Drawing.Size(73, 13);
            this.lbScoreUser1.TabIndex = 2;
            this.lbScoreUser1.Text = "ScoreUser1";
            // 
            // lbScoreUser2
            // 
            this.lbScoreUser2.AutoSize = true;
            this.lbScoreUser2.BackColor = System.Drawing.Color.DimGray;
            this.lbScoreUser2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbScoreUser2.ForeColor = System.Drawing.Color.White;
            this.lbScoreUser2.Location = new System.Drawing.Point(12, 47);
            this.lbScoreUser2.Name = "lbScoreUser2";
            this.lbScoreUser2.Size = new System.Drawing.Size(73, 13);
            this.lbScoreUser2.TabIndex = 3;
            this.lbScoreUser2.Text = "ScoreUser2";
            // 
            // Attack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 370);
            this.Controls.Add(this.lbScoreUser2);
            this.Controls.Add(this.lbScoreUser1);
            this.Controls.Add(this.lbTimeLeft);
            this.Controls.Add(this.Render);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "Attack";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Render)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox Render;
        private System.Windows.Forms.Label lbTimeLeft;
        private System.Windows.Forms.Label lbScoreUser1;
        private System.Windows.Forms.Label lbScoreUser2;
    }
}

