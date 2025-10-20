using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testing_Project
{
    public partial class NewClass : UserControl
    {
        public NewClass()
        {
            InitializeComponent();
        }

        private void tbCourseNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Display a confirmation dialog
            DialogResult result = MessageBox.Show(
                "Are you sure you wish to create this Course?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // If the user clicks 'Yes', proceed
            if (result == DialogResult.Yes)
            {
                // SQL WILL GO HERE TO ADD STUDENT TO DATABASE LATER.

                // After adding the student, navigate back to the MainPage
                var mainMenu = this.FindForm() as MainMenu;
                if (mainMenu != null)
                {
                    mainMenu.LoadPage(new MainPage());
                }
            }
            // If 'No', simply do nothing (stay on the page)
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Display a warning dialog
            DialogResult result = MessageBox.Show(
                "Are you sure you wish to cancel creation?",
                "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation
            );

            // If the user clicks 'Yes', proceed
            if (result == DialogResult.Yes)
            {
                // Quit out of the NewStudent page and return to MainPage
                var mainMenu = this.FindForm() as MainMenu;
                if (mainMenu != null)
                {
                    mainMenu.LoadPage(new SearchClasses());
                }
            }
            // If 'No', simply do nothing (stay on the page)
        }
    }
}
