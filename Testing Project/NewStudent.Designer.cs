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
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(356, 25);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(356, 60);
            this.cancelBtn.Margin = new System.Windows.Forms.Padding(2);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(109, 25);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // stdFNamelbl
            // 
            this.stdFNamelbl.AutoSize = true;
            this.stdFNamelbl.Location = new System.Drawing.Point(3, 25);
            this.stdFNamelbl.Name = "stdFNamelbl";
            this.stdFNamelbl.Size = new System.Drawing.Size(60, 13);
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
            this.semcmbo.Location = new System.Drawing.Point(69, 127);
            this.semcmbo.Name = "semcmbo";
            this.semcmbo.Size = new System.Drawing.Size(165, 21);
            this.semcmbo.TabIndex = 3;
            // 
            // stdFNametb
            // 
            this.stdFNametb.Location = new System.Drawing.Point(69, 22);
            this.stdFNametb.Name = "stdFNametb";
            this.stdFNametb.Size = new System.Drawing.Size(165, 20);
            this.stdFNametb.TabIndex = 4;
            this.stdFNametb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.stdFNametb_KeyPress);
            // 
            // semlbl
            // 
            this.semlbl.AutoSize = true;
            this.semlbl.Location = new System.Drawing.Point(9, 130);
            this.semlbl.Name = "semlbl";
            this.semlbl.Size = new System.Drawing.Size(54, 13);
            this.semlbl.TabIndex = 5;
            this.semlbl.Text = "Semester:";
            // 
            // stdLNametb
            // 
            this.stdLNametb.Location = new System.Drawing.Point(69, 57);
            this.stdLNametb.Name = "stdLNametb";
            this.stdLNametb.Size = new System.Drawing.Size(165, 20);
            this.stdLNametb.TabIndex = 7;
            this.stdLNametb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.stdLNametb_KeyPress);
            // 
            // stdnLNamelbl
            // 
            this.stdnLNamelbl.AutoSize = true;
            this.stdnLNamelbl.Location = new System.Drawing.Point(2, 60);
            this.stdnLNamelbl.Name = "stdnLNamelbl";
            this.stdnLNamelbl.Size = new System.Drawing.Size(61, 13);
            this.stdnLNamelbl.TabIndex = 6;
            this.stdnLNamelbl.Text = "Last Name:";
            // 
            // stdIdtb
            // 
            this.stdIdtb.Location = new System.Drawing.Point(69, 91);
            this.stdIdtb.Name = "stdIdtb";
            this.stdIdtb.Size = new System.Drawing.Size(165, 20);
            this.stdIdtb.TabIndex = 9;
            this.stdIdtb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.stdIdtb_KeyPress);
            // 
            // stdIdlbl
            // 
            this.stdIdlbl.AutoSize = true;
            this.stdIdlbl.Location = new System.Drawing.Point(2, 94);
            this.stdIdlbl.Name = "stdIdlbl";
            this.stdIdlbl.Size = new System.Drawing.Size(61, 13);
            this.stdIdlbl.TabIndex = 8;
            this.stdIdlbl.Text = "Student ID:";
            // 
            // commrtb
            // 
            this.commrtb.Location = new System.Drawing.Point(12, 190);
            this.commrtb.Name = "commrtb";
            this.commrtb.Size = new System.Drawing.Size(496, 89);
            this.commrtb.TabIndex = 10;
            this.commrtb.Text = "";
            // 
            // commlbl
            // 
            this.commlbl.AutoSize = true;
            this.commlbl.Location = new System.Drawing.Point(5, 171);
            this.commlbl.Name = "commlbl";
            this.commlbl.Size = new System.Drawing.Size(59, 13);
            this.commlbl.TabIndex = 12;
            this.commlbl.Text = "Comments:";
            // 
            // NewStudent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
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
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NewStudent";
            this.Size = new System.Drawing.Size(519, 291);
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
    }
}
