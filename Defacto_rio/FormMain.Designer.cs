namespace Defacto_rio
{
    partial class FormMain
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
            this.btItems = new System.Windows.Forms.Button();
            this.btGroups = new System.Windows.Forms.Button();
            this.btSubGroups = new System.Windows.Forms.Button();
            this.lbProjectName = new System.Windows.Forms.Label();
            this.btCreate = new System.Windows.Forms.Button();
            this.btOpen = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btRecipes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btItems
            // 
            this.btItems.Location = new System.Drawing.Point(15, 235);
            this.btItems.Margin = new System.Windows.Forms.Padding(6);
            this.btItems.Name = "btItems";
            this.btItems.Size = new System.Drawing.Size(212, 44);
            this.btItems.TabIndex = 0;
            this.btItems.Text = "ITEMS";
            this.btItems.UseVisualStyleBackColor = true;
            this.btItems.Click += new System.EventHandler(this.btItems_Click);
            // 
            // btGroups
            // 
            this.btGroups.Location = new System.Drawing.Point(15, 179);
            this.btGroups.Margin = new System.Windows.Forms.Padding(6);
            this.btGroups.Name = "btGroups";
            this.btGroups.Size = new System.Drawing.Size(212, 44);
            this.btGroups.TabIndex = 0;
            this.btGroups.Text = "GROUPS";
            this.btGroups.UseVisualStyleBackColor = true;
            this.btGroups.Click += new System.EventHandler(this.btGroups_Click);
            // 
            // btSubGroups
            // 
            this.btSubGroups.Location = new System.Drawing.Point(239, 179);
            this.btSubGroups.Margin = new System.Windows.Forms.Padding(6);
            this.btSubGroups.Name = "btSubGroups";
            this.btSubGroups.Size = new System.Drawing.Size(212, 44);
            this.btSubGroups.TabIndex = 0;
            this.btSubGroups.Text = "SUBGROUPS";
            this.btSubGroups.UseVisualStyleBackColor = true;
            this.btSubGroups.Click += new System.EventHandler(this.btSubGroups_Click);
            // 
            // lbProjectName
            // 
            this.lbProjectName.AutoSize = true;
            this.lbProjectName.Location = new System.Drawing.Point(350, 29);
            this.lbProjectName.Name = "lbProjectName";
            this.lbProjectName.Size = new System.Drawing.Size(101, 25);
            this.lbProjectName.TabIndex = 1;
            this.lbProjectName.Text = "No project";
            // 
            // btCreate
            // 
            this.btCreate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btCreate.Location = new System.Drawing.Point(237, 70);
            this.btCreate.Margin = new System.Windows.Forms.Padding(6);
            this.btCreate.Name = "btCreate";
            this.btCreate.Size = new System.Drawing.Size(101, 35);
            this.btCreate.TabIndex = 0;
            this.btCreate.Text = "Create";
            this.btCreate.UseVisualStyleBackColor = true;
            this.btCreate.Click += new System.EventHandler(this.btCreate_Click);
            // 
            // btOpen
            // 
            this.btOpen.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btOpen.Location = new System.Drawing.Point(350, 70);
            this.btOpen.Margin = new System.Windows.Forms.Padding(6);
            this.btOpen.Name = "btOpen";
            this.btOpen.Size = new System.Drawing.Size(101, 35);
            this.btOpen.TabIndex = 0;
            this.btOpen.Text = "Open";
            this.btOpen.UseVisualStyleBackColor = true;
            this.btOpen.Click += new System.EventHandler(this.btOpen_Click);
            // 
            // btSave
            // 
            this.btSave.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btSave.Location = new System.Drawing.Point(463, 70);
            this.btSave.Margin = new System.Windows.Forms.Padding(6);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(101, 35);
            this.btSave.TabIndex = 0;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btRecipes
            // 
            this.btRecipes.Location = new System.Drawing.Point(237, 235);
            this.btRecipes.Margin = new System.Windows.Forms.Padding(6);
            this.btRecipes.Name = "btRecipes";
            this.btRecipes.Size = new System.Drawing.Size(212, 44);
            this.btRecipes.TabIndex = 0;
            this.btRecipes.Text = "RECIPES";
            this.btRecipes.UseVisualStyleBackColor = true;
            this.btRecipes.Click += new System.EventHandler(this.btRecipes_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 404);
            this.Controls.Add(this.lbProjectName);
            this.Controls.Add(this.btSubGroups);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btOpen);
            this.Controls.Add(this.btCreate);
            this.Controls.Add(this.btGroups);
            this.Controls.Add(this.btRecipes);
            this.Controls.Add(this.btItems);
            this.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Defacto.rio";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btItems;
        private System.Windows.Forms.Button btGroups;
        private System.Windows.Forms.Button btSubGroups;
        private System.Windows.Forms.Label lbProjectName;
        private System.Windows.Forms.Button btCreate;
        private System.Windows.Forms.Button btOpen;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btRecipes;
    }
}

