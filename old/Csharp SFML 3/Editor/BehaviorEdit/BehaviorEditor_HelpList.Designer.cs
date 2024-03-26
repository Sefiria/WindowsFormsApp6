namespace Editor.BehaviorEdit
{
    partial class BehaviorEditor_HelpList
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.rtbDescription = new System.Windows.Forms.RichTextBox();
            this.btPublicVariables = new System.Windows.Forms.Button();
            this.btProperties = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Location = new System.Drawing.Point(12, 41);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(402, 317);
            this.tabControl.TabIndex = 0;
            // 
            // rtbDescription
            // 
            this.rtbDescription.Location = new System.Drawing.Point(12, 365);
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.ReadOnly = true;
            this.rtbDescription.Size = new System.Drawing.Size(402, 52);
            this.rtbDescription.TabIndex = 1;
            this.rtbDescription.Text = "";
            // 
            // btPublicVariables
            // 
            this.btPublicVariables.Location = new System.Drawing.Point(12, 12);
            this.btPublicVariables.Name = "btPublicVariables";
            this.btPublicVariables.Size = new System.Drawing.Size(122, 23);
            this.btPublicVariables.TabIndex = 2;
            this.btPublicVariables.Text = "Public Variables";
            this.btPublicVariables.UseVisualStyleBackColor = true;
            this.btPublicVariables.Click += new System.EventHandler(this.btPublicVariables_Click);
            // 
            // btProperties
            // 
            this.btProperties.Location = new System.Drawing.Point(140, 12);
            this.btProperties.Name = "btProperties";
            this.btProperties.Size = new System.Drawing.Size(122, 23);
            this.btProperties.TabIndex = 2;
            this.btProperties.Text = "Properties";
            this.btProperties.UseVisualStyleBackColor = true;
            this.btProperties.Click += new System.EventHandler(this.btProperties_Click);
            // 
            // BehaviorEditor_HelpList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 429);
            this.Controls.Add(this.btProperties);
            this.Controls.Add(this.btPublicVariables);
            this.Controls.Add(this.rtbDescription);
            this.Controls.Add(this.tabControl);
            this.Name = "BehaviorEditor_HelpList";
            this.Text = "BehaviorEditor_HelpList";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.RichTextBox rtbDescription;
        private System.Windows.Forms.Button btPublicVariables;
        private System.Windows.Forms.Button btProperties;
    }
}