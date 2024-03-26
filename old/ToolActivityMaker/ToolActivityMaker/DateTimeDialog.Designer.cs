namespace ToolActivityMaker
{
    partial class DateTimeDialog
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
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.dtValue = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(12, 38);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(93, 38);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // dtValue
            // 
            this.dtValue.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtValue.Location = new System.Drawing.Point(13, 13);
            this.dtValue.MaxDate = new System.DateTime(2199, 12, 31, 0, 0, 0, 0);
            this.dtValue.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.dtValue.Name = "dtValue";
            this.dtValue.Size = new System.Drawing.Size(155, 20);
            this.dtValue.TabIndex = 3;
            // 
            // DateTimeDialog
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(180, 70);
            this.Controls.Add(this.dtValue);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DateTimeDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SetALabelThere";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        public System.Windows.Forms.DateTimePicker dtValue;
    }
}