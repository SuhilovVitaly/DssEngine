namespace DeepSpaceSaga.UI.Screens.Dialogs.Controls
{
    partial class RpgTextOutputControl
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
            lblText = new Label();
            SuspendLayout();
            // 
            // lblText
            // 
            lblText.BackColor = Color.Transparent;
            lblText.Dock = DockStyle.Fill;
            lblText.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblText.ForeColor = Color.WhiteSmoke;
            lblText.Location = new Point(0, 0);
            lblText.Margin = new Padding(4, 0, 4, 0);
            lblText.Name = "lblText";
            lblText.Size = new Size(125, 125);
            lblText.TabIndex = 0;
            // 
            // RpgTextOutputControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(lblText);
            Margin = new Padding(4, 4, 4, 4);
            Name = "RpgTextOutputControl";
            Size = new Size(125, 125);
            ResumeLayout(false);
        }

        #endregion
        private Label lblText;
    }
}
