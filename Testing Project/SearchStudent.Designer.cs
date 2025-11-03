namespace Testing_Project
{
    partial class SearchStudent
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
            this.ckbActive = new System.Windows.Forms.CheckBox();
            this.ckbWaiting = new System.Windows.Forms.CheckBox();
            this.ckbInactive = new System.Windows.Forms.CheckBox();
            this.ckbGraduated = new System.Windows.Forms.CheckBox();
            this.createSdtbtn = new System.Windows.Forms.Button();
            this.stuslbl = new System.Windows.Forms.Label();
            this.firstNamelbl = new System.Windows.Forms.Label();
            this.idlbl = new System.Windows.Forms.Label();
            this.stdntFNametb = new System.Windows.Forms.TextBox();
            this.stdntIdtb = new System.Windows.Forms.TextBox();
            this.srchBtn = new System.Windows.Forms.Button();
            this.menubtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.stdntLNametb = new System.Windows.Forms.TextBox();
            this.lastNamelbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ckbActive
            // 
            this.ckbActive.AutoSize = true;
            this.ckbActive.Location = new System.Drawing.Point(464, 46);
            this.ckbActive.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ckbActive.Name = "ckbActive";
            this.ckbActive.Size = new System.Drawing.Size(78, 24);
            this.ckbActive.TabIndex = 1;
            this.ckbActive.Text = "Active";
            this.ckbActive.UseVisualStyleBackColor = true;
            // 
            // ckbWaiting
            // 
            this.ckbWaiting.AutoSize = true;
            this.ckbWaiting.Location = new System.Drawing.Point(370, 46);
            this.ckbWaiting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ckbWaiting.Name = "ckbWaiting";
            this.ckbWaiting.Size = new System.Drawing.Size(88, 24);
            this.ckbWaiting.TabIndex = 2;
            this.ckbWaiting.Text = "Waiting";
            this.ckbWaiting.UseVisualStyleBackColor = true;
            // 
            // ckbInactive
            // 
            this.ckbInactive.AutoSize = true;
            this.ckbInactive.Location = new System.Drawing.Point(549, 46);
            this.ckbInactive.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ckbInactive.Name = "ckbInactive";
            this.ckbInactive.Size = new System.Drawing.Size(90, 24);
            this.ckbInactive.TabIndex = 3;
            this.ckbInactive.Text = "Inactive";
            this.ckbInactive.UseVisualStyleBackColor = true;
            // 
            // ckbGraduated
            // 
            this.ckbGraduated.AutoSize = true;
            this.ckbGraduated.Location = new System.Drawing.Point(645, 46);
            this.ckbGraduated.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ckbGraduated.Name = "ckbGraduated";
            this.ckbGraduated.Size = new System.Drawing.Size(112, 24);
            this.ckbGraduated.TabIndex = 4;
            this.ckbGraduated.Text = "Graduated";
            this.ckbGraduated.UseVisualStyleBackColor = true;
            // 
            // createSdtbtn
            // 
            this.createSdtbtn.Location = new System.Drawing.Point(4, 349);
            this.createSdtbtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.createSdtbtn.Name = "createSdtbtn";
            this.createSdtbtn.Size = new System.Drawing.Size(235, 45);
            this.createSdtbtn.TabIndex = 7;
            this.createSdtbtn.Text = "New Student";
            this.createSdtbtn.UseVisualStyleBackColor = true;
            this.createSdtbtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // stuslbl
            // 
            this.stuslbl.AutoSize = true;
            this.stuslbl.Location = new System.Drawing.Point(244, 46);
            this.stuslbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.stuslbl.Name = "stuslbl";
            this.stuslbl.Size = new System.Drawing.Size(119, 20);
            this.stuslbl.TabIndex = 8;
            this.stuslbl.Text = "Filter by Status:";
            // 
            // firstNamelbl
            // 
            this.firstNamelbl.AutoSize = true;
            this.firstNamelbl.Location = new System.Drawing.Point(4, 97);
            this.firstNamelbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.firstNamelbl.Name = "firstNamelbl";
            this.firstNamelbl.Size = new System.Drawing.Size(90, 20);
            this.firstNamelbl.TabIndex = 9;
            this.firstNamelbl.Text = "First Name:";
            // 
            // idlbl
            // 
            this.idlbl.AutoSize = true;
            this.idlbl.Location = new System.Drawing.Point(4, 207);
            this.idlbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.idlbl.Name = "idlbl";
            this.idlbl.Size = new System.Drawing.Size(91, 20);
            this.idlbl.TabIndex = 10;
            this.idlbl.Text = "Student ID:";
            // 
            // stdntFNametb
            // 
            this.stdntFNametb.Location = new System.Drawing.Point(4, 121);
            this.stdntFNametb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stdntFNametb.Name = "stdntFNametb";
            this.stdntFNametb.Size = new System.Drawing.Size(230, 26);
            this.stdntFNametb.TabIndex = 11;
            this.stdntFNametb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.stdntFNametb_KeyPress);
            // 
            // stdntIdtb
            // 
            this.stdntIdtb.Location = new System.Drawing.Point(4, 232);
            this.stdntIdtb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stdntIdtb.Name = "stdntIdtb";
            this.stdntIdtb.Size = new System.Drawing.Size(230, 26);
            this.stdntIdtb.TabIndex = 12;
            this.stdntIdtb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.stdntIdtb_KeyPress);
            // 
            // srchBtn
            // 
            this.srchBtn.Location = new System.Drawing.Point(60, 297);
            this.srchBtn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.srchBtn.Name = "srchBtn";
            this.srchBtn.Size = new System.Drawing.Size(112, 35);
            this.srchBtn.TabIndex = 13;
            this.srchBtn.Text = "Search";
            this.srchBtn.UseVisualStyleBackColor = true;
            this.srchBtn.Click += new System.EventHandler(this.srchBtn_Click);
            // 
            // menubtn
            // 
            this.menubtn.Location = new System.Drawing.Point(4, 400);
            this.menubtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.menubtn.Name = "menubtn";
            this.menubtn.Size = new System.Drawing.Size(235, 41);
            this.menubtn.TabIndex = 14;
            this.menubtn.Text = "Main Menu";
            this.menubtn.UseVisualStyleBackColor = true;
            this.menubtn.Click += new System.EventHandler(this.menubtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(248, 88);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(523, 345);
            this.dataGridView1.TabIndex = 26;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // stdntLNametb
            // 
            this.stdntLNametb.Location = new System.Drawing.Point(4, 176);
            this.stdntLNametb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stdntLNametb.Name = "stdntLNametb";
            this.stdntLNametb.Size = new System.Drawing.Size(230, 26);
            this.stdntLNametb.TabIndex = 28;
            this.stdntLNametb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.stdntLNametb_KeyPress);
            // 
            // lastNamelbl
            // 
            this.lastNamelbl.AutoSize = true;
            this.lastNamelbl.Location = new System.Drawing.Point(4, 152);
            this.lastNamelbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lastNamelbl.Name = "lastNamelbl";
            this.lastNamelbl.Size = new System.Drawing.Size(90, 20);
            this.lastNamelbl.TabIndex = 27;
            this.lastNamelbl.Text = "Last Name:";
            // 
            // SearchStudent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stdntLNametb);
            this.Controls.Add(this.lastNamelbl);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menubtn);
            this.Controls.Add(this.srchBtn);
            this.Controls.Add(this.stdntIdtb);
            this.Controls.Add(this.stdntFNametb);
            this.Controls.Add(this.idlbl);
            this.Controls.Add(this.firstNamelbl);
            this.Controls.Add(this.stuslbl);
            this.Controls.Add(this.createSdtbtn);
            this.Controls.Add(this.ckbGraduated);
            this.Controls.Add(this.ckbInactive);
            this.Controls.Add(this.ckbWaiting);
            this.Controls.Add(this.ckbActive);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SearchStudent";
            this.Size = new System.Drawing.Size(778, 445);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox ckbActive;
        private System.Windows.Forms.CheckBox ckbWaiting;
        private System.Windows.Forms.CheckBox ckbInactive;
        private System.Windows.Forms.CheckBox ckbGraduated;
        private System.Windows.Forms.Button createSdtbtn;
        private System.Windows.Forms.Label stuslbl;
        private System.Windows.Forms.Label firstNamelbl;
        private System.Windows.Forms.Label idlbl;
        private System.Windows.Forms.TextBox stdntFNametb;
        private System.Windows.Forms.TextBox stdntIdtb;
        private System.Windows.Forms.Button srchBtn;
        private System.Windows.Forms.Button menubtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox stdntLNametb;
        private System.Windows.Forms.Label lastNamelbl;
    }
}
