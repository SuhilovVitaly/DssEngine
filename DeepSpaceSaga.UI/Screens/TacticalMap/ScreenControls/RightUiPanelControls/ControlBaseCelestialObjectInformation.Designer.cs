namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls.RightUiPanelControls
{
    partial class ControlBaseCelestialObjectInformation
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
            panel1 = new Panel();
            crlCelestialObjectId = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(crlCelestialObjectId);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(780, 150);
            panel1.TabIndex = 0;
            // 
            // crlCelestialObjectId
            // 
            crlCelestialObjectId.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            crlCelestialObjectId.ForeColor = Color.WhiteSmoke;
            crlCelestialObjectId.Location = new Point(285, 62);
            crlCelestialObjectId.Name = "crlCelestialObjectId";
            crlCelestialObjectId.Size = new Size(208, 25);
            crlCelestialObjectId.TabIndex = 2;
            crlCelestialObjectId.Text = "ControlBaseCelestialObjectInformation";
            crlCelestialObjectId.TextAlign = ContentAlignment.TopCenter;
            // 
            // ControlBaseCelestialObjectInformation
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(panel1);
            Name = "ControlBaseCelestialObjectInformation";
            Size = new Size(780, 150);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label crlCelestialObjectId;
    }
}
