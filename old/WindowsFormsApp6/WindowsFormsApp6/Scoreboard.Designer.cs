namespace WindowsFormsApp6
{
    partial class Scoreboard
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
            this.label1 = new System.Windows.Forms.Label();
            this.DGV = new System.Windows.Forms.DataGridView();
            this.btClose = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SBUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SBZen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SBBombs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SBSurvivalMini = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SBSurvivalMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SBHits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SBTimer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SBSwitchHits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SBGlue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(302, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "SCOREBOARD";
            // 
            // DGV
            // 
            this.DGV.AllowUserToAddRows = false;
            this.DGV.AllowUserToDeleteRows = false;
            this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SBUserName,
            this.SBZen,
            this.SBBombs,
            this.SBSurvivalMini,
            this.SBSurvivalMax,
            this.SBHits,
            this.SBTimer,
            this.SBSwitchHits,
            this.SBGlue});
            this.DGV.Location = new System.Drawing.Point(12, 47);
            this.DGV.Name = "DGV";
            this.DGV.ReadOnly = true;
            this.DGV.RowHeadersVisible = false;
            this.DGV.ShowCellErrors = false;
            this.DGV.ShowCellToolTips = false;
            this.DGV.ShowEditingIcon = false;
            this.DGV.ShowRowErrors = false;
            this.DGV.Size = new System.Drawing.Size(743, 237);
            this.DGV.TabIndex = 1;
            // 
            // btClose
            // 
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btClose.Location = new System.Drawing.Point(326, 290);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(111, 34);
            this.btClose.TabIndex = 2;
            this.btClose.Text = "CLOSE";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Lime;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(13, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SBUserName
            // 
            this.SBUserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SBUserName.HeaderText = "User Name";
            this.SBUserName.Name = "SBUserName";
            this.SBUserName.ReadOnly = true;
            this.SBUserName.Width = 85;
            // 
            // SBZen
            // 
            this.SBZen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SBZen.HeaderText = "ZEN";
            this.SBZen.Name = "SBZen";
            this.SBZen.ReadOnly = true;
            this.SBZen.Width = 54;
            // 
            // SBBombs
            // 
            this.SBBombs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SBBombs.HeaderText = "BOMBS";
            this.SBBombs.Name = "SBBombs";
            this.SBBombs.ReadOnly = true;
            this.SBBombs.Width = 70;
            // 
            // SBSurvivalMini
            // 
            this.SBSurvivalMini.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SBSurvivalMini.FillWeight = 80F;
            this.SBSurvivalMini.HeaderText = "SURVIVAL MINI";
            this.SBSurvivalMini.Name = "SBSurvivalMini";
            this.SBSurvivalMini.ReadOnly = true;
            this.SBSurvivalMini.Width = 102;
            // 
            // SBSurvivalMax
            // 
            this.SBSurvivalMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SBSurvivalMax.FillWeight = 80F;
            this.SBSurvivalMax.HeaderText = "SURVIVAL MAX";
            this.SBSurvivalMax.Name = "SBSurvivalMax";
            this.SBSurvivalMax.ReadOnly = true;
            this.SBSurvivalMax.Width = 102;
            // 
            // SBHits
            // 
            this.SBHits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SBHits.HeaderText = "HITS";
            this.SBHits.Name = "SBHits";
            this.SBHits.ReadOnly = true;
            this.SBHits.Width = 57;
            // 
            // SBTimer
            // 
            this.SBTimer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SBTimer.HeaderText = "TIMER";
            this.SBTimer.Name = "SBTimer";
            this.SBTimer.ReadOnly = true;
            this.SBTimer.Width = 66;
            // 
            // SBSwitchHits
            // 
            this.SBSwitchHits.HeaderText = "SWITCH HITS";
            this.SBSwitchHits.Name = "SBSwitchHits";
            this.SBSwitchHits.ReadOnly = true;
            // 
            // SBGlue
            // 
            this.SBGlue.HeaderText = "GLUE";
            this.SBGlue.Name = "SBGlue";
            this.SBGlue.ReadOnly = true;
            // 
            // Scoreboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 328);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.DGV);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Scoreboard";
            this.Text = "Scoreboard";
            ((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SBUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SBZen;
        private System.Windows.Forms.DataGridViewTextBoxColumn SBBombs;
        private System.Windows.Forms.DataGridViewTextBoxColumn SBSurvivalMini;
        private System.Windows.Forms.DataGridViewTextBoxColumn SBSurvivalMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn SBHits;
        private System.Windows.Forms.DataGridViewTextBoxColumn SBTimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn SBSwitchHits;
        private System.Windows.Forms.DataGridViewTextBoxColumn SBGlue;
    }
}