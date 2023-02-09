namespace WindowsFormsApp8
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
            this.listTiles = new System.Windows.Forms.ListBox();
            this.btBucket = new System.Windows.Forms.Button();
            this.btPen = new System.Windows.Forms.Button();
            this.Render = new System.Windows.Forms.PictureBox();
            this.imgUsedTool = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Render)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgUsedTool)).BeginInit();
            this.SuspendLayout();
            // 
            // listTiles
            // 
            this.listTiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.listTiles.FormattingEnabled = true;
            this.listTiles.Location = new System.Drawing.Point(841, 5);
            this.listTiles.Name = "listTiles";
            this.listTiles.Size = new System.Drawing.Size(268, 667);
            this.listTiles.TabIndex = 1;
            this.listTiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.listTiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.listTiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listTiles_MouseDown);
            // 
            // btBucket
            // 
            this.btBucket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btBucket.Image = global::WindowsFormsApp8.Properties.Resources.tool_bucket;
            this.btBucket.Location = new System.Drawing.Point(41, 3);
            this.btBucket.Name = "btBucket";
            this.btBucket.Size = new System.Drawing.Size(32, 32);
            this.btBucket.TabIndex = 2;
            this.btBucket.UseVisualStyleBackColor = true;
            this.btBucket.Click += new System.EventHandler(this.btBucket_Click);
            // 
            // btPen
            // 
            this.btPen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btPen.Image = global::WindowsFormsApp8.Properties.Resources.tool_pen;
            this.btPen.Location = new System.Drawing.Point(3, 3);
            this.btPen.Name = "btPen";
            this.btPen.Size = new System.Drawing.Size(32, 32);
            this.btPen.TabIndex = 2;
            this.btPen.UseVisualStyleBackColor = true;
            this.btPen.Click += new System.EventHandler(this.btPen_Click);
            // 
            // Render
            // 
            this.Render.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Render.Location = new System.Drawing.Point(0, 41);
            this.Render.Name = "Render";
            this.Render.Size = new System.Drawing.Size(837, 631);
            this.Render.TabIndex = 0;
            this.Render.TabStop = false;
            this.Render.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Render_MouseDown);
            this.Render.MouseLeave += new System.EventHandler(this.Render_MouseLeave);
            this.Render.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Render_MouseMove);
            this.Render.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Render_MouseUp);
            // 
            // imgUsedTool
            // 
            this.imgUsedTool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imgUsedTool.Location = new System.Drawing.Point(803, 5);
            this.imgUsedTool.Name = "imgUsedTool";
            this.imgUsedTool.Size = new System.Drawing.Size(32, 32);
            this.imgUsedTool.TabIndex = 3;
            this.imgUsedTool.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(1111, 671);
            this.Controls.Add(this.imgUsedTool);
            this.Controls.Add(this.btBucket);
            this.Controls.Add(this.btPen);
            this.Controls.Add(this.listTiles);
            this.Controls.Add(this.Render);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Render)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgUsedTool)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Render;
        private System.Windows.Forms.ListBox listTiles;
        private System.Windows.Forms.Button btPen;
        private System.Windows.Forms.Button btBucket;
        private System.Windows.Forms.PictureBox imgUsedTool;
    }
}

