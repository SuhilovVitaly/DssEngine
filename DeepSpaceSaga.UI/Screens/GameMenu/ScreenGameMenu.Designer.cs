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
            crlExitGame = new Button();
            crlNewGame = new Button();
            SuspendLayout();
            // 
            // crlExitGame
            // 
            crlExitGame.Location = new Point(255, 462);
            crlExitGame.Margin = new Padding(4, 4, 4, 4);
            crlExitGame.Name = "crlExitGame";
            crlExitGame.Size = new Size(492, 36);
            crlExitGame.TabIndex = 1;
            crlExitGame.Text = "Exit Game";
            crlExitGame.UseVisualStyleBackColor = true;
            crlExitGame.Click += crlExitGame_Click;
            // 
            // crlNewGame
            // 
            crlNewGame.Location = new Point(255, 86);
            crlNewGame.Margin = new Padding(4);
            crlNewGame.Name = "crlNewGame";
            crlNewGame.Size = new Size(492, 36);
            crlNewGame.TabIndex = 2;
            crlNewGame.Text = "New Game";
            crlNewGame.UseVisualStyleBackColor = true;
            crlNewGame.Click += Event_NewGameStart;
            // 
            // ScreenGameMenu
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(1000, 562);
            Controls.Add(crlNewGame);
            Controls.Add(crlExitGame);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 4, 4, 4);
            Name = "ScreenGameMenu";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ScreenGameMenu";
            ResumeLayout(false);
        }

        #endregion

        private Button crlExitGame;
        private Button crlNewGame;
    }
}