namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls
{
    partial class GameTacticalMap
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
            InformationAboutContol = new Label();
            SuspendLayout();
            // 
            // InformationAboutContol
            // 
            InformationAboutContol.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            InformationAboutContol.AutoSize = true;
            InformationAboutContol.ForeColor = Color.LightGray;
            InformationAboutContol.Location = new Point(630, 0);
            InformationAboutContol.Name = "InformationAboutContol";
            InformationAboutContol.Size = new Size(117, 25);
            InformationAboutContol.TabIndex = 0;
            InformationAboutContol.Text = "Tactical Map: ";
            InformationAboutContol.TextAlign = ContentAlignment.TopRight;
            // 
            // GameTacticalMap
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(22, 22, 22);
            Controls.Add(InformationAboutContol);
            DoubleBuffered = true;
            Name = "GameTacticalMap";
            Size = new Size(750, 657);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label InformationAboutContol;
    }
}
