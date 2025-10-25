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
            this.dgvDegree = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDegree)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDegree
            // 
            this.dgvDegree.AllowUserToAddRows = false;
            this.dgvDegree.AllowUserToDeleteRows = false;
            this.dgvDegree.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDegree.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvDegree.Location = new System.Drawing.Point(2, 2);
            this.dgvDegree.Margin = new System.Windows.Forms.Padding(2);
            this.dgvDegree.MultiSelect = false;
            this.dgvDegree.Name = "dgvDegree";
            this.dgvDegree.ReadOnly = true;
            this.dgvDegree.RowHeadersWidth = 62;
            this.dgvDegree.RowTemplate.Height = 28;
            this.dgvDegree.Size = new System.Drawing.Size(515, 247);
            this.dgvDegree.TabIndex = 26;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(3, 251);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(135, 37);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(381, 251);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(135, 37);
            this.btnUpdate.TabIndex = 29;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // EditDegreePlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dgvDegree);
            this.Name = "EditDegreePlan";
            this.Size = new System.Drawing.Size(519, 291);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDegree)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDegree;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnUpdate;
    }
}
