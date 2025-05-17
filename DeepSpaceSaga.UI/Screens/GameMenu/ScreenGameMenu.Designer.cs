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
            SuspendLayout();
            // 
            // crlExitGame
            // 
            crlExitGame.Location = new Point(204, 370);
            crlExitGame.Name = "crlExitGame";
            crlExitGame.Size = new Size(394, 29);
            crlExitGame.TabIndex = 1;
            crlExitGame.Text = "Exit Game";
            crlExitGame.UseVisualStyleBackColor = true;
            crlExitGame.Click += crlExitGame_Click;
            // 
            // ScreenGameMenu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(800, 450);
            Controls.Add(crlExitGame);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "ScreenGameMenu";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ScreenGameMenu";
            ResumeLayout(false);
        }

        #endregion

        private Button crlExitGame;
    }
}