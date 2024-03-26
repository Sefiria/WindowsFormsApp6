namespace Script
{
    partial class _Main
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
            this.components = new System.ComponentModel.Container();
            this.Render = new System.Windows.Forms.PictureBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Render)).BeginInit();
            this.SuspendLayout();
            // 
            // Render
            // 
            this.Render.Location = new System.Drawing.Point(0, 0);
            this.Render.Name = "Render";
            this.Render.Size = new System.Drawing.Size(480, 480);
            this.Render.TabIndex = 0;
            this.Render.TabStop = false;
            this.Render.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Render_MouseDown);
            this.Render.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Render_MouseUp);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 0;
            this.toolTip.AutoPopDelay = 20000000;
            this.toolTip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(81)))), ((int)(((byte)(81)))));
            this.toolTip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.toolTip.InitialDelay = 0;
            this.toolTip.ReshowDelay = 0;
            this.toolTip.ToolTipTitle = "Debug Infos";
            this.toolTip.UseAnimation = false;
            this.toolTip.UseFading = false;
            // 
            // _Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 480);
            this.Controls.Add(this.Render);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this._Main_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.Render)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Render;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

