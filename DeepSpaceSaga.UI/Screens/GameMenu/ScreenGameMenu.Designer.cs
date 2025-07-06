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
            crlMainMenu = new Button();
            crlResume = new Button();
            crlSave = new Button();
            crlLoad = new Button();
            crlSettings = new Button();
            SuspendLayout();
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
            crlMainMenu.Location = new Point(406, 565);
            crlMainMenu.Margin = new Padding(4);
            crlMainMenu.Name = "crlMainMenu";
            crlMainMenu.Size = new Size(188, 58);
            crlMainMenu.TabIndex = 0;
            crlMainMenu.Text = "MAIN MENU";
            crlMainMenu.UseVisualStyleBackColor = false;
            crlMainMenu.Click += OnMainMenuClick;
            // 
            // crlResume
            // 
            crlResume.BackColor = Color.FromArgb(18, 18, 18);
            crlResume.Cursor = Cursors.Hand;
            crlResume.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            crlResume.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            crlResume.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            crlResume.FlatStyle = FlatStyle.Flat;
            crlResume.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlResume.ForeColor = Color.Gainsboro;
            crlResume.Location = new Point(406, 175);
            crlResume.Margin = new Padding(4);
            crlResume.Name = "crlResume";
            crlResume.Size = new Size(188, 58);
            crlResume.TabIndex = 0;
            crlResume.Text = "RESUME";
            crlResume.UseVisualStyleBackColor = false;
            crlResume.Click += OnResumeClick;
            // 
            // crlSave
            // 
            crlSave.BackColor = Color.FromArgb(18, 18, 18);
            crlSave.Cursor = Cursors.Hand;
            crlSave.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            crlSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            crlSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            crlSave.FlatStyle = FlatStyle.Flat;
            crlSave.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlSave.ForeColor = Color.Gainsboro;
            crlSave.Location = new Point(406, 254);
            crlSave.Margin = new Padding(4);
            crlSave.Name = "crlSave";
            crlSave.Size = new Size(188, 58);
            crlSave.TabIndex = 0;
            crlSave.Text = "SAVE";
            crlSave.UseVisualStyleBackColor = false;
            crlSave.Click += OnSaveClick;
            // 
            // crlLoad
            // 
            crlLoad.BackColor = Color.FromArgb(18, 18, 18);
            crlLoad.Cursor = Cursors.Hand;
            crlLoad.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            crlLoad.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            crlLoad.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            crlLoad.FlatStyle = FlatStyle.Flat;
            crlLoad.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlLoad.ForeColor = Color.Gainsboro;
            crlLoad.Location = new Point(406, 334);
            crlLoad.Margin = new Padding(4);
            crlLoad.Name = "crlLoad";
            crlLoad.Size = new Size(188, 58);
            crlLoad.TabIndex = 0;
            crlLoad.Text = "LOAD";
            crlLoad.UseVisualStyleBackColor = false;
            crlLoad.Click += OnLoadClick;
            // 
            // crlSettings
            // 
            crlSettings.BackColor = Color.FromArgb(18, 18, 18);
            crlSettings.Cursor = Cursors.Hand;
            crlSettings.Enabled = false;
            crlSettings.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 42);
            crlSettings.FlatAppearance.MouseDownBackColor = Color.FromArgb(78, 78, 78);
            crlSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(58, 58, 58);
            crlSettings.FlatStyle = FlatStyle.Flat;
            crlSettings.Font = new Font("Verdana", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            crlSettings.ForeColor = Color.Gainsboro;
            crlSettings.Location = new Point(406, 410);
            crlSettings.Margin = new Padding(4);
            crlSettings.Name = "crlSettings";
            crlSettings.Size = new Size(188, 58);
            crlSettings.TabIndex = 0;
            crlSettings.Text = "SETTINGS";
            crlSettings.UseVisualStyleBackColor = false;
            // 
            // ScreenGameMenu
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1000, 774);
            Controls.Add(crlSettings);
            Controls.Add(crlLoad);
            Controls.Add(crlSave);
            Controls.Add(crlResume);
            Controls.Add(crlMainMenu);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4);
            Name = "ScreenGameMenu";
            Text = "MainMenuScreen";
            ResumeLayout(false);
        }

        #endregion

        private Button crlMainMenu;
        private Button crlResume;
        private Button crlSave;
        private Button crlLoad;
        private Button crlSettings;
    }
}