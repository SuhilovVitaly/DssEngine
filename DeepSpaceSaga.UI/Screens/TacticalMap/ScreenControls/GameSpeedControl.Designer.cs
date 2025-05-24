namespace DeepSpaceSaga.UI.Screens.MainGameScreen
{
    partial class GameSpeedControl
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
            cmdPause = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            crlEmpty = new Button();
            SuspendLayout();
            // 
            // cmdPause
            // 
            cmdPause.BackColor = Color.Olive;
            cmdPause.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmdPause.ForeColor = Color.DarkOrange;
            cmdPause.Location = new Point(16, 15);
            cmdPause.Name = "cmdPause";
            cmdPause.Size = new Size(48, 48);
            cmdPause.TabIndex = 0;
            cmdPause.Text = "||";
            cmdPause.UseVisualStyleBackColor = false;
            cmdPause.Click += cmdPause_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Olive;
            button1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Silver;
            button1.Location = new Point(70, 15);
            button1.Name = "button1";
            button1.Size = new Size(48, 48);
            button1.TabIndex = 1;
            button1.Text = "1";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Olive;
            button2.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.Silver;
            button2.Location = new Point(124, 15);
            button2.Name = "button2";
            button2.Size = new Size(48, 48);
            button2.TabIndex = 2;
            button2.Text = "2";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Olive;
            button3.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.Silver;
            button3.Location = new Point(178, 15);
            button3.Name = "button3";
            button3.Size = new Size(48, 48);
            button3.TabIndex = 3;
            button3.Text = "3";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.Olive;
            button4.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button4.ForeColor = Color.Silver;
            button4.Location = new Point(232, 15);
            button4.Name = "button4";
            button4.Size = new Size(48, 48);
            button4.TabIndex = 3;
            button4.Text = "4";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.BackColor = Color.Olive;
            button5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button5.ForeColor = Color.DarkGray;
            button5.Location = new Point(286, 15);
            button5.Name = "button5";
            button5.Size = new Size(48, 48);
            button5.TabIndex = 3;
            button5.Text = "5";
            button5.UseVisualStyleBackColor = false;
            button5.Click += button5_Click;
            // 
            // crlEmpty
            // 
            crlEmpty.BackColor = Color.Olive;
            crlEmpty.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlEmpty.ForeColor = Color.DarkGray;
            crlEmpty.Location = new Point(152, 125);
            crlEmpty.Name = "crlEmpty";
            crlEmpty.Size = new Size(48, 48);
            crlEmpty.TabIndex = 4;
            crlEmpty.Text = "5";
            crlEmpty.UseVisualStyleBackColor = false;
            // 
            // GameSpeedControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(12, 12, 12);
            Controls.Add(crlEmpty);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(cmdPause);
            DoubleBuffered = true;
            Name = "GameSpeedControl";
            Size = new Size(353, 298);
            ResumeLayout(false);
        }

        #endregion

        private Button cmdPause;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button crlEmpty;
    }
}
