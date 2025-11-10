namespace Testing_Project
{
    partial class SemesterForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tbSchlYear = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.semlbl = new System.Windows.Forms.Label();
            this.semcmbo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(352, 115);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(183, 31);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(352, 77);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(183, 31);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tbSchlYear
            // 
            this.tbSchlYear.Location = new System.Drawing.Point(99, 77);
            this.tbSchlYear.Name = "tbSchlYear";
            this.tbSchlYear.Size = new System.Drawing.Size(194, 22);
            this.tbSchlYear.TabIndex = 16;
            this.tbSchlYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSchlYear_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "School Year:";
            // 
            // semlbl
            // 
            this.semlbl.AutoSize = true;
            this.semlbl.Location = new System.Drawing.Point(19, 119);
            this.semlbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.semlbl.Name = "semlbl";
            this.semlbl.Size = new System.Drawing.Size(68, 16);
            this.semlbl.TabIndex = 18;
            this.semlbl.Text = "Semester:";
            // 
            // semcmbo
            // 
            this.semcmbo.AutoCompleteCustomSource.AddRange(new string[] {
            "Fall",
            "Spring"});
            this.semcmbo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.semcmbo.FormattingEnabled = true;
            this.semcmbo.Items.AddRange(new object[] {
            "Fall",
            "Spring"});
            this.semcmbo.Location = new System.Drawing.Point(99, 115);
            this.semcmbo.Margin = new System.Windows.Forms.Padding(4);
            this.semcmbo.Name = "semcmbo";
            this.semcmbo.Size = new System.Drawing.Size(194, 24);
            this.semcmbo.TabIndex = 17;
            // 
            // SemesterForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(561, 245);
            this.ControlBox = false;
            this.Controls.Add(this.semlbl);
            this.Controls.Add(this.semcmbo);
            this.Controls.Add(this.tbSchlYear);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "SemesterForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Semester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox tbSchlYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label semlbl;
        private System.Windows.Forms.ComboBox semcmbo;
    }
}