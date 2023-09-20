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
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbAuthor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numFBuild = new System.Windows.Forms.NumericUpDown();
            this.numFMin = new System.Windows.Forms.NumericUpDown();
            this.numFMaj = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.rtbDescription = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numMajor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBuild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFBuild)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFMaj)).BeginInit();
            this.SuspendLayout();
            // 
            // btValidate
            // 
            this.btValidate.Location = new System.Drawing.Point(28, 543);
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
            this.tbName.Size = new System.Drawing.Size(553, 32);
            this.tbName.TabIndex = 2;
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(555, 543);
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
            this.lbError.Location = new System.Drawing.Point(23, 112);
            this.lbError.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(88, 25);
            this.lbError.TabIndex = 1;
            this.lbError.Text = "No error.";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(23, 71);
            this.lbVersion.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(128, 25);
            this.lbVersion.TabIndex = 1;
            this.lbVersion.Text = "Mod Version :";
            // 
            // numMajor
            // 
            this.numMajor.Location = new System.Drawing.Point(150, 69);
            this.numMajor.Name = "numMajor";
            this.numMajor.Size = new System.Drawing.Size(53, 32);
            this.numMajor.TabIndex = 3;
            // 
            // numMinor
            // 
            this.numMinor.Location = new System.Drawing.Point(236, 69);
            this.numMinor.Name = "numMinor";
            this.numMinor.Size = new System.Drawing.Size(53, 32);
            this.numMinor.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(212, 71);
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
            this.label2.Location = new System.Drawing.Point(298, 71);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = ".";
            // 
            // numBuild
            // 
            this.numBuild.Location = new System.Drawing.Point(324, 69);
            this.numBuild.Name = "numBuild";
            this.numBuild.Size = new System.Drawing.Size(53, 32);
            this.numBuild.TabIndex = 3;
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(140, 180);
            this.tbTitle.Margin = new System.Windows.Forms.Padding(6);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(553, 32);
            this.tbTitle.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 183);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Title :";
            // 
            // tbAuthor
            // 
            this.tbAuthor.Location = new System.Drawing.Point(140, 224);
            this.tbAuthor.Margin = new System.Windows.Forms.Padding(6);
            this.tbAuthor.Name = "tbAuthor";
            this.tbAuthor.Size = new System.Drawing.Size(553, 32);
            this.tbAuthor.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 227);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 25);
            this.label4.TabIndex = 6;
            this.label4.Text = "Author :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 271);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(156, 25);
            this.label5.TabIndex = 8;
            this.label5.Text = "Factorio Version :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 315);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 25);
            this.label6.TabIndex = 10;
            this.label6.Text = "Description :";
            // 
            // numFBuild
            // 
            this.numFBuild.Location = new System.Drawing.Point(350, 269);
            this.numFBuild.Name = "numFBuild";
            this.numFBuild.Size = new System.Drawing.Size(53, 32);
            this.numFBuild.TabIndex = 13;
            // 
            // numFMin
            // 
            this.numFMin.Location = new System.Drawing.Point(262, 269);
            this.numFMin.Name = "numFMin";
            this.numFMin.Size = new System.Drawing.Size(53, 32);
            this.numFMin.TabIndex = 14;
            // 
            // numFMaj
            // 
            this.numFMaj.Location = new System.Drawing.Point(176, 269);
            this.numFMaj.Name = "numFMaj";
            this.numFMaj.Size = new System.Drawing.Size(53, 32);
            this.numFMaj.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(324, 271);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 25);
            this.label7.TabIndex = 11;
            this.label7.Text = ".";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(238, 271);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 25);
            this.label8.TabIndex = 12;
            this.label8.Text = ".";
            // 
            // rtbDescription
            // 
            this.rtbDescription.Location = new System.Drawing.Point(150, 315);
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.Size = new System.Drawing.Size(543, 205);
            this.rtbDescription.TabIndex = 16;
            this.rtbDescription.Text = "";
            // 
            // FormCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 602);
            this.Controls.Add(this.rtbDescription);
            this.Controls.Add(this.numFBuild);
            this.Controls.Add(this.numFMin);
            this.Controls.Add(this.numFMaj);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbAuthor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbTitle);
            this.Controls.Add(this.label3);
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
            ((System.ComponentModel.ISupportInitialize)(this.numFBuild)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFMaj)).EndInit();
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
        public System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox tbAuthor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.NumericUpDown numFBuild;
        public System.Windows.Forms.NumericUpDown numFMin;
        public System.Windows.Forms.NumericUpDown numFMaj;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox rtbDescription;
    }
}