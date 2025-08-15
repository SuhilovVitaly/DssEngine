namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls
{
    partial class HelpSystemControl
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
            crlMessage = new Label();
            crlMessageTitle = new Label();
            ((ISupportInitialize)crlPortrait).BeginInit();
            SuspendLayout();
            // 
            // crlPortrait
            // 
            crlPortrait.Location = new Point(46, 69);
            crlPortrait.Name = "crlPortrait";
            crlPortrait.Size = new Size(164, 164);
            crlPortrait.SizeMode = PictureBoxSizeMode.StretchImage;
            crlPortrait.TabIndex = 2;
            crlPortrait.TabStop = false;
            // 
            // crlRank
            // 
            crlRank.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            crlRank.ForeColor = Color.OrangeRed;
            crlRank.Location = new Point(46, 236);
            crlRank.Name = "crlRank";
            crlRank.Size = new Size(164, 25);
            crlRank.TabIndex = 3;
            crlRank.Text = "label1";
            crlRank.TextAlign = ContentAlignment.TopCenter;
            // 
            // crlName
            // 
            crlName.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            crlName.ForeColor = Color.WhiteSmoke;
            crlName.Location = new Point(46, 268);
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
            crlMessage.Location = new Point(229, 69);
            crlMessage.Name = "crlMessage";
            crlMessage.Size = new Size(825, 443);
            crlMessage.TabIndex = 4;
            // 
            // crlMessageTitle
            // 
            crlMessageTitle.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlMessageTitle.ForeColor = Color.DarkKhaki;
            crlMessageTitle.Location = new Point(46, 23);
            crlMessageTitle.Name = "crlMessageTitle";
            crlMessageTitle.Size = new Size(1008, 25);
            crlMessageTitle.TabIndex = 5;
            crlMessageTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // HelpSystemControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(crlMessageTitle);
            Controls.Add(crlMessage);
            Controls.Add(crlName);
            Controls.Add(crlRank);
            Controls.Add(crlPortrait);
            DoubleBuffered = true;
            Name = "HelpSystemControl";
            Size = new Size(1100, 700);
            ((ISupportInitialize)crlPortrait).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private PictureBox crlPortrait;
        private Label crlRank;
        private Label crlName;
        private Label crlMessage;
        private Label crlMessageTitle;
    }
}
