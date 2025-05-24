namespace DeepSpaceSaga.UI.CommonControls;

partial class ControlWindow
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
        crlTitleBar = new PictureBox();
        crlWindowTitle = new Label();
        crlCloseButton = new Label();
        ((ISupportInitialize)crlTitleBar).BeginInit();
        SuspendLayout();
        // 
        // crlTitleBar
        // 
        crlTitleBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        crlTitleBar.BackColor = Color.Black;
        crlTitleBar.Location = new Point(0, 0);
        crlTitleBar.Name = "crlTitleBar";
        crlTitleBar.Size = new Size(592, 38);
        crlTitleBar.TabIndex = 0;
        crlTitleBar.TabStop = false;
        // 
        // crlWindowTitle
        // 
        crlWindowTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        crlWindowTitle.BackColor = Color.Black;
        crlWindowTitle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        crlWindowTitle.ForeColor = Color.WhiteSmoke;
        crlWindowTitle.Location = new Point(13, 7);
        crlWindowTitle.Name = "crlWindowTitle";
        crlWindowTitle.Size = new Size(544, 25);
        crlWindowTitle.TabIndex = 1;
        crlWindowTitle.Text = "Window Title";
        // 
        // crlCloseButton
        // 
        crlCloseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        crlCloseButton.BackColor = Color.Black;
        crlCloseButton.Cursor = Cursors.Hand;
        crlCloseButton.Font = new Font("Cambria", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
        crlCloseButton.ForeColor = Color.WhiteSmoke;
        crlCloseButton.Location = new Point(563, 7);
        crlCloseButton.Name = "crlCloseButton";
        crlCloseButton.Size = new Size(19, 25);
        crlCloseButton.TabIndex = 2;
        crlCloseButton.Text = "x";
        crlCloseButton.Click += Event_CloseWindow;
        // 
        // ControlWindow
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(20, 20, 20);
        BorderStyle = BorderStyle.FixedSingle;
        Controls.Add(crlCloseButton);
        Controls.Add(crlWindowTitle);
        Controls.Add(crlTitleBar);
        Name = "ControlWindow";
        Size = new Size(592, 182);
        ((ISupportInitialize)crlTitleBar).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private PictureBox crlTitleBar;
    private Label crlWindowTitle;
    protected Label crlCloseButton;
}
