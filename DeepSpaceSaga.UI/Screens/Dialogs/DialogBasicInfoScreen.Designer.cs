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
            crlMessage = new Controls.RpgTextOutputControl();
            ExitButtonsContainer = new Controls.TransparentPanel();
            pictureBox1 = new UI.Controls.BlurredPictureBox();
            crlTitle = new Label();
            transparentPanel1 = new Controls.TransparentPanel();
            button1 = new Button();
            ((ISupportInitialize)pictureBox1).BeginInit();
            transparentPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // crlMessage
            // 
            crlMessage.BackColor = Color.Transparent;
            crlMessage.Location = new Point(67, 156);
            crlMessage.Margin = new Padding(4);
            crlMessage.Name = "crlMessage";
            crlMessage.Size = new Size(694, 521);
            crlMessage.TabIndex = 8;
            crlMessage.TextOutputSpeedMs = 100;
            // 
            // ExitButtonsContainer
            // 
            ExitButtonsContainer.BackColor = Color.Transparent;
            ExitButtonsContainer.Location = new Point(307, 700);
            ExitButtonsContainer.Name = "ExitButtonsContainer";
            ExitButtonsContainer.Size = new Size(887, 163);
            ExitButtonsContainer.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.RosyBrown;
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
            crlTitle.Font = new Font("Courier New", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlTitle.ForeColor = Color.OrangeRed;
            crlTitle.Location = new Point(141, 112);
            crlTitle.Name = "crlTitle";
            crlTitle.Size = new Size(1319, 40);
            crlTitle.TabIndex = 0;
            crlTitle.Text = "label1";
            // 
            // transparentPanel1
            // 
            transparentPanel1.BackColor = Color.Transparent;
            transparentPanel1.Controls.Add(button1);
            transparentPanel1.Location = new Point(846, 217);
            transparentPanel1.Name = "transparentPanel1";
            transparentPanel1.Size = new Size(388, 299);
            transparentPanel1.TabIndex = 10;
            // 
            // button1
            // 
            button1.Location = new Point(153, 197);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // DialogBasicInfoScreen
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1375, 875);
            Controls.Add(transparentPanel1);
            Controls.Add(crlTitle);
            Controls.Add(crlMessage);
            Controls.Add(ExitButtonsContainer);
            Controls.Add(pictureBox1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "DialogBasicInfoScreen";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "DialogBasicScreen";
            ((ISupportInitialize)pictureBox1).EndInit();
            transparentPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Controls.TransparentPanel ExitButtonsContainer;
        private Label crlTitle;
        private UI.Controls.BlurredPictureBox pictureBox1;
        private Controls.RpgTextOutputControl crlMessage;
        private Controls.TransparentPanel transparentPanel1;
        private Button button1;
    }
}