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

                        insertQuery = @"INSERT INTO Student (FirstName, LastName, StudentID, StartingSemester, Notes, SchoolYear, DegreePlanID, StudentStatus)
                                   VALUES (@FirstName, @LastName, @StudentID, @StartingSemester, @Notes, @SchoolYear, @DegreePlanID, @StudentStatus)";
                    

                    // Determine boolean value based on selected index
                    // 0 = Fall (false), 1 = Spring (true)
                    bool semesterValue = semcmbo.SelectedIndex == 1;

                    // STEP 1: Create a new DegreePlan
                    int degreePlanId;
                    using (SQLiteCommand cmd = new SQLiteCommand(
                        "INSERT INTO DegreePlan (StudentID, SchoolYear, StartSemester) VALUES (@sid, @year, @sem);", conn))
                    {
                        // StudentID is temporarily placeholder (since we create the student afterward)
                        cmd.Parameters.AddWithValue("@sid", 0); // Temporary.
                        cmd.Parameters.AddWithValue("@year", tbSchlYear.Text);
                        cmd.Parameters.AddWithValue("@sem", semesterValue);
                        cmd.ExecuteNonQuery();
                    }

                    // Get DegreePlanID
                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT last_insert_rowid();", conn))
                    {
                        degreePlanId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // STEP 2: Generate 4 semesters for this degree plan, and their classes.
                    GenerateSemesters(conn, degreePlanId, tbSchlYear.Text, semcmbo.SelectedIndex);
                    GenerateDefaultClasses(conn, degreePlanId, semcmbo.SelectedIndex);


                    using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn))
                    {

                        // Final Step, Create the Student in Full.
                        cmd.Parameters.AddWithValue("@FirstName", stdFNametb.Text);
                        cmd.Parameters.AddWithValue("@LastName", stdLNametb.Text);
                        cmd.Parameters.AddWithValue("@StudentID", stdIdtb.Text);
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

                        cmd.Parameters.AddWithValue("@SchoolYear", tbSchlYear);
                        cmd.Parameters.AddWithValue("@DegreePlanID", degreePlanId);
                        cmd.Parameters.AddWithValue("@StudentStatus", 1); // Default StudentStatus
                        cmd.Parameters.AddWithValue("@Notes", commrtb.Text);
                        cmd.Parameters.AddWithValue("@StartingSemester", semesterValue);


                        cmd.ExecuteNonQuery();
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT last_insert_rowid();", conn))
                    {
                        int studentPrimaryId = Convert.ToInt32(cmd.ExecuteScalar());
                        using (SQLiteCommand updateCmd = new SQLiteCommand("UPDATE DegreePlan SET StudentID = @sid WHERE DegreePlanID = @dpid;", conn))
                        {
                            updateCmd.Parameters.AddWithValue("@sid", studentPrimaryId);
                            updateCmd.Parameters.AddWithValue("@dpid", degreePlanId);
                            updateCmd.ExecuteNonQuery();
                        }
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


        private void GenerateSemesters(SQLiteConnection conn, int degreePlanId, string startYear, int startSemester)
        {
            // Convert start year string to an integer (e.g., "2025" → 2025)
            int currentYear = int.Parse(startYear);
            int currentSemester = startSemester; // 0 = Fall, 1 = Spring

            for (int i = 0; i < 4; i++)
            {
                string insertSemester = "INSERT INTO Semester (DegreePlanID, Semester, SchoolYear) VALUES (@dpid, @sem, @year)";
                using (SQLiteCommand cmd = new SQLiteCommand(insertSemester, conn))
                {
                    cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                    bool semester = currentSemester == 1;
                    cmd.Parameters.AddWithValue("@sem", semester);
                    cmd.Parameters.AddWithValue("@year", $"{currentYear}");
                    cmd.ExecuteNonQuery();
                }

                // Toggle semester: 0 ↔ 1
                currentSemester = (currentSemester == 0) ? 1 : 0;

                // Increment year *after* Fall (0) → Spring (1) transition
                if (currentSemester == 1)
                    currentYear++;
            }
        }

        private void GenerateDefaultClasses(SQLiteConnection conn, int degreePlanId, int startSemester)
        {
            // startSemester: 0 = Fall, 1 = Sprin

            // Adjust order depending on starting semester
            List<(bool semester, int yearOffset, List<(string Label, int CourseNumber, string ClassName)> classes)> semesterCurriculum;

            if (startSemester == 1) // if Spring start
            {
                semesterCurriculum = new List<(bool semester, int yearOffset, List<(string Label, int CourseNumber, string ClassName)> classes)>
                {
                    (true, 0, new List<(string,int,string)> { // SPRING
                        ("CSCI", 5348, "Digital Forensics"),
                        ("CSCI", 5360, "Computer Networking")
                    }),
                    (false, 0, new List<(string,int,string)> { // FALL
                        ("CSCI", 5312, "Web Security"),
                        ("CSCI", 5322, "Cryptography"),
                        ("CSCI", 5313, "Software Dev Principles")
                    }),
                    (true, 1, new List<(string,int,string)> { // SPRING
                        ("CSCI", 5345, "Malware Analysis"),
                        ("CSCI", 5347, "Cyber Security Concepts"),
                        ("CSCI", 5363, "Computer Net and Dist Systems")
                    }),
                    (false, 1, new List<(string,int,string)> { // FALL
                        ("CSCI", 5362, "Penetration Testing"),
                        ("CSCI", 5320, "Database Management Systems")
                    }),
                };
            }
            else // if Fall start
            {
                semesterCurriculum = new List<(bool semester, int yearOffset, List<(string Label, int CourseNumber, string ClassName)> classes)>
                {
                    (false, 0, new List<(string,int,string)> { // FALL
                        ("CSCI", 5312, "Web Security"),
                        ("CSCI", 5322, "Cryptography"),
                        ("CSCI", 5313, "Software Dev Principles")
                    }),
                    (true, 1, new List<(string,int,string)> { // SPRING
                        ("CSCI", 5348, "Digital Forensics"),
                        ("CSCI", 5360, "Computer Networking")
                    }),
                    (false, 1, new List<(string,int,string)> { // FALL
                        ("CSCI", 5362, "Penetration Testing"),
                        ("CSCI", 5320, "Database Management Systems")
                    }),
                    (true, 2, new List<(string,int,string)> { // SPRING
                        ("CSCI", 5345, "Malware Analysis"),
                        ("CSCI", 5347, "Cyber Security Concepts"),
                        ("CSCI", 5363, "Computer Net and Dist Systems")
                    }),
                };
            }

            // Get all semesters for this degree plan
            var semesterQuery = "SELECT SemesterID, Semester, SchoolYear FROM Semester WHERE DegreePlanID = @dpid ORDER BY SchoolYear, Semester;";
            var semesterMap = new List<(int SemesterID, bool Semester, string Year)>();
            using (var cmd = new SQLiteCommand(semesterQuery, conn))
            {
                cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        semesterMap.Add((reader.GetInt32(0), reader.GetBoolean(1), reader.GetString(2)));
                    }
                }
            }

            // Insert classes per semester
            foreach (var sem in semesterMap)
            {
                int baseYear = int.Parse(semesterMap.First().Year);
                int offset = int.Parse(sem.Year) - baseYear;

                var match = semesterCurriculum.FirstOrDefault(d => d.semester == sem.Semester && d.yearOffset == offset);
                if (match.classes != null)
                {
                    foreach (var c in match.classes)
                    {
                        // Find ClassID from Classes table
                        int classId;
                        using (var cmd = new SQLiteCommand("SELECT ClassID FROM Classes WHERE Label=@lbl AND CourseNumber=@num;", conn))
                        {
                            cmd.Parameters.AddWithValue("@lbl", c.Label);
                            cmd.Parameters.AddWithValue("@num", c.CourseNumber);
                            var result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                // If not found, insert new class
                                using (var insert = new SQLiteCommand("INSERT INTO Classes (CourseNumber, ClassName, Label, Semester) VALUES (@num, @name, @lbl, @sem);", conn))
                                {
                                    insert.Parameters.AddWithValue("@num", c.CourseNumber);
                                    insert.Parameters.AddWithValue("@name", c.ClassName);
                                    insert.Parameters.AddWithValue("@lbl", c.Label);
                                    insert.Parameters.AddWithValue("@sem", sem.Semester);
                                    insert.ExecuteNonQuery();
                                }
                                classId = Convert.ToInt32(new SQLiteCommand("SELECT last_insert_rowid();", conn).ExecuteScalar());
                            }
                            else
                            {
                                classId = Convert.ToInt32(result);
                            }
                        }

                        // Link class to semester
                        using (var cmd = new SQLiteCommand("INSERT INTO SemesterClass (SemesterID, ClassID) VALUES (@sid, @cid);", conn))
                        {
                            cmd.Parameters.AddWithValue("@sid", sem.SemesterID);
                            cmd.Parameters.AddWithValue("@cid", classId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
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

        private void tbSchlYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (like backspace) and numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void NewStudent_Load(object sender, EventArgs e)
        {
            rdbWaiting.Checked = true;
        }
    }
}
