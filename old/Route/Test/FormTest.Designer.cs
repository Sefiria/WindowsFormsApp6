namespace Test
{
    partial class FormTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTest));
            this.Render = new System.Windows.Forms.PictureBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tsbtTrafficFlow = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbtReset = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.Render)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Render
            // 
            this.Render.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Render.Location = new System.Drawing.Point(0, 0);
            this.Render.Name = "Render";
            this.Render.Size = new System.Drawing.Size(800, 785);
            this.Render.TabIndex = 0;
            this.Render.TabStop = false;
            this.Render.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Map_MouseDown);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.tsbtTrafficFlow,
            this.tsbtReset});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(800, 25);
            this.toolStrip.TabIndex = 5;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(37, 22);
            this.toolStripButton2.Text = "Load";
            this.toolStripButton2.Click += new System.EventHandler(this.MenuLoad_Click);
            // 
            // tsbtTrafficFlow
            // 
            this.tsbtTrafficFlow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtTrafficFlow.Image = ((System.Drawing.Image)(resources.GetObject("tsbtTrafficFlow.Image")));
            this.tsbtTrafficFlow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtTrafficFlow.Name = "tsbtTrafficFlow";
            this.tsbtTrafficFlow.Size = new System.Drawing.Size(80, 22);
            this.tsbtTrafficFlow.Text = "Traffic Flow";
            // 
            // tsbtReset
            // 
            this.tsbtReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtReset.Image = ((System.Drawing.Image)(resources.GetObject("tsbtReset.Image")));
            this.tsbtReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtReset.Name = "tsbtReset";
            this.tsbtReset.Size = new System.Drawing.Size(39, 22);
            this.tsbtReset.Text = "Reset";
            this.tsbtReset.Click += new System.EventHandler(this.tsbtReset_Click);
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 785);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.Render);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "FormTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Map_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Render)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Render;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripDropDownButton tsbtTrafficFlow;
        private System.Windows.Forms.ToolStripButton tsbtReset;
    }
}

