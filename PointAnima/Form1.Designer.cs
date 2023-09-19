namespace PointAnima
{
    partial class AnimaForm
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
            this.Render = new System.Windows.Forms.PictureBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.btLessFrames = new System.Windows.Forms.Button();
            this.btMoreFrames = new System.Windows.Forms.Button();
            this.lbFramePerFrameCount = new System.Windows.Forms.Label();
            this.btLerp = new System.Windows.Forms.Button();
            this.numFrameTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btPlay = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Render)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrameTime)).BeginInit();
            this.SuspendLayout();
            // 
            // Render
            // 
            this.Render.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Render.Location = new System.Drawing.Point(0, 0);
            this.Render.Name = "Render";
            this.Render.Size = new System.Drawing.Size(715, 314);
            this.Render.TabIndex = 0;
            this.Render.TabStop = false;
            this.Render.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Render_MouseDown);
            this.Render.MouseLeave += new System.EventHandler(this.Render_MouseLeave);
            this.Render.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Render_MouseMove);
            this.Render.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Render_MouseUp);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 343);
            this.trackBar1.Maximum = 3;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(692, 45);
            this.trackBar1.TabIndex = 1;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // btLessFrames
            // 
            this.btLessFrames.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btLessFrames.Location = new System.Drawing.Point(12, 376);
            this.btLessFrames.Name = "btLessFrames";
            this.btLessFrames.Size = new System.Drawing.Size(39, 41);
            this.btLessFrames.TabIndex = 2;
            this.btLessFrames.Text = "-";
            this.btLessFrames.UseVisualStyleBackColor = true;
            this.btLessFrames.Click += new System.EventHandler(this.btLessFrames_Click);
            // 
            // btMoreFrames
            // 
            this.btMoreFrames.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btMoreFrames.Location = new System.Drawing.Point(665, 376);
            this.btMoreFrames.Name = "btMoreFrames";
            this.btMoreFrames.Size = new System.Drawing.Size(39, 41);
            this.btMoreFrames.TabIndex = 2;
            this.btMoreFrames.Text = "+";
            this.btMoreFrames.UseVisualStyleBackColor = true;
            this.btMoreFrames.Click += new System.EventHandler(this.btMoreFrames_Click);
            // 
            // lbFramePerFrameCount
            // 
            this.lbFramePerFrameCount.AutoSize = true;
            this.lbFramePerFrameCount.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.lbFramePerFrameCount.Location = new System.Drawing.Point(333, 392);
            this.lbFramePerFrameCount.Name = "lbFramePerFrameCount";
            this.lbFramePerFrameCount.Size = new System.Drawing.Size(48, 25);
            this.lbFramePerFrameCount.TabIndex = 3;
            this.lbFramePerFrameCount.Text = "0 / F";
            // 
            // btLerp
            // 
            this.btLerp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btLerp.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btLerp.Location = new System.Drawing.Point(12, 434);
            this.btLerp.Name = "btLerp";
            this.btLerp.Size = new System.Drawing.Size(71, 41);
            this.btLerp.TabIndex = 4;
            this.btLerp.Text = "Lerp";
            this.btLerp.UseVisualStyleBackColor = false;
            this.btLerp.Click += new System.EventHandler(this.btLerp_Click);
            // 
            // numFrameTime
            // 
            this.numFrameTime.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.numFrameTime.Location = new System.Drawing.Point(277, 443);
            this.numFrameTime.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numFrameTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrameTime.Name = "numFrameTime";
            this.numFrameTime.Size = new System.Drawing.Size(116, 32);
            this.numFrameTime.TabIndex = 5;
            this.numFrameTime.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numFrameTime.ValueChanged += new System.EventHandler(this.numFrameTime_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.label1.Location = new System.Drawing.Point(102, 450);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Frame time (ticks) :";
            // 
            // btPlay
            // 
            this.btPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btPlay.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btPlay.Location = new System.Drawing.Point(633, 434);
            this.btPlay.Name = "btPlay";
            this.btPlay.Size = new System.Drawing.Size(71, 41);
            this.btPlay.TabIndex = 7;
            this.btPlay.Text = "Play";
            this.btPlay.UseVisualStyleBackColor = false;
            this.btPlay.Click += new System.EventHandler(this.btPlay_Click);
            // 
            // AnimaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 487);
            this.Controls.Add(this.btPlay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numFrameTime);
            this.Controls.Add(this.btLerp);
            this.Controls.Add(this.lbFramePerFrameCount);
            this.Controls.Add(this.btMoreFrames);
            this.Controls.Add(this.btLessFrames);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.Render);
            this.KeyPreview = true;
            this.Name = "AnimaForm";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Render)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrameTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Render;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button btLessFrames;
        private System.Windows.Forms.Button btMoreFrames;
        private System.Windows.Forms.Label lbFramePerFrameCount;
        private System.Windows.Forms.Button btLerp;
        private System.Windows.Forms.NumericUpDown numFrameTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btPlay;
    }
}

