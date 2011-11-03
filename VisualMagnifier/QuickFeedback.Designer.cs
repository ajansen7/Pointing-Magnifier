namespace PointingMagnifier
{
    partial class QuickFeedback
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
            this.btn_send = new System.Windows.Forms.Button();
            this.btn_ignore = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbFeedback = new System.Windows.Forms.RichTextBox();
            this.cbImage = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_send
            // 
            this.btn_send.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_send.Location = new System.Drawing.Point(554, 243);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(121, 23);
            this.btn_send.TabIndex = 0;
            this.btn_send.Text = "Send Feedback";
            this.btn_send.UseVisualStyleBackColor = true;
            // 
            // btn_ignore
            // 
            this.btn_ignore.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btn_ignore.Location = new System.Drawing.Point(330, 243);
            this.btn_ignore.Name = "btn_ignore";
            this.btn_ignore.Size = new System.Drawing.Size(121, 23);
            this.btn_ignore.TabIndex = 1;
            this.btn_ignore.Text = "Don\'t Send Feedback";
            this.btn_ignore.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(327, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please include feedback and/or comments about the Pointing Magnifier.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 35);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(309, 202);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // tbFeedback
            // 
            this.tbFeedback.Location = new System.Drawing.Point(330, 35);
            this.tbFeedback.Name = "tbFeedback";
            this.tbFeedback.Size = new System.Drawing.Size(345, 202);
            this.tbFeedback.TabIndex = 4;
            this.tbFeedback.Text = "";
            // 
            // cbImage
            // 
            this.cbImage.AutoSize = true;
            this.cbImage.Checked = true;
            this.cbImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbImage.Location = new System.Drawing.Point(12, 12);
            this.cbImage.Name = "cbImage";
            this.cbImage.Size = new System.Drawing.Size(292, 17);
            this.cbImage.TabIndex = 5;
            this.cbImage.Text = "I agree to send the screenshot below with the feedback.";
            this.cbImage.UseVisualStyleBackColor = true;
            // 
            // QuickFeedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 277);
            this.Controls.Add(this.cbImage);
            this.Controls.Add(this.tbFeedback);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_ignore);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.pictureBox1);
            this.Name = "QuickFeedback";
            this.Text = "QuickFeedback";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.Button btn_ignore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox tbFeedback;
        private System.Windows.Forms.CheckBox cbImage;
    }
}