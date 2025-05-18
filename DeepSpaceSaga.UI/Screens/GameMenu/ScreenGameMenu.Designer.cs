namespace DeepSpaceSaga.UI.Screens.GameMenu
{
    partial class ScreenGameMenu
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
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(18, 18, 18);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.Gainsboro;
            button1.Location = new Point(325, 452);
            button1.Name = "button1";
            button1.Size = new Size(150, 46);
            button1.TabIndex = 0;
            button1.Text = "EXIT";
            button1.UseVisualStyleBackColor = false;
            button1.Click += Event_ApplicationExit;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(18, 18, 18);
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            button2.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            button2.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.Gainsboro;
            button2.Location = new Point(325, 140);
            button2.Name = "button2";
            button2.Size = new Size(150, 46);
            button2.TabIndex = 0;
            button2.Text = "NEW GAME";
            button2.UseVisualStyleBackColor = false;
            button2.Click += Event_StartNewGameSession;
            // 
            // button3
            // 
            button3.BackColor = Color.FromArgb(18, 18, 18);
            button3.Cursor = Cursors.Hand;
            button3.Enabled = false;
            button3.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            button3.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            button3.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.ForeColor = Color.Gainsboro;
            button3.Location = new Point(325, 203);
            button3.Name = "button3";
            button3.Size = new Size(150, 46);
            button3.TabIndex = 0;
            button3.Text = "LOAD";
            button3.UseVisualStyleBackColor = false;
            button3.Click += Event_LoadGame;
            // 
            // MainMenuScreen
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(800, 619);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "MainMenuScreen";
            Text = "MainMenuScreen";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
    }
}