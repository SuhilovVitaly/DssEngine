namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls
{
    partial class HelpSystemTwoPersonesControl
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
            crlNamePrevious = new Label();
            crlRankPrevious = new Label();
            crlPortraitPrevious = new PictureBox();
            crlMessagePrevious = new Label();
            ((ISupportInitialize)crlPortrait).BeginInit();
            ((ISupportInitialize)crlPortraitPrevious).BeginInit();
            SuspendLayout();
            // 
            // crlPortrait
            // 
            crlPortrait.Location = new Point(46, 51);
            crlPortrait.Name = "crlPortrait";
            crlPortrait.Size = new Size(164, 164);
            crlPortrait.SizeMode = PictureBoxSizeMode.StretchImage;
            crlPortrait.TabIndex = 2;
            crlPortrait.TabStop = false;
            // 
            // crlRank
            // 
            crlRank.BackColor = Color.FromArgb(12, 12, 12, 50);
            crlRank.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            crlRank.ForeColor = Color.OrangeRed;
            crlRank.Location = new Point(46, 218);
            crlRank.Name = "crlRank";
            crlRank.Size = new Size(164, 25);
            crlRank.TabIndex = 3;
            crlRank.Text = "label1";
            crlRank.TextAlign = ContentAlignment.TopCenter;
            // 
            // crlName
            // 
            crlName.BackColor = Color.FromArgb(12, 12, 12, 50);
            crlName.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            crlName.ForeColor = Color.WhiteSmoke;
            crlName.Location = new Point(46, 243);
            crlName.Name = "crlName";
            crlName.Size = new Size(164, 25);
            crlName.TabIndex = 3;
            crlName.Text = "label1";
            crlName.TextAlign = ContentAlignment.TopCenter;
            // 
            // crlMessage
            // 
            crlMessage.BackColor = Color.Black;
            crlMessage.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            crlMessage.ForeColor = Color.WhiteSmoke;
            crlMessage.Location = new Point(229, 51);
            crlMessage.Name = "crlMessage";
            crlMessage.Size = new Size(828, 252);
            crlMessage.TabIndex = 4;
            // 
            // crlMessageTitle
            // 
            crlMessageTitle.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlMessageTitle.ForeColor = Color.DarkKhaki;
            crlMessageTitle.Location = new Point(46, 10);
            crlMessageTitle.Name = "crlMessageTitle";
            crlMessageTitle.Size = new Size(1011, 25);
            crlMessageTitle.TabIndex = 5;
            crlMessageTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // crlNamePrevious
            // 
            crlNamePrevious.BackColor = Color.FromArgb(12, 12, 12, 200);
            crlNamePrevious.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            crlNamePrevious.ForeColor = Color.WhiteSmoke;
            crlNamePrevious.Location = new Point(893, 521);
            crlNamePrevious.Name = "crlNamePrevious";
            crlNamePrevious.Size = new Size(164, 25);
            crlNamePrevious.TabIndex = 8;
            crlNamePrevious.Text = "label1";
            crlNamePrevious.TextAlign = ContentAlignment.TopCenter;
            // 
            // crlRankPrevious
            // 
            crlRankPrevious.BackColor = Color.FromArgb(12, 12, 12, 200);
            crlRankPrevious.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            crlRankPrevious.ForeColor = Color.OrangeRed;
            crlRankPrevious.Location = new Point(893, 496);
            crlRankPrevious.Name = "crlRankPrevious";
            crlRankPrevious.Size = new Size(164, 25);
            crlRankPrevious.TabIndex = 7;
            crlRankPrevious.Text = "label1";
            crlRankPrevious.TextAlign = ContentAlignment.TopCenter;
            // 
            // crlPortraitPrevious
            // 
            crlPortraitPrevious.Location = new Point(893, 328);
            crlPortraitPrevious.Name = "crlPortraitPrevious";
            crlPortraitPrevious.Size = new Size(164, 164);
            crlPortraitPrevious.SizeMode = PictureBoxSizeMode.StretchImage;
            crlPortraitPrevious.TabIndex = 6;
            crlPortraitPrevious.TabStop = false;
            // 
            // crlMessagePrevious
            // 
            crlMessagePrevious.BackColor = Color.Black;
            crlMessagePrevious.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            crlMessagePrevious.ForeColor = Color.WhiteSmoke;
            crlMessagePrevious.Location = new Point(46, 328);
            crlMessagePrevious.Name = "crlMessagePrevious";
            crlMessagePrevious.Size = new Size(830, 238);
            crlMessagePrevious.TabIndex = 9;
            // 
            // HelpSystemTwoPersonesControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(crlMessagePrevious);
            Controls.Add(crlNamePrevious);
            Controls.Add(crlRankPrevious);
            Controls.Add(crlPortraitPrevious);
            Controls.Add(crlMessageTitle);
            Controls.Add(crlMessage);
            Controls.Add(crlName);
            Controls.Add(crlRank);
            Controls.Add(crlPortrait);
            DoubleBuffered = true;
            Name = "HelpSystemTwoPersonesControl";
            Size = new Size(1100, 700);
            ((ISupportInitialize)crlPortrait).EndInit();
            ((ISupportInitialize)crlPortraitPrevious).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox crlPortrait;
        private Label crlRank;
        private Label crlName;
        private RpgTextOutputControl crlMessage;
        private Label crlMessageTitle;
        private Label crlNamePrevious;
        private Label crlRankPrevious;
        private PictureBox crlPortraitPrevious;
        private Label crlMessagePrevious;
    }
}
