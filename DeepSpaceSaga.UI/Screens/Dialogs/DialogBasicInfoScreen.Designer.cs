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
            panel4 = new Panel();
            crlMessage = new Controls.RpgTextOutputControl();
            panel3 = new Panel();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
            crlTitle = new Label();
            ExitButtonsContainer = new Panel();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            ((ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(ExitButtonsContainer);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1375, 875);
            panel1.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.BackColor = Color.Transparent;
            panel4.Controls.Add(crlMessage);
            panel4.Dock = DockStyle.Left;
            panel4.Location = new Point(32, 79);
            panel4.Name = "panel4";
            panel4.Size = new Size(824, 521);
            panel4.TabIndex = 8;
            // 
            // crlMessage
            // 
            crlMessage.BackColor = Color.Black;
            crlMessage.Dock = DockStyle.Fill;
            crlMessage.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            crlMessage.ForeColor = Color.WhiteSmoke;
            crlMessage.Location = new Point(0, 0);
            crlMessage.Margin = new Padding(4, 0, 4, 0);
            crlMessage.Name = "crlMessage";
            crlMessage.Size = new Size(824, 521);
            crlMessage.TabIndex = 5;
            crlMessage.TextOutputSpeedMs = 50;
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
            // pictureBox1
            // 
            pictureBox1.Location = new Point(862, 94);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(500, 500);
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Controls.Add(crlTitle);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1373, 79);
            panel2.TabIndex = 6;
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
            panel4.ResumeLayout(false);
            ((ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel ExitButtonsContainer;
        private Controls.RpgTextOutputControl crlMessage;
        private PictureBox pictureBox1;
        private Panel panel2;
        private Panel panel4;
        private Panel panel3;
        private Label crlTitle;
    }
}