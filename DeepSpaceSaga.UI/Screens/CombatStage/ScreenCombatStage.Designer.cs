namespace DeepSpaceSaga.UI.Screens.CombatStage
{
    partial class ScreenCombatStage
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
            crlName = new Label();
            crlRank = new Label();
            crlPortrait = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            crlMainMenu = new Button();
            ((ISupportInitialize)crlPortrait).BeginInit();
            ((ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // crlName
            // 
            crlName.BackColor = Color.FromArgb(12, 12, 12, 50);
            crlName.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            crlName.ForeColor = Color.WhiteSmoke;
            crlName.Location = new Point(13, 253);
            crlName.Margin = new Padding(4, 0, 4, 0);
            crlName.Name = "crlName";
            crlName.Size = new Size(205, 31);
            crlName.TabIndex = 6;
            crlName.Text = "label1";
            crlName.TextAlign = ContentAlignment.TopCenter;
            // 
            // crlRank
            // 
            crlRank.BackColor = Color.FromArgb(12, 12, 12, 50);
            crlRank.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            crlRank.ForeColor = Color.OrangeRed;
            crlRank.Location = new Point(13, 221);
            crlRank.Margin = new Padding(4, 0, 4, 0);
            crlRank.Name = "crlRank";
            crlRank.Size = new Size(205, 31);
            crlRank.TabIndex = 5;
            crlRank.Text = "label1";
            crlRank.TextAlign = ContentAlignment.TopCenter;
            // 
            // crlPortrait
            // 
            crlPortrait.Location = new Point(13, 13);
            crlPortrait.Margin = new Padding(4);
            crlPortrait.Name = "crlPortrait";
            crlPortrait.Size = new Size(205, 205);
            crlPortrait.SizeMode = PictureBoxSizeMode.StretchImage;
            crlPortrait.TabIndex = 4;
            crlPortrait.TabStop = false;
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(12, 12, 12, 50);
            label1.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            label1.ForeColor = Color.WhiteSmoke;
            label1.Location = new Point(935, 253);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(205, 31);
            label1.TabIndex = 9;
            label1.Text = "label1";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // label2
            // 
            label2.BackColor = Color.FromArgb(12, 12, 12, 50);
            label2.Font = new Font("Tahoma", 10.2F, FontStyle.Bold);
            label2.ForeColor = Color.OrangeRed;
            label2.Location = new Point(935, 221);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(205, 31);
            label2.TabIndex = 8;
            label2.Text = "label1";
            label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(935, 13);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(205, 205);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
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
            crlMainMenu.Location = new Point(449, 747);
            crlMainMenu.Margin = new Padding(4);
            crlMainMenu.Name = "crlMainMenu";
            crlMainMenu.Size = new Size(188, 58);
            crlMainMenu.TabIndex = 10;
            crlMainMenu.Text = "MAIN MENU";
            crlMainMenu.UseVisualStyleBackColor = false;
            crlMainMenu.Click += crlMainMenu_Click;
            // 
            // ScreenCombatStage
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1153, 827);
            Controls.Add(crlMainMenu);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(crlName);
            Controls.Add(crlRank);
            Controls.Add(crlPortrait);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ScreenCombatStage";
            ShowInTaskbar = false;
            Text = "ScreenCombatStage";
            ((ISupportInitialize)crlPortrait).EndInit();
            ((ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label crlName;
        private Label crlRank;
        private PictureBox crlPortrait;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
        private Button crlMainMenu;
    }
}