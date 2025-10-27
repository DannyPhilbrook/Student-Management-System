using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Testing_Project
{
    public partial class NewStudent : UserControl
    {
        private string dbPath = "Data Source=Database/stdmngsys.db;Version=3;";
        public NewStudent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Display a confirmation dialog
            DialogResult result = MessageBox.Show(
                "Are you sure you wish to create this Student?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // If the user clicks 'Yes', proceed
            if (result == DialogResult.Yes)
            {
                // Basic validation, kicks us out if yes
                if (string.IsNullOrWhiteSpace(stdFNametb.Text) ||
                    string.IsNullOrWhiteSpace(stdLNametb.Text) ||
                    string.IsNullOrWhiteSpace(stdIdtb.Text) ||
                    semcmbo.SelectedItem == null)
                {
                    MessageBox.Show("Please fill out all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    // SQLite connection string
                    using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                    {
                        conn.Open();

                        string insertQuery;
                        Boolean NotesFilled = false;

                        //If Notes is filled out, include it in the insert
                        if (!string.IsNullOrWhiteSpace(commrtb.Text))
                        {
                            NotesFilled = true;
                            insertQuery = @"INSERT INTO Student (FirstName, LastName, StudentID, StartingSemester, Notes, SchoolYear, DegreePlanID, StudentStatus)
                                   VALUES (@FirstName, @LastName, @StudentID, @StartingSemester, @Notes, @SchoolYear, @DegreePlanID, @StudentStatus)";
                        }
                        else
                        {
                            NotesFilled = false;
                            insertQuery = @"INSERT INTO Student (FirstName, LastName, StudentID, StartingSemester, Notes, SchoolYear, DegreePlanID, StudentStatus)
                                   VALUES (@FirstName, @LastName, @StudentID, @StartingSemester, @Notes, @SchoolYear, @DegreePlanID, @StudentStatus)";
                        }


                        using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@FirstName", stdFNametb.Text);
                            cmd.Parameters.AddWithValue("@LastName", stdLNametb.Text);
                            cmd.Parameters.AddWithValue("@StudentID", stdIdtb.Text);
                            cmd.Parameters.AddWithValue("@SchoolYear", "2024");
                            cmd.Parameters.AddWithValue("@DegreePlanID", 1); // Default DegreePlanID
                            cmd.Parameters.AddWithValue("@StudentStatus", 1); // Default StudentStatus
                            cmd.Parameters.AddWithValue("@Notes", commrtb.Text);
                            // Determine boolean value based on selected index
                            // 0 = Fall (false), 1 = Spring (true)
                            bool semesterValue = semcmbo.SelectedIndex == 1;

                            cmd.Parameters.AddWithValue("@StartingSemester", semesterValue);


                            cmd.ExecuteNonQuery();
                        }

                        conn.Close();
                    }


                    // After adding the student, navigate back to the MainPage
                    var mainMenu = this.FindForm() as MainMenu;
                    if (mainMenu != null)
                    {
                        mainMenu.LoadPage(new MainPage());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding course: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            // If 'No', simply do nothing (stay on the page)
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            // Display a confirmation dialog
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
                    mainMenu.LoadPage(new MainPage());
                }
            }
            // If 'No', simply do nothing (stay on the page)

        }

        private void stdFNametb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (like backspace), letters, and whitespace
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void stdLNametb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (like backspace), letters, and whitespace
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void stdIdtb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (like backspace) and numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }
    }
}
