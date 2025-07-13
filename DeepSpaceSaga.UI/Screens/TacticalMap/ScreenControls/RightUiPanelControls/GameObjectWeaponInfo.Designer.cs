namespace DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls.RightUiPanelControls
{
    partial class GameObjectWeaponInfo
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
            crlGameObjectWeaponInfo = new Label();
            SuspendLayout();
            // 
            // crlGameObjectWeaponInfo
            // 
            crlGameObjectWeaponInfo.AutoSize = true;
            crlGameObjectWeaponInfo.ForeColor = Color.WhiteSmoke;
            crlGameObjectWeaponInfo.Location = new Point(0, 0);
            crlGameObjectWeaponInfo.Name = "crlGameObjectWeaponInfo";
            crlGameObjectWeaponInfo.Size = new Size(208, 25);
            crlGameObjectWeaponInfo.TabIndex = 1;
            crlGameObjectWeaponInfo.Text = "GameObjectWeaponInfo";
            // 
            // GameObjectWeaponInfo
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(crlGameObjectWeaponInfo);
            Name = "GameObjectWeaponInfo";
            Size = new Size(780, 434);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label crlGameObjectWeaponInfo;
    }
}
