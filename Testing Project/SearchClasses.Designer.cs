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
            this.SuspendLayout();
            // 
            // menubtn
            // 
            this.menubtn.Location = new System.Drawing.Point(3, 320);
            this.menubtn.Name = "menubtn";
            this.menubtn.Size = new System.Drawing.Size(209, 33);
            this.menubtn.TabIndex = 15;
            this.menubtn.Text = "Main Menu";
            this.menubtn.UseVisualStyleBackColor = true;
            this.menubtn.Click += new System.EventHandler(this.menubtn_Click);
            // 
            // SearchClasses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.menubtn);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SearchClasses";
            this.Size = new System.Drawing.Size(692, 356);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button menubtn;
    }
}
