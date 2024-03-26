namespace WindowsFormsApp6
{
    partial class ListSelection
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
            this.listOnlineUsers = new System.Windows.Forms.ListBox();
            this.btSpy = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listOnlineUsers
            // 
            this.listOnlineUsers.Dock = System.Windows.Forms.DockStyle.Top;
            this.listOnlineUsers.FormattingEnabled = true;
            this.listOnlineUsers.Location = new System.Drawing.Point(0, 0);
            this.listOnlineUsers.Name = "listOnlineUsers";
            this.listOnlineUsers.Size = new System.Drawing.Size(183, 251);
            this.listOnlineUsers.TabIndex = 0;
            this.listOnlineUsers.SelectedIndexChanged += new System.EventHandler(this.listOnlineUsers_SelectedIndexChanged);
            // 
            // btSpy
            // 
            this.btSpy.Location = new System.Drawing.Point(12, 257);
            this.btSpy.Name = "btSpy";
            this.btSpy.Size = new System.Drawing.Size(75, 23);
            this.btSpy.TabIndex = 1;
            this.btSpy.Text = "Spy";
            this.btSpy.UseVisualStyleBackColor = true;
            this.btSpy.Click += new System.EventHandler(this.btSpy_Click);
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(93, 257);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // ListSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(183, 286);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btSpy);
            this.Controls.Add(this.listOnlineUsers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ListSelection";
            this.Text = "ListSelection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listOnlineUsers;
        private System.Windows.Forms.Button btSpy;
        private System.Windows.Forms.Button btCancel;
    }
}