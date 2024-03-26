namespace Editor
{
    partial class EntityIDConflitsManager
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
            this.btReloadEntities = new System.Windows.Forms.Button();
            this.btSaveIDs = new System.Windows.Forms.Button();
            this.tree = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.numLayer = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numID = new System.Windows.Forms.NumericUpDown();
            this.btUnselect = new System.Windows.Forms.Button();
            this.rbtTree = new System.Windows.Forms.RadioButton();
            this.rbtSortedListLayer = new System.Windows.Forms.RadioButton();
            this.rbtSortedListID = new System.Windows.Forms.RadioButton();
            this.listSorted = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numSearchLayer = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numSearchID = new System.Windows.Forms.NumericUpDown();
            this.btFind = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchID)).BeginInit();
            this.SuspendLayout();
            // 
            // btReloadEntities
            // 
            this.btReloadEntities.Location = new System.Drawing.Point(12, 12);
            this.btReloadEntities.Name = "btReloadEntities";
            this.btReloadEntities.Size = new System.Drawing.Size(128, 23);
            this.btReloadEntities.TabIndex = 0;
            this.btReloadEntities.Text = "Reload Entities";
            this.btReloadEntities.UseVisualStyleBackColor = true;
            this.btReloadEntities.Click += new System.EventHandler(this.btReloadEntities_Click);
            // 
            // btSaveIDs
            // 
            this.btSaveIDs.Location = new System.Drawing.Point(146, 12);
            this.btSaveIDs.Name = "btSaveIDs";
            this.btSaveIDs.Size = new System.Drawing.Size(87, 23);
            this.btSaveIDs.TabIndex = 0;
            this.btSaveIDs.Text = "Save IDs";
            this.btSaveIDs.UseVisualStyleBackColor = true;
            this.btSaveIDs.Click += new System.EventHandler(this.btSaveIDs_Click);
            // 
            // tree
            // 
            this.tree.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tree.Indent = 15;
            this.tree.Location = new System.Drawing.Point(13, 42);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(220, 342);
            this.tree.TabIndex = 1;
            this.tree.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tree_DrawNode);
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(255, 261);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Layer";
            // 
            // numLayer
            // 
            this.numLayer.Enabled = false;
            this.numLayer.Location = new System.Drawing.Point(294, 259);
            this.numLayer.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numLayer.Name = "numLayer";
            this.numLayer.ReadOnly = true;
            this.numLayer.Size = new System.Drawing.Size(112, 20);
            this.numLayer.TabIndex = 3;
            this.numLayer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 287);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "ID";
            // 
            // numID
            // 
            this.numID.Enabled = false;
            this.numID.Location = new System.Drawing.Point(294, 285);
            this.numID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numID.Name = "numID";
            this.numID.Size = new System.Drawing.Size(112, 20);
            this.numID.TabIndex = 3;
            this.numID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numID.ValueChanged += new System.EventHandler(this.numID_ValueChanged);
            // 
            // btUnselect
            // 
            this.btUnselect.Location = new System.Drawing.Point(294, 204);
            this.btUnselect.Name = "btUnselect";
            this.btUnselect.Size = new System.Drawing.Size(112, 23);
            this.btUnselect.TabIndex = 0;
            this.btUnselect.Text = "Unselect";
            this.btUnselect.UseVisualStyleBackColor = true;
            this.btUnselect.Click += new System.EventHandler(this.btUnselect_Click);
            // 
            // rbtTree
            // 
            this.rbtTree.AutoSize = true;
            this.rbtTree.Checked = true;
            this.rbtTree.Location = new System.Drawing.Point(13, 397);
            this.rbtTree.Name = "rbtTree";
            this.rbtTree.Size = new System.Drawing.Size(47, 17);
            this.rbtTree.TabIndex = 4;
            this.rbtTree.TabStop = true;
            this.rbtTree.Text = "Tree";
            this.rbtTree.UseVisualStyleBackColor = true;
            this.rbtTree.CheckedChanged += new System.EventHandler(this.rbtTree_CheckedChanged);
            // 
            // rbtSortedListLayer
            // 
            this.rbtSortedListLayer.AutoSize = true;
            this.rbtSortedListLayer.Location = new System.Drawing.Point(66, 397);
            this.rbtSortedListLayer.Name = "rbtSortedListLayer";
            this.rbtSortedListLayer.Size = new System.Drawing.Size(84, 17);
            this.rbtSortedListLayer.TabIndex = 4;
            this.rbtSortedListLayer.Text = "List by Layer";
            this.rbtSortedListLayer.UseVisualStyleBackColor = true;
            this.rbtSortedListLayer.CheckedChanged += new System.EventHandler(this.rbtSortedListLayer_CheckedChanged);
            // 
            // rbtSortedListID
            // 
            this.rbtSortedListID.AutoSize = true;
            this.rbtSortedListID.Location = new System.Drawing.Point(156, 397);
            this.rbtSortedListID.Name = "rbtSortedListID";
            this.rbtSortedListID.Size = new System.Drawing.Size(69, 17);
            this.rbtSortedListID.TabIndex = 4;
            this.rbtSortedListID.Text = "List by ID";
            this.rbtSortedListID.UseVisualStyleBackColor = true;
            this.rbtSortedListID.CheckedChanged += new System.EventHandler(this.rbtSortedListID_CheckedChanged);
            // 
            // listSorted
            // 
            this.listSorted.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listSorted.FormattingEnabled = true;
            this.listSorted.Location = new System.Drawing.Point(13, 42);
            this.listSorted.Name = "listSorted";
            this.listSorted.Size = new System.Drawing.Size(220, 342);
            this.listSorted.TabIndex = 5;
            this.listSorted.Visible = false;
            this.listSorted.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listSorted_DrawItem);
            this.listSorted.SelectedIndexChanged += new System.EventHandler(this.listSorted_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(255, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Name";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(294, 233);
            this.tbName.Name = "tbName";
            this.tbName.ReadOnly = true;
            this.tbName.Size = new System.Drawing.Size(112, 20);
            this.tbName.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(324, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Search";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(291, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Layer";
            // 
            // numSearchLayer
            // 
            this.numSearchLayer.Location = new System.Drawing.Point(330, 84);
            this.numSearchLayer.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numSearchLayer.Name = "numSearchLayer";
            this.numSearchLayer.Size = new System.Drawing.Size(57, 20);
            this.numSearchLayer.TabIndex = 3;
            this.numSearchLayer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(291, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "ID";
            // 
            // numSearchID
            // 
            this.numSearchID.Location = new System.Drawing.Point(330, 110);
            this.numSearchID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numSearchID.Name = "numSearchID";
            this.numSearchID.Size = new System.Drawing.Size(57, 20);
            this.numSearchID.TabIndex = 3;
            this.numSearchID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btFind
            // 
            this.btFind.Location = new System.Drawing.Point(294, 136);
            this.btFind.Name = "btFind";
            this.btFind.Size = new System.Drawing.Size(93, 20);
            this.btFind.TabIndex = 7;
            this.btFind.Text = "Find";
            this.btFind.UseVisualStyleBackColor = true;
            this.btFind.Click += new System.EventHandler(this.btFind_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(270, 397);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Nb: The ID \'0\' is not allowed !";
            // 
            // EntityIDConflitsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 421);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btFind);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.rbtSortedListID);
            this.Controls.Add(this.rbtSortedListLayer);
            this.Controls.Add(this.rbtTree);
            this.Controls.Add(this.numID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numSearchID);
            this.Controls.Add(this.numSearchLayer);
            this.Controls.Add(this.numLayer);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btUnselect);
            this.Controls.Add(this.btSaveIDs);
            this.Controls.Add(this.btReloadEntities);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.listSorted);
            this.Name = "EntityIDConflitsManager";
            this.Text = "Entity ID Conflits Manager";
            ((System.ComponentModel.ISupportInitialize)(this.numLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSearchID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btReloadEntities;
        private System.Windows.Forms.Button btSaveIDs;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numLayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numID;
        private System.Windows.Forms.Button btUnselect;
        private System.Windows.Forms.RadioButton rbtTree;
        private System.Windows.Forms.RadioButton rbtSortedListLayer;
        private System.Windows.Forms.RadioButton rbtSortedListID;
        private System.Windows.Forms.ListBox listSorted;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numSearchLayer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numSearchID;
        private System.Windows.Forms.Button btFind;
        private System.Windows.Forms.Label label7;
    }
}