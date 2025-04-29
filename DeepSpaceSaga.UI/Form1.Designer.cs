namespace DeepSpaceSaga.UI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            crlStartProcessing = new Button();
            crlStopProcessing = new Button();
            crlSessionInfo = new Label();
            SuspendLayout();
            // 
            // crlStartProcessing
            // 
            crlStartProcessing.Location = new Point(171, 168);
            crlStartProcessing.Name = "crlStartProcessing";
            crlStartProcessing.Size = new Size(94, 29);
            crlStartProcessing.TabIndex = 0;
            crlStartProcessing.Text = "Start";
            crlStartProcessing.UseVisualStyleBackColor = true;
            crlStartProcessing.Click += crlStartProcessing_Click;
            // 
            // crlStopProcessing
            // 
            crlStopProcessing.Location = new Point(339, 168);
            crlStopProcessing.Name = "crlStopProcessing";
            crlStopProcessing.Size = new Size(94, 29);
            crlStopProcessing.TabIndex = 0;
            crlStopProcessing.Text = "Stop";
            crlStopProcessing.UseVisualStyleBackColor = true;
            crlStopProcessing.Click += crlStopProcessing_Click;
            // 
            // crlSessionInfo
            // 
            crlSessionInfo.AutoSize = true;
            crlSessionInfo.Location = new Point(277, 96);
            crlSessionInfo.Name = "crlSessionInfo";
            crlSessionInfo.Size = new Size(50, 20);
            crlSessionInfo.TabIndex = 1;
            crlSessionInfo.Text = "label1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(crlSessionInfo);
            Controls.Add(crlStopProcessing);
            Controls.Add(crlStartProcessing);
            DoubleBuffered = true;
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button crlStartProcessing;
        private Button crlStopProcessing;
        private Label crlSessionInfo;
    }
}
