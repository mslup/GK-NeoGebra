namespace lab1
{
    partial class NeoGebra
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer = new SplitContainer();
            canvas = new PictureBox();
            offsetGroupBox = new GroupBox();
            offsetSlider = new TrackBar();
            checkBox1 = new CheckBox();
            modeGroupBox = new GroupBox();
            bresenhamButton = new RadioButton();
            winformsLineButton = new RadioButton();
            menuStrip1 = new MenuStrip();
            canvasToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            offsetGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)offsetSlider).BeginInit();
            modeGroupBox.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 24);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(canvas);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(offsetGroupBox);
            splitContainer.Panel2.Controls.Add(modeGroupBox);
            splitContainer.Size = new Size(800, 426);
            splitContainer.SplitterDistance = 579;
            splitContainer.TabIndex = 0;
            // 
            // canvas
            // 
            canvas.BackColor = Color.GhostWhite;
            canvas.Dock = DockStyle.Fill;
            canvas.Location = new Point(0, 0);
            canvas.Name = "canvas";
            canvas.Size = new Size(579, 426);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.Paint += canvas_Paint;
            canvas.MouseClick += canvas_MouseClick;
            canvas.MouseDoubleClick += canvas_MouseDoubleClick;
            canvas.MouseDown += canvas_MouseDown;
            canvas.MouseMove += canvas_MouseMove;
            canvas.MouseUp += canvas_MouseUp;
            // 
            // offsetGroupBox
            // 
            offsetGroupBox.Controls.Add(offsetSlider);
            offsetGroupBox.Controls.Add(checkBox1);
            offsetGroupBox.Dock = DockStyle.Top;
            offsetGroupBox.Location = new Point(0, 64);
            offsetGroupBox.Name = "offsetGroupBox";
            offsetGroupBox.Size = new Size(217, 74);
            offsetGroupBox.TabIndex = 1;
            offsetGroupBox.TabStop = false;
            offsetGroupBox.Text = "Offset polygon options";
            // 
            // offsetSlider
            // 
            offsetSlider.Dock = DockStyle.Fill;
            offsetSlider.LargeChange = 10;
            offsetSlider.Location = new Point(3, 38);
            offsetSlider.Maximum = 100;
            offsetSlider.Name = "offsetSlider";
            offsetSlider.Size = new Size(211, 33);
            offsetSlider.TabIndex = 1;
            offsetSlider.TickFrequency = 5;
            offsetSlider.Scroll += offsetSlider_Scroll;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Dock = DockStyle.Top;
            checkBox1.Location = new Point(3, 19);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(211, 19);
            checkBox1.TabIndex = 0;
            checkBox1.Text = "Draw offset polygon";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // modeGroupBox
            // 
            modeGroupBox.Controls.Add(bresenhamButton);
            modeGroupBox.Controls.Add(winformsLineButton);
            modeGroupBox.Dock = DockStyle.Top;
            modeGroupBox.Location = new Point(0, 0);
            modeGroupBox.Name = "modeGroupBox";
            modeGroupBox.Size = new Size(217, 64);
            modeGroupBox.TabIndex = 0;
            modeGroupBox.TabStop = false;
            modeGroupBox.Text = "Line drawing mode";
            // 
            // bresenhamButton
            // 
            bresenhamButton.AutoSize = true;
            bresenhamButton.Checked = true;
            bresenhamButton.Dock = DockStyle.Top;
            bresenhamButton.Location = new Point(3, 38);
            bresenhamButton.Name = "bresenhamButton";
            bresenhamButton.Size = new Size(211, 19);
            bresenhamButton.TabIndex = 1;
            bresenhamButton.TabStop = true;
            bresenhamButton.Text = "Bresenham Algorithm";
            bresenhamButton.UseVisualStyleBackColor = true;
            bresenhamButton.CheckedChanged += bresenhamButton_CheckedChanged;
            // 
            // winformsLineButton
            // 
            winformsLineButton.AutoSize = true;
            winformsLineButton.Dock = DockStyle.Top;
            winformsLineButton.Location = new Point(3, 19);
            winformsLineButton.Name = "winformsLineButton";
            winformsLineButton.Size = new Size(211, 19);
            winformsLineButton.TabIndex = 0;
            winformsLineButton.Text = "WinForms Library";
            winformsLineButton.UseVisualStyleBackColor = true;
            winformsLineButton.CheckedChanged += winformsLineButton_CheckedChanged;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { canvasToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // canvasToolStripMenuItem
            // 
            canvasToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { clearToolStripMenuItem });
            canvasToolStripMenuItem.Name = "canvasToolStripMenuItem";
            canvasToolStripMenuItem.Size = new Size(57, 20);
            canvasToolStripMenuItem.Text = "Canvas";
            // 
            // clearToolStripMenuItem
            // 
            clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            clearToolStripMenuItem.Size = new Size(101, 22);
            clearToolStripMenuItem.Text = "Clear";
            clearToolStripMenuItem.Click += clearToolStripMenuItem_Click;
            // 
            // NeoGebra
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer);
            Controls.Add(menuStrip1);
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            Name = "NeoGebra";
            Text = "NeoGebra";
            KeyDown += NeoGebra_KeyDown;
            KeyUp += NeoGebra_KeyUp;
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            offsetGroupBox.ResumeLayout(false);
            offsetGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)offsetSlider).EndInit();
            modeGroupBox.ResumeLayout(false);
            modeGroupBox.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer;
        private PictureBox canvas;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem canvasToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private GroupBox modeGroupBox;
        private RadioButton bresenhamButton;
        private RadioButton winformsLineButton;
        private GroupBox offsetGroupBox;
        private TrackBar offsetSlider;
        private CheckBox checkBox1;
    }
}