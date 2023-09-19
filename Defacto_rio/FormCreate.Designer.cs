namespace Defacto_rio
{
    partial class FormCreate
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
            this.btValidate = new System.Windows.Forms.Button();
            this.lbName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.lbError = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.numMajor = new System.Windows.Forms.NumericUpDown();
            this.numMinor = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numBuild = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numMajor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBuild)).BeginInit();
            this.SuspendLayout();
            // 
            // btValidate
            // 
            this.btValidate.Location = new System.Drawing.Point(28, 214);
            this.btValidate.Margin = new System.Windows.Forms.Padding(6);
            this.btValidate.Name = "btValidate";
            this.btValidate.Size = new System.Drawing.Size(138, 44);
            this.btValidate.TabIndex = 0;
            this.btValidate.Text = "Validate";
            this.btValidate.UseVisualStyleBackColor = true;
            this.btValidate.Click += new System.EventHandler(this.btValidate_Click);
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(23, 36);
            this.lbName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(115, 25);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "Mod Name :";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(140, 33);
            this.tbName.Margin = new System.Windows.Forms.Padding(6);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(302, 32);
            this.tbName.TabIndex = 2;
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(304, 214);
            this.btCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(138, 44);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // lbError
            // 
            this.lbError.AutoSize = true;
            this.lbError.Location = new System.Drawing.Point(23, 154);
            this.lbError.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(88, 25);
            this.lbError.TabIndex = 1;
            this.lbError.Text = "No error.";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(23, 92);
            this.lbVersion.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(128, 25);
            this.lbVersion.TabIndex = 1;
            this.lbVersion.Text = "Mod Version :";
            // 
            // numMajor
            // 
            this.numMajor.Location = new System.Drawing.Point(150, 90);
            this.numMajor.Name = "numMajor";
            this.numMajor.Size = new System.Drawing.Size(53, 32);
            this.numMajor.TabIndex = 3;
            // 
            // numMinor
            // 
            this.numMinor.Location = new System.Drawing.Point(236, 90);
            this.numMinor.Name = "numMinor";
            this.numMinor.Size = new System.Drawing.Size(53, 32);
            this.numMinor.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(212, 92);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(298, 92);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = ".";
            // 
            // numBuild
            // 
            this.numBuild.Location = new System.Drawing.Point(324, 90);
            this.numBuild.Name = "numBuild";
            this.numBuild.Size = new System.Drawing.Size(53, 32);
            this.numBuild.TabIndex = 3;
            // 
            // FormCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 273);
            this.Controls.Add(this.numBuild);
            this.Controls.Add(this.numMinor);
            this.Controls.Add(this.numMajor);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbError);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btValidate);
            this.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormCreate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormCreate";
            ((System.ComponentModel.ISupportInitialize)(this.numMajor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBuild)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btValidate;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lbError;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tbName;
        public System.Windows.Forms.NumericUpDown numMajor;
        public System.Windows.Forms.NumericUpDown numMinor;
        public System.Windows.Forms.NumericUpDown numBuild;
    }
}