﻿namespace WindowsFormsApp17
{
    partial class FormCatch
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
            ((System.ComponentModel.ISupportInitialize)(this.Render)).BeginInit();
            this.SuspendLayout();
            // 
            // Render
            // 
            this.Render.Cursor = System.Windows.Forms.Cursors.Default;
            this.Render.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Render.Location = new System.Drawing.Point(0, 0);
            this.Render.Name = "Render";
            this.Render.Size = new System.Drawing.Size(512, 512);
            this.Render.TabIndex = 0;
            this.Render.TabStop = false;
            // 
            // FormCatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 512);
            this.Controls.Add(this.Render);
            this.Name = "FormCatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Render)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Render;
    }
}

