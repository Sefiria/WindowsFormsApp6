namespace ObjectHierarchyBuilder
{
    partial class Form1
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
            this.treeView = new System.Windows.Forms.TreeView();
            this.btAddRoot = new System.Windows.Forms.Button();
            this.btAddChild = new System.Windows.Forms.Button();
            this.lbName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.cbbType = new System.Windows.Forms.ComboBox();
            this.lbType = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btRemoveType = new System.Windows.Forms.Button();
            this.btAddType = new System.Windows.Forms.Button();
            this.listType = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btSaveTree = new System.Windows.Forms.Button();
            this.btLoadTree = new System.Windows.Forms.Button();
            this.btClearTree = new System.Windows.Forms.Button();
            this.btRemoveSubTree = new System.Windows.Forms.Button();
            this.btRemoveSelection = new System.Windows.Forms.Button();
            this.tbType = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.picColor = new System.Windows.Forms.PictureBox();
            this.lbColor = new System.Windows.Forms.Label();
            this.lbBackColor = new System.Windows.Forms.Label();
            this.picBackColor = new System.Windows.Forms.PictureBox();
            this.btApply = new System.Windows.Forms.Button();
            this.cbbFontStyle = new System.Windows.Forms.ComboBox();
            this.lbFontStyle = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.btApplyNodeModifications = new System.Windows.Forms.Button();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.lbDescription = new System.Windows.Forms.Label();
            this.lbIndent = new System.Windows.Forms.Label();
            this.numIndent = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numSize = new System.Windows.Forms.NumericUpDown();
            this.btCreateFolders = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIndent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSize)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeView.HideSelection = false;
            this.treeView.Indent = 15;
            this.treeView.Location = new System.Drawing.Point(13, 13);
            this.treeView.Name = "treeView";
            this.treeView.ShowPlusMinus = false;
            this.treeView.Size = new System.Drawing.Size(500, 500);
            this.treeView.TabIndex = 0;
            this.treeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_DrawNode);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView_KeyDown);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            this.treeView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.treeView_PreviewKeyDown);
            // 
            // btAddRoot
            // 
            this.btAddRoot.Location = new System.Drawing.Point(522, 44);
            this.btAddRoot.Name = "btAddRoot";
            this.btAddRoot.Size = new System.Drawing.Size(238, 23);
            this.btAddRoot.TabIndex = 1;
            this.btAddRoot.Text = "Add Root";
            this.btAddRoot.UseVisualStyleBackColor = true;
            this.btAddRoot.Click += new System.EventHandler(this.btAddRoot_Click);
            // 
            // btAddChild
            // 
            this.btAddChild.Location = new System.Drawing.Point(522, 73);
            this.btAddChild.Name = "btAddChild";
            this.btAddChild.Size = new System.Drawing.Size(238, 23);
            this.btAddChild.TabIndex = 1;
            this.btAddChild.Text = "Add Child";
            this.btAddChild.UseVisualStyleBackColor = true;
            this.btAddChild.Click += new System.EventHandler(this.btAddChild_Click);
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(547, 105);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(35, 13);
            this.lbName.TabIndex = 2;
            this.lbName.Text = "Name";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(588, 102);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(171, 20);
            this.tbName.TabIndex = 3;
            this.tbName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbName_KeyDown);
            // 
            // cbbType
            // 
            this.cbbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbType.FormattingEnabled = true;
            this.cbbType.Location = new System.Drawing.Point(588, 129);
            this.cbbType.Name = "cbbType";
            this.cbbType.Size = new System.Drawing.Size(171, 21);
            this.cbbType.TabIndex = 4;
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Location = new System.Drawing.Point(547, 132);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(31, 13);
            this.lbType.TabIndex = 2;
            this.lbType.Text = "Type";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(522, 250);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(240, 3);
            this.label3.TabIndex = 2;
            // 
            // btRemoveType
            // 
            this.btRemoveType.Location = new System.Drawing.Point(654, 256);
            this.btRemoveType.Name = "btRemoveType";
            this.btRemoveType.Size = new System.Drawing.Size(110, 23);
            this.btRemoveType.TabIndex = 5;
            this.btRemoveType.Text = "Remove Type";
            this.btRemoveType.UseVisualStyleBackColor = true;
            this.btRemoveType.Click += new System.EventHandler(this.btRemoveType_Click);
            // 
            // btAddType
            // 
            this.btAddType.Location = new System.Drawing.Point(523, 256);
            this.btAddType.Name = "btAddType";
            this.btAddType.Size = new System.Drawing.Size(115, 23);
            this.btAddType.TabIndex = 5;
            this.btAddType.Text = "Add Type";
            this.btAddType.UseVisualStyleBackColor = true;
            this.btAddType.Click += new System.EventHandler(this.btAddType_Click);
            // 
            // listType
            // 
            this.listType.FormattingEnabled = true;
            this.listType.Location = new System.Drawing.Point(655, 285);
            this.listType.Name = "listType";
            this.listType.Size = new System.Drawing.Size(109, 95);
            this.listType.TabIndex = 6;
            this.listType.SelectedIndexChanged += new System.EventHandler(this.listType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(645, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(3, 255);
            this.label4.TabIndex = 7;
            // 
            // btSaveTree
            // 
            this.btSaveTree.Location = new System.Drawing.Point(654, 399);
            this.btSaveTree.Name = "btSaveTree";
            this.btSaveTree.Size = new System.Drawing.Size(109, 23);
            this.btSaveTree.TabIndex = 8;
            this.btSaveTree.Text = "Save Tree";
            this.btSaveTree.UseVisualStyleBackColor = true;
            this.btSaveTree.Click += new System.EventHandler(this.btSaveTree_Click);
            // 
            // btLoadTree
            // 
            this.btLoadTree.Location = new System.Drawing.Point(654, 428);
            this.btLoadTree.Name = "btLoadTree";
            this.btLoadTree.Size = new System.Drawing.Size(109, 23);
            this.btLoadTree.TabIndex = 8;
            this.btLoadTree.Text = "Load Tree";
            this.btLoadTree.UseVisualStyleBackColor = true;
            this.btLoadTree.Click += new System.EventHandler(this.btLoadTree_Click);
            // 
            // btClearTree
            // 
            this.btClearTree.Location = new System.Drawing.Point(655, 457);
            this.btClearTree.Name = "btClearTree";
            this.btClearTree.Size = new System.Drawing.Size(109, 23);
            this.btClearTree.TabIndex = 8;
            this.btClearTree.Text = "Clear Tree";
            this.btClearTree.UseVisualStyleBackColor = true;
            this.btClearTree.Click += new System.EventHandler(this.btClearTree_Click);
            // 
            // btRemoveSubTree
            // 
            this.btRemoveSubTree.Location = new System.Drawing.Point(523, 214);
            this.btRemoveSubTree.Name = "btRemoveSubTree";
            this.btRemoveSubTree.Size = new System.Drawing.Size(238, 23);
            this.btRemoveSubTree.TabIndex = 1;
            this.btRemoveSubTree.Text = "Remove SubTree";
            this.btRemoveSubTree.UseVisualStyleBackColor = true;
            this.btRemoveSubTree.Click += new System.EventHandler(this.btRemoveSubTree_Click);
            // 
            // btRemoveSelection
            // 
            this.btRemoveSelection.Location = new System.Drawing.Point(523, 185);
            this.btRemoveSelection.Name = "btRemoveSelection";
            this.btRemoveSelection.Size = new System.Drawing.Size(238, 23);
            this.btRemoveSelection.TabIndex = 1;
            this.btRemoveSelection.Text = "Remove Selection";
            this.btRemoveSelection.UseVisualStyleBackColor = true;
            this.btRemoveSelection.Click += new System.EventHandler(this.btRemoveSelection_Click);
            // 
            // tbType
            // 
            this.tbType.Location = new System.Drawing.Point(555, 285);
            this.tbType.MaxLength = 1;
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(25, 20);
            this.tbType.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(522, 288);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Type";
            // 
            // picColor
            // 
            this.picColor.BackColor = System.Drawing.Color.Black;
            this.picColor.Location = new System.Drawing.Point(555, 311);
            this.picColor.Name = "picColor";
            this.picColor.Size = new System.Drawing.Size(80, 20);
            this.picColor.TabIndex = 9;
            this.picColor.TabStop = false;
            this.picColor.Click += new System.EventHandler(this.picColor_Click);
            // 
            // lbColor
            // 
            this.lbColor.AutoSize = true;
            this.lbColor.Location = new System.Drawing.Point(519, 313);
            this.lbColor.Name = "lbColor";
            this.lbColor.Size = new System.Drawing.Size(31, 13);
            this.lbColor.TabIndex = 2;
            this.lbColor.Text = "Front";
            // 
            // lbBackColor
            // 
            this.lbBackColor.AutoSize = true;
            this.lbBackColor.Location = new System.Drawing.Point(519, 340);
            this.lbBackColor.Name = "lbBackColor";
            this.lbBackColor.Size = new System.Drawing.Size(32, 13);
            this.lbBackColor.TabIndex = 2;
            this.lbBackColor.Text = "Back";
            // 
            // picBackColor
            // 
            this.picBackColor.BackColor = System.Drawing.Color.White;
            this.picBackColor.Location = new System.Drawing.Point(555, 337);
            this.picBackColor.Name = "picBackColor";
            this.picBackColor.Size = new System.Drawing.Size(80, 20);
            this.picBackColor.TabIndex = 9;
            this.picBackColor.TabStop = false;
            this.picBackColor.Click += new System.EventHandler(this.picBackColor_Click);
            // 
            // btApply
            // 
            this.btApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btApply.Location = new System.Drawing.Point(519, 486);
            this.btApply.Name = "btApply";
            this.btApply.Size = new System.Drawing.Size(115, 23);
            this.btApply.TabIndex = 5;
            this.btApply.Text = "Apply";
            this.btApply.UseVisualStyleBackColor = true;
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // cbbFontStyle
            // 
            this.cbbFontStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbFontStyle.FormattingEnabled = true;
            this.cbbFontStyle.Location = new System.Drawing.Point(552, 368);
            this.cbbFontStyle.Name = "cbbFontStyle";
            this.cbbFontStyle.Size = new System.Drawing.Size(83, 21);
            this.cbbFontStyle.TabIndex = 10;
            // 
            // lbFontStyle
            // 
            this.lbFontStyle.AutoSize = true;
            this.lbFontStyle.Location = new System.Drawing.Point(519, 371);
            this.lbFontStyle.Name = "lbFontStyle";
            this.lbFontStyle.Size = new System.Drawing.Size(30, 13);
            this.lbFontStyle.TabIndex = 2;
            this.lbFontStyle.Text = "Style";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Tree files|*.tree";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(654, 389);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 3);
            this.label1.TabIndex = 2;
            // 
            // btApplyNodeModifications
            // 
            this.btApplyNodeModifications.Location = new System.Drawing.Point(523, 156);
            this.btApplyNodeModifications.Name = "btApplyNodeModifications";
            this.btApplyNodeModifications.Size = new System.Drawing.Size(238, 23);
            this.btApplyNodeModifications.TabIndex = 1;
            this.btApplyNodeModifications.Text = "Apply";
            this.btApplyNodeModifications.UseVisualStyleBackColor = true;
            this.btApplyNodeModifications.Click += new System.EventHandler(this.btApplyNodeModifications_Click);
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(522, 415);
            this.tbDescription.Multiline = true;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(112, 65);
            this.tbDescription.TabIndex = 11;
            // 
            // lbDescription
            // 
            this.lbDescription.AutoSize = true;
            this.lbDescription.Location = new System.Drawing.Point(546, 399);
            this.lbDescription.Name = "lbDescription";
            this.lbDescription.Size = new System.Drawing.Size(60, 13);
            this.lbDescription.TabIndex = 2;
            this.lbDescription.Text = "Description";
            // 
            // lbIndent
            // 
            this.lbIndent.AutoSize = true;
            this.lbIndent.Location = new System.Drawing.Point(519, 20);
            this.lbIndent.Name = "lbIndent";
            this.lbIndent.Size = new System.Drawing.Size(37, 13);
            this.lbIndent.TabIndex = 2;
            this.lbIndent.Text = "Indent";
            // 
            // numIndent
            // 
            this.numIndent.Location = new System.Drawing.Point(562, 18);
            this.numIndent.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numIndent.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numIndent.Name = "numIndent";
            this.numIndent.Size = new System.Drawing.Size(48, 20);
            this.numIndent.TabIndex = 12;
            this.numIndent.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numIndent.ValueChanged += new System.EventHandler(this.numIndent_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(624, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Size";
            // 
            // numSize
            // 
            this.numSize.Location = new System.Drawing.Point(657, 18);
            this.numSize.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numSize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numSize.Name = "numSize";
            this.numSize.Size = new System.Drawing.Size(48, 20);
            this.numSize.TabIndex = 12;
            this.numSize.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numSize.ValueChanged += new System.EventHandler(this.numSize_ValueChanged);
            // 
            // btCreateFolders
            // 
            this.btCreateFolders.Location = new System.Drawing.Point(655, 486);
            this.btCreateFolders.Name = "btCreateFolders";
            this.btCreateFolders.Size = new System.Drawing.Size(109, 23);
            this.btCreateFolders.TabIndex = 8;
            this.btCreateFolders.Text = "Create Folders";
            this.btCreateFolders.UseVisualStyleBackColor = true;
            this.btCreateFolders.Click += new System.EventHandler(this.btCreateFolders_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 525);
            this.Controls.Add(this.numSize);
            this.Controls.Add(this.numIndent);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.cbbFontStyle);
            this.Controls.Add(this.picBackColor);
            this.Controls.Add(this.picColor);
            this.Controls.Add(this.btCreateFolders);
            this.Controls.Add(this.btClearTree);
            this.Controls.Add(this.btLoadTree);
            this.Controls.Add(this.btSaveTree);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listType);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.btAddType);
            this.Controls.Add(this.btRemoveType);
            this.Controls.Add(this.cbbType);
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lbBackColor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbFontStyle);
            this.Controls.Add(this.lbColor);
            this.Controls.Add(this.lbDescription);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbType);
            this.Controls.Add(this.lbIndent);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.btApplyNodeModifications);
            this.Controls.Add(this.btRemoveSelection);
            this.Controls.Add(this.btRemoveSubTree);
            this.Controls.Add(this.btAddChild);
            this.Controls.Add(this.btAddRoot);
            this.Controls.Add(this.treeView);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numIndent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Button btAddRoot;
        private System.Windows.Forms.Button btAddChild;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.ComboBox cbbType;
        private System.Windows.Forms.Label lbType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btRemoveType;
        private System.Windows.Forms.Button btAddType;
        private System.Windows.Forms.ListBox listType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btSaveTree;
        private System.Windows.Forms.Button btLoadTree;
        private System.Windows.Forms.Button btClearTree;
        private System.Windows.Forms.Button btRemoveSubTree;
        private System.Windows.Forms.Button btRemoveSelection;
        private System.Windows.Forms.TextBox tbType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.PictureBox picColor;
        private System.Windows.Forms.Label lbColor;
        private System.Windows.Forms.Label lbBackColor;
        private System.Windows.Forms.PictureBox picBackColor;
        private System.Windows.Forms.Button btApply;
        private System.Windows.Forms.ComboBox cbbFontStyle;
        private System.Windows.Forms.Label lbFontStyle;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btApplyNodeModifications;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label lbDescription;
        private System.Windows.Forms.Label lbIndent;
        private System.Windows.Forms.NumericUpDown numIndent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numSize;
        private System.Windows.Forms.Button btCreateFolders;
    }
}

