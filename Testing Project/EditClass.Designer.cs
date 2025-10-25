namespace Testing_Project
{
    partial class EditClass
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.cmbSemester = new System.Windows.Forms.ComboBox();
            this.cmbCourseLabel = new System.Windows.Forms.ComboBox();
            this.lblSemester = new System.Windows.Forms.Label();
            this.tbClassName = new System.Windows.Forms.TextBox();
            this.tbCourseNum = new System.Windows.Forms.TextBox();
            this.lblClassNum = new System.Windows.Forms.Label();
            this.lblClassLabel = new System.Windows.Forms.Label();
            this.lblCourseName = new System.Windows.Forms.Label();
            this.delBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(392, 184);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 24);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(392, 79);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(2);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(107, 24);
            this.btnSubmit.TabIndex = 18;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // cmbSemester
            // 
            this.cmbSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSemester.FormattingEnabled = true;
            this.cmbSemester.Items.AddRange(new object[] {
            "Fall",
            "Spring"});
            this.cmbSemester.Location = new System.Drawing.Point(110, 213);
            this.cmbSemester.Margin = new System.Windows.Forms.Padding(2);
            this.cmbSemester.Name = "cmbSemester";
            this.cmbSemester.Size = new System.Drawing.Size(154, 21);
            this.cmbSemester.TabIndex = 17;
            // 
            // cmbCourseLabel
            // 
            this.cmbCourseLabel.FormattingEnabled = true;
            this.cmbCourseLabel.Location = new System.Drawing.Point(94, 110);
            this.cmbCourseLabel.Margin = new System.Windows.Forms.Padding(2);
            this.cmbCourseLabel.Name = "cmbCourseLabel";
            this.cmbCourseLabel.Size = new System.Drawing.Size(170, 21);
            this.cmbCourseLabel.TabIndex = 16;
            // 
            // lblSemester
            // 
            this.lblSemester.AutoSize = true;
            this.lblSemester.Location = new System.Drawing.Point(20, 216);
            this.lblSemester.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSemester.Name = "lblSemester";
            this.lblSemester.Size = new System.Drawing.Size(90, 13);
            this.lblSemester.TabIndex = 15;
            this.lblSemester.Text = "Course Semester:";
            // 
            // tbClassName
            // 
            this.tbClassName.Location = new System.Drawing.Point(94, 57);
            this.tbClassName.Margin = new System.Windows.Forms.Padding(2);
            this.tbClassName.Name = "tbClassName";
            this.tbClassName.Size = new System.Drawing.Size(170, 20);
            this.tbClassName.TabIndex = 14;
            // 
            // tbCourseNum
            // 
            this.tbCourseNum.Location = new System.Drawing.Point(103, 159);
            this.tbCourseNum.Margin = new System.Windows.Forms.Padding(2);
            this.tbCourseNum.Name = "tbCourseNum";
            this.tbCourseNum.Size = new System.Drawing.Size(162, 20);
            this.tbCourseNum.TabIndex = 13;
            // 
            // lblClassNum
            // 
            this.lblClassNum.AutoSize = true;
            this.lblClassNum.Location = new System.Drawing.Point(20, 161);
            this.lblClassNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClassNum.Name = "lblClassNum";
            this.lblClassNum.Size = new System.Drawing.Size(83, 13);
            this.lblClassNum.TabIndex = 12;
            this.lblClassNum.Text = "Course Number:";
            // 
            // lblClassLabel
            // 
            this.lblClassLabel.AutoSize = true;
            this.lblClassLabel.Location = new System.Drawing.Point(20, 110);
            this.lblClassLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClassLabel.Name = "lblClassLabel";
            this.lblClassLabel.Size = new System.Drawing.Size(72, 13);
            this.lblClassLabel.TabIndex = 11;
            this.lblClassLabel.Text = "Course Label:";
            // 
            // lblCourseName
            // 
            this.lblCourseName.AutoSize = true;
            this.lblCourseName.Location = new System.Drawing.Point(20, 59);
            this.lblCourseName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCourseName.Name = "lblCourseName";
            this.lblCourseName.Size = new System.Drawing.Size(74, 13);
            this.lblCourseName.TabIndex = 10;
            this.lblCourseName.Text = "Course Name:";
            // 
            // delBtn
            // 
            this.delBtn.Location = new System.Drawing.Point(392, 131);
            this.delBtn.Margin = new System.Windows.Forms.Padding(2);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(107, 24);
            this.delBtn.TabIndex = 20;
            this.delBtn.Text = "Delete";
            this.delBtn.UseVisualStyleBackColor = true;
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // EditClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.delBtn);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.cmbSemester);
            this.Controls.Add(this.cmbCourseLabel);
            this.Controls.Add(this.lblSemester);
            this.Controls.Add(this.tbClassName);
            this.Controls.Add(this.tbCourseNum);
            this.Controls.Add(this.lblClassNum);
            this.Controls.Add(this.lblClassLabel);
            this.Controls.Add(this.lblCourseName);
            this.Name = "EditClass";
            this.Size = new System.Drawing.Size(519, 291);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.ComboBox cmbSemester;
        private System.Windows.Forms.ComboBox cmbCourseLabel;
        private System.Windows.Forms.Label lblSemester;
        private System.Windows.Forms.TextBox tbClassName;
        private System.Windows.Forms.TextBox tbCourseNum;
        private System.Windows.Forms.Label lblClassNum;
        private System.Windows.Forms.Label lblClassLabel;
        private System.Windows.Forms.Label lblCourseName;
        private System.Windows.Forms.Button delBtn;
    }
}
