namespace Script
{
    partial class InputScriptEditor
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
            this.btHelpList = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btTest = new System.Windows.Forms.Button();
            this.rtbScript = new Script.RichTextBoxExtended();
            this.timerErrorHint = new System.Windows.Forms.Timer(this.components);
            this.rtbErrorReason = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btHelpList
            // 
            this.btHelpList.Location = new System.Drawing.Point(12, 12);
            this.btHelpList.Name = "btHelpList";
            this.btHelpList.Size = new System.Drawing.Size(75, 23);
            this.btHelpList.TabIndex = 0;
            this.btHelpList.Text = "Help List";
            this.btHelpList.UseVisualStyleBackColor = true;
            this.btHelpList.Click += new System.EventHandler(this.btHelpList_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(247, 432);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(227, 23);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(12, 432);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(229, 23);
            this.btTest.TabIndex = 0;
            this.btTest.Text = "Test";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // rtbScript
            // 
            this.rtbScript.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.rtbScript.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.rtbScript.Location = new System.Drawing.Point(12, 41);
            this.rtbScript.Name = "rtbScript";
            this.rtbScript.Size = new System.Drawing.Size(462, 344);
            this.rtbScript.TabIndex = 1;
            this.rtbScript.Text = "";
            this.rtbScript.OnDraw += new Script.RichTextBoxExtended.DrawFieldEventHandler(this.rtbScript_OnDraw);
            this.rtbScript.TextChanged += new System.EventHandler(this.rtbScript_TextChanged);
            // 
            // timerErrorHint
            // 
            this.timerErrorHint.Enabled = true;
            this.timerErrorHint.Interval = 10;
            this.timerErrorHint.Tick += new System.EventHandler(this.timerErrorHint_Tick);
            // 
            // rtbErrorReason
            // 
            this.rtbErrorReason.Location = new System.Drawing.Point(13, 392);
            this.rtbErrorReason.Name = "rtbErrorReason";
            this.rtbErrorReason.ReadOnly = true;
            this.rtbErrorReason.Size = new System.Drawing.Size(461, 34);
            this.rtbErrorReason.TabIndex = 2;
            this.rtbErrorReason.Text = "";
            // 
            // InputScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 467);
            this.Controls.Add(this.rtbErrorReason);
            this.Controls.Add(this.rtbScript);
            this.Controls.Add(this.btTest);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btHelpList);
            this.Name = "InputScriptEditor";
            this.Text = "InputScriptEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btHelpList;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btTest;
        private Script.RichTextBoxExtended rtbScript;
        private System.Windows.Forms.Timer timerErrorHint;
        private System.Windows.Forms.RichTextBox rtbErrorReason;
    }
}