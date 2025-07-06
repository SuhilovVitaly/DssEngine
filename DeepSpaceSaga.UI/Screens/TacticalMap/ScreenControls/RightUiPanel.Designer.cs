namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls
{
    partial class RightUiPanel
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
            crlCloseRightPanel = new PictureBox();
            ((ISupportInitialize)crlCloseRightPanel).BeginInit();
            SuspendLayout();
            // 
            // crlCloseRightPanel
            // 
            crlCloseRightPanel.BackColor = Color.Black;
            crlCloseRightPanel.Cursor = Cursors.Hand;
            crlCloseRightPanel.Location = new Point(745, 3);
            crlCloseRightPanel.Name = "crlCloseRightPanel";
            crlCloseRightPanel.Size = new Size(32, 32);
            crlCloseRightPanel.TabIndex = 0;
            crlCloseRightPanel.TabStop = false;
            crlCloseRightPanel.Click += pictureBox1_Click;
            // 
            // RightUiPanel
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(crlCloseRightPanel);
            DoubleBuffered = true;
            Name = "RightUiPanel";
            Size = new Size(780, 966);
            ((ISupportInitialize)crlCloseRightPanel).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox crlCloseRightPanel;
    }
}
