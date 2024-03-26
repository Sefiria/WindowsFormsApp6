namespace BGAnim
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
            this.RenderMG = new System.Windows.Forms.PictureBox();
            this.RenderBG = new System.Windows.Forms.PictureBox();
            this.RenderFG = new System.Windows.Forms.PictureBox();
            this.cbbDirectionBG = new System.Windows.Forms.ComboBox();
            this.cbbDirectionFG = new System.Windows.Forms.ComboBox();
            this.RenderResult = new System.Windows.Forms.PictureBox();
            this.btPen = new System.Windows.Forms.Button();
            this.btFill = new System.Windows.Forms.Button();
            this.btEraser = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btPenColor = new System.Windows.Forms.Button();
            this.btFillColor = new System.Windows.Forms.Button();
            this.btBGColor = new System.Windows.Forms.Button();
            this.PenSize = new System.Windows.Forms.TrackBar();
            this.btClearBG = new System.Windows.Forms.Button();
            this.btClearMid = new System.Windows.Forms.Button();
            this.btClearFG = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btLoad = new System.Windows.Forms.Button();
            this.SpeedCUR = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SpeedNUM = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RenderMG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderBG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderFG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PenSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedCUR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedNUM)).BeginInit();
            this.SuspendLayout();
            // 
            // RenderMG
            // 
            this.RenderMG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RenderMG.Location = new System.Drawing.Point(338, 12);
            this.RenderMG.Name = "RenderMG";
            this.RenderMG.Size = new System.Drawing.Size(320, 320);
            this.RenderMG.TabIndex = 0;
            this.RenderMG.TabStop = false;
            this.RenderMG.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderMG_MouseDown);
            this.RenderMG.MouseLeave += new System.EventHandler(this.RenderMG_MouseLeave);
            this.RenderMG.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderMG_MouseMove);
            this.RenderMG.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RenderMG_MouseUp);
            // 
            // RenderBG
            // 
            this.RenderBG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RenderBG.Location = new System.Drawing.Point(12, 12);
            this.RenderBG.Name = "RenderBG";
            this.RenderBG.Size = new System.Drawing.Size(320, 320);
            this.RenderBG.TabIndex = 0;
            this.RenderBG.TabStop = false;
            this.RenderBG.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderBG_MouseDown);
            this.RenderBG.MouseLeave += new System.EventHandler(this.RenderBG_MouseLeave);
            this.RenderBG.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderBG_MouseMove);
            this.RenderBG.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RenderBG_MouseUp);
            // 
            // RenderFG
            // 
            this.RenderFG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RenderFG.Location = new System.Drawing.Point(664, 12);
            this.RenderFG.Name = "RenderFG";
            this.RenderFG.Size = new System.Drawing.Size(320, 320);
            this.RenderFG.TabIndex = 0;
            this.RenderFG.TabStop = false;
            this.RenderFG.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderFG_MouseDown);
            this.RenderFG.MouseLeave += new System.EventHandler(this.RenderFG_MouseLeave);
            this.RenderFG.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderFG_MouseMove);
            this.RenderFG.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RenderFG_MouseUp);
            // 
            // cbbDirectionBG
            // 
            this.cbbDirectionBG.DisplayMember = "eft";
            this.cbbDirectionBG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDirectionBG.FormattingEnabled = true;
            this.cbbDirectionBG.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Up",
            "Down"});
            this.cbbDirectionBG.Location = new System.Drawing.Point(12, 339);
            this.cbbDirectionBG.Name = "cbbDirectionBG";
            this.cbbDirectionBG.Size = new System.Drawing.Size(320, 21);
            this.cbbDirectionBG.TabIndex = 1;
            this.cbbDirectionBG.SelectedIndexChanged += new System.EventHandler(this.cbbDirectionBG_SelectedIndexChanged);
            // 
            // cbbDirectionFG
            // 
            this.cbbDirectionFG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDirectionFG.FormattingEnabled = true;
            this.cbbDirectionFG.Items.AddRange(new object[] {
            "Left",
            "Right",
            "Up",
            "Down"});
            this.cbbDirectionFG.Location = new System.Drawing.Point(664, 338);
            this.cbbDirectionFG.Name = "cbbDirectionFG";
            this.cbbDirectionFG.Size = new System.Drawing.Size(320, 21);
            this.cbbDirectionFG.TabIndex = 1;
            this.cbbDirectionFG.SelectedIndexChanged += new System.EventHandler(this.cbbDirectionFG_SelectedIndexChanged);
            // 
            // RenderResult
            // 
            this.RenderResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RenderResult.Location = new System.Drawing.Point(338, 339);
            this.RenderResult.Name = "RenderResult";
            this.RenderResult.Size = new System.Drawing.Size(320, 320);
            this.RenderResult.TabIndex = 0;
            this.RenderResult.TabStop = false;
            // 
            // btPen
            // 
            this.btPen.Location = new System.Drawing.Point(39, 425);
            this.btPen.Name = "btPen";
            this.btPen.Size = new System.Drawing.Size(75, 23);
            this.btPen.TabIndex = 2;
            this.btPen.Text = "Pen";
            this.btPen.UseVisualStyleBackColor = true;
            this.btPen.Click += new System.EventHandler(this.btPen_Click);
            // 
            // btFill
            // 
            this.btFill.Location = new System.Drawing.Point(120, 425);
            this.btFill.Name = "btFill";
            this.btFill.Size = new System.Drawing.Size(75, 23);
            this.btFill.TabIndex = 2;
            this.btFill.Text = "Fill";
            this.btFill.UseVisualStyleBackColor = true;
            this.btFill.Click += new System.EventHandler(this.btFill_Click);
            // 
            // btEraser
            // 
            this.btEraser.Location = new System.Drawing.Point(201, 425);
            this.btEraser.Name = "btEraser";
            this.btEraser.Size = new System.Drawing.Size(75, 23);
            this.btEraser.TabIndex = 2;
            this.btEraser.Text = "Eraser";
            this.btEraser.UseVisualStyleBackColor = true;
            this.btEraser.Click += new System.EventHandler(this.btEraser_Click);
            // 
            // btPenColor
            // 
            this.btPenColor.BackColor = System.Drawing.Color.Black;
            this.btPenColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btPenColor.Location = new System.Drawing.Point(62, 454);
            this.btPenColor.Name = "btPenColor";
            this.btPenColor.Size = new System.Drawing.Size(32, 32);
            this.btPenColor.TabIndex = 3;
            this.btPenColor.UseVisualStyleBackColor = false;
            this.btPenColor.Click += new System.EventHandler(this.btPenColor_Click);
            // 
            // btFillColor
            // 
            this.btFillColor.BackColor = System.Drawing.Color.White;
            this.btFillColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btFillColor.Location = new System.Drawing.Point(142, 454);
            this.btFillColor.Name = "btFillColor";
            this.btFillColor.Size = new System.Drawing.Size(32, 32);
            this.btFillColor.TabIndex = 3;
            this.btFillColor.UseVisualStyleBackColor = false;
            this.btFillColor.Click += new System.EventHandler(this.btFillColor_Click);
            // 
            // btBGColor
            // 
            this.btBGColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btBGColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btBGColor.Location = new System.Drawing.Point(39, 403);
            this.btBGColor.Name = "btBGColor";
            this.btBGColor.Size = new System.Drawing.Size(237, 16);
            this.btBGColor.TabIndex = 3;
            this.btBGColor.UseVisualStyleBackColor = false;
            this.btBGColor.Click += new System.EventHandler(this.btBGColor_Click);
            // 
            // PenSize
            // 
            this.PenSize.Location = new System.Drawing.Point(91, 506);
            this.PenSize.Maximum = 50;
            this.PenSize.Minimum = 1;
            this.PenSize.Name = "PenSize";
            this.PenSize.Size = new System.Drawing.Size(104, 45);
            this.PenSize.TabIndex = 4;
            this.PenSize.Value = 5;
            // 
            // btClearBG
            // 
            this.btClearBG.BackColor = System.Drawing.Color.Red;
            this.btClearBG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClearBG.Location = new System.Drawing.Point(4, 12);
            this.btClearBG.Name = "btClearBG";
            this.btClearBG.Size = new System.Drawing.Size(8, 8);
            this.btClearBG.TabIndex = 5;
            this.btClearBG.UseVisualStyleBackColor = false;
            this.btClearBG.Click += new System.EventHandler(this.btClearBG_Click);
            // 
            // btClearMid
            // 
            this.btClearMid.BackColor = System.Drawing.Color.Red;
            this.btClearMid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClearMid.Location = new System.Drawing.Point(330, 12);
            this.btClearMid.Name = "btClearMid";
            this.btClearMid.Size = new System.Drawing.Size(8, 8);
            this.btClearMid.TabIndex = 5;
            this.btClearMid.UseVisualStyleBackColor = false;
            this.btClearMid.Click += new System.EventHandler(this.btClearMid_Click);
            // 
            // btClearFG
            // 
            this.btClearFG.BackColor = System.Drawing.Color.Red;
            this.btClearFG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btClearFG.Location = new System.Drawing.Point(656, 12);
            this.btClearFG.Name = "btClearFG";
            this.btClearFG.Size = new System.Drawing.Size(8, 8);
            this.btClearFG.TabIndex = 5;
            this.btClearFG.UseVisualStyleBackColor = false;
            this.btClearFG.Click += new System.EventHandler(this.btClearFG_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(790, 454);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 6;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btLoad
            // 
            this.btLoad.Location = new System.Drawing.Point(790, 425);
            this.btLoad.Name = "btLoad";
            this.btLoad.Size = new System.Drawing.Size(75, 23);
            this.btLoad.TabIndex = 6;
            this.btLoad.Text = "Load";
            this.btLoad.UseVisualStyleBackColor = true;
            this.btLoad.Click += new System.EventHandler(this.btLoad_Click);
            // 
            // SpeedCUR
            // 
            this.SpeedCUR.Location = new System.Drawing.Point(91, 557);
            this.SpeedCUR.Maximum = 200;
            this.SpeedCUR.Minimum = 1;
            this.SpeedCUR.Name = "SpeedCUR";
            this.SpeedCUR.Size = new System.Drawing.Size(104, 45);
            this.SpeedCUR.TabIndex = 7;
            this.SpeedCUR.Value = 1;
            this.SpeedCUR.Scroll += new System.EventHandler(this.SpeedCUR_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 506);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 557);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Speed";
            // 
            // SpeedNUM
            // 
            this.SpeedNUM.Location = new System.Drawing.Point(201, 557);
            this.SpeedNUM.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.SpeedNUM.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpeedNUM.Name = "SpeedNUM";
            this.SpeedNUM.Size = new System.Drawing.Size(50, 20);
            this.SpeedNUM.TabIndex = 9;
            this.SpeedNUM.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpeedNUM.ValueChanged += new System.EventHandler(this.SpeedNUM_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(268, 559);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "/ 10";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 668);
            this.Controls.Add(this.SpeedNUM);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SpeedCUR);
            this.Controls.Add(this.btLoad);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btClearFG);
            this.Controls.Add(this.btClearMid);
            this.Controls.Add(this.btClearBG);
            this.Controls.Add(this.PenSize);
            this.Controls.Add(this.btFillColor);
            this.Controls.Add(this.btBGColor);
            this.Controls.Add(this.btPenColor);
            this.Controls.Add(this.btEraser);
            this.Controls.Add(this.btFill);
            this.Controls.Add(this.btPen);
            this.Controls.Add(this.cbbDirectionFG);
            this.Controls.Add(this.cbbDirectionBG);
            this.Controls.Add(this.RenderBG);
            this.Controls.Add(this.RenderFG);
            this.Controls.Add(this.RenderResult);
            this.Controls.Add(this.RenderMG);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.RenderMG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderBG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderFG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RenderResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PenSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedCUR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedNUM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox RenderMG;
        private System.Windows.Forms.PictureBox RenderBG;
        private System.Windows.Forms.PictureBox RenderFG;
        private System.Windows.Forms.ComboBox cbbDirectionBG;
        private System.Windows.Forms.ComboBox cbbDirectionFG;
        private System.Windows.Forms.PictureBox RenderResult;
        private System.Windows.Forms.Button btPen;
        private System.Windows.Forms.Button btFill;
        private System.Windows.Forms.Button btEraser;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button btPenColor;
        private System.Windows.Forms.Button btFillColor;
        private System.Windows.Forms.Button btBGColor;
        private System.Windows.Forms.TrackBar PenSize;
        private System.Windows.Forms.Button btClearBG;
        private System.Windows.Forms.Button btClearMid;
        private System.Windows.Forms.Button btClearFG;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.TrackBar SpeedCUR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown SpeedNUM;
        private System.Windows.Forms.Label label3;
    }
}

