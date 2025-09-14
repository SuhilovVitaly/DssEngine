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
            crlTitle = new Label();
            crlMessageStatic = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // crlTitle
            // 
            crlTitle.BackColor = Color.Transparent;
            crlTitle.Dock = DockStyle.Top;
            crlTitle.Font = new Font("Courier New", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlTitle.ForeColor = Color.OrangeRed;
            crlTitle.Location = new Point(0, 0);
            crlTitle.Name = "crlTitle";
            crlTitle.Size = new Size(1406, 40);
            crlTitle.TabIndex = 0;
            crlTitle.Text = "label1";
            // 
            // crlMessageStatic
            // 
            crlMessageStatic.BackColor = Color.Transparent;
            crlMessageStatic.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlMessageStatic.ForeColor = Color.Gainsboro;
            crlMessageStatic.Location = new Point(12, 12);
            crlMessageStatic.Name = "crlMessageStatic";
            crlMessageStatic.Size = new Size(695, 590);
            crlMessageStatic.TabIndex = 10;
            crlMessageStatic.Text = "label1";
            // 
            // panel1
            // 
            panel1.Controls.Add(crlMessageStatic);
            panel1.Location = new Point(15, 53);
            panel1.Name = "panel1";
            panel1.Size = new Size(1375, 666);
            panel1.TabIndex = 11;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(crlTitle);
            panel2.Controls.Add(panel1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1408, 919);
            panel2.TabIndex = 12;
            // 
            // DialogBasicInfoScreen
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1408, 919);
            Controls.Add(panel2);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "DialogBasicInfoScreen";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "DialogBasicScreen";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Label crlTitle;
        private Label crlMessageStatic;
        private Panel panel1;
        private Panel panel2;
    }
}