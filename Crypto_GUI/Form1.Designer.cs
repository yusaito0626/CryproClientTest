namespace Crypto_GUI
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
            button1 = new Button();
            labeltest1 = new Label();
            labeltest2 = new Label();
            labeltest3 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(685, 840);
            button1.Name = "button1";
            button1.Size = new Size(150, 46);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // labeltest1
            // 
            labeltest1.Location = new Point(89, 102);
            labeltest1.Name = "labeltest1";
            labeltest1.Size = new Size(597, 596);
            labeltest1.TabIndex = 1;
            labeltest1.Text = "label1";
            // 
            // labeltest2
            // 
            labeltest2.Location = new Point(766, 102);
            labeltest2.Name = "labeltest2";
            labeltest2.Size = new Size(735, 622);
            labeltest2.TabIndex = 2;
            labeltest2.Text = "label1";
            // 
            // labeltest3
            // 
            labeltest3.Location = new Point(766, 676);
            labeltest3.Name = "labeltest3";
            labeltest3.Size = new Size(662, 102);
            labeltest3.TabIndex = 3;
            labeltest3.Text = "label1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1571, 962);
            Controls.Add(labeltest3);
            Controls.Add(labeltest2);
            Controls.Add(labeltest1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Label labeltest1;
        private Label labeltest2;
        private Label labeltest3;
    }
}
