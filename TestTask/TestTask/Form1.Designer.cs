namespace TestTask
{
    partial class TestTask
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
            menuStrip1 = new MenuStrip();
            FileMenuChoice = new ToolStripMenuItem();
            ExitChoice = new ToolStripMenuItem();
            CameraLabel = new Label();
            CameraChoice = new ComboBox();
            Viewer = new Button();
            FramePictures = new PictureBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)FramePictures).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { FileMenuChoice });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(946, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // FileMenuChoice
            // 
            FileMenuChoice.DropDownItems.AddRange(new ToolStripItem[] { ExitChoice });
            FileMenuChoice.Name = "FileMenuChoice";
            FileMenuChoice.Size = new Size(59, 24);
            FileMenuChoice.Text = "Файл";
            // 
            // ExitChoice
            // 
            ExitChoice.Name = "ExitChoice";
            ExitChoice.Size = new Size(136, 26);
            ExitChoice.Text = "Выход";
            // 
            // CameraLabel
            // 
            CameraLabel.AutoSize = true;
            CameraLabel.Location = new Point(12, 30);
            CameraLabel.Name = "CameraLabel";
            CameraLabel.Size = new Size(65, 20);
            CameraLabel.TabIndex = 1;
            CameraLabel.Text = "Камера:";
            // 
            // CameraChoice
            // 
            CameraChoice.FormattingEnabled = true;
            CameraChoice.Location = new Point(83, 27);
            CameraChoice.Name = "CameraChoice";
            CameraChoice.Size = new Size(151, 28);
            CameraChoice.TabIndex = 2;
            CameraChoice.SelectedIndexChanged += CameraChoiceSelectedIndexChange;
            // 
            // Viewer
            // 
            Viewer.Location = new Point(240, 26);
            Viewer.Name = "Viewer";
            Viewer.Size = new Size(119, 29);
            Viewer.TabIndex = 3;
            Viewer.Text = "Смотреть";
            Viewer.UseVisualStyleBackColor = true;
            Viewer.Click += Viewer_Click;
            // 
            // FramePictures
            // 
            FramePictures.Location = new Point(-102, 75);
            FramePictures.Name = "FramePictures";
            FramePictures.Size = new Size(1048, 518);
            FramePictures.SizeMode = PictureBoxSizeMode.Zoom;
            FramePictures.TabIndex = 4;
            FramePictures.TabStop = false;
            // 
            // TestTask
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(946, 593);
            Controls.Add(FramePictures);
            Controls.Add(Viewer);
            Controls.Add(CameraChoice);
            Controls.Add(CameraLabel);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "TestTask";
            Text = "TestTask";
            Load += TestTask_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)FramePictures).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem FileMenuChoice;
        private ToolStripMenuItem ExitChoice;
        private Label CameraLabel;
        private ComboBox CameraChoice;
        private Button Viewer;
        private PictureBox FramePictures;
    }
}
