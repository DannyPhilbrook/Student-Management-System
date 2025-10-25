namespace Testing_Project
{
    partial class SearchClasses
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
            this.menubtn = new System.Windows.Forms.Button();
            this.btnMkClass = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.LabelTextBox = new System.Windows.Forms.TextBox();
            this.NumberTextBox = new System.Windows.Forms.TextBox();
            this.SearchClass = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menubtn
            // 
            this.menubtn.Location = new System.Drawing.Point(2, 260);
            this.menubtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.menubtn.Name = "menubtn";
            this.menubtn.Size = new System.Drawing.Size(157, 27);
            this.menubtn.TabIndex = 15;
            this.menubtn.Text = "Main Menu";
            this.menubtn.UseVisualStyleBackColor = true;
            this.menubtn.Click += new System.EventHandler(this.menubtn_Click);
            // 
            // btnMkClass
            // 
            this.btnMkClass.Location = new System.Drawing.Point(2, 228);
            this.btnMkClass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnMkClass.Name = "btnMkClass";
            this.btnMkClass.Size = new System.Drawing.Size(157, 27);
            this.btnMkClass.TabIndex = 16;
            this.btnMkClass.Text = "Create Class";
            this.btnMkClass.UseVisualStyleBackColor = true;
            this.btnMkClass.Click += new System.EventHandler(this.btnMkClass_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Label:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 86);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Number:";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(2, 34);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(128, 20);
            this.NameTextBox.TabIndex = 21;
            this.NameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NameTextBox_KeyPress);
            // 
            // LabelTextBox
            // 
            this.LabelTextBox.Location = new System.Drawing.Point(2, 68);
            this.LabelTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.LabelTextBox.Name = "LabelTextBox";
            this.LabelTextBox.Size = new System.Drawing.Size(128, 20);
            this.LabelTextBox.TabIndex = 22;
            this.LabelTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LabelTextBox_KeyPress);
            // 
            // NumberTextBox
            // 
            this.NumberTextBox.Location = new System.Drawing.Point(2, 101);
            this.NumberTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.NumberTextBox.Name = "NumberTextBox";
            this.NumberTextBox.Size = new System.Drawing.Size(128, 20);
            this.NumberTextBox.TabIndex = 23;
            this.NumberTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumberTextBox_KeyPress);
            // 
            // SearchClass
            // 
            this.SearchClass.Location = new System.Drawing.Point(5, 123);
            this.SearchClass.Margin = new System.Windows.Forms.Padding(2);
            this.SearchClass.Name = "SearchClass";
            this.SearchClass.Size = new System.Drawing.Size(119, 29);
            this.SearchClass.TabIndex = 24;
            this.SearchClass.Text = "Search";
            this.SearchClass.UseVisualStyleBackColor = true;
            this.SearchClass.Click += new System.EventHandler(this.SearchClass_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(134, 9);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.Size = new System.Drawing.Size(370, 214);
            this.dataGridView1.TabIndex = 25;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // SearchClasses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.SearchClass);
            this.Controls.Add(this.NumberTextBox);
            this.Controls.Add(this.LabelTextBox);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMkClass);
            this.Controls.Add(this.menubtn);
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "SearchClasses";
            this.Size = new System.Drawing.Size(519, 289);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button menubtn;
        private System.Windows.Forms.Button btnMkClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.TextBox LabelTextBox;
        private System.Windows.Forms.TextBox NumberTextBox;
        private System.Windows.Forms.Button SearchClass;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}
