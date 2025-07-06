namespace DeepSpaceSaga.UI.Screens.TacticalMap
{
    partial class ScreenTacticalMap
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
            ControlGameSpeed = new MainGameScreen.GameSpeedControl();
            sessionInformationControl = new GameSessionInformation();
            GameTacticalMapControl = new GameTacticalMap();
            crlRightToolbar = new RightUiToolbar();
            crlRightPanel = new RightUiPanel();
            SuspendLayout();
            // 
            // ControlGameSpeed
            // 
            ControlGameSpeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ControlGameSpeed.BackColor = Color.FromArgb(12, 12, 12);
            ControlGameSpeed.Location = new Point(14, 387);
            ControlGameSpeed.Margin = new Padding(4, 4, 4, 4);
            ControlGameSpeed.Name = "ControlGameSpeed";
            ControlGameSpeed.Size = new Size(441, 97);
            ControlGameSpeed.TabIndex = 0;
            // 
            // sessionInformationControl
            // 
            sessionInformationControl.BackColor = Color.FromArgb(12, 12, 12);
            sessionInformationControl.BorderStyle = BorderStyle.FixedSingle;
            sessionInformationControl.GameManager = null;
            sessionInformationControl.IsDraggible = true;
            sessionInformationControl.IsResizible = true;
            sessionInformationControl.Location = new Point(14, 14);
            sessionInformationControl.Margin = new Padding(5);
            sessionInformationControl.Name = "sessionInformationControl";
            sessionInformationControl.Size = new Size(351, 336);
            sessionInformationControl.TabIndex = 1;
            sessionInformationControl.Title = "Game Session Information";
            // 
            // GameTacticalMapControl
            // 
            GameTacticalMapControl.BackColor = Color.FromArgb(8, 8, 8);
            GameTacticalMapControl.Location = new Point(516, 92);
            GameTacticalMapControl.Name = "GameTacticalMapControl";
            GameTacticalMapControl.Size = new Size(1018, 833);
            GameTacticalMapControl.TabIndex = 2;
            // 
            // crlRightToolbar
            // 
            crlRightToolbar.BackColor = Color.Black;
            crlRightToolbar.BorderStyle = BorderStyle.FixedSingle;
            crlRightToolbar.Dock = DockStyle.Right;
            crlRightToolbar.Location = new Point(2151, 0);
            crlRightToolbar.Name = "crlRightToolbar";
            crlRightToolbar.Size = new Size(80, 1035);
            crlRightToolbar.TabIndex = 3;
            // 
            // crlRightPanel
            // 
            crlRightPanel.BackColor = Color.Black;
            crlRightPanel.BorderStyle = BorderStyle.FixedSingle;
            crlRightPanel.Dock = DockStyle.Right;
            crlRightPanel.Location = new Point(1371, 0);
            crlRightPanel.Name = "crlRightPanel";
            crlRightPanel.Size = new Size(780, 1035);
            crlRightPanel.TabIndex = 4;
            // 
            // ScreenTacticalMap
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(2231, 1035);
            Controls.Add(crlRightPanel);
            Controls.Add(crlRightToolbar);
            Controls.Add(sessionInformationControl);
            Controls.Add(ControlGameSpeed);
            Controls.Add(GameTacticalMapControl);
            DoubleBuffered = true;
            ForeColor = Color.WhiteSmoke;
            FormBorderStyle = FormBorderStyle.None;
            Name = "ScreenTacticalMap";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ScreenTacticalMap";
            ResumeLayout(false);
        }

        #endregion

        private MainGameScreen.GameSpeedControl ControlGameSpeed;
        private ScreenControls.GameSessionInformation sessionInformationControl;
        private GameTacticalMap GameTacticalMapControl;
        private RightUiToolbar rightUiToolbar1;
        private RightUiToolbar crlRightToolbar;
        private RightUiPanel crlRightPanel;
    }
}