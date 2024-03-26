namespace EditSign
{
    partial class FormEditSign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditSign));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btFlip = new System.Windows.Forms.Button();
            this.btRotate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPaletteName = new System.Windows.Forms.TextBox();
            this.btRemovePalette = new System.Windows.Forms.Button();
            this.btLoadPalette = new System.Windows.Forms.Button();
            this.btNewPalette = new System.Windows.Forms.Button();
            this.btSavePalette = new System.Windows.Forms.Button();
            this.listPalettes = new System.Windows.Forms.ListBox();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.Render = new System.Windows.Forms.PictureBox();
            this.btPen = new System.Windows.Forms.Button();
            this.btFill = new System.Windows.Forms.Button();
            this.btEraser = new System.Windows.Forms.Button();
            this.btColor = new System.Windows.Forms.Button();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.btColorTransparent = new System.Windows.Forms.Button();
            this.btColorSave1 = new System.Windows.Forms.Button();
            this.btColorSave10 = new System.Windows.Forms.Button();
            this.btColorSave3 = new System.Windows.Forms.Button();
            this.btColorSave8 = new System.Windows.Forms.Button();
            this.btColorSave5 = new System.Windows.Forms.Button();
            this.btColorSave6 = new System.Windows.Forms.Button();
            this.btColorSave7 = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.btColorSave4 = new System.Windows.Forms.Button();
            this.btColorSave9 = new System.Windows.Forms.Button();
            this.btColorSave2 = new System.Windows.Forms.Button();
            this.groupImage = new System.Windows.Forms.GroupBox();
            this.lbIndex = new System.Windows.Forms.Label();
            this.nupIndex = new System.Windows.Forms.NumericUpDown();
            this.groupMiscs = new System.Windows.Forms.GroupBox();
            this.groupBehavior = new System.Windows.Forms.GroupBox();
            this.btEditScript = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Render)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.groupImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupIndex)).BeginInit();
            this.groupMiscs.SuspendLayout();
            this.groupBehavior.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(515, 289);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btFlip
            // 
            this.btFlip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btFlip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btFlip.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btFlip.Location = new System.Drawing.Point(217, 16);
            this.btFlip.Name = "btFlip";
            this.btFlip.Size = new System.Drawing.Size(14, 13);
            this.btFlip.TabIndex = 6;
            this.btFlip.UseVisualStyleBackColor = false;
            this.btFlip.Click += new System.EventHandler(this.btFlip_Click);
            // 
            // btRotate
            // 
            this.btRotate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btRotate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btRotate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btRotate.Location = new System.Drawing.Point(197, 16);
            this.btRotate.Name = "btRotate";
            this.btRotate.Size = new System.Drawing.Size(14, 13);
            this.btRotate.TabIndex = 6;
            this.btRotate.UseVisualStyleBackColor = false;
            this.btRotate.Click += new System.EventHandler(this.btRotate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(237, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Palette Name";
            // 
            // tbPaletteName
            // 
            this.tbPaletteName.Location = new System.Drawing.Point(195, 31);
            this.tbPaletteName.Name = "tbPaletteName";
            this.tbPaletteName.Size = new System.Drawing.Size(154, 20);
            this.tbPaletteName.TabIndex = 4;
            // 
            // btRemovePalette
            // 
            this.btRemovePalette.Location = new System.Drawing.Point(294, 153);
            this.btRemovePalette.Name = "btRemovePalette";
            this.btRemovePalette.Size = new System.Drawing.Size(55, 26);
            this.btRemovePalette.TabIndex = 3;
            this.btRemovePalette.Text = "Remove";
            this.btRemovePalette.UseVisualStyleBackColor = true;
            this.btRemovePalette.Click += new System.EventHandler(this.btRemovePalette_Click);
            // 
            // btLoadPalette
            // 
            this.btLoadPalette.Location = new System.Drawing.Point(294, 121);
            this.btLoadPalette.Name = "btLoadPalette";
            this.btLoadPalette.Size = new System.Drawing.Size(55, 26);
            this.btLoadPalette.TabIndex = 3;
            this.btLoadPalette.Text = "Load";
            this.btLoadPalette.UseVisualStyleBackColor = true;
            this.btLoadPalette.Click += new System.EventHandler(this.btLoadPalette_Click);
            // 
            // btNewPalette
            // 
            this.btNewPalette.Location = new System.Drawing.Point(294, 57);
            this.btNewPalette.Name = "btNewPalette";
            this.btNewPalette.Size = new System.Drawing.Size(55, 26);
            this.btNewPalette.TabIndex = 3;
            this.btNewPalette.Text = "New";
            this.btNewPalette.UseVisualStyleBackColor = true;
            this.btNewPalette.Click += new System.EventHandler(this.btNewPalette_Click);
            // 
            // btSavePalette
            // 
            this.btSavePalette.Location = new System.Drawing.Point(294, 89);
            this.btSavePalette.Name = "btSavePalette";
            this.btSavePalette.Size = new System.Drawing.Size(55, 26);
            this.btSavePalette.TabIndex = 3;
            this.btSavePalette.Text = "Save";
            this.btSavePalette.UseVisualStyleBackColor = true;
            this.btSavePalette.Click += new System.EventHandler(this.btSavePalette_Click);
            // 
            // listPalettes
            // 
            this.listPalettes.FormattingEnabled = true;
            this.listPalettes.Location = new System.Drawing.Point(195, 57);
            this.listPalettes.Name = "listPalettes";
            this.listPalettes.Size = new System.Drawing.Size(93, 121);
            this.listPalettes.TabIndex = 2;
            this.listPalettes.SelectedIndexChanged += new System.EventHandler(this.listPalettes_SelectedIndexChanged);
            this.listPalettes.DoubleClick += new System.EventHandler(this.listPalettes_DoubleClick);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(48, 22);
            this.toolStripButton4.Text = "SaveAs";
            this.toolStripButton4.Click += new System.EventHandler(this.MenuBtSaveAs_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton3.Text = "Save";
            this.toolStripButton3.Click += new System.EventHandler(this.MenuBtSave_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(37, 22);
            this.toolStripButton2.Text = "Load";
            this.toolStripButton2.Click += new System.EventHandler(this.MenuBtLoad_Click);
            // 
            // Render
            // 
            this.Render.Location = new System.Drawing.Point(6, 57);
            this.Render.Name = "Render";
            this.Render.Size = new System.Drawing.Size(128, 128);
            this.Render.TabIndex = 0;
            this.Render.TabStop = false;
            this.Render.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Render_MouseDown);
            this.Render.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Render_MouseMove);
            // 
            // btPen
            // 
            this.btPen.BackColor = System.Drawing.Color.White;
            this.btPen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btPen.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btPen.Image = global::EditSign.Properties.Resources.Pen;
            this.btPen.Location = new System.Drawing.Point(6, 19);
            this.btPen.Name = "btPen";
            this.btPen.Size = new System.Drawing.Size(32, 32);
            this.btPen.TabIndex = 1;
            this.btPen.UseVisualStyleBackColor = false;
            this.btPen.Click += new System.EventHandler(this.btPen_Click);
            // 
            // btFill
            // 
            this.btFill.BackColor = System.Drawing.Color.White;
            this.btFill.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btFill.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btFill.Image = global::EditSign.Properties.Resources.Fill;
            this.btFill.Location = new System.Drawing.Point(82, 19);
            this.btFill.Name = "btFill";
            this.btFill.Size = new System.Drawing.Size(32, 32);
            this.btFill.TabIndex = 1;
            this.btFill.UseVisualStyleBackColor = false;
            this.btFill.Click += new System.EventHandler(this.btFill_Click);
            // 
            // btEraser
            // 
            this.btEraser.BackColor = System.Drawing.Color.White;
            this.btEraser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btEraser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btEraser.Image = global::EditSign.Properties.Resources.Eraser;
            this.btEraser.Location = new System.Drawing.Point(44, 19);
            this.btEraser.Name = "btEraser";
            this.btEraser.Size = new System.Drawing.Size(32, 32);
            this.btEraser.TabIndex = 1;
            this.btEraser.UseVisualStyleBackColor = false;
            this.btEraser.Click += new System.EventHandler(this.btEraser_Click);
            // 
            // btColor
            // 
            this.btColor.BackColor = System.Drawing.Color.Black;
            this.btColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColor.Location = new System.Drawing.Point(120, 19);
            this.btColor.Name = "btColor";
            this.btColor.Size = new System.Drawing.Size(32, 32);
            this.btColor.TabIndex = 1;
            this.btColor.UseVisualStyleBackColor = false;
            this.btColor.Click += new System.EventHandler(this.btColor_Click);
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
            this.toolStrip.Size = new System.Drawing.Size(515, 25);
            this.toolStrip.TabIndex = 8;
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
            this.toolStripButton1.Click += new System.EventHandler(this.MenuBtNew_Click);
            // 
            // btColorTransparent
            // 
            this.btColorTransparent.BackColor = System.Drawing.Color.LightGray;
            this.btColorTransparent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorTransparent.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorTransparent.Location = new System.Drawing.Point(158, 19);
            this.btColorTransparent.Name = "btColorTransparent";
            this.btColorTransparent.Size = new System.Drawing.Size(32, 32);
            this.btColorTransparent.TabIndex = 1;
            this.btColorTransparent.UseVisualStyleBackColor = false;
            this.btColorTransparent.Click += new System.EventHandler(this.btColorTransparent_Click);
            // 
            // btColorSave1
            // 
            this.btColorSave1.BackColor = System.Drawing.Color.Black;
            this.btColorSave1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave1.Location = new System.Drawing.Point(140, 57);
            this.btColorSave1.Name = "btColorSave1";
            this.btColorSave1.Size = new System.Drawing.Size(21, 20);
            this.btColorSave1.TabIndex = 1;
            this.btColorSave1.UseVisualStyleBackColor = false;
            // 
            // btColorSave10
            // 
            this.btColorSave10.BackColor = System.Drawing.Color.White;
            this.btColorSave10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave10.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave10.Location = new System.Drawing.Point(167, 161);
            this.btColorSave10.Name = "btColorSave10";
            this.btColorSave10.Size = new System.Drawing.Size(21, 20);
            this.btColorSave10.TabIndex = 1;
            this.btColorSave10.UseVisualStyleBackColor = false;
            // 
            // btColorSave3
            // 
            this.btColorSave3.BackColor = System.Drawing.Color.White;
            this.btColorSave3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave3.Location = new System.Drawing.Point(140, 83);
            this.btColorSave3.Name = "btColorSave3";
            this.btColorSave3.Size = new System.Drawing.Size(21, 20);
            this.btColorSave3.TabIndex = 1;
            this.btColorSave3.UseVisualStyleBackColor = false;
            // 
            // btColorSave8
            // 
            this.btColorSave8.BackColor = System.Drawing.Color.White;
            this.btColorSave8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave8.Location = new System.Drawing.Point(167, 135);
            this.btColorSave8.Name = "btColorSave8";
            this.btColorSave8.Size = new System.Drawing.Size(21, 20);
            this.btColorSave8.TabIndex = 1;
            this.btColorSave8.UseVisualStyleBackColor = false;
            // 
            // btColorSave5
            // 
            this.btColorSave5.BackColor = System.Drawing.Color.White;
            this.btColorSave5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave5.Location = new System.Drawing.Point(140, 109);
            this.btColorSave5.Name = "btColorSave5";
            this.btColorSave5.Size = new System.Drawing.Size(21, 20);
            this.btColorSave5.TabIndex = 1;
            this.btColorSave5.UseVisualStyleBackColor = false;
            // 
            // btColorSave6
            // 
            this.btColorSave6.BackColor = System.Drawing.Color.White;
            this.btColorSave6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave6.Location = new System.Drawing.Point(167, 109);
            this.btColorSave6.Name = "btColorSave6";
            this.btColorSave6.Size = new System.Drawing.Size(21, 20);
            this.btColorSave6.TabIndex = 1;
            this.btColorSave6.UseVisualStyleBackColor = false;
            // 
            // btColorSave7
            // 
            this.btColorSave7.BackColor = System.Drawing.Color.White;
            this.btColorSave7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave7.Location = new System.Drawing.Point(140, 135);
            this.btColorSave7.Name = "btColorSave7";
            this.btColorSave7.Size = new System.Drawing.Size(21, 20);
            this.btColorSave7.TabIndex = 1;
            this.btColorSave7.UseVisualStyleBackColor = false;
            // 
            // btColorSave4
            // 
            this.btColorSave4.BackColor = System.Drawing.Color.White;
            this.btColorSave4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave4.Location = new System.Drawing.Point(167, 83);
            this.btColorSave4.Name = "btColorSave4";
            this.btColorSave4.Size = new System.Drawing.Size(21, 20);
            this.btColorSave4.TabIndex = 1;
            this.btColorSave4.UseVisualStyleBackColor = false;
            // 
            // btColorSave9
            // 
            this.btColorSave9.BackColor = System.Drawing.Color.White;
            this.btColorSave9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave9.Location = new System.Drawing.Point(140, 161);
            this.btColorSave9.Name = "btColorSave9";
            this.btColorSave9.Size = new System.Drawing.Size(21, 20);
            this.btColorSave9.TabIndex = 1;
            this.btColorSave9.UseVisualStyleBackColor = false;
            // 
            // btColorSave2
            // 
            this.btColorSave2.BackColor = System.Drawing.Color.White;
            this.btColorSave2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave2.Location = new System.Drawing.Point(167, 57);
            this.btColorSave2.Name = "btColorSave2";
            this.btColorSave2.Size = new System.Drawing.Size(21, 20);
            this.btColorSave2.TabIndex = 1;
            this.btColorSave2.UseVisualStyleBackColor = false;
            // 
            // groupImage
            // 
            this.groupImage.Controls.Add(this.btFlip);
            this.groupImage.Controls.Add(this.btRotate);
            this.groupImage.Controls.Add(this.label1);
            this.groupImage.Controls.Add(this.tbPaletteName);
            this.groupImage.Controls.Add(this.btRemovePalette);
            this.groupImage.Controls.Add(this.btLoadPalette);
            this.groupImage.Controls.Add(this.btNewPalette);
            this.groupImage.Controls.Add(this.btSavePalette);
            this.groupImage.Controls.Add(this.listPalettes);
            this.groupImage.Controls.Add(this.Render);
            this.groupImage.Controls.Add(this.btPen);
            this.groupImage.Controls.Add(this.btFill);
            this.groupImage.Controls.Add(this.btEraser);
            this.groupImage.Controls.Add(this.btColor);
            this.groupImage.Controls.Add(this.btColorTransparent);
            this.groupImage.Controls.Add(this.btColorSave1);
            this.groupImage.Controls.Add(this.btColorSave10);
            this.groupImage.Controls.Add(this.btColorSave3);
            this.groupImage.Controls.Add(this.btColorSave8);
            this.groupImage.Controls.Add(this.btColorSave5);
            this.groupImage.Controls.Add(this.btColorSave6);
            this.groupImage.Controls.Add(this.btColorSave7);
            this.groupImage.Controls.Add(this.btColorSave4);
            this.groupImage.Controls.Add(this.btColorSave9);
            this.groupImage.Controls.Add(this.btColorSave2);
            this.groupImage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupImage.Location = new System.Drawing.Point(12, 28);
            this.groupImage.Name = "groupImage";
            this.groupImage.Size = new System.Drawing.Size(355, 193);
            this.groupImage.TabIndex = 5;
            this.groupImage.TabStop = false;
            this.groupImage.Text = "Image";
            // 
            // lbIndex
            // 
            this.lbIndex.AutoSize = true;
            this.lbIndex.Location = new System.Drawing.Point(7, 20);
            this.lbIndex.Name = "lbIndex";
            this.lbIndex.Size = new System.Drawing.Size(33, 13);
            this.lbIndex.TabIndex = 0;
            this.lbIndex.Text = "Index";
            // 
            // nupIndex
            // 
            this.nupIndex.Location = new System.Drawing.Point(46, 18);
            this.nupIndex.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nupIndex.Name = "nupIndex";
            this.nupIndex.Size = new System.Drawing.Size(67, 20);
            this.nupIndex.TabIndex = 1;
            // 
            // groupMiscs
            // 
            this.groupMiscs.Controls.Add(this.nupIndex);
            this.groupMiscs.Controls.Add(this.lbIndex);
            this.groupMiscs.Location = new System.Drawing.Point(13, 227);
            this.groupMiscs.Name = "groupMiscs";
            this.groupMiscs.Size = new System.Drawing.Size(490, 50);
            this.groupMiscs.TabIndex = 9;
            this.groupMiscs.TabStop = false;
            this.groupMiscs.Text = "Miscellaneous";
            // 
            // groupBehavior
            // 
            this.groupBehavior.Controls.Add(this.btEditScript);
            this.groupBehavior.Location = new System.Drawing.Point(374, 29);
            this.groupBehavior.Name = "groupBehavior";
            this.groupBehavior.Size = new System.Drawing.Size(129, 76);
            this.groupBehavior.TabIndex = 10;
            this.groupBehavior.TabStop = false;
            this.groupBehavior.Text = "Behavior";
            // 
            // btEditScript
            // 
            this.btEditScript.Location = new System.Drawing.Point(28, 30);
            this.btEditScript.Name = "btEditScript";
            this.btEditScript.Size = new System.Drawing.Size(75, 23);
            this.btEditScript.TabIndex = 0;
            this.btEditScript.Text = "Edit Script";
            this.btEditScript.UseVisualStyleBackColor = true;
            this.btEditScript.Click += new System.EventHandler(this.btEditScript_Click);
            // 
            // FormEditSign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 289);
            this.Controls.Add(this.groupBehavior);
            this.Controls.Add(this.groupMiscs);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.groupImage);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FormEditSign";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Render)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.groupImage.ResumeLayout(false);
            this.groupImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupIndex)).EndInit();
            this.groupMiscs.ResumeLayout(false);
            this.groupMiscs.PerformLayout();
            this.groupBehavior.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btFlip;
        private System.Windows.Forms.Button btRotate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPaletteName;
        private System.Windows.Forms.Button btRemovePalette;
        private System.Windows.Forms.Button btLoadPalette;
        private System.Windows.Forms.Button btNewPalette;
        private System.Windows.Forms.Button btSavePalette;
        private System.Windows.Forms.ListBox listPalettes;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.PictureBox Render;
        private System.Windows.Forms.Button btPen;
        private System.Windows.Forms.Button btFill;
        private System.Windows.Forms.Button btEraser;
        private System.Windows.Forms.Button btColor;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Button btColorTransparent;
        private System.Windows.Forms.Button btColorSave1;
        private System.Windows.Forms.Button btColorSave10;
        private System.Windows.Forms.Button btColorSave3;
        private System.Windows.Forms.Button btColorSave8;
        private System.Windows.Forms.Button btColorSave5;
        private System.Windows.Forms.Button btColorSave6;
        private System.Windows.Forms.Button btColorSave7;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button btColorSave4;
        private System.Windows.Forms.Button btColorSave9;
        private System.Windows.Forms.Button btColorSave2;
        private System.Windows.Forms.GroupBox groupImage;
        private System.Windows.Forms.Label lbIndex;
        private System.Windows.Forms.NumericUpDown nupIndex;
        private System.Windows.Forms.GroupBox groupMiscs;
        private System.Windows.Forms.GroupBox groupBehavior;
        private System.Windows.Forms.Button btEditScript;
    }
}

