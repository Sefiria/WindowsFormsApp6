namespace Csharp_SFML
{
    partial class MainForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel4 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel5 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.CustomLabel customLabel6 = new System.Windows.Forms.DataVisualization.Charting.CustomLabel();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.RenderPart = new System.Windows.Forms.Panel();
            this.btDrawPart_ColorA = new System.Windows.Forms.Button();
            this.btDrawPart_ColorB = new System.Windows.Forms.Button();
            this.timerMouseMovePart = new System.Windows.Forms.Timer(this.components);
            this.tbPartName = new System.Windows.Forms.TextBox();
            this.listPartsInstantiated = new System.Windows.Forms.ListBox();
            this.panelPalette = new System.Windows.Forms.Panel();
            this.Timeline = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.listFrames = new System.Windows.Forms.ListBox();
            this.btNewFrame = new System.Windows.Forms.Button();
            this.btMoveUp = new System.Windows.Forms.Button();
            this.btMoveDown = new System.Windows.Forms.Button();
            this.btRender = new System.Windows.Forms.Button();
            this.tbFrameName = new System.Windows.Forms.TextBox();
            this.btKeepPartLoad = new System.Windows.Forms.Button();
            this.btKeepPartClear = new System.Windows.Forms.Button();
            this.btKeepPartSave = new System.Windows.Forms.Button();
            this.btRemoveFrame = new System.Windows.Forms.Button();
            this.timerRenderView = new System.Windows.Forms.Timer(this.components);
            this.ctbRenderViewInterval = new System.Windows.Forms.TrackBar();
            this.nupRenderViewInterval = new System.Windows.Forms.NumericUpDown();
            this.btRenderLerp = new System.Windows.Forms.Button();
            this.timerRenderViewLerp = new System.Windows.Forms.Timer(this.components);
            this.cbReverseLastLerpRotation = new System.Windows.Forms.CheckBox();
            this.btRenderExport = new System.Windows.Forms.Button();
            this.btRenderLerpExport = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.nupIntervalRenderLerp = new System.Windows.Forms.NumericUpDown();
            this.lbSpeed = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportLerpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.eXitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savePaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearPaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.renderColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.snapSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snapAllPartsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudTargetX = new System.Windows.Forms.NumericUpDown();
            this.nudTargetY = new System.Windows.Forms.NumericUpDown();
            this.nudTargetWidth = new System.Windows.Forms.NumericUpDown();
            this.nudTargetHeight = new System.Windows.Forms.NumericUpDown();
            this.nudTargetSize = new System.Windows.Forms.NumericUpDown();
            this.btCenterTarget = new System.Windows.Forms.Button();
            this.lbTargetWidth = new System.Windows.Forms.Label();
            this.lbTargetSize = new System.Windows.Forms.Label();
            this.lbTargetHeight = new System.Windows.Forms.Label();
            this.lbTargetX = new System.Windows.Forms.Label();
            this.lbTargetY = new System.Windows.Forms.Label();
            this.numPartPositionX = new System.Windows.Forms.NumericUpDown();
            this.lbPartPositionX = new System.Windows.Forms.Label();
            this.numPartPositionY = new System.Windows.Forms.NumericUpDown();
            this.lbPartPositionY = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btPartPivotReset = new System.Windows.Forms.Button();
            this.lbPartAreaSize = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btPartAreaSizeCancel = new System.Windows.Forms.Button();
            this.btPartAreaSizeApply = new System.Windows.Forms.Button();
            this.numPartAreaSizeWidth = new System.Windows.Forms.NumericUpDown();
            this.lbPartAreaSizeHeight = new System.Windows.Forms.Label();
            this.numPartAreaSizeHeight = new System.Windows.Forms.NumericUpDown();
            this.lbPartAreaSizeWidth = new System.Windows.Forms.Label();
            this.numPartScaleX = new System.Windows.Forms.NumericUpDown();
            this.lbPartScaleY = new System.Windows.Forms.Label();
            this.numPartScaleY = new System.Windows.Forms.NumericUpDown();
            this.lbPartScaleX = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbPartInfos = new System.Windows.Forms.Label();
            this.lbPartScale = new System.Windows.Forms.Label();
            this.lbPartPivot = new System.Windows.Forms.Label();
            this.lbPartRotation = new System.Windows.Forms.Label();
            this.numPartAngle = new System.Windows.Forms.NumericUpDown();
            this.lbPartAngle = new System.Windows.Forms.Label();
            this.lbPartSeparator = new System.Windows.Forms.Label();
            this.lbPartPosition = new System.Windows.Forms.Label();
            this.btPartUnselect = new System.Windows.Forms.Button();
            this.btPartAreaSizeOpenSeparatedWindow = new System.Windows.Forms.Button();
            this.btDrawPart_ColorPicker = new System.Windows.Forms.Button();
            this.btPartNameApply = new System.Windows.Forms.Button();
            this.cbSnap = new System.Windows.Forms.CheckBox();
            this.lbSnap = new System.Windows.Forms.Label();
            this.btTargetColor = new System.Windows.Forms.Button();
            this.lbTargetColor = new System.Windows.Forms.Label();
            this.lbTargetBorderSize = new System.Windows.Forms.Label();
            this.nudTargetBorderSize = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbPartID = new System.Windows.Forms.Label();
            this.nudPartID = new System.Windows.Forms.NumericUpDown();
            this.btPartShowIndexes = new System.Windows.Forms.Button();
            this.btFrameShowAllIndexes = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Timeline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctbRenderViewInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupRenderViewInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupIntervalRenderLerp)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartPositionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartPositionY)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPartAreaSizeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartAreaSizeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetBorderSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPartID)).BeginInit();
            this.SuspendLayout();
            // 
            // RenderPanel
            // 
            this.RenderPanel.Location = new System.Drawing.Point(13, 39);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(603, 407);
            this.RenderPanel.TabIndex = 0;
            this.RenderPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderPanel_MouseDown);
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // RenderPart
            // 
            this.RenderPart.Location = new System.Drawing.Point(13, 456);
            this.RenderPart.Name = "RenderPart";
            this.RenderPart.Size = new System.Drawing.Size(240, 240);
            this.RenderPart.TabIndex = 1;
            this.RenderPart.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RenderPart_MouseDown);
            this.RenderPart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderPart_MouseMove);
            // 
            // btDrawPart_ColorA
            // 
            this.btDrawPart_ColorA.BackColor = System.Drawing.Color.Black;
            this.btDrawPart_ColorA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDrawPart_ColorA.Location = new System.Drawing.Point(262, 483);
            this.btDrawPart_ColorA.Name = "btDrawPart_ColorA";
            this.btDrawPart_ColorA.Size = new System.Drawing.Size(31, 31);
            this.btDrawPart_ColorA.TabIndex = 2;
            this.btDrawPart_ColorA.UseVisualStyleBackColor = false;
            this.btDrawPart_ColorA.Click += new System.EventHandler(this.btDrawPart_ColorA_Click);
            // 
            // btDrawPart_ColorB
            // 
            this.btDrawPart_ColorB.BackColor = System.Drawing.Color.White;
            this.btDrawPart_ColorB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDrawPart_ColorB.Location = new System.Drawing.Point(299, 483);
            this.btDrawPart_ColorB.Name = "btDrawPart_ColorB";
            this.btDrawPart_ColorB.Size = new System.Drawing.Size(31, 31);
            this.btDrawPart_ColorB.TabIndex = 2;
            this.btDrawPart_ColorB.UseVisualStyleBackColor = false;
            this.btDrawPart_ColorB.Click += new System.EventHandler(this.btDrawPart_ColorB_Click);
            // 
            // timerMouseMovePart
            // 
            this.timerMouseMovePart.Interval = 1;
            this.timerMouseMovePart.Tick += new System.EventHandler(this.timerMouseMovePartAndPivot_Tick);
            // 
            // tbPartName
            // 
            this.tbPartName.Location = new System.Drawing.Point(262, 456);
            this.tbPartName.Name = "tbPartName";
            this.tbPartName.Size = new System.Drawing.Size(256, 20);
            this.tbPartName.TabIndex = 4;
            this.tbPartName.Text = "Unnamed";
            this.tbPartName.TextChanged += new System.EventHandler(this.tbPartName_TextChanged);
            // 
            // listPartsInstantiated
            // 
            this.listPartsInstantiated.FormattingEnabled = true;
            this.listPartsInstantiated.Location = new System.Drawing.Point(825, 515);
            this.listPartsInstantiated.Name = "listPartsInstantiated";
            this.listPartsInstantiated.Size = new System.Drawing.Size(269, 186);
            this.listPartsInstantiated.TabIndex = 6;
            this.listPartsInstantiated.SelectedIndexChanged += new System.EventHandler(this.listPartsInstantiated_SelectedIndexChanged);
            this.listPartsInstantiated.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listPartsInstantiated_KeyDown);
            // 
            // panelPalette
            // 
            this.panelPalette.AutoScroll = true;
            this.panelPalette.Location = new System.Drawing.Point(262, 520);
            this.panelPalette.Name = "panelPalette";
            this.panelPalette.Size = new System.Drawing.Size(330, 179);
            this.panelPalette.TabIndex = 7;
            // 
            // Timeline
            // 
            this.Timeline.BackColor = System.Drawing.Color.DimGray;
            chartArea2.AxisX.Crossing = 1.7976931348623157E+308D;
            chartArea2.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea2.AxisX.InterlacedColor = System.Drawing.Color.Gray;
            chartArea2.AxisX.Interval = 10D;
            chartArea2.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea2.AxisX.IsMarginVisible = false;
            chartArea2.AxisX.IsStartedFromZero = false;
            chartArea2.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea2.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea2.AxisX.MinorGrid.LineColor = System.Drawing.Color.White;
            chartArea2.AxisX.MinorTickMark.LineColor = System.Drawing.Color.White;
            chartArea2.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea2.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea2.AxisX2.InterlacedColor = System.Drawing.SystemColors.ScrollBar;
            chartArea2.AxisX2.Interval = 10D;
            chartArea2.AxisX2.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea2.AxisX2.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea2.AxisX2.Minimum = 0D;
            chartArea2.AxisY.Crossing = 1.7976931348623157E+308D;
            customLabel4.ToPosition = 2D;
            customLabel5.RowIndex = 1;
            customLabel6.FromPosition = 1D;
            customLabel6.RowIndex = 2;
            chartArea2.AxisY.CustomLabels.Add(customLabel4);
            chartArea2.AxisY.CustomLabels.Add(customLabel5);
            chartArea2.AxisY.CustomLabels.Add(customLabel6);
            chartArea2.AxisY.InterlacedColor = System.Drawing.Color.Gray;
            chartArea2.AxisY.Interval = 0.5D;
            chartArea2.AxisY.IsMarginVisible = false;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea2.AxisY.Maximum = 2D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.AxisY2.InterlacedColor = System.Drawing.Color.Red;
            chartArea2.BackColor = System.Drawing.Color.Gray;
            chartArea2.BackSecondaryColor = System.Drawing.Color.Gray;
            chartArea2.BorderColor = System.Drawing.Color.Gray;
            chartArea2.CursorX.Interval = 100D;
            chartArea2.CursorX.IntervalOffset = 50D;
            chartArea2.CursorX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Milliseconds;
            chartArea2.CursorX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Milliseconds;
            chartArea2.CursorX.LineColor = System.Drawing.Color.White;
            chartArea2.Name = "ChartArea1";
            this.Timeline.ChartAreas.Add(chartArea2);
            this.Timeline.Enabled = false;
            this.Timeline.Location = new System.Drawing.Point(149, 65);
            this.Timeline.Name = "Timeline";
            this.Timeline.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Grayscale;
            series2.BorderColor = System.Drawing.Color.White;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series2.Color = System.Drawing.Color.White;
            series2.EmptyPointStyle.BorderColor = System.Drawing.Color.White;
            series2.IsVisibleInLegend = false;
            series2.MarkerBorderColor = System.Drawing.Color.Gainsboro;
            series2.MarkerBorderWidth = 3;
            series2.MarkerColor = System.Drawing.Color.DarkGray;
            series2.MarkerSize = 8;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Diamond;
            series2.Name = "Series1";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            this.Timeline.Series.Add(series2);
            this.Timeline.Size = new System.Drawing.Size(398, 61);
            this.Timeline.TabIndex = 10;
            this.Timeline.Visible = false;
            // 
            // listFrames
            // 
            this.listFrames.FormattingEnabled = true;
            this.listFrames.Location = new System.Drawing.Point(923, 65);
            this.listFrames.Name = "listFrames";
            this.listFrames.Size = new System.Drawing.Size(171, 381);
            this.listFrames.TabIndex = 11;
            this.listFrames.SelectedIndexChanged += new System.EventHandler(this.listFrames_SelectedIndexChanged);
            // 
            // btNewFrame
            // 
            this.btNewFrame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btNewFrame.Location = new System.Drawing.Point(140, 64);
            this.btNewFrame.Name = "btNewFrame";
            this.btNewFrame.Size = new System.Drawing.Size(113, 23);
            this.btNewFrame.TabIndex = 12;
            this.btNewFrame.Text = "New Frame";
            this.btNewFrame.UseVisualStyleBackColor = false;
            this.btNewFrame.Click += new System.EventHandler(this.btNewFrame_Click);
            // 
            // btMoveUp
            // 
            this.btMoveUp.Location = new System.Drawing.Point(140, 35);
            this.btMoveUp.Name = "btMoveUp";
            this.btMoveUp.Size = new System.Drawing.Size(113, 23);
            this.btMoveUp.TabIndex = 12;
            this.btMoveUp.Text = "Move Up";
            this.btMoveUp.UseVisualStyleBackColor = true;
            this.btMoveUp.Click += new System.EventHandler(this.btMoveUp_Click);
            // 
            // btMoveDown
            // 
            this.btMoveDown.Location = new System.Drawing.Point(140, 93);
            this.btMoveDown.Name = "btMoveDown";
            this.btMoveDown.Size = new System.Drawing.Size(113, 23);
            this.btMoveDown.TabIndex = 12;
            this.btMoveDown.Text = "Move Down";
            this.btMoveDown.UseVisualStyleBackColor = true;
            this.btMoveDown.Click += new System.EventHandler(this.btMoveDown_Click);
            // 
            // btRender
            // 
            this.btRender.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRender.Location = new System.Drawing.Point(141, 227);
            this.btRender.Name = "btRender";
            this.btRender.Size = new System.Drawing.Size(73, 52);
            this.btRender.TabIndex = 12;
            this.btRender.Text = "Render";
            this.btRender.UseVisualStyleBackColor = true;
            this.btRender.Click += new System.EventHandler(this.btRender_Click);
            // 
            // tbFrameName
            // 
            this.tbFrameName.Location = new System.Drawing.Point(6, 9);
            this.tbFrameName.Name = "tbFrameName";
            this.tbFrameName.Size = new System.Drawing.Size(247, 20);
            this.tbFrameName.TabIndex = 13;
            this.tbFrameName.Text = "Frame 0";
            // 
            // btKeepPartLoad
            // 
            this.btKeepPartLoad.BackColor = System.Drawing.SystemColors.Control;
            this.btKeepPartLoad.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btKeepPartLoad.BackgroundImage")));
            this.btKeepPartLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btKeepPartLoad.Enabled = false;
            this.btKeepPartLoad.Location = new System.Drawing.Point(561, 482);
            this.btKeepPartLoad.Name = "btKeepPartLoad";
            this.btKeepPartLoad.Size = new System.Drawing.Size(31, 31);
            this.btKeepPartLoad.TabIndex = 8;
            this.btKeepPartLoad.UseVisualStyleBackColor = false;
            this.btKeepPartLoad.Click += new System.EventHandler(this.btKeepPartLoad_Click);
            // 
            // btKeepPartClear
            // 
            this.btKeepPartClear.BackgroundImage = global::Csharp_SFML.Properties.Resources.KeepPart_Clear;
            this.btKeepPartClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btKeepPartClear.Location = new System.Drawing.Point(487, 482);
            this.btKeepPartClear.Name = "btKeepPartClear";
            this.btKeepPartClear.Size = new System.Drawing.Size(31, 31);
            this.btKeepPartClear.TabIndex = 8;
            this.btKeepPartClear.UseVisualStyleBackColor = true;
            this.btKeepPartClear.Click += new System.EventHandler(this.btKeepPartClear_Click);
            // 
            // btKeepPartSave
            // 
            this.btKeepPartSave.BackgroundImage = global::Csharp_SFML.Properties.Resources.KeepPart_Save;
            this.btKeepPartSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btKeepPartSave.Location = new System.Drawing.Point(524, 482);
            this.btKeepPartSave.Name = "btKeepPartSave";
            this.btKeepPartSave.Size = new System.Drawing.Size(31, 31);
            this.btKeepPartSave.TabIndex = 8;
            this.btKeepPartSave.UseVisualStyleBackColor = true;
            this.btKeepPartSave.Click += new System.EventHandler(this.btKeepPartSave_Click);
            // 
            // btRemoveFrame
            // 
            this.btRemoveFrame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btRemoveFrame.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRemoveFrame.Location = new System.Drawing.Point(140, 122);
            this.btRemoveFrame.Name = "btRemoveFrame";
            this.btRemoveFrame.Size = new System.Drawing.Size(113, 23);
            this.btRemoveFrame.TabIndex = 14;
            this.btRemoveFrame.Text = "Remove Frame";
            this.btRemoveFrame.UseVisualStyleBackColor = false;
            this.btRemoveFrame.Click += new System.EventHandler(this.btRemoveFrame_Click);
            // 
            // timerRenderView
            // 
            this.timerRenderView.Interval = 200;
            this.timerRenderView.Tick += new System.EventHandler(this.timerRenderView_Tick);
            // 
            // ctbRenderViewInterval
            // 
            this.ctbRenderViewInterval.BackColor = System.Drawing.SystemColors.Control;
            this.ctbRenderViewInterval.LargeChange = 100;
            this.ctbRenderViewInterval.Location = new System.Drawing.Point(142, 190);
            this.ctbRenderViewInterval.Maximum = 1000;
            this.ctbRenderViewInterval.Minimum = 10;
            this.ctbRenderViewInterval.Name = "ctbRenderViewInterval";
            this.ctbRenderViewInterval.Size = new System.Drawing.Size(81, 45);
            this.ctbRenderViewInterval.SmallChange = 100;
            this.ctbRenderViewInterval.TabIndex = 15;
            this.ctbRenderViewInterval.Value = 200;
            this.ctbRenderViewInterval.ValueChanged += new System.EventHandler(this.ctbRenderViewInterval_ValueChanged);
            // 
            // nupRenderViewInterval
            // 
            this.nupRenderViewInterval.Location = new System.Drawing.Point(160, 164);
            this.nupRenderViewInterval.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nupRenderViewInterval.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nupRenderViewInterval.Name = "nupRenderViewInterval";
            this.nupRenderViewInterval.Size = new System.Drawing.Size(67, 20);
            this.nupRenderViewInterval.TabIndex = 16;
            this.nupRenderViewInterval.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nupRenderViewInterval.ValueChanged += new System.EventHandler(this.nupRenderViewInterval_ValueChanged);
            // 
            // btRenderLerp
            // 
            this.btRenderLerp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRenderLerp.Location = new System.Drawing.Point(139, 334);
            this.btRenderLerp.Name = "btRenderLerp";
            this.btRenderLerp.Size = new System.Drawing.Size(75, 63);
            this.btRenderLerp.TabIndex = 12;
            this.btRenderLerp.Text = "Render Lerp";
            this.btRenderLerp.UseVisualStyleBackColor = true;
            this.btRenderLerp.Click += new System.EventHandler(this.btRenderLerp_Click);
            // 
            // timerRenderViewLerp
            // 
            this.timerRenderViewLerp.Interval = 10;
            this.timerRenderViewLerp.Tick += new System.EventHandler(this.timerRenderViewLerp_Tick);
            // 
            // cbReverseLastLerpRotation
            // 
            this.cbReverseLastLerpRotation.AutoSize = true;
            this.cbReverseLastLerpRotation.Checked = true;
            this.cbReverseLastLerpRotation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReverseLastLerpRotation.Location = new System.Drawing.Point(140, 285);
            this.cbReverseLastLerpRotation.Name = "cbReverseLastLerpRotation";
            this.cbReverseLastLerpRotation.Size = new System.Drawing.Size(119, 17);
            this.cbReverseLastLerpRotation.TabIndex = 17;
            this.cbReverseLastLerpRotation.Text = "Reverse Last Angle";
            this.cbReverseLastLerpRotation.UseVisualStyleBackColor = true;
            // 
            // btRenderExport
            // 
            this.btRenderExport.BackColor = System.Drawing.Color.White;
            this.btRenderExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRenderExport.Location = new System.Drawing.Point(223, 190);
            this.btRenderExport.Name = "btRenderExport";
            this.btRenderExport.Size = new System.Drawing.Size(30, 89);
            this.btRenderExport.TabIndex = 18;
            this.btRenderExport.Text = "E\r\nx\r\np\r\no\r\nr\r\nt";
            this.btRenderExport.UseVisualStyleBackColor = false;
            this.btRenderExport.Click += new System.EventHandler(this.btRenderExport_Click);
            // 
            // btRenderLerpExport
            // 
            this.btRenderLerpExport.BackColor = System.Drawing.Color.White;
            this.btRenderLerpExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRenderLerpExport.Location = new System.Drawing.Point(223, 308);
            this.btRenderLerpExport.Name = "btRenderLerpExport";
            this.btRenderLerpExport.Size = new System.Drawing.Size(30, 88);
            this.btRenderLerpExport.TabIndex = 18;
            this.btRenderLerpExport.Text = "E\r\nx\r\np\r\no\r\nr\r\nt";
            this.btRenderLerpExport.UseVisualStyleBackColor = false;
            this.btRenderLerpExport.Click += new System.EventHandler(this.btRenderLerpExport_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "bmp";
            this.saveFileDialog.Filter = "SpriteSet (*.png)|*.png|Bmp (*.bmp)|*.bmp|Png (*.png)|*.png|Jpeg (*.jpeg)|*.jpeg|" +
    "Icon (*.icon)|*.icon|Gif (*.gif)|*.gif";
            // 
            // nupIntervalRenderLerp
            // 
            this.nupIntervalRenderLerp.Location = new System.Drawing.Point(180, 309);
            this.nupIntervalRenderLerp.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nupIntervalRenderLerp.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupIntervalRenderLerp.Name = "nupIntervalRenderLerp";
            this.nupIntervalRenderLerp.Size = new System.Drawing.Size(42, 20);
            this.nupIntervalRenderLerp.TabIndex = 16;
            this.nupIntervalRenderLerp.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nupIntervalRenderLerp.ValueChanged += new System.EventHandler(this.nupIntervalRenderLerp_ValueChanged);
            // 
            // lbSpeed
            // 
            this.lbSpeed.AutoSize = true;
            this.lbSpeed.Location = new System.Drawing.Point(155, 148);
            this.lbSpeed.Name = "lbSpeed";
            this.lbSpeed.Size = new System.Drawing.Size(80, 13);
            this.lbSpeed.TabIndex = 19;
            this.lbSpeed.Text = "Render Interval";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Interval";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1106, 24);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportToolStripMenuItem,
            this.exportLerpToolStripMenuItem,
            this.toolStripSeparator2,
            this.eXitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // exportLerpToolStripMenuItem
            // 
            this.exportLerpToolStripMenuItem.Name = "exportLerpToolStripMenuItem";
            this.exportLerpToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.exportLerpToolStripMenuItem.Text = "Export Lerp...";
            this.exportLerpToolStripMenuItem.Click += new System.EventHandler(this.exportLerpToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(139, 6);
            // 
            // eXitToolStripMenuItem
            // 
            this.eXitToolStripMenuItem.Name = "eXitToolStripMenuItem";
            this.eXitToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.eXitToolStripMenuItem.Text = "Exit";
            this.eXitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadPaletteToolStripMenuItem,
            this.savePaletteToolStripMenuItem,
            this.clearPaletteToolStripMenuItem,
            this.toolStripSeparator3,
            this.renderColorToolStripMenuItem,
            this.toolStripSeparator4,
            this.snapSettingsToolStripMenuItem,
            this.snapAllPartsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // loadPaletteToolStripMenuItem
            // 
            this.loadPaletteToolStripMenuItem.Name = "loadPaletteToolStripMenuItem";
            this.loadPaletteToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.loadPaletteToolStripMenuItem.Text = "Load Palette...";
            this.loadPaletteToolStripMenuItem.Click += new System.EventHandler(this.loadPaletteToolStripMenuItem_Click);
            // 
            // savePaletteToolStripMenuItem
            // 
            this.savePaletteToolStripMenuItem.Name = "savePaletteToolStripMenuItem";
            this.savePaletteToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.savePaletteToolStripMenuItem.Text = "Save Palette...";
            this.savePaletteToolStripMenuItem.Click += new System.EventHandler(this.savePaletteToolStripMenuItem_Click);
            // 
            // clearPaletteToolStripMenuItem
            // 
            this.clearPaletteToolStripMenuItem.Name = "clearPaletteToolStripMenuItem";
            this.clearPaletteToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.clearPaletteToolStripMenuItem.Text = "Clear Palette";
            this.clearPaletteToolStripMenuItem.Click += new System.EventHandler(this.clearPaletteToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(214, 6);
            // 
            // renderColorToolStripMenuItem
            // 
            this.renderColorToolStripMenuItem.Name = "renderColorToolStripMenuItem";
            this.renderColorToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.renderColorToolStripMenuItem.Text = "Render Transparent Color...";
            this.renderColorToolStripMenuItem.Click += new System.EventHandler(this.renderColorToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(214, 6);
            // 
            // snapSettingsToolStripMenuItem
            // 
            this.snapSettingsToolStripMenuItem.Name = "snapSettingsToolStripMenuItem";
            this.snapSettingsToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.snapSettingsToolStripMenuItem.Text = "Snap Settings...";
            this.snapSettingsToolStripMenuItem.Click += new System.EventHandler(this.snapSettingsToolStripMenuItem_Click);
            // 
            // snapAllPartsToolStripMenuItem
            // 
            this.snapAllPartsToolStripMenuItem.Name = "snapAllPartsToolStripMenuItem";
            this.snapAllPartsToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.snapAllPartsToolStripMenuItem.Text = "Snap All Parts";
            this.snapAllPartsToolStripMenuItem.Click += new System.EventHandler(this.snapAllPartsToolStripMenuItem_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(622, 82);
            this.trackBar1.Maximum = 200;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 363);
            this.trackBar1.TabIndex = 21;
            this.trackBar1.Value = 100;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nudTargetX);
            this.groupBox1.Controls.Add(this.nudTargetY);
            this.groupBox1.Controls.Add(this.nudTargetWidth);
            this.groupBox1.Controls.Add(this.nudTargetHeight);
            this.groupBox1.Controls.Add(this.nudTargetBorderSize);
            this.groupBox1.Controls.Add(this.nudTargetSize);
            this.groupBox1.Controls.Add(this.btCenterTarget);
            this.groupBox1.Controls.Add(this.lbTargetWidth);
            this.groupBox1.Controls.Add(this.lbTargetColor);
            this.groupBox1.Controls.Add(this.lbTargetBorderSize);
            this.groupBox1.Controls.Add(this.lbTargetSize);
            this.groupBox1.Controls.Add(this.lbTargetHeight);
            this.groupBox1.Controls.Add(this.tbFrameName);
            this.groupBox1.Controls.Add(this.btNewFrame);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btMoveUp);
            this.groupBox1.Controls.Add(this.lbSpeed);
            this.groupBox1.Controls.Add(this.btMoveDown);
            this.groupBox1.Controls.Add(this.btRender);
            this.groupBox1.Controls.Add(this.btRenderLerp);
            this.groupBox1.Controls.Add(this.btTargetColor);
            this.groupBox1.Controls.Add(this.btRenderLerpExport);
            this.groupBox1.Controls.Add(this.btRemoveFrame);
            this.groupBox1.Controls.Add(this.btRenderExport);
            this.groupBox1.Controls.Add(this.ctbRenderViewInterval);
            this.groupBox1.Controls.Add(this.cbReverseLastLerpRotation);
            this.groupBox1.Controls.Add(this.nupRenderViewInterval);
            this.groupBox1.Controls.Add(this.nupIntervalRenderLerp);
            this.groupBox1.Controls.Add(this.lbTargetX);
            this.groupBox1.Controls.Add(this.lbTargetY);
            this.groupBox1.Location = new System.Drawing.Point(658, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 412);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            // 
            // nudTargetX
            // 
            this.nudTargetX.Location = new System.Drawing.Point(79, 35);
            this.nudTargetX.Maximum = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.nudTargetX.Minimum = new decimal(new int[] {
            4999,
            0,
            0,
            -2147483648});
            this.nudTargetX.Name = "nudTargetX";
            this.nudTargetX.Size = new System.Drawing.Size(55, 20);
            this.nudTargetX.TabIndex = 32;
            this.nudTargetX.ValueChanged += new System.EventHandler(this.nudTargetX_ValueChanged);
            // 
            // nudTargetY
            // 
            this.nudTargetY.Location = new System.Drawing.Point(79, 64);
            this.nudTargetY.Maximum = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.nudTargetY.Minimum = new decimal(new int[] {
            4999,
            0,
            0,
            -2147483648});
            this.nudTargetY.Name = "nudTargetY";
            this.nudTargetY.Size = new System.Drawing.Size(55, 20);
            this.nudTargetY.TabIndex = 32;
            this.nudTargetY.ValueChanged += new System.EventHandler(this.nudTargetY_ValueChanged);
            // 
            // nudTargetWidth
            // 
            this.nudTargetWidth.Location = new System.Drawing.Point(79, 93);
            this.nudTargetWidth.Maximum = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.nudTargetWidth.Minimum = new decimal(new int[] {
            4999,
            0,
            0,
            -2147483648});
            this.nudTargetWidth.Name = "nudTargetWidth";
            this.nudTargetWidth.Size = new System.Drawing.Size(55, 20);
            this.nudTargetWidth.TabIndex = 32;
            this.nudTargetWidth.ValueChanged += new System.EventHandler(this.nudTargetWidth_ValueChanged);
            // 
            // nudTargetHeight
            // 
            this.nudTargetHeight.Location = new System.Drawing.Point(79, 122);
            this.nudTargetHeight.Maximum = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.nudTargetHeight.Minimum = new decimal(new int[] {
            4999,
            0,
            0,
            -2147483648});
            this.nudTargetHeight.Name = "nudTargetHeight";
            this.nudTargetHeight.Size = new System.Drawing.Size(55, 20);
            this.nudTargetHeight.TabIndex = 32;
            this.nudTargetHeight.ValueChanged += new System.EventHandler(this.nudTargetHeight_ValueChanged);
            // 
            // nudTargetSize
            // 
            this.nudTargetSize.Location = new System.Drawing.Point(79, 205);
            this.nudTargetSize.Maximum = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.nudTargetSize.Minimum = new decimal(new int[] {
            4999,
            0,
            0,
            -2147483648});
            this.nudTargetSize.Name = "nudTargetSize";
            this.nudTargetSize.Size = new System.Drawing.Size(55, 20);
            this.nudTargetSize.TabIndex = 32;
            this.nudTargetSize.ValueChanged += new System.EventHandler(this.nudTargetSize_ValueChanged);
            // 
            // btCenterTarget
            // 
            this.btCenterTarget.Location = new System.Drawing.Point(11, 178);
            this.btCenterTarget.Name = "btCenterTarget";
            this.btCenterTarget.Size = new System.Drawing.Size(123, 20);
            this.btCenterTarget.TabIndex = 31;
            this.btCenterTarget.Text = "Center Target";
            this.btCenterTarget.UseVisualStyleBackColor = true;
            this.btCenterTarget.Click += new System.EventHandler(this.btCenterTarget_Click);
            // 
            // lbTargetWidth
            // 
            this.lbTargetWidth.AutoSize = true;
            this.lbTargetWidth.Location = new System.Drawing.Point(6, 95);
            this.lbTargetWidth.Name = "lbTargetWidth";
            this.lbTargetWidth.Size = new System.Drawing.Size(69, 13);
            this.lbTargetWidth.TabIndex = 28;
            this.lbTargetWidth.Text = "Target Width";
            // 
            // lbTargetSize
            // 
            this.lbTargetSize.AutoSize = true;
            this.lbTargetSize.Location = new System.Drawing.Point(8, 207);
            this.lbTargetSize.Name = "lbTargetSize";
            this.lbTargetSize.Size = new System.Drawing.Size(61, 13);
            this.lbTargetSize.TabIndex = 29;
            this.lbTargetSize.Text = "Target Size";
            // 
            // lbTargetHeight
            // 
            this.lbTargetHeight.AutoSize = true;
            this.lbTargetHeight.Location = new System.Drawing.Point(6, 124);
            this.lbTargetHeight.Name = "lbTargetHeight";
            this.lbTargetHeight.Size = new System.Drawing.Size(72, 13);
            this.lbTargetHeight.TabIndex = 29;
            this.lbTargetHeight.Text = "Target Height";
            // 
            // lbTargetX
            // 
            this.lbTargetX.AutoSize = true;
            this.lbTargetX.Location = new System.Drawing.Point(6, 40);
            this.lbTargetX.Name = "lbTargetX";
            this.lbTargetX.Size = new System.Drawing.Size(48, 13);
            this.lbTargetX.TabIndex = 25;
            this.lbTargetX.Text = "Target X";
            // 
            // lbTargetY
            // 
            this.lbTargetY.AutoSize = true;
            this.lbTargetY.Location = new System.Drawing.Point(6, 66);
            this.lbTargetY.Name = "lbTargetY";
            this.lbTargetY.Size = new System.Drawing.Size(48, 13);
            this.lbTargetY.TabIndex = 25;
            this.lbTargetY.Text = "Target Y";
            // 
            // numPartPositionX
            // 
            this.numPartPositionX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPartPositionX.Location = new System.Drawing.Point(34, 58);
            this.numPartPositionX.Maximum = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.numPartPositionX.Minimum = new decimal(new int[] {
            4999,
            0,
            0,
            -2147483648});
            this.numPartPositionX.Name = "numPartPositionX";
            this.numPartPositionX.Size = new System.Drawing.Size(55, 20);
            this.numPartPositionX.TabIndex = 24;
            this.numPartPositionX.ValueChanged += new System.EventHandler(this.numPartPositionX_ValueChanged);
            // 
            // lbPartPositionX
            // 
            this.lbPartPositionX.AutoSize = true;
            this.lbPartPositionX.Location = new System.Drawing.Point(14, 61);
            this.lbPartPositionX.Name = "lbPartPositionX";
            this.lbPartPositionX.Size = new System.Drawing.Size(14, 13);
            this.lbPartPositionX.TabIndex = 25;
            this.lbPartPositionX.Text = "X";
            // 
            // numPartPositionY
            // 
            this.numPartPositionY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPartPositionY.Location = new System.Drawing.Point(117, 58);
            this.numPartPositionY.Maximum = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.numPartPositionY.Minimum = new decimal(new int[] {
            4999,
            0,
            0,
            -2147483648});
            this.numPartPositionY.Name = "numPartPositionY";
            this.numPartPositionY.Size = new System.Drawing.Size(55, 20);
            this.numPartPositionY.TabIndex = 24;
            this.numPartPositionY.ValueChanged += new System.EventHandler(this.numPartPositionY_ValueChanged);
            // 
            // lbPartPositionY
            // 
            this.lbPartPositionY.AutoSize = true;
            this.lbPartPositionY.Location = new System.Drawing.Point(97, 61);
            this.lbPartPositionY.Name = "lbPartPositionY";
            this.lbPartPositionY.Size = new System.Drawing.Size(14, 13);
            this.lbPartPositionY.TabIndex = 25;
            this.lbPartPositionY.Text = "Y";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btPartPivotReset);
            this.groupBox2.Controls.Add(this.lbPartAreaSize);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btPartAreaSizeCancel);
            this.groupBox2.Controls.Add(this.btPartAreaSizeApply);
            this.groupBox2.Controls.Add(this.numPartAreaSizeWidth);
            this.groupBox2.Controls.Add(this.lbPartAreaSizeHeight);
            this.groupBox2.Controls.Add(this.numPartAreaSizeHeight);
            this.groupBox2.Controls.Add(this.lbPartAreaSizeWidth);
            this.groupBox2.Controls.Add(this.numPartScaleX);
            this.groupBox2.Controls.Add(this.lbPartScaleY);
            this.groupBox2.Controls.Add(this.numPartScaleY);
            this.groupBox2.Controls.Add(this.lbPartScaleX);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lbPartInfos);
            this.groupBox2.Controls.Add(this.lbPartScale);
            this.groupBox2.Controls.Add(this.lbPartPivot);
            this.groupBox2.Controls.Add(this.lbPartRotation);
            this.groupBox2.Controls.Add(this.numPartAngle);
            this.groupBox2.Controls.Add(this.lbPartAngle);
            this.groupBox2.Controls.Add(this.lbPartSeparator);
            this.groupBox2.Controls.Add(this.lbPartPosition);
            this.groupBox2.Controls.Add(this.numPartPositionX);
            this.groupBox2.Controls.Add(this.lbPartPositionY);
            this.groupBox2.Controls.Add(this.numPartPositionY);
            this.groupBox2.Controls.Add(this.lbPartPositionX);
            this.groupBox2.Location = new System.Drawing.Point(635, 450);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(184, 251);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            // 
            // btPartPivotReset
            // 
            this.btPartPivotReset.BackColor = System.Drawing.Color.White;
            this.btPartPivotReset.Enabled = false;
            this.btPartPivotReset.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btPartPivotReset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btPartPivotReset.Location = new System.Drawing.Point(109, 104);
            this.btPartPivotReset.Name = "btPartPivotReset";
            this.btPartPivotReset.Size = new System.Drawing.Size(63, 20);
            this.btPartPivotReset.TabIndex = 45;
            this.btPartPivotReset.Text = "Reset";
            this.btPartPivotReset.UseVisualStyleBackColor = false;
            this.btPartPivotReset.Click += new System.EventHandler(this.btPartPivotReset_Click);
            // 
            // lbPartAreaSize
            // 
            this.lbPartAreaSize.AutoSize = true;
            this.lbPartAreaSize.Location = new System.Drawing.Point(59, 178);
            this.lbPartAreaSize.Name = "lbPartAreaSize";
            this.lbPartAreaSize.Size = new System.Drawing.Size(74, 13);
            this.lbPartAreaSize.TabIndex = 39;
            this.lbPartAreaSize.Text = "Part Area Size";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.label3.Location = new System.Drawing.Point(12, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "__________________________";
            // 
            // btPartAreaSizeCancel
            // 
            this.btPartAreaSizeCancel.BackColor = System.Drawing.SystemColors.Control;
            this.btPartAreaSizeCancel.Enabled = false;
            this.btPartAreaSizeCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btPartAreaSizeCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btPartAreaSizeCancel.Location = new System.Drawing.Point(100, 227);
            this.btPartAreaSizeCancel.Name = "btPartAreaSizeCancel";
            this.btPartAreaSizeCancel.Size = new System.Drawing.Size(80, 18);
            this.btPartAreaSizeCancel.TabIndex = 44;
            this.btPartAreaSizeCancel.Text = "Cancel";
            this.btPartAreaSizeCancel.UseVisualStyleBackColor = false;
            this.btPartAreaSizeCancel.Click += new System.EventHandler(this.btPartAreaSizeCancel_Click);
            // 
            // btPartAreaSizeApply
            // 
            this.btPartAreaSizeApply.BackColor = System.Drawing.SystemColors.Control;
            this.btPartAreaSizeApply.Enabled = false;
            this.btPartAreaSizeApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btPartAreaSizeApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btPartAreaSizeApply.Location = new System.Drawing.Point(5, 227);
            this.btPartAreaSizeApply.Name = "btPartAreaSizeApply";
            this.btPartAreaSizeApply.Size = new System.Drawing.Size(80, 18);
            this.btPartAreaSizeApply.TabIndex = 44;
            this.btPartAreaSizeApply.Text = "Apply";
            this.btPartAreaSizeApply.UseVisualStyleBackColor = false;
            this.btPartAreaSizeApply.Click += new System.EventHandler(this.btPartAreaSizeApply_Click);
            // 
            // numPartAreaSizeWidth
            // 
            this.numPartAreaSizeWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPartAreaSizeWidth.Location = new System.Drawing.Point(34, 203);
            this.numPartAreaSizeWidth.Maximum = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.numPartAreaSizeWidth.Name = "numPartAreaSizeWidth";
            this.numPartAreaSizeWidth.Size = new System.Drawing.Size(55, 20);
            this.numPartAreaSizeWidth.TabIndex = 40;
            this.numPartAreaSizeWidth.ValueChanged += new System.EventHandler(this.numPartAreaSizeWidth_ValueChanged);
            // 
            // lbPartAreaSizeHeight
            // 
            this.lbPartAreaSizeHeight.AutoSize = true;
            this.lbPartAreaSizeHeight.Location = new System.Drawing.Point(97, 206);
            this.lbPartAreaSizeHeight.Name = "lbPartAreaSizeHeight";
            this.lbPartAreaSizeHeight.Size = new System.Drawing.Size(15, 13);
            this.lbPartAreaSizeHeight.TabIndex = 42;
            this.lbPartAreaSizeHeight.Text = "H";
            // 
            // numPartAreaSizeHeight
            // 
            this.numPartAreaSizeHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPartAreaSizeHeight.Location = new System.Drawing.Point(117, 203);
            this.numPartAreaSizeHeight.Maximum = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.numPartAreaSizeHeight.Name = "numPartAreaSizeHeight";
            this.numPartAreaSizeHeight.Size = new System.Drawing.Size(55, 20);
            this.numPartAreaSizeHeight.TabIndex = 41;
            this.numPartAreaSizeHeight.ValueChanged += new System.EventHandler(this.numPartAreaSizeHeight_ValueChanged);
            // 
            // lbPartAreaSizeWidth
            // 
            this.lbPartAreaSizeWidth.AutoSize = true;
            this.lbPartAreaSizeWidth.Location = new System.Drawing.Point(14, 206);
            this.lbPartAreaSizeWidth.Name = "lbPartAreaSizeWidth";
            this.lbPartAreaSizeWidth.Size = new System.Drawing.Size(18, 13);
            this.lbPartAreaSizeWidth.TabIndex = 43;
            this.lbPartAreaSizeWidth.Text = "W";
            // 
            // numPartScaleX
            // 
            this.numPartScaleX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPartScaleX.DecimalPlaces = 4;
            this.numPartScaleX.Location = new System.Drawing.Point(34, 147);
            this.numPartScaleX.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numPartScaleX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numPartScaleX.Name = "numPartScaleX";
            this.numPartScaleX.Size = new System.Drawing.Size(55, 20);
            this.numPartScaleX.TabIndex = 33;
            this.numPartScaleX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPartScaleX.ValueChanged += new System.EventHandler(this.numPartScaleX_ValueChanged);
            // 
            // lbPartScaleY
            // 
            this.lbPartScaleY.AutoSize = true;
            this.lbPartScaleY.Location = new System.Drawing.Point(97, 150);
            this.lbPartScaleY.Name = "lbPartScaleY";
            this.lbPartScaleY.Size = new System.Drawing.Size(14, 13);
            this.lbPartScaleY.TabIndex = 35;
            this.lbPartScaleY.Text = "Y";
            // 
            // numPartScaleY
            // 
            this.numPartScaleY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPartScaleY.DecimalPlaces = 4;
            this.numPartScaleY.Location = new System.Drawing.Point(117, 147);
            this.numPartScaleY.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numPartScaleY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numPartScaleY.Name = "numPartScaleY";
            this.numPartScaleY.Size = new System.Drawing.Size(55, 20);
            this.numPartScaleY.TabIndex = 34;
            this.numPartScaleY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPartScaleY.ValueChanged += new System.EventHandler(this.numPartScaleY_ValueChanged);
            // 
            // lbPartScaleX
            // 
            this.lbPartScaleX.AutoSize = true;
            this.lbPartScaleX.Location = new System.Drawing.Point(14, 150);
            this.lbPartScaleX.Name = "lbPartScaleX";
            this.lbPartScaleX.Size = new System.Drawing.Size(14, 13);
            this.lbPartScaleX.TabIndex = 36;
            this.lbPartScaleX.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.label2.Location = new System.Drawing.Point(12, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 13);
            this.label2.TabIndex = 37;
            this.label2.Text = "__________________________";
            // 
            // lbPartInfos
            // 
            this.lbPartInfos.AutoSize = true;
            this.lbPartInfos.Location = new System.Drawing.Point(67, 13);
            this.lbPartInfos.Name = "lbPartInfos";
            this.lbPartInfos.Size = new System.Drawing.Size(51, 13);
            this.lbPartInfos.TabIndex = 26;
            this.lbPartInfos.Text = "Part infos";
            // 
            // lbPartScale
            // 
            this.lbPartScale.AutoSize = true;
            this.lbPartScale.Location = new System.Drawing.Point(65, 127);
            this.lbPartScale.Name = "lbPartScale";
            this.lbPartScale.Size = new System.Drawing.Size(52, 13);
            this.lbPartScale.TabIndex = 32;
            this.lbPartScale.Text = "-- Scale --";
            // 
            // lbPartPivot
            // 
            this.lbPartPivot.AutoSize = true;
            this.lbPartPivot.Location = new System.Drawing.Point(97, 85);
            this.lbPartPivot.Name = "lbPartPivot";
            this.lbPartPivot.Size = new System.Drawing.Size(76, 13);
            this.lbPartPivot.TabIndex = 31;
            this.lbPartPivot.Text = "-- Pivot Point --";
            // 
            // lbPartRotation
            // 
            this.lbPartRotation.AutoSize = true;
            this.lbPartRotation.Location = new System.Drawing.Point(20, 85);
            this.lbPartRotation.Name = "lbPartRotation";
            this.lbPartRotation.Size = new System.Drawing.Size(65, 13);
            this.lbPartRotation.TabIndex = 31;
            this.lbPartRotation.Text = "-- Rotation --";
            // 
            // numPartAngle
            // 
            this.numPartAngle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numPartAngle.Location = new System.Drawing.Point(47, 104);
            this.numPartAngle.Maximum = new decimal(new int[] {
            359,
            0,
            0,
            0});
            this.numPartAngle.Name = "numPartAngle";
            this.numPartAngle.Size = new System.Drawing.Size(55, 20);
            this.numPartAngle.TabIndex = 27;
            this.numPartAngle.ValueChanged += new System.EventHandler(this.numPartAngle_ValueChanged);
            // 
            // lbPartAngle
            // 
            this.lbPartAngle.AutoSize = true;
            this.lbPartAngle.Location = new System.Drawing.Point(7, 106);
            this.lbPartAngle.Name = "lbPartAngle";
            this.lbPartAngle.Size = new System.Drawing.Size(34, 13);
            this.lbPartAngle.TabIndex = 30;
            this.lbPartAngle.Text = "Angle";
            // 
            // lbPartSeparator
            // 
            this.lbPartSeparator.AutoSize = true;
            this.lbPartSeparator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPartSeparator.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.lbPartSeparator.Location = new System.Drawing.Point(11, 18);
            this.lbPartSeparator.Name = "lbPartSeparator";
            this.lbPartSeparator.Size = new System.Drawing.Size(163, 13);
            this.lbPartSeparator.TabIndex = 26;
            this.lbPartSeparator.Text = "__________________________";
            // 
            // lbPartPosition
            // 
            this.lbPartPosition.AutoSize = true;
            this.lbPartPosition.Location = new System.Drawing.Point(61, 39);
            this.lbPartPosition.Name = "lbPartPosition";
            this.lbPartPosition.Size = new System.Drawing.Size(62, 13);
            this.lbPartPosition.TabIndex = 26;
            this.lbPartPosition.Text = "-- Position --";
            // 
            // btPartUnselect
            // 
            this.btPartUnselect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btPartUnselect.Location = new System.Drawing.Point(825, 489);
            this.btPartUnselect.Name = "btPartUnselect";
            this.btPartUnselect.Size = new System.Drawing.Size(269, 20);
            this.btPartUnselect.TabIndex = 27;
            this.btPartUnselect.Text = "Unselect part";
            this.btPartUnselect.UseVisualStyleBackColor = true;
            this.btPartUnselect.Click += new System.EventHandler(this.btPartUnselect_Click);
            // 
            // btPartAreaSizeOpenSeparatedWindow
            // 
            this.btPartAreaSizeOpenSeparatedWindow.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btPartAreaSizeOpenSeparatedWindow.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlLight;
            this.btPartAreaSizeOpenSeparatedWindow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btPartAreaSizeOpenSeparatedWindow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.AliceBlue;
            this.btPartAreaSizeOpenSeparatedWindow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btPartAreaSizeOpenSeparatedWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btPartAreaSizeOpenSeparatedWindow.Location = new System.Drawing.Point(598, 455);
            this.btPartAreaSizeOpenSeparatedWindow.Name = "btPartAreaSizeOpenSeparatedWindow";
            this.btPartAreaSizeOpenSeparatedWindow.Size = new System.Drawing.Size(31, 246);
            this.btPartAreaSizeOpenSeparatedWindow.TabIndex = 28;
            this.btPartAreaSizeOpenSeparatedWindow.Text = "O\r\np\r\ne\r\nn \r\n \r\nW\r\ni\r\nn\r\nd\r\no\r\nw\r\ne\r\nd";
            this.btPartAreaSizeOpenSeparatedWindow.UseVisualStyleBackColor = false;
            this.btPartAreaSizeOpenSeparatedWindow.Click += new System.EventHandler(this.btPartAreaSizeOpenSeparatedWindow_Click);
            // 
            // btDrawPart_ColorPicker
            // 
            this.btDrawPart_ColorPicker.BackColor = System.Drawing.Color.Transparent;
            this.btDrawPart_ColorPicker.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonFace;
            this.btDrawPart_ColorPicker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btDrawPart_ColorPicker.Location = new System.Drawing.Point(336, 483);
            this.btDrawPart_ColorPicker.Name = "btDrawPart_ColorPicker";
            this.btDrawPart_ColorPicker.Size = new System.Drawing.Size(31, 31);
            this.btDrawPart_ColorPicker.TabIndex = 2;
            this.btDrawPart_ColorPicker.UseVisualStyleBackColor = false;
            this.btDrawPart_ColorPicker.Click += new System.EventHandler(this.btDrawPart_ColorB_Click);
            // 
            // btPartNameApply
            // 
            this.btPartNameApply.BackColor = System.Drawing.Color.White;
            this.btPartNameApply.Enabled = false;
            this.btPartNameApply.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btPartNameApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btPartNameApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btPartNameApply.Location = new System.Drawing.Point(524, 456);
            this.btPartNameApply.Name = "btPartNameApply";
            this.btPartNameApply.Size = new System.Drawing.Size(68, 20);
            this.btPartNameApply.TabIndex = 29;
            this.btPartNameApply.Text = "Apply";
            this.btPartNameApply.UseVisualStyleBackColor = false;
            this.btPartNameApply.Click += new System.EventHandler(this.btPartNameApply_Click);
            // 
            // cbSnap
            // 
            this.cbSnap.AutoSize = true;
            this.cbSnap.Location = new System.Drawing.Point(630, 65);
            this.cbSnap.Name = "cbSnap";
            this.cbSnap.Size = new System.Drawing.Size(15, 14);
            this.cbSnap.TabIndex = 30;
            this.cbSnap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbSnap.UseVisualStyleBackColor = true;
            this.cbSnap.CheckedChanged += new System.EventHandler(this.cbSnap_CheckedChanged);
            // 
            // lbSnap
            // 
            this.lbSnap.AutoSize = true;
            this.lbSnap.Location = new System.Drawing.Point(622, 43);
            this.lbSnap.Name = "lbSnap";
            this.lbSnap.Size = new System.Drawing.Size(32, 13);
            this.lbSnap.TabIndex = 31;
            this.lbSnap.Text = "Snap";
            // 
            // btTargetColor
            // 
            this.btTargetColor.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btTargetColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btTargetColor.Location = new System.Drawing.Point(94, 234);
            this.btTargetColor.Name = "btTargetColor";
            this.btTargetColor.Size = new System.Drawing.Size(31, 31);
            this.btTargetColor.TabIndex = 2;
            this.btTargetColor.UseVisualStyleBackColor = false;
            this.btTargetColor.Click += new System.EventHandler(this.btTargetColor_Click);
            // 
            // lbTargetColor
            // 
            this.lbTargetColor.AutoSize = true;
            this.lbTargetColor.Location = new System.Drawing.Point(8, 243);
            this.lbTargetColor.Name = "lbTargetColor";
            this.lbTargetColor.Size = new System.Drawing.Size(65, 13);
            this.lbTargetColor.TabIndex = 29;
            this.lbTargetColor.Text = "Target Color";
            // 
            // lbTargetBorderSize
            // 
            this.lbTargetBorderSize.AutoSize = true;
            this.lbTargetBorderSize.Location = new System.Drawing.Point(8, 277);
            this.lbTargetBorderSize.Name = "lbTargetBorderSize";
            this.lbTargetBorderSize.Size = new System.Drawing.Size(72, 26);
            this.lbTargetBorderSize.TabIndex = 29;
            this.lbTargetBorderSize.Text = "Target Border\r\n        Size";
            // 
            // nudTargetBorderSize
            // 
            this.nudTargetBorderSize.Location = new System.Drawing.Point(94, 280);
            this.nudTargetBorderSize.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudTargetBorderSize.Name = "nudTargetBorderSize";
            this.nudTargetBorderSize.Size = new System.Drawing.Size(40, 20);
            this.nudTargetBorderSize.TabIndex = 32;
            this.nudTargetBorderSize.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudTargetBorderSize.ValueChanged += new System.EventHandler(this.nudTargetBorderSize_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.label4.Location = new System.Drawing.Point(3, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "______________________";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.label5.Location = new System.Drawing.Point(3, 305);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "____________________";
            // 
            // lbPartID
            // 
            this.lbPartID.AutoSize = true;
            this.lbPartID.Location = new System.Drawing.Point(832, 467);
            this.lbPartID.Name = "lbPartID";
            this.lbPartID.Size = new System.Drawing.Size(40, 13);
            this.lbPartID.TabIndex = 35;
            this.lbPartID.Text = "Part ID";
            // 
            // nudPartID
            // 
            this.nudPartID.Enabled = false;
            this.nudPartID.Location = new System.Drawing.Point(878, 463);
            this.nudPartID.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudPartID.Name = "nudPartID";
            this.nudPartID.Size = new System.Drawing.Size(48, 20);
            this.nudPartID.TabIndex = 32;
            this.nudPartID.ValueChanged += new System.EventHandler(this.nudPartID_ValueChanged);
            // 
            // btPartShowIndexes
            // 
            this.btPartShowIndexes.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btPartShowIndexes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btPartShowIndexes.Location = new System.Drawing.Point(932, 463);
            this.btPartShowIndexes.Name = "btPartShowIndexes";
            this.btPartShowIndexes.Size = new System.Drawing.Size(162, 20);
            this.btPartShowIndexes.TabIndex = 27;
            this.btPartShowIndexes.Text = "Show Indexes";
            this.btPartShowIndexes.UseVisualStyleBackColor = false;
            this.btPartShowIndexes.Click += new System.EventHandler(this.btPartShowIndexes_Click);
            // 
            // btFrameShowAllIndexes
            // 
            this.btFrameShowAllIndexes.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btFrameShowAllIndexes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btFrameShowAllIndexes.Location = new System.Drawing.Point(923, 39);
            this.btFrameShowAllIndexes.Name = "btFrameShowAllIndexes";
            this.btFrameShowAllIndexes.Size = new System.Drawing.Size(171, 20);
            this.btFrameShowAllIndexes.TabIndex = 27;
            this.btFrameShowAllIndexes.Text = "Show All Indexes";
            this.btFrameShowAllIndexes.UseVisualStyleBackColor = false;
            this.btFrameShowAllIndexes.Click += new System.EventHandler(this.btFrameShowAllIndexes_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1106, 713);
            this.Controls.Add(this.lbPartID);
            this.Controls.Add(this.lbSnap);
            this.Controls.Add(this.cbSnap);
            this.Controls.Add(this.btPartNameApply);
            this.Controls.Add(this.btPartAreaSizeOpenSeparatedWindow);
            this.Controls.Add(this.btFrameShowAllIndexes);
            this.Controls.Add(this.btPartShowIndexes);
            this.Controls.Add(this.btPartUnselect);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.nudPartID);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.listFrames);
            this.Controls.Add(this.btKeepPartLoad);
            this.Controls.Add(this.btKeepPartClear);
            this.Controls.Add(this.btKeepPartSave);
            this.Controls.Add(this.panelPalette);
            this.Controls.Add(this.listPartsInstantiated);
            this.Controls.Add(this.tbPartName);
            this.Controls.Add(this.btDrawPart_ColorPicker);
            this.Controls.Add(this.btDrawPart_ColorB);
            this.Controls.Add(this.btDrawPart_ColorA);
            this.Controls.Add(this.RenderPart);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.RenderPanel);
            this.Controls.Add(this.Timeline);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "2DAnimationMaker (2018) Freeware";
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.Timeline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctbRenderViewInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupRenderViewInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupIntervalRenderLerp)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartPositionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartPositionY)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPartAreaSizeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartAreaSizeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPartAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetBorderSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPartID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btDrawPart_ColorA;
        private System.Windows.Forms.Button btDrawPart_ColorB;
        private System.Windows.Forms.Panel RenderPart;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.Timer timerMouseMovePart;
        private System.Windows.Forms.TextBox tbPartName;
        private System.Windows.Forms.ListBox listPartsInstantiated;
        private System.Windows.Forms.Panel panelPalette;
        private System.Windows.Forms.Button btKeepPartSave;
        private System.Windows.Forms.Button btKeepPartLoad;
        private System.Windows.Forms.DataVisualization.Charting.Chart Timeline;
        private System.Windows.Forms.ListBox listFrames;
        private System.Windows.Forms.Button btNewFrame;
        private System.Windows.Forms.Button btMoveUp;
        private System.Windows.Forms.Button btMoveDown;
        private System.Windows.Forms.Button btRender;
        private System.Windows.Forms.TextBox tbFrameName;
        private System.Windows.Forms.Button btKeepPartClear;
        private System.Windows.Forms.Button btRemoveFrame;
        private System.Windows.Forms.Timer timerRenderView;
        private System.Windows.Forms.TrackBar ctbRenderViewInterval;
        private System.Windows.Forms.NumericUpDown nupRenderViewInterval;
        private System.Windows.Forms.Button btRenderLerp;
        private System.Windows.Forms.Timer timerRenderViewLerp;
        private System.Windows.Forms.CheckBox cbReverseLastLerpRotation;
        private System.Windows.Forms.Button btRenderExport;
        private System.Windows.Forms.Button btRenderLerpExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.NumericUpDown nupIntervalRenderLerp;
        private System.Windows.Forms.Label lbSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportLerpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem eXitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPaletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savePaletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearPaletteToolStripMenuItem;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numPartPositionX;
        private System.Windows.Forms.Label lbPartPositionX;
        private System.Windows.Forms.NumericUpDown numPartPositionY;
        private System.Windows.Forms.Label lbPartPositionY;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbPartPosition;
        private System.Windows.Forms.Label lbPartRotation;
        private System.Windows.Forms.NumericUpDown numPartAngle;
        private System.Windows.Forms.Label lbPartAngle;
        private System.Windows.Forms.NumericUpDown numPartScaleX;
        private System.Windows.Forms.Label lbPartScaleY;
        private System.Windows.Forms.NumericUpDown numPartScaleY;
        private System.Windows.Forms.Label lbPartScaleX;
        private System.Windows.Forms.Label lbPartScale;
        private System.Windows.Forms.Button btPartUnselect;
        private System.Windows.Forms.Label lbPartInfos;
        private System.Windows.Forms.Label lbPartSeparator;
        private System.Windows.Forms.Label lbPartAreaSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numPartAreaSizeWidth;
        private System.Windows.Forms.Label lbPartAreaSizeHeight;
        private System.Windows.Forms.NumericUpDown numPartAreaSizeHeight;
        private System.Windows.Forms.Label lbPartAreaSizeWidth;
        private System.Windows.Forms.Button btPartAreaSizeApply;
        private System.Windows.Forms.Button btPartAreaSizeOpenSeparatedWindow;
        private System.Windows.Forms.Button btPartAreaSizeCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem renderColorToolStripMenuItem;
        private System.Windows.Forms.Button btDrawPart_ColorPicker;
        private System.Windows.Forms.Button btPartNameApply;
        private System.Windows.Forms.Button btPartPivotReset;
        private System.Windows.Forms.Label lbPartPivot;
        private System.Windows.Forms.CheckBox cbSnap;
        private System.Windows.Forms.Label lbSnap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem snapSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem snapAllPartsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Label lbTargetX;
        private System.Windows.Forms.Label lbTargetY;
        private System.Windows.Forms.Label lbTargetWidth;
        private System.Windows.Forms.Label lbTargetHeight;
        private System.Windows.Forms.NumericUpDown nudTargetSize;
        private System.Windows.Forms.Button btCenterTarget;
        private System.Windows.Forms.Label lbTargetSize;
        private System.Windows.Forms.NumericUpDown nudTargetX;
        private System.Windows.Forms.NumericUpDown nudTargetY;
        private System.Windows.Forms.NumericUpDown nudTargetWidth;
        private System.Windows.Forms.NumericUpDown nudTargetHeight;
        private System.Windows.Forms.Label lbTargetColor;
        private System.Windows.Forms.Button btTargetColor;
        private System.Windows.Forms.Label lbTargetBorderSize;
        private System.Windows.Forms.NumericUpDown nudTargetBorderSize;
        private System.Windows.Forms.Label lbPartID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudPartID;
        private System.Windows.Forms.Button btPartShowIndexes;
        private System.Windows.Forms.Button btFrameShowAllIndexes;
    }
}