namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls.RightUiPanelControls
{
    partial class GameObjectGeneralInfo
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
            crlGameObjectGeneralInfo = new Label();
            SuspendLayout();
            // 
            // crlGameObjectGeneralInfo
            // 
            crlGameObjectGeneralInfo.AutoSize = true;
            crlGameObjectGeneralInfo.ForeColor = Color.WhiteSmoke;
            crlGameObjectGeneralInfo.Location = new Point(0, 0);
            crlGameObjectGeneralInfo.Name = "crlGameObjectGeneralInfo";
            crlGameObjectGeneralInfo.Size = new Size(201, 25);
            crlGameObjectGeneralInfo.TabIndex = 0;
            crlGameObjectGeneralInfo.Text = "GameObjectGeneralInfo";
            // 
            // GameObjectGeneralInfo
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(crlGameObjectGeneralInfo);
            Name = "GameObjectGeneralInfo";
            Size = new Size(780, 382);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label crlGameObjectGeneralInfo;
    }
}
