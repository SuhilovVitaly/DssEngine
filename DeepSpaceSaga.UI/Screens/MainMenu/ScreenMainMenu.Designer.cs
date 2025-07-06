namespace DeepSpaceSaga.UI.Screens.MainMenu
{
    partial class ScreenMainMenu
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
            crlNewGame = new Button();
            crlLoadGame = new Button();
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
            crlExit.Location = new Point(406, 565);
            crlExit.Margin = new Padding(4);
            crlExit.Name = "crlExit";
            crlExit.Size = new Size(188, 58);
            crlExit.TabIndex = 0;
            crlExit.Text = "EXIT";
            crlExit.UseVisualStyleBackColor = false;
            crlExit.Click += Event_ApplicationExit;
            // 
            // crlNewGame
            // 
            crlNewGame.BackColor = Color.FromArgb(18, 18, 18);
            crlNewGame.Cursor = Cursors.Hand;
            crlNewGame.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            crlNewGame.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            crlNewGame.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            crlNewGame.FlatStyle = FlatStyle.Flat;
            crlNewGame.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlNewGame.ForeColor = Color.Gainsboro;
            crlNewGame.Location = new Point(406, 175);
            crlNewGame.Margin = new Padding(4);
            crlNewGame.Name = "crlNewGame";
            crlNewGame.Size = new Size(188, 58);
            crlNewGame.TabIndex = 0;
            crlNewGame.Text = "NEW GAME";
            crlNewGame.UseVisualStyleBackColor = false;
            crlNewGame.Click += Event_StartNewGameSession;
            // 
            // crlLoadGame
            // 
            crlLoadGame.BackColor = Color.FromArgb(18, 18, 18);
            crlLoadGame.Cursor = Cursors.Hand;
            crlLoadGame.Enabled = false;
            crlLoadGame.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            crlLoadGame.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            crlLoadGame.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            crlLoadGame.FlatStyle = FlatStyle.Flat;
            crlLoadGame.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlLoadGame.ForeColor = Color.Gainsboro;
            crlLoadGame.Location = new Point(406, 254);
            crlLoadGame.Margin = new Padding(4);
            crlLoadGame.Name = "crlLoadGame";
            crlLoadGame.Size = new Size(188, 58);
            crlLoadGame.TabIndex = 0;
            crlLoadGame.Text = "LOAD";
            crlLoadGame.UseVisualStyleBackColor = false;
            crlLoadGame.Click += Event_LoadGame;
            // 
            // ScreenMainMenu
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1000, 774);
            Controls.Add(crlLoadGame);
            Controls.Add(crlNewGame);
            Controls.Add(crlExit);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "ScreenMainMenu";
            Text = "MainMenuScreen";
            ResumeLayout(false);
        }

        #endregion

        private Button crlExit;
        private Button crlNewGame;
        private Button crlLoadGame;
    }
}