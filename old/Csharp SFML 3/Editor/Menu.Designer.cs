namespace Editor
{
    partial class Menu
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
            this.btEntityEditor = new System.Windows.Forms.Button();
            this.btLevelEditor = new System.Windows.Forms.Button();
            this.btEntityIDConflictsManager = new System.Windows.Forms.Button();
            this.btBehaviorEditor = new System.Windows.Forms.Button();
            this.btPlayTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btEntityEditor
            // 
            this.btEntityEditor.Location = new System.Drawing.Point(13, 13);
            this.btEntityEditor.Name = "btEntityEditor";
            this.btEntityEditor.Size = new System.Drawing.Size(96, 35);
            this.btEntityEditor.TabIndex = 0;
            this.btEntityEditor.Text = "Entity Editor";
            this.btEntityEditor.UseVisualStyleBackColor = true;
            this.btEntityEditor.Click += new System.EventHandler(this.btEntityEditor_Click);
            // 
            // btLevelEditor
            // 
            this.btLevelEditor.Location = new System.Drawing.Point(115, 13);
            this.btLevelEditor.Name = "btLevelEditor";
            this.btLevelEditor.Size = new System.Drawing.Size(96, 35);
            this.btLevelEditor.TabIndex = 0;
            this.btLevelEditor.Text = "Level Editor";
            this.btLevelEditor.UseVisualStyleBackColor = true;
            this.btLevelEditor.Click += new System.EventHandler(this.btLevelEditor_Click);
            // 
            // btEntityIDConflictsManager
            // 
            this.btEntityIDConflictsManager.Location = new System.Drawing.Point(13, 54);
            this.btEntityIDConflictsManager.Name = "btEntityIDConflictsManager";
            this.btEntityIDConflictsManager.Size = new System.Drawing.Size(301, 35);
            this.btEntityIDConflictsManager.TabIndex = 0;
            this.btEntityIDConflictsManager.Text = "Entity ID Conflicts Manager";
            this.btEntityIDConflictsManager.UseVisualStyleBackColor = true;
            this.btEntityIDConflictsManager.Click += new System.EventHandler(this.btEntityIDConflictsManager_Click);
            // 
            // btBehaviorEditor
            // 
            this.btBehaviorEditor.Location = new System.Drawing.Point(217, 13);
            this.btBehaviorEditor.Name = "btBehaviorEditor";
            this.btBehaviorEditor.Size = new System.Drawing.Size(96, 35);
            this.btBehaviorEditor.TabIndex = 0;
            this.btBehaviorEditor.Text = "Behavior Editor";
            this.btBehaviorEditor.UseVisualStyleBackColor = true;
            this.btBehaviorEditor.Click += new System.EventHandler(this.btBehaviorEditor_Click);
            // 
            // btPlayTest
            // 
            this.btPlayTest.Location = new System.Drawing.Point(13, 95);
            this.btPlayTest.Name = "btPlayTest";
            this.btPlayTest.Size = new System.Drawing.Size(301, 35);
            this.btPlayTest.TabIndex = 0;
            this.btPlayTest.Text = "PlayTest";
            this.btPlayTest.UseVisualStyleBackColor = true;
            this.btPlayTest.Click += new System.EventHandler(this.btPlayTest_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 143);
            this.Controls.Add(this.btBehaviorEditor);
            this.Controls.Add(this.btLevelEditor);
            this.Controls.Add(this.btPlayTest);
            this.Controls.Add(this.btEntityIDConflictsManager);
            this.Controls.Add(this.btEntityEditor);
            this.Name = "Menu";
            this.Text = "Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btEntityEditor;
        private System.Windows.Forms.Button btLevelEditor;
        private System.Windows.Forms.Button btEntityIDConflictsManager;
        private System.Windows.Forms.Button btBehaviorEditor;
        private System.Windows.Forms.Button btPlayTest;
    }
}