using DeepSpaceSaga.UI.Screens.Dialogs.Controls;

namespace DeepSpaceSaga.UI.Screens.Dialogs
{
    partial class DialogBasicInfoScreen
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
            crlMessage = new RpgTextOutputControl();
            pictureBox1 = new BlurredPictureBox();
            crlTitle = new Label();
            ((ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // crlMessage
            // 
            crlMessage.BackColor = Color.Transparent;
            crlMessage.Location = new Point(159, 109);
            crlMessage.Margin = new Padding(4);
            crlMessage.Name = "crlMessage";
            crlMessage.Size = new Size(694, 521);
            crlMessage.TabIndex = 8;
            crlMessage.TextOutputSpeedMs = 100;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.DarkBlue;
            pictureBox1.BlurIntensity = 0.8F;
            pictureBox1.BlurSteps = 20;
            pictureBox1.BlurWidth = 100;
            pictureBox1.Location = new Point(434, 32);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1054, 874);
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // crlTitle
            // 
            crlTitle.BackColor = Color.Transparent;
            crlTitle.BorderStyle = BorderStyle.FixedSingle;
            crlTitle.Dock = DockStyle.Top;
            crlTitle.Font = new Font("Courier New", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlTitle.ForeColor = Color.OrangeRed;
            crlTitle.Location = new Point(0, 0);
            crlTitle.Name = "crlTitle";
            crlTitle.Size = new Size(1375, 40);
            crlTitle.TabIndex = 0;
            crlTitle.Text = "label1";
            // 
            // DialogBasicInfoScreen
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1375, 875);
            Controls.Add(crlTitle);
            Controls.Add(crlMessage);
            Controls.Add(pictureBox1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "DialogBasicInfoScreen";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "DialogBasicScreen";
            ((ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label crlTitle;
        private BlurredPictureBox pictureBox1;
        private Controls.RpgTextOutputControl crlMessage;
    }
}