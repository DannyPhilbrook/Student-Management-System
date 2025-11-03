using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testing_Project
{
    public partial class EditStudent : UserControl
    {
        private int StudentID;
        private string dbPath = "Data Source=Database/stdmngsys.db;Version=3;";
        public EditStudent(int StudentIDInput, string FirstName, string LastName, Boolean Semester, string Notes, int StudentStatus)
        {
            InitializeComponent();

            stdFNametb.Text = FirstName;
            stdLNametb.Text = LastName;
            stdIdtb.Text = StudentIDInput.ToString();
            semcmbo.SelectedIndex = Semester ? 1 : 0; // 0 = Fall (false), 1 = Spring (true)
            commrtb.Text = Notes;

            if (StudentStatus == 0)
            {
                rdbWaiting.Checked = true;
            }
            else if (StudentStatus == 1)
            {
                rdbInactive.Checked = true;
            }
            else if (StudentStatus == 2)
            {
                rdbActive.Checked = true;
            }
            else
            {
                rdbGraduated.Checked = true;
            }

            StudentID = StudentIDInput;

        }

        private void cancelBtn_Click(object sender, EventArgs e)
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
                    mainMenu.LoadPage(new SearchStudent());
                }
            }
            // If 'No', simply do nothing (stay on the page)
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                    {
                        conn.Open();
                        string query = "DELETE FROM Student WHERE StudentID = @StudentID";
                        using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@StudentID", StudentID);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Student deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting student: {ex.Message}", "Database Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                var mainMenu = this.FindForm() as MainMenu;
                if (mainMenu != null)
                {
                    mainMenu.LoadPage(new SearchStudent());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Display a confirmation dialog
            DialogResult result = MessageBox.Show(
                "Are you sure you wish to edit this student's information?",
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

                        insertQuery = @"UPDATE Student Set FirstName=@FirstName, LastName=@LastName, StudentID=@NewStudentID, StartingSemester=@StartingSemester, Notes=@Notes, StudentStatus=@StudentStatus, SchoolYear=@SchoolYear, DegreePlanID=@DegreePlanID " +
                                   "WHERE StudentID=@OldStudentID OR (FirstName=@FirstName AND LastName=@LastName)";

                        using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@FirstName", stdFNametb.Text);
                            cmd.Parameters.AddWithValue("@LastName", stdLNametb.Text);
                            cmd.Parameters.AddWithValue("@NewStudentID", stdIdtb.Text);
                            cmd.Parameters.AddWithValue("@OldStudentID", StudentID);
                            cmd.Parameters.AddWithValue("@SchoolYear", "2024");
                            cmd.Parameters.AddWithValue("@DegreePlanID", 1); // Default DegreePlanID

                            if (rdbWaiting.Checked)
                            {
                                cmd.Parameters.AddWithValue("@StudentStatus", 0);
                            }
                            else if (rdbInactive.Checked)
                            {
                                cmd.Parameters.AddWithValue("@StudentStatus", 1);
                            }
                            else if (rdbActive.Checked)
                            {
                                cmd.Parameters.AddWithValue("@StudentStatus", 2);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@StudentStatus", 3);
                            }

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
                        mainMenu.LoadPage(new SearchStudent());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding course: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            // If 'No', simply do nothing (stay on the page)
        }
    }
}
