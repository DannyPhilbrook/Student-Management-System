namespace Testing_Project
{
    partial class EditDegreePlan
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.dgvSemester = new System.Windows.Forms.DataGridView();
            this.cmbSem = new System.Windows.Forms.ComboBox();
            this.lblSemester = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.btnAddSemester = new System.Windows.Forms.Button();
            this.btnRmvSem = new System.Windows.Forms.Button();
            this.cmbClass = new System.Windows.Forms.ComboBox();
            this.lblClass = new System.Windows.Forms.Label();
            this.btnAddClass = new System.Windows.Forms.Button();
            this.btnRemoveClass = new System.Windows.Forms.Button();
            this.lblInstruction = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSemester)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(4, 437);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(180, 46);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(648, 437);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(180, 46);
            this.btnUpdate.TabIndex = 29;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // dgvSemester
            // 
            this.dgvSemester.AllowUserToAddRows = false;
            this.dgvSemester.AllowUserToDeleteRows = false;
            this.dgvSemester.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSemester.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSemester.Location = new System.Drawing.Point(7, 52);
            this.dgvSemester.MultiSelect = false;
            this.dgvSemester.Name = "dgvSemester";
            this.dgvSemester.ReadOnly = true;
            this.dgvSemester.RowHeadersWidth = 51;
            this.dgvSemester.RowTemplate.Height = 24;
            this.dgvSemester.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSemester.Size = new System.Drawing.Size(565, 368);
            this.dgvSemester.TabIndex = 30;
            // 
            // cmbSem
            // 
            this.cmbSem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSem.FormattingEnabled = true;
            this.cmbSem.Location = new System.Drawing.Point(78, 22);
            this.cmbSem.Name = "cmbSem";
            this.cmbSem.Size = new System.Drawing.Size(133, 24);
            this.cmbSem.TabIndex = 31;
            // 
            // lblSemester
            // 
            this.lblSemester.AutoSize = true;
            this.lblSemester.Location = new System.Drawing.Point(4, 25);
            this.lblSemester.Name = "lblSemester";
            this.lblSemester.Size = new System.Drawing.Size(68, 16);
            this.lblSemester.TabIndex = 32;
            this.lblSemester.Text = "Semester:";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(288, 22);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(109, 24);
            this.cmbYear.TabIndex = 33;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(243, 25);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(39, 16);
            this.lblYear.TabIndex = 34;
            this.lblYear.Text = "Year:";
            // 
            // btnAddSemester
            // 
            this.btnAddSemester.Location = new System.Drawing.Point(575, 23);
            this.btnAddSemester.Name = "btnAddSemester";
            this.btnAddSemester.Size = new System.Drawing.Size(125, 24);
            this.btnAddSemester.TabIndex = 35;
            this.btnAddSemester.Text = "Add Semester";
            this.btnAddSemester.UseVisualStyleBackColor = true;
            this.btnAddSemester.Click += new System.EventHandler(this.btnAddSemester_Click);
            // 
            // btnRmvSem
            // 
            this.btnRmvSem.Location = new System.Drawing.Point(706, 23);
            this.btnRmvSem.Name = "btnRmvSem";
            this.btnRmvSem.Size = new System.Drawing.Size(122, 23);
            this.btnRmvSem.TabIndex = 36;
            this.btnRmvSem.Text = "Remove Semester";
            this.btnRmvSem.UseVisualStyleBackColor = true;
            this.btnRmvSem.Click += new System.EventHandler(this.btnRmvSem_Click);
            // 
            // cmbClass
            // 
            this.cmbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClass.FormattingEnabled = true;
            this.cmbClass.Location = new System.Drawing.Point(579, 150);
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.Size = new System.Drawing.Size(246, 24);
            this.cmbClass.TabIndex = 37;
            // 
            // lblClass
            // 
            this.lblClass.AutoSize = true;
            this.lblClass.Location = new System.Drawing.Point(655, 131);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(85, 16);
            this.lblClass.TabIndex = 38;
            this.lblClass.Text = "All Class List:";
            // 
            // btnAddClass
            // 
            this.btnAddClass.Location = new System.Drawing.Point(578, 180);
            this.btnAddClass.Name = "btnAddClass";
            this.btnAddClass.Size = new System.Drawing.Size(246, 38);
            this.btnAddClass.TabIndex = 39;
            this.btnAddClass.Text = "Add Class";
            this.btnAddClass.UseVisualStyleBackColor = true;
            this.btnAddClass.Click += new System.EventHandler(this.btnAddClass_Click);
            // 
            // btnRemoveClass
            // 
            this.btnRemoveClass.Location = new System.Drawing.Point(582, 274);
            this.btnRemoveClass.Name = "btnRemoveClass";
            this.btnRemoveClass.Size = new System.Drawing.Size(246, 41);
            this.btnRemoveClass.TabIndex = 40;
            this.btnRemoveClass.Text = "Remove Class";
            this.btnRemoveClass.UseVisualStyleBackColor = true;
            this.btnRemoveClass.Click += new System.EventHandler(this.btnRemoveClass_Click);
            // 
            // lblInstruction
            // 
            this.lblInstruction.AutoSize = true;
            this.lblInstruction.Location = new System.Drawing.Point(630, 255);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(149, 16);
            this.lblInstruction.TabIndex = 41;
            this.lblInstruction.Text = "Remove Selected Row:";
            // 
            // EditDegreePlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblInstruction);
            this.Controls.Add(this.btnRemoveClass);
            this.Controls.Add(this.btnAddClass);
            this.Controls.Add(this.lblClass);
            this.Controls.Add(this.cmbClass);
            this.Controls.Add(this.btnRmvSem);
            this.Controls.Add(this.btnAddSemester);
            this.Controls.Add(this.lblYear);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.lblSemester);
            this.Controls.Add(this.cmbSem);
            this.Controls.Add(this.dgvSemester);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "EditDegreePlan";
            this.Size = new System.Drawing.Size(832, 487);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSemester)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.DataGridView dgvSemester;
        private System.Windows.Forms.ComboBox cmbSem;
        private System.Windows.Forms.Label lblSemester;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Button btnAddSemester;
        private System.Windows.Forms.Button btnRmvSem;
        private System.Windows.Forms.ComboBox cmbClass;
        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.Button btnAddClass;
        private System.Windows.Forms.Button btnRemoveClass;
        private System.Windows.Forms.Label lblInstruction;
    }
}
