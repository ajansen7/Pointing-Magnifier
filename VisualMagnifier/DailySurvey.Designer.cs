namespace PointingMagnifier
{
    partial class DailySurvey
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
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.Submit = new System.Windows.Forms.Button();
            this.mental = new PointingMagnifier.likert();
            this.physical = new PointingMagnifier.likert();
            this.temporal = new PointingMagnifier.likert();
            this.performance = new PointingMagnifier.likert();
            this.effort = new PointingMagnifier.likert();
            this.frustration = new PointingMagnifier.likert();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(336, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mental Demand: How Mentally demanding was it to use the software?";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(349, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Physical Demand: How physically demanding was it to use the software?";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 207);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(390, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Performance: How successful were you in accomplishing what you wished to do?";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 140);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(333, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Temporal Demand: How hurried or rushed was it to use the software?";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(12, 343);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(347, 26);
            this.label15.TabIndex = 21;
            this.label15.Text = "Frustration: How insecure, discouraged, irritated, stressed, and annoyed \r\nwere y" +
                "ou when using the software?";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 275);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(382, 13);
            this.label18.TabIndex = 17;
            this.label18.Text = "Effort: How hard did you have to work to accomplish your level of performance?";
            // 
            // Submit
            // 
            this.Submit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Submit.Location = new System.Drawing.Point(340, 417);
            this.Submit.Name = "Submit";
            this.Submit.Size = new System.Drawing.Size(75, 23);
            this.Submit.TabIndex = 24;
            this.Submit.Text = "Submit";
            this.Submit.UseVisualStyleBackColor = true;
            // 
            // mental
            // 
            this.mental.Location = new System.Drawing.Point(12, 25);
            this.mental.Name = "mental";
            this.mental.Size = new System.Drawing.Size(403, 38);
            this.mental.TabIndex = 26;
            // 
            // physical
            // 
            this.physical.Location = new System.Drawing.Point(12, 92);
            this.physical.Name = "physical";
            this.physical.Size = new System.Drawing.Size(403, 38);
            this.physical.TabIndex = 27;
            // 
            // temporal
            // 
            this.temporal.Location = new System.Drawing.Point(12, 157);
            this.temporal.Name = "temporal";
            this.temporal.Size = new System.Drawing.Size(403, 38);
            this.temporal.TabIndex = 28;
            // 
            // performance
            // 
            this.performance.Location = new System.Drawing.Point(12, 224);
            this.performance.Name = "performance";
            this.performance.Size = new System.Drawing.Size(403, 38);
            this.performance.TabIndex = 29;
            // 
            // effort
            // 
            this.effort.Location = new System.Drawing.Point(12, 292);
            this.effort.Name = "effort";
            this.effort.Size = new System.Drawing.Size(403, 38);
            this.effort.TabIndex = 30;
            // 
            // frustration
            // 
            this.frustration.Location = new System.Drawing.Point(12, 372);
            this.frustration.Name = "frustration";
            this.frustration.Size = new System.Drawing.Size(403, 38);
            this.frustration.TabIndex = 31;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 416);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // DailySurvey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 452);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.frustration);
            this.Controls.Add(this.effort);
            this.Controls.Add(this.performance);
            this.Controls.Add(this.temporal);
            this.Controls.Add(this.physical);
            this.Controls.Add(this.mental);
            this.Controls.Add(this.Submit);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Name = "DailySurvey";
            this.Text = "DailySurvey";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button Submit;
        private likert mental;
        private likert physical;
        private likert temporal;
        private likert performance;
        private likert effort;
        private likert frustration;
        private System.Windows.Forms.Button btnCancel;
    }
}