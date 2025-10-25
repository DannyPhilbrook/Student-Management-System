using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Testing_Project
{
    public partial class NewClass : UserControl
    {
        private string dbPath = "Data Source=Database/stdmngsys.db;Version=3;";

        public NewClass()
        {
            InitializeComponent();
            LoadCourseLabels();
        }

        private void LoadCourseLabels()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    conn.Open();

                    string query = "SELECT DISTINCT Label FROM Classes ORDER BY Label ASC;";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        cmbCourseLabel.Items.Clear(); // clear existing items

                        while (reader.Read())
                        {
                            cmbCourseLabel.Items.Add(reader["Label"].ToString());
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading course labels: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                // Basic validation, kicks us out if yes
                if (string.IsNullOrWhiteSpace(tbCourseNum.Text) ||
                    string.IsNullOrWhiteSpace(tbClassName.Text) ||
                    string.IsNullOrWhiteSpace(cmbCourseLabel.Text) ||
                    cmbSemester.SelectedItem == null)
                {
                    MessageBox.Show("Please fill out all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // SQL WILL GO HERE TO ADD STUDENT TO DATABASE LATER.
                try
                {
                    // SQLite connection string
                    using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                    {
                        conn.Open();

                        string insertQuery = @"INSERT INTO Classes (CourseNumber, ClassName, Label, Semester)
                                   VALUES (@CourseNumber, @ClassName, @Label, @Semester)";

                        using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@CourseNumber", tbCourseNum.Text);
                            cmd.Parameters.AddWithValue("@ClassName", tbClassName.Text);
                            cmd.Parameters.AddWithValue("@Label", cmbCourseLabel.Text);
                            // Determine boolean value based on selected index
                            // 0 = Fall (false), 1 = Spring (true)
                            bool semesterValue = cmbSemester.SelectedIndex == 1;

                            cmd.Parameters.AddWithValue("@Semester", semesterValue);


                            cmd.ExecuteNonQuery();
                        }

                        conn.Close();
                    }

                    MessageBox.Show("Course added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // After adding the class, navigate back to the MainPage
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
