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
            drawButton = new RadioButton();
            selectButton = new RadioButton();
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
            modeGroupBox.Controls.Add(drawButton);
            modeGroupBox.Controls.Add(selectButton);
            modeGroupBox.Dock = DockStyle.Top;
            modeGroupBox.Location = new Point(0, 0);
            modeGroupBox.Name = "modeGroupBox";
            modeGroupBox.Size = new Size(217, 100);
            modeGroupBox.TabIndex = 0;
            modeGroupBox.TabStop = false;
            modeGroupBox.Text = "Mode";
            // 
            // drawButton
            // 
            drawButton.AutoSize = true;
            drawButton.Dock = DockStyle.Top;
            drawButton.Location = new Point(3, 38);
            drawButton.Name = "drawButton";
            drawButton.Size = new Size(211, 19);
            drawButton.TabIndex = 1;
            drawButton.TabStop = true;
            drawButton.Text = "Draw";
            drawButton.UseVisualStyleBackColor = true;
            // 
            // selectButton
            // 
            selectButton.AutoSize = true;
            selectButton.Dock = DockStyle.Top;
            selectButton.Location = new Point(3, 19);
            selectButton.Name = "selectButton";
            selectButton.Size = new Size(211, 19);
            selectButton.TabIndex = 0;
            selectButton.TabStop = true;
            selectButton.Text = "Select";
            selectButton.UseVisualStyleBackColor = true;
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
        private RadioButton drawButton;
        private RadioButton selectButton;
    }
}