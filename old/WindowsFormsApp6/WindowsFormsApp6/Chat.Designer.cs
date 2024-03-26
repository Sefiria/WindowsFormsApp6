namespace WindowsFormsApp6
{
    partial class Chat
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
            this.btMinimize = new System.Windows.Forms.Button();
            this.ChatBox = new System.Windows.Forms.RichTextBox();
            this.ChatBoxInput = new System.Windows.Forms.TextBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listUsers = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btMinimize
            // 
            this.btMinimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(124)))), ((int)(((byte)(124)))));
            this.btMinimize.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btMinimize.Location = new System.Drawing.Point(610, 12);
            this.btMinimize.Name = "btMinimize";
            this.btMinimize.Size = new System.Drawing.Size(20, 12);
            this.btMinimize.TabIndex = 0;
            this.btMinimize.Text = "Ξ";
            this.btMinimize.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btMinimize.UseVisualStyleBackColor = false;
            this.btMinimize.Click += new System.EventHandler(this.btMinimize_Click);
            // 
            // ChatBox
            // 
            this.ChatBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ChatBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ChatBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ChatBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.ChatBox.Location = new System.Drawing.Point(16, 50);
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.ReadOnly = true;
            this.ChatBox.Size = new System.Drawing.Size(455, 229);
            this.ChatBox.TabIndex = 1;
            this.ChatBox.Text = "";
            // 
            // ChatBoxInput
            // 
            this.ChatBoxInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.ChatBoxInput.ForeColor = System.Drawing.Color.White;
            this.ChatBoxInput.Location = new System.Drawing.Point(108, 286);
            this.ChatBoxInput.MaxLength = 59;
            this.ChatBoxInput.Name = "ChatBoxInput";
            this.ChatBoxInput.Size = new System.Drawing.Size(363, 20);
            this.ChatBoxInput.TabIndex = 2;
            this.ChatBoxInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatBoxInput_KeyDown);
            // 
            // tbUserName
            // 
            this.tbUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(62)))));
            this.tbUserName.ForeColor = System.Drawing.Color.White;
            this.tbUserName.Location = new System.Drawing.Point(16, 286);
            this.tbUserName.MaxLength = 12;
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.ReadOnly = true;
            this.tbUserName.Size = new System.Drawing.Size(86, 20);
            this.tbUserName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.label1.Location = new System.Drawing.Point(16, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Chatbox";
            // 
            // listUsers
            // 
            this.listUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.listUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listUsers.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.listUsers.FormattingEnabled = true;
            this.listUsers.ItemHeight = 16;
            this.listUsers.Location = new System.Drawing.Point(478, 66);
            this.listUsers.Name = "listUsers";
            this.listUsers.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listUsers.Size = new System.Drawing.Size(152, 208);
            this.listUsers.Sorted = true;
            this.listUsers.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.label2.Location = new System.Drawing.Point(516, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Online Users";
            // 
            // Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 317);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listUsers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.ChatBoxInput);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.btMinimize);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Chat";
            this.Text = "Chat";
            this.SizeChanged += new System.EventHandler(this.Chat_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Chat_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Chat_MouseMove);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btMinimize;
        private System.Windows.Forms.TextBox ChatBoxInput;
        public System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.RichTextBox ChatBox;
        public System.Windows.Forms.ListBox listUsers;
        private System.Windows.Forms.Label label2;
    }
}