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
            SuspendLayout();
            // 
            // ControlGameSpeed
            // 
            ControlGameSpeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ControlGameSpeed.BackColor = Color.FromArgb(12, 12, 12);
            ControlGameSpeed.Location = new Point(1777, 925);
            ControlGameSpeed.Margin = new Padding(4, 4, 4, 4);
            ControlGameSpeed.Name = "ControlGameSpeed";
            ControlGameSpeed.Size = new Size(441, 97);
            ControlGameSpeed.TabIndex = 0;
            // 
            // ScreenTacticalMap
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(2231, 1035);
            Controls.Add(ControlGameSpeed);
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
    }
}