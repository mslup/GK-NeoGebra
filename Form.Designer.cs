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
            splitContainer1 = new SplitContainer();
            canvas = new PictureBox();
            modeGroupBox = new GroupBox();
            bresenhamButton = new RadioButton();
            winformsLineButton = new RadioButton();
            menuStrip1 = new MenuStrip();
            canvasToolStripMenuItem = new ToolStripMenuItem();
            clearToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            modeGroupBox.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(canvas);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(modeGroupBox);
            splitContainer1.Size = new Size(800, 426);
            splitContainer1.SplitterDistance = 579;
            splitContainer1.TabIndex = 0;
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
            // modeGroupBox
            // 
            modeGroupBox.Controls.Add(bresenhamButton);
            modeGroupBox.Controls.Add(winformsLineButton);
            modeGroupBox.Dock = DockStyle.Top;
            modeGroupBox.Location = new Point(0, 0);
            modeGroupBox.Name = "modeGroupBox";
            modeGroupBox.Size = new Size(217, 100);
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
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            Name = "NeoGebra";
            Text = "NeoGebra";
            KeyDown += NeoGebra_KeyDown;
            KeyUp += NeoGebra_KeyUp;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            modeGroupBox.ResumeLayout(false);
            modeGroupBox.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer1;
        private PictureBox canvas;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem canvasToolStripMenuItem;
        private ToolStripMenuItem clearToolStripMenuItem;
        private GroupBox modeGroupBox;
        private RadioButton bresenhamButton;
        private RadioButton winformsLineButton;
    }
}