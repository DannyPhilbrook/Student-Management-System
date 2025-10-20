namespace Testing_Project
{
    partial class NewClass
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
            this.lblCourseName = new System.Windows.Forms.Label();
            this.lblClassLabel = new System.Windows.Forms.Label();
            this.lblClassNum = new System.Windows.Forms.Label();
            this.tbCourseNum = new System.Windows.Forms.TextBox();
            this.tbClassName = new System.Windows.Forms.TextBox();
            this.lblSemester = new System.Windows.Forms.Label();
            this.cmbCourseLabel = new System.Windows.Forms.ComboBox();
            this.cmbSemester = new System.Windows.Forms.ComboBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCourseName
            // 
            this.lblCourseName.AutoSize = true;
            this.lblCourseName.Location = new System.Drawing.Point(3, 48);
            this.lblCourseName.Name = "lblCourseName";
            this.lblCourseName.Size = new System.Drawing.Size(93, 16);
            this.lblCourseName.TabIndex = 0;
            this.lblCourseName.Text = "Course Name:";
            // 
            // lblClassLabel
            // 
            this.lblClassLabel.AutoSize = true;
            this.lblClassLabel.Location = new System.Drawing.Point(3, 111);
            this.lblClassLabel.Name = "lblClassLabel";
            this.lblClassLabel.Size = new System.Drawing.Size(90, 16);
            this.lblClassLabel.TabIndex = 1;
            this.lblClassLabel.Text = "Course Label:";
            // 
            // lblClassNum
            // 
            this.lblClassNum.AutoSize = true;
            this.lblClassNum.Location = new System.Drawing.Point(3, 174);
            this.lblClassNum.Name = "lblClassNum";
            this.lblClassNum.Size = new System.Drawing.Size(104, 16);
            this.lblClassNum.TabIndex = 2;
            this.lblClassNum.Text = "Course Number:";
            // 
            // tbCourseNum
            // 
            this.tbCourseNum.Location = new System.Drawing.Point(113, 171);
            this.tbCourseNum.Name = "tbCourseNum";
            this.tbCourseNum.Size = new System.Drawing.Size(214, 22);
            this.tbCourseNum.TabIndex = 3;
            this.tbCourseNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbCourseNum_KeyPress);
            // 
            // tbClassName
            // 
            this.tbClassName.Location = new System.Drawing.Point(102, 45);
            this.tbClassName.Name = "tbClassName";
            this.tbClassName.Size = new System.Drawing.Size(225, 22);
            this.tbClassName.TabIndex = 4;
            // 
            // lblSemester
            // 
            this.lblSemester.AutoSize = true;
            this.lblSemester.Location = new System.Drawing.Point(3, 241);
            this.lblSemester.Name = "lblSemester";
            this.lblSemester.Size = new System.Drawing.Size(114, 16);
            this.lblSemester.TabIndex = 5;
            this.lblSemester.Text = "Course Semester:";
            // 
            // cmbCourseLabel
            // 
            this.cmbCourseLabel.FormattingEnabled = true;
            this.cmbCourseLabel.Location = new System.Drawing.Point(102, 111);
            this.cmbCourseLabel.Name = "cmbCourseLabel";
            this.cmbCourseLabel.Size = new System.Drawing.Size(225, 24);
            this.cmbCourseLabel.TabIndex = 6;
            // 
            // cmbSemester
            // 
            this.cmbSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSemester.FormattingEnabled = true;
            this.cmbSemester.Items.AddRange(new object[] {
            "Fall",
            "Spring"});
            this.cmbSemester.Location = new System.Drawing.Point(123, 238);
            this.cmbSemester.Name = "cmbSemester";
            this.cmbSemester.Size = new System.Drawing.Size(204, 24);
            this.cmbSemester.TabIndex = 7;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(499, 73);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(143, 30);
            this.btnSubmit.TabIndex = 8;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(499, 202);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(143, 30);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // NewClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Name = "NewClass";
            this.Size = new System.Drawing.Size(692, 358);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCourseName;
        private System.Windows.Forms.Label lblClassLabel;
        private System.Windows.Forms.Label lblClassNum;
        private System.Windows.Forms.TextBox tbCourseNum;
        private System.Windows.Forms.TextBox tbClassName;
        private System.Windows.Forms.Label lblSemester;
        private System.Windows.Forms.ComboBox cmbCourseLabel;
        private System.Windows.Forms.ComboBox cmbSemester;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
    }
}
