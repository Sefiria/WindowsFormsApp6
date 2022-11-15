namespace WindowsFormsApp7
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
            this.Render = new System.Windows.Forms.PictureBox();
            this.RenderPal = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Render)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderPal)).BeginInit();
            this.SuspendLayout();
            // 
            // Render
            // 
            this.Render.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Render.BackColor = System.Drawing.Color.Black;
            this.Render.Location = new System.Drawing.Point(12, 12);
            this.Render.Name = "Render";
            this.Render.Size = new System.Drawing.Size(568, 487);
            this.Render.TabIndex = 0;
            this.Render.TabStop = false;
            this.Render.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Render_MouseDown);
            this.Render.MouseLeave += new System.EventHandler(this.Render_MouseLeave);
            this.Render.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Render_MouseMove);
            this.Render.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Render_MouseUp);
            // 
            // RenderPal
            // 
            this.RenderPal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RenderPal.BackColor = System.Drawing.Color.Black;
            this.RenderPal.Location = new System.Drawing.Point(586, 13);
            this.RenderPal.Name = "RenderPal";
            this.RenderPal.Size = new System.Drawing.Size(417, 487);
            this.RenderPal.TabIndex = 1;
            this.RenderPal.TabStop = false;
            this.RenderPal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderPal_MouseDown);
            this.RenderPal.MouseLeave += new System.EventHandler(this.RenderPal_MouseLeave);
            this.RenderPal.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderPal_MouseMove);
            this.RenderPal.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RenderPal_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(1015, 512);
            this.Controls.Add(this.RenderPal);
            this.Controls.Add(this.Render);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Render)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderPal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Render;
        private System.Windows.Forms.PictureBox RenderPal;
    }
}

