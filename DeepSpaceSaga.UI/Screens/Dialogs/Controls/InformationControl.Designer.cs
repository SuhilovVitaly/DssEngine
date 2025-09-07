namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls
{
    partial class InformationControl
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
            crlPortrait = new PictureBox();
            crlRank = new Label();
            crlName = new Label();
            crlMessage = new RpgTextOutputControl();
            crlMessageTitle = new Label();
            ((ISupportInitialize)crlPortrait).BeginInit();
            SuspendLayout();
            // 
            // crlPortrait
            // 
            crlPortrait.Location = new Point(58, 86);
            crlPortrait.Margin = new Padding(4);
            crlPortrait.Name = "crlPortrait";
            crlPortrait.Size = new Size(205, 205);
            crlPortrait.SizeMode = PictureBoxSizeMode.StretchImage;
            crlPortrait.TabIndex = 2;
            crlPortrait.TabStop = false;
            // 
            // crlRank
            // 
            crlRank.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            crlRank.ForeColor = Color.OrangeRed;
            crlRank.Location = new Point(58, 295);
            crlRank.Margin = new Padding(4, 0, 4, 0);
            crlRank.Name = "crlRank";
            crlRank.Size = new Size(205, 31);
            crlRank.TabIndex = 3;
            crlRank.Text = "label1";
            crlRank.TextAlign = ContentAlignment.TopCenter;
            // 
            // crlName
            // 
            crlName.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            crlName.ForeColor = Color.WhiteSmoke;
            crlName.Location = new Point(58, 335);
            crlName.Margin = new Padding(4, 0, 4, 0);
            crlName.Name = "crlName";
            crlName.Size = new Size(205, 31);
            crlName.TabIndex = 3;
            crlName.Text = "label1";
            crlName.TextAlign = ContentAlignment.TopCenter;
            // 
            // crlMessage
            // 
            crlMessage.BackColor = Color.Black;
            crlMessage.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            crlMessage.ForeColor = Color.WhiteSmoke;
            crlMessage.Location = new Point(286, 86);
            crlMessage.Margin = new Padding(4, 0, 4, 0);
            crlMessage.Name = "crlMessage";
            crlMessage.Size = new Size(1031, 554);
            crlMessage.TabIndex = 4;
            crlMessage.TextOutputSpeedMs = 50;
            // 
            // crlMessageTitle
            // 
            crlMessageTitle.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlMessageTitle.ForeColor = Color.DarkKhaki;
            crlMessageTitle.Location = new Point(58, 29);
            crlMessageTitle.Margin = new Padding(4, 0, 4, 0);
            crlMessageTitle.Name = "crlMessageTitle";
            crlMessageTitle.Size = new Size(1260, 31);
            crlMessageTitle.TabIndex = 5;
            crlMessageTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // HelpSystemControl
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.Black;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(crlMessageTitle);
            Controls.Add(crlMessage);
            Controls.Add(crlName);
            Controls.Add(crlRank);
            Controls.Add(crlPortrait);
            DoubleBuffered = true;
            Margin = new Padding(4);
            Name = "HelpSystemControl";
            Size = new Size(1375, 875);
            ((ISupportInitialize)crlPortrait).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox crlPortrait;
        private Label crlRank;
        private Label crlName;
        private RpgTextOutputControl crlMessage;
        private Label crlMessageTitle;
    }
}
