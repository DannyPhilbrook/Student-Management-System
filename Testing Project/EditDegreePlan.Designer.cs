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
            this.btnCancel.Location = new System.Drawing.Point(6, 267);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(114, 21);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(396, 267);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(114, 21);
            this.btnUpdate.TabIndex = 29;
            this.btnUpdate.Text = "Finish Changes";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // dgvSemester
            // 
            this.dgvSemester.AllowUserToAddRows = false;
            this.dgvSemester.AllowUserToDeleteRows = false;
            this.dgvSemester.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSemester.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvSemester.Location = new System.Drawing.Point(5, 42);
            this.dgvSemester.Margin = new System.Windows.Forms.Padding(2);
            this.dgvSemester.MultiSelect = false;
            this.dgvSemester.Name = "dgvSemester";
            this.dgvSemester.ReadOnly = true;
            this.dgvSemester.RowHeadersWidth = 51;
            this.dgvSemester.RowTemplate.Height = 24;
            this.dgvSemester.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSemester.Size = new System.Drawing.Size(371, 214);
            this.dgvSemester.TabIndex = 30;
            this.dgvSemester.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSemester_CellMouseDoubleClick);
            // 
            // cmbSem
            // 
            this.cmbSem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSem.FormattingEnabled = true;
            this.cmbSem.Location = new System.Drawing.Point(58, 18);
            this.cmbSem.Margin = new System.Windows.Forms.Padding(2);
            this.cmbSem.Name = "cmbSem";
            this.cmbSem.Size = new System.Drawing.Size(101, 21);
            this.cmbSem.TabIndex = 31;
            this.cmbSem.SelectedIndexChanged += new System.EventHandler(this.cmbSem_SelectedIndexChanged);
            // 
            // lblSemester
            // 
            this.lblSemester.AutoSize = true;
            this.lblSemester.Location = new System.Drawing.Point(3, 20);
            this.lblSemester.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSemester.Name = "lblSemester";
            this.lblSemester.Size = new System.Drawing.Size(54, 13);
            this.lblSemester.TabIndex = 32;
            this.lblSemester.Text = "Semester:";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(205, 17);
            this.cmbYear.Margin = new System.Windows.Forms.Padding(2);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(83, 21);
            this.cmbYear.TabIndex = 33;
            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(169, 22);
            this.lblYear.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(32, 13);
            this.lblYear.TabIndex = 34;
            this.lblYear.Text = "Year:";
            // 
            // btnAddSemester
            // 
            this.btnAddSemester.Location = new System.Drawing.Point(316, 16);
            this.btnAddSemester.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddSemester.Name = "btnAddSemester";
            this.btnAddSemester.Size = new System.Drawing.Size(94, 20);
            this.btnAddSemester.TabIndex = 35;
            this.btnAddSemester.Text = "Add Semester";
            this.btnAddSemester.UseVisualStyleBackColor = true;
            this.btnAddSemester.Click += new System.EventHandler(this.btnAddSemester_Click);
            // 
            // btnRmvSem
            // 
            this.btnRmvSem.Location = new System.Drawing.Point(415, 17);
            this.btnRmvSem.Margin = new System.Windows.Forms.Padding(2);
            this.btnRmvSem.Name = "btnRmvSem";
            this.btnRmvSem.Size = new System.Drawing.Size(92, 19);
            this.btnRmvSem.TabIndex = 36;
            this.btnRmvSem.Text = "Remove Semester";
            this.btnRmvSem.UseVisualStyleBackColor = true;
            this.btnRmvSem.Click += new System.EventHandler(this.btnRmvSem_Click);
            // 
            // cmbClass
            // 
            this.cmbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClass.FormattingEnabled = true;
            this.cmbClass.Location = new System.Drawing.Point(380, 106);
            this.cmbClass.Margin = new System.Windows.Forms.Padding(2);
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.Size = new System.Drawing.Size(137, 21);
            this.cmbClass.TabIndex = 37;
            // 
            // lblClass
            // 
            this.lblClass.AutoSize = true;
            this.lblClass.Location = new System.Drawing.Point(412, 91);
            this.lblClass.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(68, 13);
            this.lblClass.TabIndex = 38;
            this.lblClass.Text = "All Class List:";
            // 
            // btnAddClass
            // 
            this.btnAddClass.Location = new System.Drawing.Point(380, 131);
            this.btnAddClass.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddClass.Name = "btnAddClass";
            this.btnAddClass.Size = new System.Drawing.Size(137, 19);
            this.btnAddClass.TabIndex = 39;
            this.btnAddClass.Text = "Add Class";
            this.btnAddClass.UseVisualStyleBackColor = true;
            this.btnAddClass.Click += new System.EventHandler(this.btnAddClass_Click);
            // 
            // btnRemoveClass
            // 
            this.btnRemoveClass.Location = new System.Drawing.Point(380, 187);
            this.btnRemoveClass.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveClass.Name = "btnRemoveClass";
            this.btnRemoveClass.Size = new System.Drawing.Size(137, 23);
            this.btnRemoveClass.TabIndex = 40;
            this.btnRemoveClass.Text = "Remove Class";
            this.btnRemoveClass.UseVisualStyleBackColor = true;
            this.btnRemoveClass.Click += new System.EventHandler(this.btnRemoveClass_Click);
            // 
            // lblInstruction
            // 
            this.lblInstruction.AutoSize = true;
            this.lblInstruction.Location = new System.Drawing.Point(390, 172);
            this.lblInstruction.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(120, 13);
            this.lblInstruction.TabIndex = 41;
            this.lblInstruction.Text = "Remove Selected Row:";
            // 
            // EditDegreePlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
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
            this.Name = "EditDegreePlan";
            this.Size = new System.Drawing.Size(519, 291);
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
