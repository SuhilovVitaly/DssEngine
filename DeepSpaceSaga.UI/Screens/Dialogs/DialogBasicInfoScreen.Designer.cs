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
            ExitButtonsContainer = new Panel();
            crlMainMenu = new Button();
            crlMessage = new Controls.RpgTextOutputControl();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Black;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(crlMessage);
            panel1.Controls.Add(ExitButtonsContainer);
            panel1.Controls.Add(crlMainMenu);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1375, 875);
            panel1.TabIndex = 0;
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
            // crlMainMenu
            // 
            crlMainMenu.BackColor = Color.FromArgb(18, 18, 18);
            crlMainMenu.Cursor = Cursors.Hand;
            crlMainMenu.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            crlMainMenu.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            crlMainMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            crlMainMenu.FlatStyle = FlatStyle.Flat;
            crlMainMenu.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlMainMenu.ForeColor = Color.Gainsboro;
            crlMainMenu.Location = new Point(1173, 12);
            crlMainMenu.Margin = new Padding(4);
            crlMainMenu.Name = "crlMainMenu";
            crlMainMenu.Size = new Size(188, 58);
            crlMainMenu.TabIndex = 1;
            crlMainMenu.Text = "CLOSE";
            crlMainMenu.UseVisualStyleBackColor = false;
            crlMainMenu.Click += crlMainMenu_Click;
            // 
            // crlMessage
            // 
            crlMessage.BackColor = Color.Black;
            crlMessage.Dock = DockStyle.Left;
            crlMessage.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            crlMessage.ForeColor = Color.WhiteSmoke;
            crlMessage.Location = new Point(0, 0);
            crlMessage.Margin = new Padding(4, 0, 4, 0);
            crlMessage.Name = "crlMessage";
            crlMessage.Size = new Size(793, 600);
            crlMessage.TabIndex = 5;
            crlMessage.TextOutputSpeedMs = 50;
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
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button crlMainMenu;
        private Panel ExitButtonsContainer;
        private Controls.RpgTextOutputControl crlMessage;
    }
}