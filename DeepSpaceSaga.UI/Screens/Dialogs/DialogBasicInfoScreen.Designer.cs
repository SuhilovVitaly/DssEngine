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
            panel1 = new Panel();
            panel3 = new Panel();
            panel2 = new Panel();
            pictureBox1 = new UI.Controls.BlurredPictureBox();
            crlTitle = new Label();
            ExitButtonsContainer = new Panel();
            crlMessage = new Controls.RpgTextOutputControl();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(crlMessage);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(ExitButtonsContainer);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1375, 875);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Transparent;
            panel3.Dock = DockStyle.Left;
            panel3.Location = new Point(0, 79);
            panel3.Name = "panel3";
            panel3.Size = new Size(32, 521);
            panel3.TabIndex = 7;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(pictureBox1);
            panel2.Controls.Add(crlTitle);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1373, 79);
            panel2.TabIndex = 6;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Black;
            pictureBox1.BlurIntensity = 0.8F;
            pictureBox1.BlurSteps = 20;
            pictureBox1.BlurWidth = 100;
            pictureBox1.Location = new Point(316, -1);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1054, 874);
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // crlTitle
            // 
            crlTitle.Font = new Font("Courier New", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlTitle.ForeColor = Color.OrangeRed;
            crlTitle.Location = new Point(32, 20);
            crlTitle.Name = "crlTitle";
            crlTitle.Size = new Size(1319, 40);
            crlTitle.TabIndex = 0;
            crlTitle.Text = "label1";
            // 
            // ExitButtonsContainer
            // 
            ExitButtonsContainer.BackColor = Color.Transparent;
            ExitButtonsContainer.Dock = DockStyle.Bottom;
            ExitButtonsContainer.Location = new Point(0, 600);
            ExitButtonsContainer.Name = "ExitButtonsContainer";
            ExitButtonsContainer.Size = new Size(1373, 273);
            ExitButtonsContainer.TabIndex = 2;
            // 
            // crlMessage
            // 
            crlMessage.BackColor = Color.Black;
            crlMessage.Location = new Point(392, 250);
            crlMessage.Margin = new Padding(4, 4, 4, 4);
            crlMessage.Name = "crlMessage";
            crlMessage.Size = new Size(188, 188);
            crlMessage.TabIndex = 8;
            crlMessage.TextOutputSpeedMs = 100;
            // 
            // DialogBasicInfoScreen
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(1375, 875);
            Controls.Add(panel1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "DialogBasicInfoScreen";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "DialogBasicScreen";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel ExitButtonsContainer;
        private Panel panel2;
        private Panel panel3;
        private Label crlTitle;
        private UI.Controls.BlurredPictureBox pictureBox1;
        private Controls.RpgTextOutputControl crlMessage;
    }
}