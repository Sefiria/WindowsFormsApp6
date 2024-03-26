namespace Editor
{
    partial class EntityEditor
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
            this.timerDraw = new System.Windows.Forms.Timer(this.components);
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.cbbEntityLevel1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbbEntityLevel2 = new System.Windows.Forms.ComboBox();
            this.cbbEntityLevel3 = new System.Windows.Forms.ComboBox();
            this.cbbEntityLevel4 = new System.Windows.Forms.ComboBox();
            this.btCreate = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btClearBehavior = new System.Windows.Forms.Button();
            this.btSetBehavior = new System.Windows.Forms.Button();
            this.btEditBehavior = new System.Windows.Forms.Button();
            this.lbSelectedBehavior = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numLayer = new System.Windows.Forms.NumericUpDown();
            this.numID = new System.Windows.Forms.NumericUpDown();
            this.numHP = new System.Windows.Forms.NumericUpDown();
            this.cbGravityEffect = new System.Windows.Forms.CheckBox();
            this.cbTriggerCollision = new System.Windows.Forms.CheckBox();
            this.cbIndestructible = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupAutoTile = new System.Windows.Forms.GroupBox();
            this.btAutotileHideTemplate = new System.Windows.Forms.Button();
            this.RenderPanelAutoTile = new System.Windows.Forms.Panel();
            this.btAutotileShowTemplate = new System.Windows.Forms.Button();
            this.panelAutoTile = new System.Windows.Forms.Panel();
            this.btAutoTile1 = new System.Windows.Forms.Button();
            this.btAutoTile12 = new System.Windows.Forms.Button();
            this.btAutoTile2 = new System.Windows.Forms.Button();
            this.btAutoTile9 = new System.Windows.Forms.Button();
            this.btAutoTile4 = new System.Windows.Forms.Button();
            this.btAutoTile6 = new System.Windows.Forms.Button();
            this.btAutoTile5 = new System.Windows.Forms.Button();
            this.btAutoTile3 = new System.Windows.Forms.Button();
            this.btAutoTile7 = new System.Windows.Forms.Button();
            this.btAutoTile11 = new System.Windows.Forms.Button();
            this.btAutoTile8 = new System.Windows.Forms.Button();
            this.btAutoTile10 = new System.Windows.Forms.Button();
            this.cbAutoTile = new System.Windows.Forms.CheckBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btNew = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btLoad = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btPen = new System.Windows.Forms.Button();
            this.btEraser = new System.Windows.Forms.Button();
            this.btColorTransparent = new System.Windows.Forms.Button();
            this.btColorSave10 = new System.Windows.Forms.Button();
            this.btColorSave8 = new System.Windows.Forms.Button();
            this.btColorSave6 = new System.Windows.Forms.Button();
            this.btColorSave4 = new System.Windows.Forms.Button();
            this.btColorSave2 = new System.Windows.Forms.Button();
            this.btColorSave9 = new System.Windows.Forms.Button();
            this.btColorSave7 = new System.Windows.Forms.Button();
            this.btColorSave5 = new System.Windows.Forms.Button();
            this.btColorSave3 = new System.Windows.Forms.Button();
            this.btColorSave1 = new System.Windows.Forms.Button();
            this.btColor = new System.Windows.Forms.Button();
            this.btFill = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.btChange = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHP)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupAutoTile.SuspendLayout();
            this.panelAutoTile.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerDraw
            // 
            this.timerDraw.Enabled = true;
            this.timerDraw.Interval = 10;
            this.timerDraw.Tick += new System.EventHandler(this.timerDraw_Tick);
            // 
            // RenderPanel
            // 
            this.RenderPanel.Location = new System.Drawing.Point(10, 50);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(128, 128);
            this.RenderPanel.TabIndex = 0;
            this.RenderPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseDown);
            this.RenderPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseMove);
            // 
            // cbbEntityLevel1
            // 
            this.cbbEntityLevel1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbEntityLevel1.FormattingEnabled = true;
            this.cbbEntityLevel1.Location = new System.Drawing.Point(51, 13);
            this.cbbEntityLevel1.Name = "cbbEntityLevel1";
            this.cbbEntityLevel1.Size = new System.Drawing.Size(92, 21);
            this.cbbEntityLevel1.TabIndex = 1;
            this.cbbEntityLevel1.SelectedIndexChanged += new System.EventHandler(this.cbbEntityLevel1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Entity";
            // 
            // cbbEntityLevel2
            // 
            this.cbbEntityLevel2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbEntityLevel2.Enabled = false;
            this.cbbEntityLevel2.FormattingEnabled = true;
            this.cbbEntityLevel2.Location = new System.Drawing.Point(149, 13);
            this.cbbEntityLevel2.Name = "cbbEntityLevel2";
            this.cbbEntityLevel2.Size = new System.Drawing.Size(92, 21);
            this.cbbEntityLevel2.TabIndex = 2;
            this.cbbEntityLevel2.SelectedIndexChanged += new System.EventHandler(this.cbbEntityLevel2_SelectedIndexChanged);
            // 
            // cbbEntityLevel3
            // 
            this.cbbEntityLevel3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbEntityLevel3.Enabled = false;
            this.cbbEntityLevel3.FormattingEnabled = true;
            this.cbbEntityLevel3.Location = new System.Drawing.Point(247, 13);
            this.cbbEntityLevel3.Name = "cbbEntityLevel3";
            this.cbbEntityLevel3.Size = new System.Drawing.Size(92, 21);
            this.cbbEntityLevel3.TabIndex = 3;
            this.cbbEntityLevel3.SelectedIndexChanged += new System.EventHandler(this.cbbEntityLevel3_SelectedIndexChanged);
            // 
            // cbbEntityLevel4
            // 
            this.cbbEntityLevel4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbEntityLevel4.Enabled = false;
            this.cbbEntityLevel4.FormattingEnabled = true;
            this.cbbEntityLevel4.Location = new System.Drawing.Point(345, 13);
            this.cbbEntityLevel4.Name = "cbbEntityLevel4";
            this.cbbEntityLevel4.Size = new System.Drawing.Size(92, 21);
            this.cbbEntityLevel4.TabIndex = 4;
            this.cbbEntityLevel4.SelectedIndexChanged += new System.EventHandler(this.cbbEntityLevel4_SelectedIndexChanged);
            // 
            // btCreate
            // 
            this.btCreate.Enabled = false;
            this.btCreate.Location = new System.Drawing.Point(443, 13);
            this.btCreate.Name = "btCreate";
            this.btCreate.Size = new System.Drawing.Size(60, 21);
            this.btCreate.TabIndex = 5;
            this.btCreate.Text = "Create";
            this.btCreate.UseVisualStyleBackColor = true;
            this.btCreate.Click += new System.EventHandler(this.btCreate_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btClearBehavior);
            this.panel1.Controls.Add(this.btSetBehavior);
            this.panel1.Controls.Add(this.btEditBehavior);
            this.panel1.Controls.Add(this.lbSelectedBehavior);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.numLayer);
            this.panel1.Controls.Add(this.numID);
            this.panel1.Controls.Add(this.numHP);
            this.panel1.Controls.Add(this.cbGravityEffect);
            this.panel1.Controls.Add(this.cbTriggerCollision);
            this.panel1.Controls.Add(this.cbIndestructible);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.cbAutoTile);
            this.panel1.Controls.Add(this.tbName);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btNew);
            this.panel1.Controls.Add(this.btSave);
            this.panel1.Controls.Add(this.btLoad);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(13, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 403);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // btClearBehavior
            // 
            this.btClearBehavior.Location = new System.Drawing.Point(257, 246);
            this.btClearBehavior.Name = "btClearBehavior";
            this.btClearBehavior.Size = new System.Drawing.Size(233, 20);
            this.btClearBehavior.TabIndex = 14;
            this.btClearBehavior.Text = "Clear Behavior";
            this.btClearBehavior.UseVisualStyleBackColor = true;
            this.btClearBehavior.Click += new System.EventHandler(this.btClearBehavior_Click);
            // 
            // btSetBehavior
            // 
            this.btSetBehavior.Enabled = false;
            this.btSetBehavior.Location = new System.Drawing.Point(257, 275);
            this.btSetBehavior.Name = "btSetBehavior";
            this.btSetBehavior.Size = new System.Drawing.Size(233, 20);
            this.btSetBehavior.TabIndex = 14;
            this.btSetBehavior.Text = "Set Behavior";
            this.btSetBehavior.UseVisualStyleBackColor = true;
            this.btSetBehavior.Click += new System.EventHandler(this.btSetBehavior_Click);
            // 
            // btEditBehavior
            // 
            this.btEditBehavior.Enabled = false;
            this.btEditBehavior.Location = new System.Drawing.Point(257, 220);
            this.btEditBehavior.Name = "btEditBehavior";
            this.btEditBehavior.Size = new System.Drawing.Size(233, 20);
            this.btEditBehavior.TabIndex = 14;
            this.btEditBehavior.Text = "Edit Behavior";
            this.btEditBehavior.UseVisualStyleBackColor = true;
            this.btEditBehavior.Click += new System.EventHandler(this.btEditBehavior_Click);
            // 
            // lbSelectedBehavior
            // 
            this.lbSelectedBehavior.AutoSize = true;
            this.lbSelectedBehavior.Enabled = false;
            this.lbSelectedBehavior.Location = new System.Drawing.Point(254, 303);
            this.lbSelectedBehavior.Name = "lbSelectedBehavior";
            this.lbSelectedBehavior.Size = new System.Drawing.Size(114, 13);
            this.lbSelectedBehavior.TabIndex = 13;
            this.lbSelectedBehavior.Text = "Selected Behavior ID :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(320, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Layer";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(219, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(329, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "HP";
            // 
            // numLayer
            // 
            this.numLayer.Location = new System.Drawing.Point(358, 142);
            this.numLayer.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numLayer.Name = "numLayer";
            this.numLayer.Size = new System.Drawing.Size(66, 20);
            this.numLayer.TabIndex = 12;
            this.numLayer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // numID
            // 
            this.numID.Location = new System.Drawing.Point(248, 142);
            this.numID.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numID.Name = "numID";
            this.numID.Size = new System.Drawing.Size(66, 20);
            this.numID.TabIndex = 12;
            this.numID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numHP
            // 
            this.numHP.Location = new System.Drawing.Point(358, 172);
            this.numHP.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numHP.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHP.Name = "numHP";
            this.numHP.Size = new System.Drawing.Size(66, 20);
            this.numHP.TabIndex = 12;
            this.numHP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numHP.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbGravityEffect
            // 
            this.cbGravityEffect.AutoSize = true;
            this.cbGravityEffect.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbGravityEffect.Location = new System.Drawing.Point(327, 198);
            this.cbGravityEffect.Name = "cbGravityEffect";
            this.cbGravityEffect.Size = new System.Drawing.Size(87, 17);
            this.cbGravityEffect.TabIndex = 11;
            this.cbGravityEffect.Text = "GravityEffect";
            this.cbGravityEffect.UseVisualStyleBackColor = true;
            // 
            // cbTriggerCollision
            // 
            this.cbTriggerCollision.AutoSize = true;
            this.cbTriggerCollision.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbTriggerCollision.Location = new System.Drawing.Point(226, 198);
            this.cbTriggerCollision.Name = "cbTriggerCollision";
            this.cbTriggerCollision.Size = new System.Drawing.Size(97, 17);
            this.cbTriggerCollision.TabIndex = 11;
            this.cbTriggerCollision.Text = "TriggerCollision";
            this.cbTriggerCollision.UseVisualStyleBackColor = true;
            // 
            // cbIndestructible
            // 
            this.cbIndestructible.AutoSize = true;
            this.cbIndestructible.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbIndestructible.Location = new System.Drawing.Point(234, 173);
            this.cbIndestructible.Name = "cbIndestructible";
            this.cbIndestructible.Size = new System.Drawing.Size(89, 17);
            this.cbIndestructible.TabIndex = 11;
            this.cbIndestructible.Text = "Indestructible";
            this.cbIndestructible.UseVisualStyleBackColor = true;
            this.cbIndestructible.CheckedChanged += new System.EventHandler(this.cbIndestructible_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupAutoTile);
            this.groupBox1.Location = new System.Drawing.Point(3, 212);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 180);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // groupAutoTile
            // 
            this.groupAutoTile.Controls.Add(this.btAutotileHideTemplate);
            this.groupAutoTile.Controls.Add(this.RenderPanelAutoTile);
            this.groupAutoTile.Controls.Add(this.btAutotileShowTemplate);
            this.groupAutoTile.Controls.Add(this.panelAutoTile);
            this.groupAutoTile.Location = new System.Drawing.Point(6, 8);
            this.groupAutoTile.Name = "groupAutoTile";
            this.groupAutoTile.Size = new System.Drawing.Size(233, 166);
            this.groupAutoTile.TabIndex = 9;
            this.groupAutoTile.TabStop = false;
            this.groupAutoTile.Visible = false;
            // 
            // btAutotileHideTemplate
            // 
            this.btAutotileHideTemplate.Location = new System.Drawing.Point(56, 11);
            this.btAutotileHideTemplate.Name = "btAutotileHideTemplate";
            this.btAutotileHideTemplate.Size = new System.Drawing.Size(40, 22);
            this.btAutotileHideTemplate.TabIndex = 9;
            this.btAutotileHideTemplate.Text = "○";
            this.btAutotileHideTemplate.UseVisualStyleBackColor = true;
            this.btAutotileHideTemplate.Click += new System.EventHandler(this.btAutotileHideTemplate_Click);
            // 
            // RenderPanelAutoTile
            // 
            this.RenderPanelAutoTile.Location = new System.Drawing.Point(99, 32);
            this.RenderPanelAutoTile.Name = "RenderPanelAutoTile";
            this.RenderPanelAutoTile.Size = new System.Drawing.Size(128, 128);
            this.RenderPanelAutoTile.TabIndex = 0;
            this.RenderPanelAutoTile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseDown);
            this.RenderPanelAutoTile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseMove);
            // 
            // btAutotileShowTemplate
            // 
            this.btAutotileShowTemplate.Location = new System.Drawing.Point(6, 11);
            this.btAutotileShowTemplate.Name = "btAutotileShowTemplate";
            this.btAutotileShowTemplate.Size = new System.Drawing.Size(40, 22);
            this.btAutotileShowTemplate.TabIndex = 9;
            this.btAutotileShowTemplate.Text = "•";
            this.btAutotileShowTemplate.UseVisualStyleBackColor = true;
            this.btAutotileShowTemplate.Click += new System.EventHandler(this.btAutotileShowTemplate_Click);
            // 
            // panelAutoTile
            // 
            this.panelAutoTile.Controls.Add(this.btAutoTile1);
            this.panelAutoTile.Controls.Add(this.btAutoTile12);
            this.panelAutoTile.Controls.Add(this.btAutoTile2);
            this.panelAutoTile.Controls.Add(this.btAutoTile9);
            this.panelAutoTile.Controls.Add(this.btAutoTile4);
            this.panelAutoTile.Controls.Add(this.btAutoTile6);
            this.panelAutoTile.Controls.Add(this.btAutoTile5);
            this.panelAutoTile.Controls.Add(this.btAutoTile3);
            this.panelAutoTile.Controls.Add(this.btAutoTile7);
            this.panelAutoTile.Controls.Add(this.btAutoTile11);
            this.panelAutoTile.Controls.Add(this.btAutoTile8);
            this.panelAutoTile.Controls.Add(this.btAutoTile10);
            this.panelAutoTile.Location = new System.Drawing.Point(6, 39);
            this.panelAutoTile.Name = "panelAutoTile";
            this.panelAutoTile.Size = new System.Drawing.Size(90, 121);
            this.panelAutoTile.TabIndex = 8;
            // 
            // btAutoTile1
            // 
            this.btAutoTile1.BackColor = System.Drawing.Color.White;
            this.btAutoTile1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile1.Location = new System.Drawing.Point(3, 3);
            this.btAutoTile1.Name = "btAutoTile1";
            this.btAutoTile1.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile1.TabIndex = 1;
            this.btAutoTile1.UseVisualStyleBackColor = false;
            // 
            // btAutoTile12
            // 
            this.btAutoTile12.BackColor = System.Drawing.Color.White;
            this.btAutoTile12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile12.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile12.Location = new System.Drawing.Point(63, 93);
            this.btAutoTile12.Name = "btAutoTile12";
            this.btAutoTile12.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile12.TabIndex = 1;
            this.btAutoTile12.UseVisualStyleBackColor = false;
            // 
            // btAutoTile2
            // 
            this.btAutoTile2.BackColor = System.Drawing.Color.White;
            this.btAutoTile2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile2.Location = new System.Drawing.Point(33, 3);
            this.btAutoTile2.Name = "btAutoTile2";
            this.btAutoTile2.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile2.TabIndex = 1;
            this.btAutoTile2.UseVisualStyleBackColor = false;
            // 
            // btAutoTile9
            // 
            this.btAutoTile9.BackColor = System.Drawing.Color.White;
            this.btAutoTile9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile9.Location = new System.Drawing.Point(63, 63);
            this.btAutoTile9.Name = "btAutoTile9";
            this.btAutoTile9.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile9.TabIndex = 1;
            this.btAutoTile9.UseVisualStyleBackColor = false;
            // 
            // btAutoTile4
            // 
            this.btAutoTile4.BackColor = System.Drawing.Color.White;
            this.btAutoTile4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile4.Location = new System.Drawing.Point(3, 33);
            this.btAutoTile4.Name = "btAutoTile4";
            this.btAutoTile4.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile4.TabIndex = 1;
            this.btAutoTile4.UseVisualStyleBackColor = false;
            // 
            // btAutoTile6
            // 
            this.btAutoTile6.BackColor = System.Drawing.Color.White;
            this.btAutoTile6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile6.Location = new System.Drawing.Point(63, 33);
            this.btAutoTile6.Name = "btAutoTile6";
            this.btAutoTile6.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile6.TabIndex = 1;
            this.btAutoTile6.UseVisualStyleBackColor = false;
            // 
            // btAutoTile5
            // 
            this.btAutoTile5.BackColor = System.Drawing.Color.White;
            this.btAutoTile5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile5.Location = new System.Drawing.Point(33, 33);
            this.btAutoTile5.Name = "btAutoTile5";
            this.btAutoTile5.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile5.TabIndex = 1;
            this.btAutoTile5.UseVisualStyleBackColor = false;
            // 
            // btAutoTile3
            // 
            this.btAutoTile3.BackColor = System.Drawing.Color.White;
            this.btAutoTile3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile3.Location = new System.Drawing.Point(63, 3);
            this.btAutoTile3.Name = "btAutoTile3";
            this.btAutoTile3.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile3.TabIndex = 1;
            this.btAutoTile3.UseVisualStyleBackColor = false;
            // 
            // btAutoTile7
            // 
            this.btAutoTile7.BackColor = System.Drawing.Color.White;
            this.btAutoTile7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile7.Location = new System.Drawing.Point(3, 63);
            this.btAutoTile7.Name = "btAutoTile7";
            this.btAutoTile7.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile7.TabIndex = 1;
            this.btAutoTile7.UseVisualStyleBackColor = false;
            // 
            // btAutoTile11
            // 
            this.btAutoTile11.BackColor = System.Drawing.Color.White;
            this.btAutoTile11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile11.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile11.Location = new System.Drawing.Point(33, 93);
            this.btAutoTile11.Name = "btAutoTile11";
            this.btAutoTile11.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile11.TabIndex = 1;
            this.btAutoTile11.UseVisualStyleBackColor = false;
            // 
            // btAutoTile8
            // 
            this.btAutoTile8.BackColor = System.Drawing.Color.White;
            this.btAutoTile8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile8.Location = new System.Drawing.Point(33, 63);
            this.btAutoTile8.Name = "btAutoTile8";
            this.btAutoTile8.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile8.TabIndex = 1;
            this.btAutoTile8.UseVisualStyleBackColor = false;
            // 
            // btAutoTile10
            // 
            this.btAutoTile10.BackColor = System.Drawing.Color.White;
            this.btAutoTile10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btAutoTile10.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btAutoTile10.Location = new System.Drawing.Point(3, 93);
            this.btAutoTile10.Name = "btAutoTile10";
            this.btAutoTile10.Size = new System.Drawing.Size(24, 24);
            this.btAutoTile10.TabIndex = 1;
            this.btAutoTile10.UseVisualStyleBackColor = false;
            // 
            // cbAutoTile
            // 
            this.cbAutoTile.AutoSize = true;
            this.cbAutoTile.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbAutoTile.Enabled = false;
            this.cbAutoTile.Location = new System.Drawing.Point(90, 198);
            this.cbAutoTile.Name = "cbAutoTile";
            this.cbAutoTile.Size = new System.Drawing.Size(65, 17);
            this.cbAutoTile.TabIndex = 7;
            this.cbAutoTile.Text = "AutoTile";
            this.cbAutoTile.UseVisualStyleBackColor = true;
            this.cbAutoTile.CheckedChanged += new System.EventHandler(this.cbAutoTile_CheckedChanged);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(257, 109);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(233, 20);
            this.tbName.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Name";
            // 
            // btNew
            // 
            this.btNew.Location = new System.Drawing.Point(216, 4);
            this.btNew.Name = "btNew";
            this.btNew.Size = new System.Drawing.Size(274, 23);
            this.btNew.TabIndex = 4;
            this.btNew.Text = "New";
            this.btNew.UseVisualStyleBackColor = true;
            this.btNew.Click += new System.EventHandler(this.btNew_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(216, 33);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(274, 23);
            this.btSave.TabIndex = 4;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btLoad
            // 
            this.btLoad.Location = new System.Drawing.Point(216, 62);
            this.btLoad.Name = "btLoad";
            this.btLoad.Size = new System.Drawing.Size(274, 23);
            this.btLoad.TabIndex = 4;
            this.btLoad.Text = "Load";
            this.btLoad.UseVisualStyleBackColor = true;
            this.btLoad.Click += new System.EventHandler(this.btLoad_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btPen);
            this.panel2.Controls.Add(this.RenderPanel);
            this.panel2.Controls.Add(this.btEraser);
            this.panel2.Controls.Add(this.btColorTransparent);
            this.panel2.Controls.Add(this.btColorSave10);
            this.panel2.Controls.Add(this.btColorSave8);
            this.panel2.Controls.Add(this.btColorSave6);
            this.panel2.Controls.Add(this.btColorSave4);
            this.panel2.Controls.Add(this.btColorSave2);
            this.panel2.Controls.Add(this.btColorSave9);
            this.panel2.Controls.Add(this.btColorSave7);
            this.panel2.Controls.Add(this.btColorSave5);
            this.panel2.Controls.Add(this.btColorSave3);
            this.panel2.Controls.Add(this.btColorSave1);
            this.panel2.Controls.Add(this.btColor);
            this.panel2.Controls.Add(this.btFill);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(206, 189);
            this.panel2.TabIndex = 3;
            // 
            // btPen
            // 
            this.btPen.BackColor = System.Drawing.Color.White;
            this.btPen.BackgroundImage = global::Editor.Properties.Resources.Pen;
            this.btPen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btPen.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btPen.Location = new System.Drawing.Point(10, 12);
            this.btPen.Name = "btPen";
            this.btPen.Size = new System.Drawing.Size(32, 32);
            this.btPen.TabIndex = 1;
            this.btPen.UseVisualStyleBackColor = false;
            this.btPen.Click += new System.EventHandler(this.btPen_Click);
            // 
            // btEraser
            // 
            this.btEraser.BackColor = System.Drawing.Color.White;
            this.btEraser.BackgroundImage = global::Editor.Properties.Resources.Eraser;
            this.btEraser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btEraser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btEraser.Location = new System.Drawing.Point(48, 12);
            this.btEraser.Name = "btEraser";
            this.btEraser.Size = new System.Drawing.Size(32, 32);
            this.btEraser.TabIndex = 1;
            this.btEraser.UseVisualStyleBackColor = false;
            this.btEraser.Click += new System.EventHandler(this.btEraser_Click);
            // 
            // btColorTransparent
            // 
            this.btColorTransparent.BackColor = System.Drawing.Color.White;
            this.btColorTransparent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorTransparent.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorTransparent.Location = new System.Drawing.Point(162, 12);
            this.btColorTransparent.Name = "btColorTransparent";
            this.btColorTransparent.Size = new System.Drawing.Size(32, 32);
            this.btColorTransparent.TabIndex = 1;
            this.btColorTransparent.UseVisualStyleBackColor = false;
            this.btColorTransparent.Click += new System.EventHandler(this.btColorTransparent_Click);
            // 
            // btColorSave10
            // 
            this.btColorSave10.BackColor = System.Drawing.Color.White;
            this.btColorSave10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave10.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave10.Location = new System.Drawing.Point(171, 154);
            this.btColorSave10.Name = "btColorSave10";
            this.btColorSave10.Size = new System.Drawing.Size(21, 20);
            this.btColorSave10.TabIndex = 1;
            this.btColorSave10.UseVisualStyleBackColor = false;
            // 
            // btColorSave8
            // 
            this.btColorSave8.BackColor = System.Drawing.Color.White;
            this.btColorSave8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave8.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave8.Location = new System.Drawing.Point(171, 128);
            this.btColorSave8.Name = "btColorSave8";
            this.btColorSave8.Size = new System.Drawing.Size(21, 20);
            this.btColorSave8.TabIndex = 1;
            this.btColorSave8.UseVisualStyleBackColor = false;
            // 
            // btColorSave6
            // 
            this.btColorSave6.BackColor = System.Drawing.Color.White;
            this.btColorSave6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave6.Location = new System.Drawing.Point(171, 102);
            this.btColorSave6.Name = "btColorSave6";
            this.btColorSave6.Size = new System.Drawing.Size(21, 20);
            this.btColorSave6.TabIndex = 1;
            this.btColorSave6.UseVisualStyleBackColor = false;
            // 
            // btColorSave4
            // 
            this.btColorSave4.BackColor = System.Drawing.Color.White;
            this.btColorSave4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave4.Location = new System.Drawing.Point(171, 76);
            this.btColorSave4.Name = "btColorSave4";
            this.btColorSave4.Size = new System.Drawing.Size(21, 20);
            this.btColorSave4.TabIndex = 1;
            this.btColorSave4.UseVisualStyleBackColor = false;
            // 
            // btColorSave2
            // 
            this.btColorSave2.BackColor = System.Drawing.Color.White;
            this.btColorSave2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave2.Location = new System.Drawing.Point(171, 50);
            this.btColorSave2.Name = "btColorSave2";
            this.btColorSave2.Size = new System.Drawing.Size(21, 20);
            this.btColorSave2.TabIndex = 1;
            this.btColorSave2.UseVisualStyleBackColor = false;
            // 
            // btColorSave9
            // 
            this.btColorSave9.BackColor = System.Drawing.Color.White;
            this.btColorSave9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave9.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave9.Location = new System.Drawing.Point(144, 154);
            this.btColorSave9.Name = "btColorSave9";
            this.btColorSave9.Size = new System.Drawing.Size(21, 20);
            this.btColorSave9.TabIndex = 1;
            this.btColorSave9.UseVisualStyleBackColor = false;
            // 
            // btColorSave7
            // 
            this.btColorSave7.BackColor = System.Drawing.Color.White;
            this.btColorSave7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave7.Location = new System.Drawing.Point(144, 128);
            this.btColorSave7.Name = "btColorSave7";
            this.btColorSave7.Size = new System.Drawing.Size(21, 20);
            this.btColorSave7.TabIndex = 1;
            this.btColorSave7.UseVisualStyleBackColor = false;
            // 
            // btColorSave5
            // 
            this.btColorSave5.BackColor = System.Drawing.Color.White;
            this.btColorSave5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave5.Location = new System.Drawing.Point(144, 102);
            this.btColorSave5.Name = "btColorSave5";
            this.btColorSave5.Size = new System.Drawing.Size(21, 20);
            this.btColorSave5.TabIndex = 1;
            this.btColorSave5.UseVisualStyleBackColor = false;
            // 
            // btColorSave3
            // 
            this.btColorSave3.BackColor = System.Drawing.Color.White;
            this.btColorSave3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave3.Location = new System.Drawing.Point(144, 76);
            this.btColorSave3.Name = "btColorSave3";
            this.btColorSave3.Size = new System.Drawing.Size(21, 20);
            this.btColorSave3.TabIndex = 1;
            this.btColorSave3.UseVisualStyleBackColor = false;
            // 
            // btColorSave1
            // 
            this.btColorSave1.BackColor = System.Drawing.Color.Black;
            this.btColorSave1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColorSave1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColorSave1.Location = new System.Drawing.Point(144, 50);
            this.btColorSave1.Name = "btColorSave1";
            this.btColorSave1.Size = new System.Drawing.Size(21, 20);
            this.btColorSave1.TabIndex = 1;
            this.btColorSave1.UseVisualStyleBackColor = false;
            // 
            // btColor
            // 
            this.btColor.BackColor = System.Drawing.Color.Black;
            this.btColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btColor.Location = new System.Drawing.Point(124, 12);
            this.btColor.Name = "btColor";
            this.btColor.Size = new System.Drawing.Size(32, 32);
            this.btColor.TabIndex = 1;
            this.btColor.UseVisualStyleBackColor = false;
            this.btColor.Click += new System.EventHandler(this.btColor_Click);
            // 
            // btFill
            // 
            this.btFill.BackColor = System.Drawing.Color.White;
            this.btFill.BackgroundImage = global::Editor.Properties.Resources.Fill;
            this.btFill.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btFill.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btFill.Location = new System.Drawing.Point(86, 12);
            this.btFill.Name = "btFill";
            this.btFill.Size = new System.Drawing.Size(32, 32);
            this.btFill.TabIndex = 1;
            this.btFill.UseVisualStyleBackColor = false;
            this.btFill.Click += new System.EventHandler(this.btFill_Click);
            // 
            // btChange
            // 
            this.btChange.Enabled = false;
            this.btChange.Location = new System.Drawing.Point(443, 40);
            this.btChange.Name = "btChange";
            this.btChange.Size = new System.Drawing.Size(60, 21);
            this.btChange.TabIndex = 6;
            this.btChange.Text = "Change";
            this.btChange.UseVisualStyleBackColor = true;
            this.btChange.Click += new System.EventHandler(this.btChange_Click);
            // 
            // EntityEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 486);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btChange);
            this.Controls.Add(this.btCreate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbbEntityLevel4);
            this.Controls.Add(this.cbbEntityLevel3);
            this.Controls.Add(this.cbbEntityLevel2);
            this.Controls.Add(this.cbbEntityLevel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EntityEditor";
            this.Text = "MainForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHP)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupAutoTile.ResumeLayout(false);
            this.panelAutoTile.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerDraw;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.ComboBox cbbEntityLevel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbEntityLevel2;
        private System.Windows.Forms.ComboBox cbbEntityLevel3;
        private System.Windows.Forms.ComboBox cbbEntityLevel4;
        private System.Windows.Forms.Button btCreate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btFill;
        private System.Windows.Forms.Button btEraser;
        private System.Windows.Forms.Button btPen;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btColorTransparent;
        private System.Windows.Forms.Button btColor;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.Button btColorSave10;
        private System.Windows.Forms.Button btColorSave8;
        private System.Windows.Forms.Button btColorSave6;
        private System.Windows.Forms.Button btColorSave4;
        private System.Windows.Forms.Button btColorSave2;
        private System.Windows.Forms.Button btColorSave9;
        private System.Windows.Forms.Button btColorSave7;
        private System.Windows.Forms.Button btColorSave5;
        private System.Windows.Forms.Button btColorSave3;
        private System.Windows.Forms.Button btColorSave1;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.Button btNew;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbAutoTile;
        private System.Windows.Forms.Button btChange;
        private System.Windows.Forms.Button btAutoTile1;
        private System.Windows.Forms.Button btAutoTile12;
        private System.Windows.Forms.Button btAutoTile9;
        private System.Windows.Forms.Button btAutoTile6;
        private System.Windows.Forms.Button btAutoTile3;
        private System.Windows.Forms.Button btAutoTile11;
        private System.Windows.Forms.Button btAutoTile10;
        private System.Windows.Forms.Button btAutoTile8;
        private System.Windows.Forms.Button btAutoTile7;
        private System.Windows.Forms.Button btAutoTile5;
        private System.Windows.Forms.Button btAutoTile4;
        private System.Windows.Forms.Button btAutoTile2;
        private System.Windows.Forms.Panel panelAutoTile;
        private System.Windows.Forms.GroupBox groupAutoTile;
        private System.Windows.Forms.Button btAutotileShowTemplate;
        private System.Windows.Forms.Button btAutotileHideTemplate;
        private System.Windows.Forms.Panel RenderPanelAutoTile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbIndestructible;
        private System.Windows.Forms.NumericUpDown numHP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numLayer;
        private System.Windows.Forms.CheckBox cbTriggerCollision;
        private System.Windows.Forms.Button btEditBehavior;
        private System.Windows.Forms.Button btSetBehavior;
        private System.Windows.Forms.Label lbSelectedBehavior;
        private System.Windows.Forms.CheckBox cbGravityEffect;
        private System.Windows.Forms.Button btClearBehavior;
    }
}