namespace WindowsFormsApp6
{
    partial class MAIN
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
            this.btZen = new System.Windows.Forms.Button();
            this.btSurvivalMini = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btSurvivalMax = new System.Windows.Forms.Button();
            this.btHits = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btResendScores = new System.Windows.Forms.Button();
            this.btSaveName = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btTimer = new System.Windows.Forms.Button();
            this.btSwitchHits = new System.Windows.Forms.Button();
            this.btGlue = new System.Windows.Forms.Button();
            this.btAttackWaitOpponent = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btScoreBoard = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btZen
            // 
            this.btZen.Location = new System.Drawing.Point(3, 3);
            this.btZen.Name = "btZen";
            this.btZen.Size = new System.Drawing.Size(84, 23);
            this.btZen.TabIndex = 0;
            this.btZen.Text = "Zen";
            this.toolTip1.SetToolTip(this.btZen, "Make the Highest Score ! Close to save & send score. Continue playing at anytime " +
        "!");
            this.btZen.UseVisualStyleBackColor = true;
            this.btZen.Click += new System.EventHandler(this.SelectMode);
            // 
            // btSurvivalMini
            // 
            this.btSurvivalMini.Location = new System.Drawing.Point(3, 32);
            this.btSurvivalMini.Name = "btSurvivalMini";
            this.btSurvivalMini.Size = new System.Drawing.Size(84, 23);
            this.btSurvivalMini.TabIndex = 0;
            this.btSurvivalMini.Text = "Survival Mini";
            this.toolTip1.SetToolTip(this.btSurvivalMini, "Leave a Minimum of Blocks !");
            this.btSurvivalMini.UseVisualStyleBackColor = true;
            this.btSurvivalMini.Click += new System.EventHandler(this.SelectMode);
            // 
            // btSurvivalMax
            // 
            this.btSurvivalMax.Location = new System.Drawing.Point(93, 32);
            this.btSurvivalMax.Name = "btSurvivalMax";
            this.btSurvivalMax.Size = new System.Drawing.Size(84, 23);
            this.btSurvivalMax.TabIndex = 0;
            this.btSurvivalMax.Text = "Survival Max";
            this.toolTip1.SetToolTip(this.btSurvivalMax, "Leave a Maximum of Blocks !");
            this.btSurvivalMax.UseVisualStyleBackColor = true;
            this.btSurvivalMax.Click += new System.EventHandler(this.SelectMode);
            // 
            // btHits
            // 
            this.btHits.Location = new System.Drawing.Point(3, 59);
            this.btHits.Name = "btHits";
            this.btHits.Size = new System.Drawing.Size(84, 23);
            this.btHits.TabIndex = 0;
            this.btHits.Text = "Hits";
            this.toolTip1.SetToolTip(this.btHits, "Do max score in counted hits");
            this.btHits.UseVisualStyleBackColor = true;
            this.btHits.Click += new System.EventHandler(this.SelectMode);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(52, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "More soon...";
            this.toolTip1.SetToolTip(this.label2, "Secret :3");
            // 
            // btResendScores
            // 
            this.btResendScores.BackColor = System.Drawing.Color.Aquamarine;
            this.btResendScores.Location = new System.Drawing.Point(12, 208);
            this.btResendScores.Name = "btResendScores";
            this.btResendScores.Size = new System.Drawing.Size(179, 23);
            this.btResendScores.TabIndex = 5;
            this.btResendScores.Text = "Resend Scores";
            this.toolTip1.SetToolTip(this.btResendScores, "Send scores from data to server");
            this.btResendScores.UseVisualStyleBackColor = false;
            this.btResendScores.Click += new System.EventHandler(this.btResendScores_Click);
            // 
            // btSaveName
            // 
            this.btSaveName.BackColor = System.Drawing.Color.Cyan;
            this.btSaveName.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btSaveName.Location = new System.Drawing.Point(171, 17);
            this.btSaveName.Name = "btSaveName";
            this.btSaveName.Size = new System.Drawing.Size(12, 12);
            this.btSaveName.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btSaveName, "Save UserName");
            this.btSaveName.UseVisualStyleBackColor = false;
            this.btSaveName.Click += new System.EventHandler(this.btSaveName_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(93, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Bombs";
            this.toolTip1.SetToolTip(this.button1, "Explode a bomb with a combo before it explodes itself !");
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.SelectMode);
            // 
            // btTimer
            // 
            this.btTimer.Location = new System.Drawing.Point(93, 59);
            this.btTimer.Name = "btTimer";
            this.btTimer.Size = new System.Drawing.Size(84, 23);
            this.btTimer.TabIndex = 0;
            this.btTimer.Text = "Timer";
            this.toolTip1.SetToolTip(this.btTimer, "Do max score before the end ! Destroy combos gives bonus time.");
            this.btTimer.UseVisualStyleBackColor = true;
            this.btTimer.Click += new System.EventHandler(this.SelectMode);
            // 
            // btSwitchHits
            // 
            this.btSwitchHits.BackColor = System.Drawing.SystemColors.Control;
            this.btSwitchHits.Location = new System.Drawing.Point(3, 87);
            this.btSwitchHits.Name = "btSwitchHits";
            this.btSwitchHits.Size = new System.Drawing.Size(84, 23);
            this.btSwitchHits.TabIndex = 0;
            this.btSwitchHits.Text = "SwitchHits";
            this.toolTip1.SetToolTip(this.btSwitchHits, "In a CandyCrush way, switch two blocks in a limit of Hits");
            this.btSwitchHits.UseVisualStyleBackColor = false;
            this.btSwitchHits.Click += new System.EventHandler(this.SelectMode);
            // 
            // btGlue
            // 
            this.btGlue.BackColor = System.Drawing.SystemColors.Control;
            this.btGlue.Location = new System.Drawing.Point(93, 87);
            this.btGlue.Name = "btGlue";
            this.btGlue.Size = new System.Drawing.Size(84, 23);
            this.btGlue.TabIndex = 0;
            this.btGlue.Text = "Glue";
            this.toolTip1.SetToolTip(this.btGlue, "Destroy blocks for destroy glue. Removes all the glue in the grid");
            this.btGlue.UseVisualStyleBackColor = false;
            this.btGlue.Click += new System.EventHandler(this.SelectMode);
            // 
            // btAttackWaitOpponent
            // 
            this.btAttackWaitOpponent.BackColor = System.Drawing.Color.DarkTurquoise;
            this.btAttackWaitOpponent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAttackWaitOpponent.Location = new System.Drawing.Point(3, 116);
            this.btAttackWaitOpponent.Name = "btAttackWaitOpponent";
            this.btAttackWaitOpponent.Size = new System.Drawing.Size(174, 23);
            this.btAttackWaitOpponent.TabIndex = 0;
            this.btAttackWaitOpponent.Text = "Attack!";
            this.toolTip1.SetToolTip(this.btAttackWaitOpponent, "Let\'s go for the d-d-d-DUEL!");
            this.btAttackWaitOpponent.UseVisualStyleBackColor = false;
            this.btAttackWaitOpponent.Click += new System.EventHandler(this.SelectMode);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "UserName";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(80, 13);
            this.textBox1.MaxLength = 12;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(83, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Toto";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btZen);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btSurvivalMini);
            this.panel1.Controls.Add(this.btAttackWaitOpponent);
            this.panel1.Controls.Add(this.btGlue);
            this.panel1.Controls.Add(this.btSwitchHits);
            this.panel1.Controls.Add(this.btTimer);
            this.panel1.Controls.Add(this.btHits);
            this.panel1.Controls.Add(this.btSurvivalMax);
            this.panel1.Location = new System.Drawing.Point(12, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 164);
            this.panel1.TabIndex = 4;
            // 
            // btScoreBoard
            // 
            this.btScoreBoard.BackColor = System.Drawing.Color.GreenYellow;
            this.btScoreBoard.Location = new System.Drawing.Point(12, 238);
            this.btScoreBoard.Name = "btScoreBoard";
            this.btScoreBoard.Size = new System.Drawing.Size(179, 23);
            this.btScoreBoard.TabIndex = 7;
            this.btScoreBoard.Text = "ScoreBoard";
            this.btScoreBoard.UseVisualStyleBackColor = false;
            this.btScoreBoard.Click += new System.EventHandler(this.btScoreBoard_Click);
            // 
            // MAIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(204, 269);
            this.Controls.Add(this.btScoreBoard);
            this.Controls.Add(this.btSaveName);
            this.Controls.Add(this.btResendScores);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "MAIN";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MAIN";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btZen;
        private System.Windows.Forms.Button btSurvivalMini;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btSurvivalMax;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btHits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btResendScores;
        private System.Windows.Forms.Button btSaveName;
        private System.Windows.Forms.Button btScoreBoard;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btTimer;
        private System.Windows.Forms.Button btSwitchHits;
        private System.Windows.Forms.Button btGlue;
        private System.Windows.Forms.Button btAttackWaitOpponent;
    }
}