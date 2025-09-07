namespace DeepSpaceSaga.UI.Screens.Dialogs
{
    partial class Form1
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
            crlExit = new Button();
            SuspendLayout();
            // 
            // crlExit
            // 
            crlExit.BackColor = Color.FromArgb(18, 18, 18);
            crlExit.Cursor = Cursors.Hand;
            crlExit.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            crlExit.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            crlExit.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            crlExit.FlatStyle = FlatStyle.Flat;
            crlExit.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlExit.ForeColor = Color.Gainsboro;
            crlExit.Location = new Point(273, 334);
            crlExit.Margin = new Padding(4);
            crlExit.Name = "crlExit";
            crlExit.Size = new Size(188, 58);
            crlExit.TabIndex = 1;
            crlExit.Text = "EXIT";
            crlExit.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(crlExit);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;
            Name = "Form1";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button crlExit;
    }
}