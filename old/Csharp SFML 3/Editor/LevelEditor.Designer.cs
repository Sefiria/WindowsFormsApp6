namespace Editor
{
    partial class LevelEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.timerDraw = new System.Windows.Forms.Timer(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btSetBackground = new System.Windows.Forms.Button();
            this.btGridWhite = new System.Windows.Forms.Button();
            this.btGridGray = new System.Windows.Forms.Button();
            this.btGridBlack = new System.Windows.Forms.Button();
            this.hScrollBarX = new System.Windows.Forms.HScrollBar();
            this.btLayer0 = new System.Windows.Forms.Button();
            this.btLayer1 = new System.Windows.Forms.Button();
            this.btLayer2 = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.btLoad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.cbDisplayLayer0 = new System.Windows.Forms.CheckBox();
            this.cbDisplayLayer1 = new System.Windows.Forms.CheckBox();
            this.cbDisplayLayer2 = new System.Windows.Forms.CheckBox();
            this.btDisplayLayerAll = new System.Windows.Forms.Button();
            this.btNew = new System.Windows.Forms.Button();
            this.timerMouseHold = new System.Windows.Forms.Timer(this.components);
            this.btSelectionAllLayers = new System.Windows.Forms.Button();
            this.timerCommandKey = new System.Windows.Forms.Timer(this.components);
            this.btFillSelectionReplace = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tkbBgContrast = new System.Windows.Forms.TrackBar();
            this.comboBoxIconed1 = new Editor.ComboBoxIconed();
            this.label3 = new System.Windows.Forms.Label();
            this.vScrollBarY = new System.Windows.Forms.VScrollBar();
            this.btCameraGo = new System.Windows.Forms.Button();
            this.btCameraColor = new System.Windows.Forms.Button();
            this.nupCameraX = new System.Windows.Forms.NumericUpDown();
            this.nupCameraY = new System.Windows.Forms.NumericUpDown();
            this.lbCameraX = new System.Windows.Forms.Label();
            this.lbCameraY = new System.Windows.Forms.Label();
            this.lbCameraW = new System.Windows.Forms.Label();
            this.nupCameraW = new System.Windows.Forms.NumericUpDown();
            this.rtbTileInfo = new System.Windows.Forms.RichTextBox();
            this.btEdit = new System.Windows.Forms.Button();
            this.btSelect = new System.Windows.Forms.Button();
            this.btEraser = new System.Windows.Forms.Button();
            this.btWarpsProperties = new System.Windows.Forms.Button();
            this.btDoorProperties = new System.Windows.Forms.Button();
            this.btFillSelection = new System.Windows.Forms.Button();
            this.btFill = new System.Windows.Forms.Button();
            this.btCamera = new System.Windows.Forms.Button();
            this.btWarpExit = new System.Windows.Forms.Button();
            this.btWarpEnter = new System.Windows.Forms.Button();
            this.btEditEntities = new System.Windows.Forms.Button();
            this.btEditBackground = new System.Windows.Forms.Button();
            this.btRefreshRender = new System.Windows.Forms.Button();
            this.btRecalculateAutotile = new System.Windows.Forms.Button();
            this.btPen = new System.Windows.Forms.Button();
            this.btElectricPanel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tkbBgContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupCameraX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupCameraY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupCameraW)).BeginInit();
            this.SuspendLayout();
            // 
            // RenderPanel
            // 
            this.RenderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RenderPanel.Location = new System.Drawing.Point(12, 161);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(672, 256);
            this.RenderPanel.TabIndex = 0;
            this.RenderPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseDown);
            this.RenderPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseDown);
            this.RenderPanel.MouseLeave += new System.EventHandler(this.RenderPanel_MouseLeave);
            this.RenderPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseMove);
            this.RenderPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseUp);
            // 
            // timerDraw
            // 
            this.timerDraw.Interval = 10;
            this.timerDraw.Tick += new System.EventHandler(this.timerDraw_Tick);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(115, 68);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(131, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btSetBackground
            // 
            this.btSetBackground.Location = new System.Drawing.Point(252, 94);
            this.btSetBackground.Name = "btSetBackground";
            this.btSetBackground.Size = new System.Drawing.Size(32, 23);
            this.btSetBackground.TabIndex = 4;
            this.btSetBackground.Text = "Set";
            this.btSetBackground.UseVisualStyleBackColor = true;
            this.btSetBackground.Visible = false;
            this.btSetBackground.Click += new System.EventHandler(this.btSetBackground_Click);
            // 
            // btGridWhite
            // 
            this.btGridWhite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btGridWhite.BackColor = System.Drawing.Color.White;
            this.btGridWhite.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btGridWhite.Location = new System.Drawing.Point(472, 12);
            this.btGridWhite.Name = "btGridWhite";
            this.btGridWhite.Size = new System.Drawing.Size(16, 16);
            this.btGridWhite.TabIndex = 5;
            this.btGridWhite.UseVisualStyleBackColor = false;
            this.btGridWhite.Click += new System.EventHandler(this.btGridWhite_Click);
            // 
            // btGridGray
            // 
            this.btGridGray.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btGridGray.BackColor = System.Drawing.Color.Gray;
            this.btGridGray.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btGridGray.Location = new System.Drawing.Point(493, 12);
            this.btGridGray.Name = "btGridGray";
            this.btGridGray.Size = new System.Drawing.Size(16, 16);
            this.btGridGray.TabIndex = 5;
            this.btGridGray.UseVisualStyleBackColor = false;
            this.btGridGray.Click += new System.EventHandler(this.btGridWhite_Click);
            // 
            // btGridBlack
            // 
            this.btGridBlack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btGridBlack.BackColor = System.Drawing.Color.Black;
            this.btGridBlack.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btGridBlack.Location = new System.Drawing.Point(515, 12);
            this.btGridBlack.Name = "btGridBlack";
            this.btGridBlack.Size = new System.Drawing.Size(16, 16);
            this.btGridBlack.TabIndex = 5;
            this.btGridBlack.UseVisualStyleBackColor = false;
            this.btGridBlack.Click += new System.EventHandler(this.btGridWhite_Click);
            // 
            // hScrollBarX
            // 
            this.hScrollBarX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBarX.LargeChange = 16;
            this.hScrollBarX.Location = new System.Drawing.Point(12, 420);
            this.hScrollBarX.Maximum = 255;
            this.hScrollBarX.Name = "hScrollBarX";
            this.hScrollBarX.Size = new System.Drawing.Size(672, 17);
            this.hScrollBarX.TabIndex = 6;
            this.hScrollBarX.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBarX_Scroll);
            this.hScrollBarX.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
            // 
            // btLayer0
            // 
            this.btLayer0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btLayer0.BackColor = System.Drawing.Color.White;
            this.btLayer0.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btLayer0.Font = new System.Drawing.Font("Arial", 5F);
            this.btLayer0.Location = new System.Drawing.Point(537, 12);
            this.btLayer0.Name = "btLayer0";
            this.btLayer0.Size = new System.Drawing.Size(16, 16);
            this.btLayer0.TabIndex = 5;
            this.btLayer0.Text = "0";
            this.btLayer0.UseVisualStyleBackColor = false;
            this.btLayer0.Click += new System.EventHandler(this.btLayer0_Click);
            // 
            // btLayer1
            // 
            this.btLayer1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btLayer1.BackColor = System.Drawing.Color.LightGray;
            this.btLayer1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btLayer1.Font = new System.Drawing.Font("Arial", 5F);
            this.btLayer1.Location = new System.Drawing.Point(558, 12);
            this.btLayer1.Name = "btLayer1";
            this.btLayer1.Size = new System.Drawing.Size(16, 16);
            this.btLayer1.TabIndex = 5;
            this.btLayer1.Text = "1";
            this.btLayer1.UseVisualStyleBackColor = false;
            this.btLayer1.Click += new System.EventHandler(this.btLayer0_Click);
            // 
            // btLayer2
            // 
            this.btLayer2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btLayer2.BackColor = System.Drawing.Color.LightGray;
            this.btLayer2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btLayer2.Font = new System.Drawing.Font("Arial", 5F);
            this.btLayer2.Location = new System.Drawing.Point(580, 12);
            this.btLayer2.Name = "btLayer2";
            this.btLayer2.Size = new System.Drawing.Size(16, 16);
            this.btLayer2.TabIndex = 5;
            this.btLayer2.Text = "2";
            this.btLayer2.UseVisualStyleBackColor = false;
            this.btLayer2.Click += new System.EventHandler(this.btLayer0_Click);
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Location = new System.Drawing.Point(633, 41);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 7;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btLoad
            // 
            this.btLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btLoad.Location = new System.Drawing.Point(633, 70);
            this.btLoad.Name = "btLoad";
            this.btLoad.Size = new System.Drawing.Size(75, 23);
            this.btLoad.TabIndex = 7;
            this.btLoad.Text = "Load";
            this.btLoad.UseVisualStyleBackColor = true;
            this.btLoad.Click += new System.EventHandler(this.btLoad_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Level Files|*.level";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Level Files|*.level";
            // 
            // cbDisplayLayer0
            // 
            this.cbDisplayLayer0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDisplayLayer0.AutoSize = true;
            this.cbDisplayLayer0.Location = new System.Drawing.Point(538, 30);
            this.cbDisplayLayer0.Name = "cbDisplayLayer0";
            this.cbDisplayLayer0.Size = new System.Drawing.Size(15, 14);
            this.cbDisplayLayer0.TabIndex = 8;
            this.cbDisplayLayer0.UseVisualStyleBackColor = true;
            // 
            // cbDisplayLayer1
            // 
            this.cbDisplayLayer1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDisplayLayer1.AutoSize = true;
            this.cbDisplayLayer1.Location = new System.Drawing.Point(559, 30);
            this.cbDisplayLayer1.Name = "cbDisplayLayer1";
            this.cbDisplayLayer1.Size = new System.Drawing.Size(15, 14);
            this.cbDisplayLayer1.TabIndex = 8;
            this.cbDisplayLayer1.UseVisualStyleBackColor = true;
            // 
            // cbDisplayLayer2
            // 
            this.cbDisplayLayer2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDisplayLayer2.AutoSize = true;
            this.cbDisplayLayer2.BackColor = System.Drawing.SystemColors.Control;
            this.cbDisplayLayer2.Location = new System.Drawing.Point(581, 30);
            this.cbDisplayLayer2.Name = "cbDisplayLayer2";
            this.cbDisplayLayer2.Size = new System.Drawing.Size(15, 14);
            this.cbDisplayLayer2.TabIndex = 8;
            this.cbDisplayLayer2.UseVisualStyleBackColor = false;
            // 
            // btDisplayLayerAll
            // 
            this.btDisplayLayerAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDisplayLayerAll.BackColor = System.Drawing.Color.Azure;
            this.btDisplayLayerAll.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btDisplayLayerAll.Font = new System.Drawing.Font("Arial", 5F);
            this.btDisplayLayerAll.Location = new System.Drawing.Point(602, 30);
            this.btDisplayLayerAll.Name = "btDisplayLayerAll";
            this.btDisplayLayerAll.Size = new System.Drawing.Size(14, 14);
            this.btDisplayLayerAll.TabIndex = 5;
            this.btDisplayLayerAll.UseVisualStyleBackColor = false;
            this.btDisplayLayerAll.Click += new System.EventHandler(this.btDisplayLayerAll_Click);
            // 
            // btNew
            // 
            this.btNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btNew.Location = new System.Drawing.Point(633, 12);
            this.btNew.Name = "btNew";
            this.btNew.Size = new System.Drawing.Size(75, 23);
            this.btNew.TabIndex = 7;
            this.btNew.Text = "New";
            this.btNew.UseVisualStyleBackColor = true;
            this.btNew.Click += new System.EventHandler(this.btNew_Click);
            // 
            // timerMouseHold
            // 
            this.timerMouseHold.Interval = 10;
            this.timerMouseHold.Tick += new System.EventHandler(this.timerMouseHold_Tick);
            // 
            // btSelectionAllLayers
            // 
            this.btSelectionAllLayers.BackColor = System.Drawing.Color.White;
            this.btSelectionAllLayers.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btSelectionAllLayers.Font = new System.Drawing.Font("Arial", 6F);
            this.btSelectionAllLayers.Location = new System.Drawing.Point(229, 47);
            this.btSelectionAllLayers.Name = "btSelectionAllLayers";
            this.btSelectionAllLayers.Size = new System.Drawing.Size(32, 16);
            this.btSelectionAllLayers.TabIndex = 5;
            this.btSelectionAllLayers.Text = "All";
            this.btSelectionAllLayers.UseVisualStyleBackColor = false;
            this.btSelectionAllLayers.Click += new System.EventHandler(this.btSelectionAllLayers_Click);
            // 
            // timerCommandKey
            // 
            this.timerCommandKey.Interval = 10;
            this.timerCommandKey.Tick += new System.EventHandler(this.timerCommandKey_Tick);
            // 
            // btFillSelectionReplace
            // 
            this.btFillSelectionReplace.BackColor = System.Drawing.Color.White;
            this.btFillSelectionReplace.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btFillSelectionReplace.Font = new System.Drawing.Font("Arial", 6F);
            this.btFillSelectionReplace.Location = new System.Drawing.Point(153, 47);
            this.btFillSelectionReplace.Name = "btFillSelectionReplace";
            this.btFillSelectionReplace.Size = new System.Drawing.Size(32, 16);
            this.btFillSelectionReplace.TabIndex = 5;
            this.btFillSelectionReplace.Text = "Repl.";
            this.btFillSelectionReplace.UseVisualStyleBackColor = false;
            this.btFillSelectionReplace.Click += new System.EventHandler(this.btFillSelectionReplace_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Entity";
            // 
            // tkbBgContrast
            // 
            this.tkbBgContrast.LargeChange = 1;
            this.tkbBgContrast.Location = new System.Drawing.Point(15, 89);
            this.tkbBgContrast.Maximum = 4;
            this.tkbBgContrast.Name = "tkbBgContrast";
            this.tkbBgContrast.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tkbBgContrast.Size = new System.Drawing.Size(45, 66);
            this.tkbBgContrast.TabIndex = 10;
            this.tkbBgContrast.Value = 2;
            this.tkbBgContrast.ValueChanged += new System.EventHandler(this.tkbBgContrast_ValueChanged);
            // 
            // comboBoxIconed1
            // 
            this.comboBoxIconed1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIconed1.FormattingEnabled = true;
            this.comboBoxIconed1.Location = new System.Drawing.Point(115, 95);
            this.comboBoxIconed1.Name = "comboBoxIconed1";
            this.comboBoxIconed1.Size = new System.Drawing.Size(131, 21);
            this.comboBoxIconed1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.Location = new System.Drawing.Point(60, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(5, 140);
            this.label3.TabIndex = 11;
            // 
            // vScrollBarY
            // 
            this.vScrollBarY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBarY.LargeChange = 16;
            this.vScrollBarY.Location = new System.Drawing.Point(691, 161);
            this.vScrollBarY.Maximum = 255;
            this.vScrollBarY.Name = "vScrollBarY";
            this.vScrollBarY.Size = new System.Drawing.Size(17, 256);
            this.vScrollBarY.TabIndex = 12;
            this.vScrollBarY.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBarY_Scroll);
            this.vScrollBarY.ValueChanged += new System.EventHandler(this.vScrollBarY_ValueChanged);
            // 
            // btCameraGo
            // 
            this.btCameraGo.BackColor = System.Drawing.Color.White;
            this.btCameraGo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btCameraGo.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btCameraGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCameraGo.Font = new System.Drawing.Font("Imprint MT Shadow", 6F);
            this.btCameraGo.Location = new System.Drawing.Point(381, 47);
            this.btCameraGo.Name = "btCameraGo";
            this.btCameraGo.Size = new System.Drawing.Size(32, 17);
            this.btCameraGo.TabIndex = 1;
            this.btCameraGo.Text = "Go";
            this.btCameraGo.UseVisualStyleBackColor = false;
            this.btCameraGo.Visible = false;
            this.btCameraGo.Click += new System.EventHandler(this.btCameraGo_Click);
            // 
            // btCameraColor
            // 
            this.btCameraColor.BackColor = System.Drawing.Color.Red;
            this.btCameraColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btCameraColor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btCameraColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCameraColor.Font = new System.Drawing.Font("Imprint MT Shadow", 6F);
            this.btCameraColor.Location = new System.Drawing.Point(381, 92);
            this.btCameraColor.Name = "btCameraColor";
            this.btCameraColor.Size = new System.Drawing.Size(32, 17);
            this.btCameraColor.TabIndex = 1;
            this.btCameraColor.UseVisualStyleBackColor = false;
            this.btCameraColor.Visible = false;
            this.btCameraColor.Click += new System.EventHandler(this.btCameraColor_Click);
            // 
            // nupCameraX
            // 
            this.nupCameraX.Location = new System.Drawing.Point(381, 113);
            this.nupCameraX.Name = "nupCameraX";
            this.nupCameraX.Size = new System.Drawing.Size(47, 20);
            this.nupCameraX.TabIndex = 13;
            this.nupCameraX.Visible = false;
            this.nupCameraX.ValueChanged += new System.EventHandler(this.nupCameraX_ValueChanged);
            // 
            // nupCameraY
            // 
            this.nupCameraY.Location = new System.Drawing.Point(381, 137);
            this.nupCameraY.Name = "nupCameraY";
            this.nupCameraY.Size = new System.Drawing.Size(47, 20);
            this.nupCameraY.TabIndex = 13;
            this.nupCameraY.Visible = false;
            this.nupCameraY.ValueChanged += new System.EventHandler(this.nupCameraY_ValueChanged);
            // 
            // lbCameraX
            // 
            this.lbCameraX.AutoSize = true;
            this.lbCameraX.Location = new System.Drawing.Point(363, 118);
            this.lbCameraX.Name = "lbCameraX";
            this.lbCameraX.Size = new System.Drawing.Size(14, 13);
            this.lbCameraX.TabIndex = 9;
            this.lbCameraX.Text = "X";
            this.lbCameraX.Visible = false;
            // 
            // lbCameraY
            // 
            this.lbCameraY.AutoSize = true;
            this.lbCameraY.Location = new System.Drawing.Point(363, 144);
            this.lbCameraY.Name = "lbCameraY";
            this.lbCameraY.Size = new System.Drawing.Size(14, 13);
            this.lbCameraY.TabIndex = 9;
            this.lbCameraY.Text = "Y";
            this.lbCameraY.Visible = false;
            // 
            // lbCameraW
            // 
            this.lbCameraW.AutoSize = true;
            this.lbCameraW.Location = new System.Drawing.Point(363, 71);
            this.lbCameraW.Name = "lbCameraW";
            this.lbCameraW.Size = new System.Drawing.Size(18, 13);
            this.lbCameraW.TabIndex = 9;
            this.lbCameraW.Text = "W";
            this.lbCameraW.Visible = false;
            // 
            // nupCameraW
            // 
            this.nupCameraW.Location = new System.Drawing.Point(381, 68);
            this.nupCameraW.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nupCameraW.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupCameraW.Name = "nupCameraW";
            this.nupCameraW.Size = new System.Drawing.Size(47, 20);
            this.nupCameraW.TabIndex = 13;
            this.nupCameraW.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nupCameraW.Visible = false;
            this.nupCameraW.ValueChanged += new System.EventHandler(this.nupCameraW_ValueChanged);
            // 
            // rtbTileInfo
            // 
            this.rtbTileInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbTileInfo.Location = new System.Drawing.Point(-1, 446);
            this.rtbTileInfo.Name = "rtbTileInfo";
            this.rtbTileInfo.ReadOnly = true;
            this.rtbTileInfo.Size = new System.Drawing.Size(721, 22);
            this.rtbTileInfo.TabIndex = 14;
            this.rtbTileInfo.Text = "";
            // 
            // btEdit
            // 
            this.btEdit.BackColor = System.Drawing.Color.White;
            this.btEdit.BackgroundImage = global::Editor.Properties.Resources.Edit;
            this.btEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btEdit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btEdit.Location = new System.Drawing.Point(267, 12);
            this.btEdit.Name = "btEdit";
            this.btEdit.Size = new System.Drawing.Size(32, 32);
            this.btEdit.TabIndex = 1;
            this.btEdit.UseVisualStyleBackColor = false;
            this.btEdit.Click += new System.EventHandler(this.ToolMode_Click);
            // 
            // btSelect
            // 
            this.btSelect.BackColor = System.Drawing.Color.White;
            this.btSelect.BackgroundImage = global::Editor.Properties.Resources.Selection;
            this.btSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btSelect.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSelect.Location = new System.Drawing.Point(229, 13);
            this.btSelect.Name = "btSelect";
            this.btSelect.Size = new System.Drawing.Size(32, 32);
            this.btSelect.TabIndex = 1;
            this.btSelect.UseVisualStyleBackColor = false;
            this.btSelect.Click += new System.EventHandler(this.ToolMode_Click);
            // 
            // btEraser
            // 
            this.btEraser.BackColor = System.Drawing.Color.White;
            this.btEraser.BackgroundImage = global::Editor.Properties.Resources.Eraser;
            this.btEraser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btEraser.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btEraser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btEraser.Location = new System.Drawing.Point(191, 13);
            this.btEraser.Name = "btEraser";
            this.btEraser.Size = new System.Drawing.Size(32, 32);
            this.btEraser.TabIndex = 1;
            this.btEraser.UseVisualStyleBackColor = false;
            this.btEraser.Click += new System.EventHandler(this.ToolMode_Click);
            // 
            // btWarpsProperties
            // 
            this.btWarpsProperties.BackColor = System.Drawing.Color.White;
            this.btWarpsProperties.BackgroundImage = global::Editor.Properties.Resources.WarpsProperties;
            this.btWarpsProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btWarpsProperties.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btWarpsProperties.Location = new System.Drawing.Point(115, 123);
            this.btWarpsProperties.Name = "btWarpsProperties";
            this.btWarpsProperties.Size = new System.Drawing.Size(32, 32);
            this.btWarpsProperties.TabIndex = 1;
            this.btWarpsProperties.UseVisualStyleBackColor = false;
            this.btWarpsProperties.Click += new System.EventHandler(this.btWarpsProperties_Click);
            // 
            // btDoorProperties
            // 
            this.btDoorProperties.BackColor = System.Drawing.Color.White;
            this.btDoorProperties.BackgroundImage = global::Editor.Properties.Resources.Door;
            this.btDoorProperties.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btDoorProperties.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btDoorProperties.Location = new System.Drawing.Point(77, 123);
            this.btDoorProperties.Name = "btDoorProperties";
            this.btDoorProperties.Size = new System.Drawing.Size(32, 32);
            this.btDoorProperties.TabIndex = 1;
            this.btDoorProperties.UseVisualStyleBackColor = false;
            this.btDoorProperties.Click += new System.EventHandler(this.btDoorProperties_Click);
            // 
            // btFillSelection
            // 
            this.btFillSelection.BackColor = System.Drawing.Color.White;
            this.btFillSelection.BackgroundImage = global::Editor.Properties.Resources.FillSelection;
            this.btFillSelection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btFillSelection.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btFillSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btFillSelection.Location = new System.Drawing.Point(153, 13);
            this.btFillSelection.Name = "btFillSelection";
            this.btFillSelection.Size = new System.Drawing.Size(32, 32);
            this.btFillSelection.TabIndex = 1;
            this.btFillSelection.UseVisualStyleBackColor = false;
            this.btFillSelection.Click += new System.EventHandler(this.btFillSelection_Click);
            // 
            // btFill
            // 
            this.btFill.BackColor = System.Drawing.Color.White;
            this.btFill.BackgroundImage = global::Editor.Properties.Resources.Fill;
            this.btFill.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btFill.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btFill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btFill.Location = new System.Drawing.Point(115, 13);
            this.btFill.Name = "btFill";
            this.btFill.Size = new System.Drawing.Size(32, 32);
            this.btFill.TabIndex = 1;
            this.btFill.UseVisualStyleBackColor = false;
            this.btFill.Click += new System.EventHandler(this.ToolMode_Click);
            // 
            // btCamera
            // 
            this.btCamera.BackColor = System.Drawing.Color.White;
            this.btCamera.BackgroundImage = global::Editor.Properties.Resources.Camera;
            this.btCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btCamera.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btCamera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCamera.Location = new System.Drawing.Point(381, 12);
            this.btCamera.Name = "btCamera";
            this.btCamera.Size = new System.Drawing.Size(32, 32);
            this.btCamera.TabIndex = 1;
            this.btCamera.UseVisualStyleBackColor = false;
            this.btCamera.Click += new System.EventHandler(this.btCamera_Click);
            // 
            // btWarpExit
            // 
            this.btWarpExit.BackColor = System.Drawing.Color.White;
            this.btWarpExit.BackgroundImage = global::Editor.Properties.Resources.WarpExitLevel;
            this.btWarpExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btWarpExit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btWarpExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btWarpExit.Location = new System.Drawing.Point(343, 12);
            this.btWarpExit.Name = "btWarpExit";
            this.btWarpExit.Size = new System.Drawing.Size(32, 32);
            this.btWarpExit.TabIndex = 1;
            this.btWarpExit.UseVisualStyleBackColor = false;
            this.btWarpExit.Click += new System.EventHandler(this.ToolMode_Click);
            // 
            // btWarpEnter
            // 
            this.btWarpEnter.BackColor = System.Drawing.Color.White;
            this.btWarpEnter.BackgroundImage = global::Editor.Properties.Resources.WarpEnterLevel;
            this.btWarpEnter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btWarpEnter.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btWarpEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btWarpEnter.Location = new System.Drawing.Point(305, 12);
            this.btWarpEnter.Name = "btWarpEnter";
            this.btWarpEnter.Size = new System.Drawing.Size(32, 32);
            this.btWarpEnter.TabIndex = 1;
            this.btWarpEnter.UseVisualStyleBackColor = false;
            this.btWarpEnter.Click += new System.EventHandler(this.ToolMode_Click);
            // 
            // btEditEntities
            // 
            this.btEditEntities.BackColor = System.Drawing.Color.White;
            this.btEditEntities.BackgroundImage = global::Editor.Properties.Resources.EditEntities;
            this.btEditEntities.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btEditEntities.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btEditEntities.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btEditEntities.Location = new System.Drawing.Point(15, 13);
            this.btEditEntities.Name = "btEditEntities";
            this.btEditEntities.Size = new System.Drawing.Size(32, 32);
            this.btEditEntities.TabIndex = 1;
            this.btEditEntities.UseVisualStyleBackColor = false;
            this.btEditEntities.Click += new System.EventHandler(this.btEditEntities_Click);
            // 
            // btEditBackground
            // 
            this.btEditBackground.BackColor = System.Drawing.Color.White;
            this.btEditBackground.BackgroundImage = global::Editor.Properties.Resources.EditBackground;
            this.btEditBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btEditBackground.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btEditBackground.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btEditBackground.Location = new System.Drawing.Point(15, 51);
            this.btEditBackground.Name = "btEditBackground";
            this.btEditBackground.Size = new System.Drawing.Size(32, 32);
            this.btEditBackground.TabIndex = 1;
            this.btEditBackground.UseVisualStyleBackColor = false;
            this.btEditBackground.Click += new System.EventHandler(this.btEditBackground_Click);
            // 
            // btRefreshRender
            // 
            this.btRefreshRender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btRefreshRender.BackColor = System.Drawing.Color.White;
            this.btRefreshRender.BackgroundImage = global::Editor.Properties.Resources.RefreshRender;
            this.btRefreshRender.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btRefreshRender.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btRefreshRender.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRefreshRender.Location = new System.Drawing.Point(690, 420);
            this.btRefreshRender.Name = "btRefreshRender";
            this.btRefreshRender.Size = new System.Drawing.Size(20, 20);
            this.btRefreshRender.TabIndex = 1;
            this.btRefreshRender.UseVisualStyleBackColor = false;
            this.btRefreshRender.Click += new System.EventHandler(this.btRefreshRender_Click);
            // 
            // btRecalculateAutotile
            // 
            this.btRecalculateAutotile.BackColor = System.Drawing.Color.White;
            this.btRecalculateAutotile.BackgroundImage = global::Editor.Properties.Resources.RecalculateAutoTile;
            this.btRecalculateAutotile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btRecalculateAutotile.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btRecalculateAutotile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRecalculateAutotile.Location = new System.Drawing.Point(419, 13);
            this.btRecalculateAutotile.Name = "btRecalculateAutotile";
            this.btRecalculateAutotile.Size = new System.Drawing.Size(32, 32);
            this.btRecalculateAutotile.TabIndex = 1;
            this.btRecalculateAutotile.UseVisualStyleBackColor = false;
            this.btRecalculateAutotile.Click += new System.EventHandler(this.ToolMode_Click);
            // 
            // btPen
            // 
            this.btPen.BackColor = System.Drawing.Color.White;
            this.btPen.BackgroundImage = global::Editor.Properties.Resources.Pen;
            this.btPen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btPen.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btPen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btPen.Location = new System.Drawing.Point(77, 13);
            this.btPen.Name = "btPen";
            this.btPen.Size = new System.Drawing.Size(32, 32);
            this.btPen.TabIndex = 1;
            this.btPen.UseVisualStyleBackColor = false;
            this.btPen.Click += new System.EventHandler(this.ToolMode_Click);
            // 
            // btElectricPanel
            // 
            this.btElectricPanel.BackColor = System.Drawing.Color.White;
            this.btElectricPanel.BackgroundImage = global::Editor.Properties.Resources.ElectricPanel;
            this.btElectricPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btElectricPanel.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btElectricPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btElectricPanel.Location = new System.Drawing.Point(676, 99);
            this.btElectricPanel.Name = "btElectricPanel";
            this.btElectricPanel.Size = new System.Drawing.Size(32, 32);
            this.btElectricPanel.TabIndex = 15;
            this.btElectricPanel.UseVisualStyleBackColor = false;
            this.btElectricPanel.Click += new System.EventHandler(this.btElectricPanel_Click);
            // 
            // LevelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 468);
            this.Controls.Add(this.btElectricPanel);
            this.Controls.Add(this.rtbTileInfo);
            this.Controls.Add(this.nupCameraY);
            this.Controls.Add(this.nupCameraW);
            this.Controls.Add(this.nupCameraX);
            this.Controls.Add(this.vScrollBarY);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tkbBgContrast);
            this.Controls.Add(this.lbCameraY);
            this.Controls.Add(this.lbCameraW);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbCameraX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbDisplayLayer2);
            this.Controls.Add(this.cbDisplayLayer1);
            this.Controls.Add(this.cbDisplayLayer0);
            this.Controls.Add(this.btNew);
            this.Controls.Add(this.btLoad);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.hScrollBarX);
            this.Controls.Add(this.btGridBlack);
            this.Controls.Add(this.btGridGray);
            this.Controls.Add(this.btDisplayLayerAll);
            this.Controls.Add(this.btLayer2);
            this.Controls.Add(this.btLayer1);
            this.Controls.Add(this.btFillSelectionReplace);
            this.Controls.Add(this.btSelectionAllLayers);
            this.Controls.Add(this.btLayer0);
            this.Controls.Add(this.btGridWhite);
            this.Controls.Add(this.btSetBackground);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.comboBoxIconed1);
            this.Controls.Add(this.btEdit);
            this.Controls.Add(this.btSelect);
            this.Controls.Add(this.btEraser);
            this.Controls.Add(this.btWarpsProperties);
            this.Controls.Add(this.btDoorProperties);
            this.Controls.Add(this.btFillSelection);
            this.Controls.Add(this.btFill);
            this.Controls.Add(this.btCameraColor);
            this.Controls.Add(this.btCameraGo);
            this.Controls.Add(this.btCamera);
            this.Controls.Add(this.btWarpExit);
            this.Controls.Add(this.btWarpEnter);
            this.Controls.Add(this.btEditEntities);
            this.Controls.Add(this.btEditBackground);
            this.Controls.Add(this.btRefreshRender);
            this.Controls.Add(this.btRecalculateAutotile);
            this.Controls.Add(this.btPen);
            this.Controls.Add(this.RenderPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "LevelEditor";
            this.Text = "LevelEditor";
            ((System.ComponentModel.ISupportInitialize)(this.tkbBgContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupCameraX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupCameraY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupCameraW)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.Timer timerDraw;
        private System.Windows.Forms.Button btPen;
        private System.Windows.Forms.Button btEraser;
        private System.Windows.Forms.Button btSelect;
        private ComboBoxIconed comboBoxIconed1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btSetBackground;
        private System.Windows.Forms.Button btGridWhite;
        private System.Windows.Forms.Button btGridGray;
        private System.Windows.Forms.Button btGridBlack;
        private System.Windows.Forms.HScrollBar hScrollBarX;
        private System.Windows.Forms.Button btLayer0;
        private System.Windows.Forms.Button btLayer1;
        private System.Windows.Forms.Button btLayer2;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox cbDisplayLayer0;
        private System.Windows.Forms.CheckBox cbDisplayLayer1;
        private System.Windows.Forms.CheckBox cbDisplayLayer2;
        private System.Windows.Forms.Button btDisplayLayerAll;
        private System.Windows.Forms.Button btNew;
        private System.Windows.Forms.Button btDoorProperties;
        private System.Windows.Forms.Timer timerMouseHold;
        private System.Windows.Forms.Button btFill;
        private System.Windows.Forms.Button btSelectionAllLayers;
        private System.Windows.Forms.Timer timerCommandKey;
        private System.Windows.Forms.Button btFillSelection;
        private System.Windows.Forms.Button btFillSelectionReplace;
        private System.Windows.Forms.Button btWarpEnter;
        private System.Windows.Forms.Button btWarpExit;
        private System.Windows.Forms.Button btWarpsProperties;
        private System.Windows.Forms.Button btEdit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btEditBackground;
        private System.Windows.Forms.Button btEditEntities;
        private System.Windows.Forms.TrackBar tkbBgContrast;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.VScrollBar vScrollBarY;
        private System.Windows.Forms.Button btRefreshRender;
        private System.Windows.Forms.Button btCamera;
        private System.Windows.Forms.Button btCameraGo;
        private System.Windows.Forms.Button btCameraColor;
        private System.Windows.Forms.NumericUpDown nupCameraX;
        private System.Windows.Forms.NumericUpDown nupCameraY;
        private System.Windows.Forms.Label lbCameraX;
        private System.Windows.Forms.Label lbCameraY;
        private System.Windows.Forms.Label lbCameraW;
        private System.Windows.Forms.NumericUpDown nupCameraW;
        private System.Windows.Forms.RichTextBox rtbTileInfo;
        private System.Windows.Forms.Button btRecalculateAutotile;
        private System.Windows.Forms.Button btElectricPanel;
    }
}