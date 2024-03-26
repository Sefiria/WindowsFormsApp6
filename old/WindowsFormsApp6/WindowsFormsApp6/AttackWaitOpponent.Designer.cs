namespace WindowsFormsApp6
{
    partial class AttackWaitOpponent
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
            this.listUsers = new System.Windows.Forms.ListBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.btAskDuel = new System.Windows.Forms.Button();
            this.btRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listUsers
            // 
            this.listUsers.BackColor = System.Drawing.Color.LightGreen;
            this.listUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listUsers.FormattingEnabled = true;
            this.listUsers.Location = new System.Drawing.Point(9, 37);
            this.listUsers.Name = "listUsers";
            this.listUsers.Size = new System.Drawing.Size(152, 95);
            this.listUsers.TabIndex = 0;
            this.listUsers.SelectedIndexChanged += new System.EventHandler(this.listUsers_SelectedIndexChanged);
            // 
            // btCancel
            // 
            this.btCancel.BackColor = System.Drawing.Color.Red;
            this.btCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCancel.ForeColor = System.Drawing.Color.White;
            this.btCancel.Location = new System.Drawing.Point(9, 139);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(152, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = false;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btAskDuel
            // 
            this.btAskDuel.BackColor = System.Drawing.Color.Blue;
            this.btAskDuel.Enabled = false;
            this.btAskDuel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAskDuel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAskDuel.ForeColor = System.Drawing.Color.White;
            this.btAskDuel.Location = new System.Drawing.Point(167, 11);
            this.btAskDuel.Name = "btAskDuel";
            this.btAskDuel.Size = new System.Drawing.Size(22, 151);
            this.btAskDuel.TabIndex = 1;
            this.btAskDuel.Text = "ASK  DUEL";
            this.btAskDuel.UseVisualStyleBackColor = false;
            this.btAskDuel.Click += new System.EventHandler(this.btAskDuel_Click);
            // 
            // btRefresh
            // 
            this.btRefresh.BackColor = System.Drawing.Color.ForestGreen;
            this.btRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btRefresh.Font = new System.Drawing.Font("Microsoft JhengHei", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRefresh.ForeColor = System.Drawing.Color.White;
            this.btRefresh.Location = new System.Drawing.Point(9, 11);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(152, 23);
            this.btRefresh.TabIndex = 1;
            this.btRefresh.Text = "Refresh List";
            this.btRefresh.UseVisualStyleBackColor = false;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // AttackWaitOpponent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(201, 169);
            this.Controls.Add(this.btAskDuel);
            this.Controls.Add(this.btRefresh);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.listUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AttackWaitOpponent";
            this.Text = "Attack! Room Selection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listUsers;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btAskDuel;
        private System.Windows.Forms.Button btRefresh;
    }
}