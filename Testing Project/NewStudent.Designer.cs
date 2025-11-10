namespace Testing_Project
{
    partial class NewStudent
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
            this.button1 = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.stdFNamelbl = new System.Windows.Forms.Label();
            this.semcmbo = new System.Windows.Forms.ComboBox();
            this.stdFNametb = new System.Windows.Forms.TextBox();
            this.semlbl = new System.Windows.Forms.Label();
            this.stdLNametb = new System.Windows.Forms.TextBox();
            this.stdnLNamelbl = new System.Windows.Forms.Label();
            this.stdIdtb = new System.Windows.Forms.TextBox();
            this.stdIdlbl = new System.Windows.Forms.Label();
            this.commrtb = new System.Windows.Forms.RichTextBox();
            this.commlbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSchlYear = new System.Windows.Forms.TextBox();
            this.rdbWaiting = new System.Windows.Forms.RadioButton();
            this.rdbActive = new System.Windows.Forms.RadioButton();
            this.rdbInactive = new System.Windows.Forms.RadioButton();
            this.rdbGraduated = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(531, 160);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 31);
            this.button1.TabIndex = 0;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(347, 160);
            this.cancelBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(145, 31);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // stdFNamelbl
            // 
            this.stdFNamelbl.AutoSize = true;
            this.stdFNamelbl.Location = new System.Drawing.Point(4, 31);
            this.stdFNamelbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stdFNamelbl.Name = "stdFNamelbl";
            this.stdFNamelbl.Size = new System.Drawing.Size(75, 16);
            this.stdFNamelbl.TabIndex = 2;
            this.stdFNamelbl.Text = "First Name:";
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
            this.semcmbo.Location = new System.Drawing.Point(92, 156);
            this.semcmbo.Margin = new System.Windows.Forms.Padding(4);
            this.semcmbo.Name = "semcmbo";
            this.semcmbo.Size = new System.Drawing.Size(219, 24);
            this.semcmbo.TabIndex = 3;
            // 
            // stdFNametb
            // 
            this.stdFNametb.Location = new System.Drawing.Point(92, 27);
            this.stdFNametb.Margin = new System.Windows.Forms.Padding(4);
            this.stdFNametb.Name = "stdFNametb";
            this.stdFNametb.Size = new System.Drawing.Size(219, 22);
            this.stdFNametb.TabIndex = 4;
            this.stdFNametb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.stdFNametb_KeyPress);
            // 
            // semlbl
            // 
            this.semlbl.AutoSize = true;
            this.semlbl.Location = new System.Drawing.Point(12, 160);
            this.semlbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.semlbl.Name = "semlbl";
            this.semlbl.Size = new System.Drawing.Size(68, 16);
            this.semlbl.TabIndex = 5;
            this.semlbl.Text = "Semester:";
            // 
            // stdLNametb
            // 
            this.stdLNametb.Location = new System.Drawing.Point(92, 70);
            this.stdLNametb.Margin = new System.Windows.Forms.Padding(4);
            this.stdLNametb.Name = "stdLNametb";
            this.stdLNametb.Size = new System.Drawing.Size(219, 22);
            this.stdLNametb.TabIndex = 7;
            this.stdLNametb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.stdLNametb_KeyPress);
            // 
            // stdnLNamelbl
            // 
            this.stdnLNamelbl.AutoSize = true;
            this.stdnLNamelbl.Location = new System.Drawing.Point(3, 74);
            this.stdnLNamelbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stdnLNamelbl.Name = "stdnLNamelbl";
            this.stdnLNamelbl.Size = new System.Drawing.Size(75, 16);
            this.stdnLNamelbl.TabIndex = 6;
            this.stdnLNamelbl.Text = "Last Name:";
            // 
            // stdIdtb
            // 
            this.stdIdtb.Location = new System.Drawing.Point(92, 112);
            this.stdIdtb.Margin = new System.Windows.Forms.Padding(4);
            this.stdIdtb.Name = "stdIdtb";
            this.stdIdtb.Size = new System.Drawing.Size(219, 22);
            this.stdIdtb.TabIndex = 9;
            this.stdIdtb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.stdIdtb_KeyPress);
            // 
            // stdIdlbl
            // 
            this.stdIdlbl.AutoSize = true;
            this.stdIdlbl.Location = new System.Drawing.Point(3, 116);
            this.stdIdlbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stdIdlbl.Name = "stdIdlbl";
            this.stdIdlbl.Size = new System.Drawing.Size(71, 16);
            this.stdIdlbl.TabIndex = 8;
            this.stdIdlbl.Text = "Student ID:";
            // 
            // commrtb
            // 
            this.commrtb.Location = new System.Drawing.Point(15, 230);
            this.commrtb.Margin = new System.Windows.Forms.Padding(4);
            this.commrtb.Name = "commrtb";
            this.commrtb.Size = new System.Drawing.Size(660, 109);
            this.commrtb.TabIndex = 10;
            this.commrtb.Text = "";
            // 
            // commlbl
            // 
            this.commlbl.AutoSize = true;
            this.commlbl.Location = new System.Drawing.Point(7, 210);
            this.commlbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.commlbl.Name = "commlbl";
            this.commlbl.Size = new System.Drawing.Size(74, 16);
            this.commlbl.TabIndex = 12;
            this.commlbl.Text = "Comments:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(377, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = "School Year:";
            // 
            // tbSchlYear
            // 
            this.tbSchlYear.Location = new System.Drawing.Point(467, 27);
            this.tbSchlYear.Name = "tbSchlYear";
            this.tbSchlYear.Size = new System.Drawing.Size(194, 22);
            this.tbSchlYear.TabIndex = 14;
            this.tbSchlYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSchlYear_KeyPress);
            // 
            // rdbWaiting
            // 
            this.rdbWaiting.AutoSize = true;
            this.rdbWaiting.Location = new System.Drawing.Point(382, 70);
            this.rdbWaiting.Name = "rdbWaiting";
            this.rdbWaiting.Size = new System.Drawing.Size(73, 20);
            this.rdbWaiting.TabIndex = 13;
            this.rdbWaiting.TabStop = true;
            this.rdbWaiting.Text = "Waiting";
            this.rdbWaiting.UseVisualStyleBackColor = true;
            // 
            // rdbActive
            // 
            this.rdbActive.AutoSize = true;
            this.rdbActive.Location = new System.Drawing.Point(382, 112);
            this.rdbActive.Name = "rdbActive";
            this.rdbActive.Size = new System.Drawing.Size(65, 20);
            this.rdbActive.TabIndex = 14;
            this.rdbActive.TabStop = true;
            this.rdbActive.Text = "Active";
            this.rdbActive.UseVisualStyleBackColor = true;
            // 
            // rdbInactive
            // 
            this.rdbInactive.AutoSize = true;
            this.rdbInactive.Location = new System.Drawing.Point(577, 70);
            this.rdbInactive.Name = "rdbInactive";
            this.rdbInactive.Size = new System.Drawing.Size(74, 20);
            this.rdbInactive.TabIndex = 15;
            this.rdbInactive.TabStop = true;
            this.rdbInactive.Text = "Inactive";
            this.rdbInactive.UseVisualStyleBackColor = true;
            // 
            // rdbGraduated
            // 
            this.rdbGraduated.AutoSize = true;
            this.rdbGraduated.Location = new System.Drawing.Point(577, 112);
            this.rdbGraduated.Name = "rdbGraduated";
            this.rdbGraduated.Size = new System.Drawing.Size(92, 20);
            this.rdbGraduated.TabIndex = 16;
            this.rdbGraduated.TabStop = true;
            this.rdbGraduated.Text = "Graduated";
            this.rdbGraduated.UseVisualStyleBackColor = true;
            // 
            // NewStudent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tbSchlYear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdbGraduated);
            this.Controls.Add(this.rdbInactive);
            this.Controls.Add(this.rdbActive);
            this.Controls.Add(this.rdbWaiting);
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
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "NewStudent";
            this.Size = new System.Drawing.Size(692, 358);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Label stdFNamelbl;
        private System.Windows.Forms.ComboBox semcmbo;
        private System.Windows.Forms.TextBox stdFNametb;
        private System.Windows.Forms.Label semlbl;
        private System.Windows.Forms.TextBox stdLNametb;
        private System.Windows.Forms.Label stdnLNamelbl;
        private System.Windows.Forms.TextBox stdIdtb;
        private System.Windows.Forms.Label stdIdlbl;
        private System.Windows.Forms.RichTextBox commrtb;
        private System.Windows.Forms.Label commlbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSchlYear;
        private System.Windows.Forms.RadioButton rdbWaiting;
        private System.Windows.Forms.RadioButton rdbActive;
        private System.Windows.Forms.RadioButton rdbInactive;
        private System.Windows.Forms.RadioButton rdbGraduated;
    }
}
