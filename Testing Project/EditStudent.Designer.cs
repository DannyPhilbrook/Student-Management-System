namespace Testing_Project
{
    partial class EditStudent
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
            this.commlbl = new System.Windows.Forms.Label();
            this.commrtb = new System.Windows.Forms.RichTextBox();
            this.stdIdtb = new System.Windows.Forms.TextBox();
            this.stdIdlbl = new System.Windows.Forms.Label();
            this.stdLNametb = new System.Windows.Forms.TextBox();
            this.stdnLNamelbl = new System.Windows.Forms.Label();
            this.semlbl = new System.Windows.Forms.Label();
            this.stdFNametb = new System.Windows.Forms.TextBox();
            this.semcmbo = new System.Windows.Forms.ComboBox();
            this.stdFNamelbl = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // commlbl
            // 
            this.commlbl.AutoSize = true;
            this.commlbl.Location = new System.Drawing.Point(14, 255);
            this.commlbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.commlbl.Name = "commlbl";
            this.commlbl.Size = new System.Drawing.Size(90, 20);
            this.commlbl.TabIndex = 24;
            this.commlbl.Text = "Comments:";
            // 
            // commrtb
            // 
            this.commrtb.Location = new System.Drawing.Point(24, 285);
            this.commrtb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.commrtb.Name = "commrtb";
            this.commrtb.Size = new System.Drawing.Size(742, 135);
            this.commrtb.TabIndex = 23;
            this.commrtb.Text = "";
            // 
            // stdIdtb
            // 
            this.stdIdtb.Location = new System.Drawing.Point(110, 132);
            this.stdIdtb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stdIdtb.Name = "stdIdtb";
            this.stdIdtb.Size = new System.Drawing.Size(246, 26);
            this.stdIdtb.TabIndex = 22;
            // 
            // stdIdlbl
            // 
            this.stdIdlbl.AutoSize = true;
            this.stdIdlbl.Location = new System.Drawing.Point(9, 137);
            this.stdIdlbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stdIdlbl.Name = "stdIdlbl";
            this.stdIdlbl.Size = new System.Drawing.Size(91, 20);
            this.stdIdlbl.TabIndex = 21;
            this.stdIdlbl.Text = "Student ID:";
            // 
            // stdLNametb
            // 
            this.stdLNametb.Location = new System.Drawing.Point(110, 80);
            this.stdLNametb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stdLNametb.Name = "stdLNametb";
            this.stdLNametb.Size = new System.Drawing.Size(246, 26);
            this.stdLNametb.TabIndex = 20;
            // 
            // stdnLNamelbl
            // 
            this.stdnLNamelbl.AutoSize = true;
            this.stdnLNamelbl.Location = new System.Drawing.Point(9, 85);
            this.stdnLNamelbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stdnLNamelbl.Name = "stdnLNamelbl";
            this.stdnLNamelbl.Size = new System.Drawing.Size(90, 20);
            this.stdnLNamelbl.TabIndex = 19;
            this.stdnLNamelbl.Text = "Last Name:";
            // 
            // semlbl
            // 
            this.semlbl.AutoSize = true;
            this.semlbl.Location = new System.Drawing.Point(20, 192);
            this.semlbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.semlbl.Name = "semlbl";
            this.semlbl.Size = new System.Drawing.Size(82, 20);
            this.semlbl.TabIndex = 18;
            this.semlbl.Text = "Semester:";
            // 
            // stdFNametb
            // 
            this.stdFNametb.Location = new System.Drawing.Point(110, 26);
            this.stdFNametb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stdFNametb.Name = "stdFNametb";
            this.stdFNametb.Size = new System.Drawing.Size(246, 26);
            this.stdFNametb.TabIndex = 17;
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
            this.semcmbo.Location = new System.Drawing.Point(110, 188);
            this.semcmbo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.semcmbo.Name = "semcmbo";
            this.semcmbo.Size = new System.Drawing.Size(246, 28);
            this.semcmbo.TabIndex = 16;
            // 
            // stdFNamelbl
            // 
            this.stdFNamelbl.AutoSize = true;
            this.stdFNamelbl.Location = new System.Drawing.Point(10, 31);
            this.stdFNamelbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stdFNamelbl.Name = "stdFNamelbl";
            this.stdFNamelbl.Size = new System.Drawing.Size(90, 20);
            this.stdFNamelbl.TabIndex = 15;
            this.stdFNamelbl.Text = "First Name:";
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(540, 174);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(164, 38);
            this.cancelBtn.TabIndex = 14;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(540, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(164, 38);
            this.button1.TabIndex = 13;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(540, 98);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(164, 38);
            this.btnDelete.TabIndex = 25;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // EditStudent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.commlbl);
            this.Controls.Add(this.commrtb);
            this.Controls.Add(this.stdIdtb);
            this.Controls.Add(this.stdIdlbl);
            this.Controls.Add(this.stdLNametb);
            this.Controls.Add(this.stdnLNamelbl);
            this.Controls.Add(this.semlbl);
            this.Controls.Add(this.stdFNametb);
            this.Controls.Add(this.semcmbo);
            this.Controls.Add(this.stdFNamelbl);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "EditStudent";
            this.Size = new System.Drawing.Size(778, 448);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label commlbl;
        private System.Windows.Forms.RichTextBox commrtb;
        private System.Windows.Forms.TextBox stdIdtb;
        private System.Windows.Forms.Label stdIdlbl;
        private System.Windows.Forms.TextBox stdLNametb;
        private System.Windows.Forms.Label stdnLNamelbl;
        private System.Windows.Forms.Label semlbl;
        private System.Windows.Forms.TextBox stdFNametb;
        private System.Windows.Forms.ComboBox semcmbo;
        private System.Windows.Forms.Label stdFNamelbl;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDelete;
    }
}
