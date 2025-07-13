namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls
{
    partial class RightUiToolbar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            crlTargetSystem = new HoverPictureBox();
            hoverPictureBox1 = new HoverPictureBox();
            ((ISupportInitialize)crlTargetSystem).BeginInit();
            ((ISupportInitialize)hoverPictureBox1).BeginInit();
            SuspendLayout();
            // 
            // crlTargetSystem
            // 
            crlTargetSystem.Image = Properties.Resources.right_toolbar;
            crlTargetSystem.Location = new Point(22, 18);
            crlTargetSystem.Name = "crlTargetSystem";
            crlTargetSystem.NormalImage = Properties.Resources.right_toolbar;
            crlTargetSystem.SelectedImage = Properties.Resources.right_toolbar_selected;
            crlTargetSystem.Size = new Size(36, 36);
            crlTargetSystem.SizeMode = PictureBoxSizeMode.StretchImage;
            crlTargetSystem.TabIndex = 0;
            crlTargetSystem.TabStop = false;
            crlTargetSystem.Click += crlTargetSystem_Click;
            // 
            // hoverPictureBox1
            // 
            hoverPictureBox1.Image = Properties.Resources.right_toolbar;
            hoverPictureBox1.Location = new Point(22, 75);
            hoverPictureBox1.Name = "hoverPictureBox1";
            hoverPictureBox1.NormalImage = Properties.Resources.right_toolbar;
            hoverPictureBox1.SelectedImage = Properties.Resources.right_toolbar_selected;
            hoverPictureBox1.Size = new Size(36, 36);
            hoverPictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            hoverPictureBox1.TabIndex = 1;
            hoverPictureBox1.TabStop = false;
            hoverPictureBox1.Click += hoverPictureBox1_Click;
            // 
            // RightUiToolbar
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(hoverPictureBox1);
            Controls.Add(crlTargetSystem);
            DoubleBuffered = true;
            Name = "RightUiToolbar";
            Size = new Size(80, 974);
            ((ISupportInitialize)crlTargetSystem).EndInit();
            ((ISupportInitialize)hoverPictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private CommonControls.HoverPictureBox crlTargetSystem;
        private HoverPictureBox hoverPictureBox1;
    }
}
