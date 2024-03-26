namespace EditMap
{
    partial class FormEditMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditMap));
            this.Render = new System.Windows.Forms.PictureBox();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.btPen = new System.Windows.Forms.Button();
            this.btFill = new System.Windows.Forms.Button();
            this.btEraser = new System.Windows.Forms.Button();
            this.CurrentTileView = new System.Windows.Forms.PictureBox();
            this.listTiles = new System.Windows.Forms.ListView();
            this.btClear = new System.Windows.Forms.Button();
            this.cbShowDots = new System.Windows.Forms.CheckBox();
            this.infoPathDotsNone = new System.Windows.Forms.PictureBox();
            this.infoPathDotsIn = new System.Windows.Forms.PictureBox();
            this.infoPathDotsWay = new System.Windows.Forms.PictureBox();
            this.infoPathDotsOut = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Render)).BeginInit();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentTileView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPathDotsNone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPathDotsIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPathDotsWay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPathDotsOut)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Render
            // 
            this.Render.Location = new System.Drawing.Point(12, 66);
            this.Render.Name = "Render";
            this.Render.Size = new System.Drawing.Size(400, 400);
            this.Render.TabIndex = 0;
            this.Render.TabStop = false;
            this.Render.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Render_MouseDown);
            this.Render.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Render_MouseMove);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(617, 25);
            this.toolStrip.TabIndex = 4;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton1.Text = "New";
            this.toolStripButton1.Click += new System.EventHandler(this.MenuNew_Click);
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
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton3.Text = "Save";
            this.toolStripButton3.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(48, 22);
            this.toolStripButton4.Text = "SaveAs";
            this.toolStripButton4.Click += new System.EventHandler(this.MenuSaveAs_Click);
            // 
            // btPen
            // 
            this.btPen.BackColor = System.Drawing.Color.White;
            this.btPen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btPen.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btPen.Image = global::EditMap.Properties.Resources.Pen;
            this.btPen.Location = new System.Drawing.Point(50, 28);
            this.btPen.Name = "btPen";
            this.btPen.Size = new System.Drawing.Size(32, 32);
            this.btPen.TabIndex = 5;
            this.btPen.UseVisualStyleBackColor = false;
            this.btPen.Click += new System.EventHandler(this.btPen_Click);
            // 
            // btFill
            // 
            this.btFill.BackColor = System.Drawing.Color.White;
            this.btFill.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btFill.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btFill.Image = global::EditMap.Properties.Resources.Fill;
            this.btFill.Location = new System.Drawing.Point(126, 28);
            this.btFill.Name = "btFill";
            this.btFill.Size = new System.Drawing.Size(32, 32);
            this.btFill.TabIndex = 6;
            this.btFill.UseVisualStyleBackColor = false;
            this.btFill.Click += new System.EventHandler(this.btFill_Click);
            // 
            // btEraser
            // 
            this.btEraser.BackColor = System.Drawing.Color.White;
            this.btEraser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btEraser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btEraser.Image = global::EditMap.Properties.Resources.Eraser;
            this.btEraser.Location = new System.Drawing.Point(88, 28);
            this.btEraser.Name = "btEraser";
            this.btEraser.Size = new System.Drawing.Size(32, 32);
            this.btEraser.TabIndex = 7;
            this.btEraser.UseVisualStyleBackColor = false;
            this.btEraser.Click += new System.EventHandler(this.btEraser_Click);
            // 
            // CurrentTileView
            // 
            this.CurrentTileView.Location = new System.Drawing.Point(164, 28);
            this.CurrentTileView.Name = "CurrentTileView";
            this.CurrentTileView.Size = new System.Drawing.Size(32, 32);
            this.CurrentTileView.TabIndex = 8;
            this.CurrentTileView.TabStop = false;
            // 
            // listTiles
            // 
            this.listTiles.BackColor = System.Drawing.Color.White;
            this.listTiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listTiles.GridLines = true;
            this.listTiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listTiles.HideSelection = false;
            this.listTiles.Location = new System.Drawing.Point(419, 66);
            this.listTiles.MultiSelect = false;
            this.listTiles.Name = "listTiles";
            this.listTiles.Size = new System.Drawing.Size(186, 400);
            this.listTiles.TabIndex = 9;
            this.listTiles.TileSize = new System.Drawing.Size(64, 64);
            this.listTiles.UseCompatibleStateImageBehavior = false;
            this.listTiles.SelectedIndexChanged += new System.EventHandler(this.listTiles_SelectedIndexChanged);
            this.listTiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listTiles_MouseClick);
            // 
            // btClear
            // 
            this.btClear.BackColor = System.Drawing.Color.White;
            this.btClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btClear.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btClear.Location = new System.Drawing.Point(12, 28);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(32, 32);
            this.btClear.TabIndex = 5;
            this.btClear.UseVisualStyleBackColor = false;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // cbShowDots
            // 
            this.cbShowDots.AutoSize = true;
            this.cbShowDots.Location = new System.Drawing.Point(202, 37);
            this.cbShowDots.Name = "cbShowDots";
            this.cbShowDots.Size = new System.Drawing.Size(15, 14);
            this.cbShowDots.TabIndex = 10;
            this.cbShowDots.UseVisualStyleBackColor = true;
            this.cbShowDots.CheckedChanged += new System.EventHandler(this.cbShowDots_CheckedChanged);
            // 
            // infoPathDotsNone
            // 
            this.infoPathDotsNone.BackColor = System.Drawing.Color.White;
            this.infoPathDotsNone.Location = new System.Drawing.Point(16, 17);
            this.infoPathDotsNone.Name = "infoPathDotsNone";
            this.infoPathDotsNone.Size = new System.Drawing.Size(8, 8);
            this.infoPathDotsNone.TabIndex = 8;
            this.infoPathDotsNone.TabStop = false;
            // 
            // infoPathDotsIn
            // 
            this.infoPathDotsIn.BackColor = System.Drawing.Color.Red;
            this.infoPathDotsIn.Location = new System.Drawing.Point(33, 17);
            this.infoPathDotsIn.Name = "infoPathDotsIn";
            this.infoPathDotsIn.Size = new System.Drawing.Size(8, 8);
            this.infoPathDotsIn.TabIndex = 8;
            this.infoPathDotsIn.TabStop = false;
            // 
            // infoPathDotsWay
            // 
            this.infoPathDotsWay.BackColor = System.Drawing.Color.Lime;
            this.infoPathDotsWay.Location = new System.Drawing.Point(47, 17);
            this.infoPathDotsWay.Name = "infoPathDotsWay";
            this.infoPathDotsWay.Size = new System.Drawing.Size(8, 8);
            this.infoPathDotsWay.TabIndex = 8;
            this.infoPathDotsWay.TabStop = false;
            // 
            // infoPathDotsOut
            // 
            this.infoPathDotsOut.BackColor = System.Drawing.Color.Blue;
            this.infoPathDotsOut.Location = new System.Drawing.Point(65, 17);
            this.infoPathDotsOut.Name = "infoPathDotsOut";
            this.infoPathDotsOut.Size = new System.Drawing.Size(8, 8);
            this.infoPathDotsOut.TabIndex = 8;
            this.infoPathDotsOut.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "none";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "in";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "way";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "out";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.infoPathDotsNone);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.infoPathDotsIn);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.infoPathDotsWay);
            this.panel1.Controls.Add(this.infoPathDotsOut);
            this.panel1.Location = new System.Drawing.Point(223, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(83, 41);
            this.panel1.TabIndex = 12;
            // 
            // FormEditMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 478);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cbShowDots);
            this.Controls.Add(this.listTiles);
            this.Controls.Add(this.CurrentTileView);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.btPen);
            this.Controls.Add(this.btFill);
            this.Controls.Add(this.btEraser);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.Render);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "FormEditMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Render_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Render)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentTileView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPathDotsNone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPathDotsIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPathDotsWay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPathDotsOut)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Render;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.Button btPen;
        private System.Windows.Forms.Button btFill;
        private System.Windows.Forms.Button btEraser;
        private System.Windows.Forms.PictureBox CurrentTileView;
        private System.Windows.Forms.ListView listTiles;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.CheckBox cbShowDots;
        private System.Windows.Forms.PictureBox infoPathDotsNone;
        private System.Windows.Forms.PictureBox infoPathDotsIn;
        private System.Windows.Forms.PictureBox infoPathDotsWay;
        private System.Windows.Forms.PictureBox infoPathDotsOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
    }
}

