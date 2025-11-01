using Microsoft.Office.Core;
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

namespace Testing_Project
{
    public partial class EditDegreePlan : UserControl
    {
        int degreePlanId;
        public EditDegreePlan(int degreePlanId)
        {
            InitializeComponent();
            LoadData(degreePlanId);
            this.degreePlanId = degreePlanId;
        }

        private void LoadData(int degreePlanId)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=Database/stdmngsys.db;Version=3;"))
                {
                    conn.Open();

                    // Load distinct SchoolYears into cmbYear

                    string yearQuery = "SELECT DISTINCT SchoolYear FROM Semester WHERE DegreePlanID = @dpid ORDER BY SchoolYear ASC;";
                    using (SQLiteCommand cmd = new SQLiteCommand(yearQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            cmbYear.Items.Clear();
                            while (reader.Read())
                            {
                                cmbYear.Items.Add(reader["SchoolYear"].ToString());
                            }
                        }
                    }
                    cmbYear.SelectedIndex = 0;

                    // Auto-select the first available year

                    string firstYear = cmbYear.Items.Count > 0 ? cmbYear.Items[0].ToString() : null;

                    // Load only semesters that exist for the selected year
                    cmbSem.Items.Clear();
                    string semQuery = "SELECT Semester FROM Semester WHERE DegreePlanID = @dpid AND SchoolYear = @year ORDER BY SemesterID ASC;";

                    using (SQLiteCommand cmd = new SQLiteCommand(semQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        cmd.Parameters.AddWithValue("@year", firstYear);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bool semValue = reader.GetBoolean(reader.GetOrdinal("Semester"));
                                string semText = semValue ? "Spring" : "Fall";
                                cmbSem.Items.Add(new { Text = semText, Value = semValue });
                            }
                        }
                    }

                    cmbSem.DisplayMember = "Text";
                    cmbSem.ValueMember = "Value";

                    // Auto-select the first semester for that year

                    if (cmbSem.Items.Count > 0)
                        cmbSem.SelectedIndex = 0;

                    // Load Classes for the selected semester

                    if (cmbSem.SelectedItem != null && firstYear != null)
                    {
                        bool selectedSemester = string.Equals(cmbSem.Text, "Spring", StringComparison.OrdinalIgnoreCase);

                        string classQuery = @"
                            SELECT c.ClassID, c.CourseNumber, c.ClassName
                            FROM Classes c
                            WHERE c.Semester = @sem
                              AND c.ClassID NOT IN (
                                  SELECT sc.ClassID
                                  FROM SemesterClass sc
                                  JOIN Semester s ON sc.SemesterID = s.SemesterID
                                  WHERE s.DegreePlanID = @dpid
                              )
                            ORDER BY c.CourseNumber ASC;";

                        using (SQLiteCommand cmd = new SQLiteCommand(classQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                            cmd.Parameters.AddWithValue("@sem", selectedSemester);

                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                cmbClass.Items.Clear();
                                while (reader.Read())
                                {
                                    int classId = Convert.ToInt32(reader["ClassID"]);
                                    string courseNum = reader["CourseNumber"].ToString();
                                    string className = reader["ClassName"].ToString();
                                    string display = $"{courseNum} – {className}";

                                    cmbClass.Items.Add(new { Text = display, Value = classId });
                                }

                                cmbClass.DisplayMember = "Text";
                                cmbClass.ValueMember = "Value";
                            }
                        }
                        // Load the DataGridView for that semester

                        LoadSemesterClasses(conn, degreePlanId, selectedSemester, firstYear);
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadSemesterClasses(SQLiteConnection conn, int degreePlanId, bool semester, string schoolYear)
        {
            try
            {
                string query = @"
                SELECT c.CourseNumber, c.ClassName, sc.Grade
                FROM SemesterClass sc
                JOIN Semester s ON sc.SemesterID = s.SemesterID
                JOIN Classes c ON sc.ClassID = c.ClassID
                WHERE s.DegreePlanID = @dpid AND s.Semester = @sem AND s.SchoolYear = @year;";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                    cmd.Parameters.AddWithValue("@sem", semester);
                    cmd.Parameters.AddWithValue("@year", schoolYear);

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvSemester.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading semester classes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnAddSemester_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to add a new semester?",
                "Confirm Add Semester",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=Database/stdmngsys.db;Version=3;"))
                {
                    conn.Open();

                    // Find the most recent semester for this Degree Plan
                    string lastSemQuery = @"
                        SELECT Semester, SchoolYear
                        FROM Semester
                        WHERE DegreePlanID = @dpid
                        ORDER BY SchoolYear DESC, Semester ASC
                        LIMIT 1;";

                    bool? lastSemester = null;
                    int lastYear = 0;

                    using (SQLiteCommand cmd = new SQLiteCommand(lastSemQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lastSemester = reader.GetBoolean(reader.GetOrdinal("Semester"));
                                lastYear = int.Parse(reader.GetString(reader.GetOrdinal("SchoolYear")));
                            }
                        }
                    }

                    // Determine next semester/year
                    bool newSemester;
                    int newYear;

                    if (lastSemester == null)
                    {
                        // No semester exists — default to Fall of current year
                        newSemester = false;
                        newYear = DateTime.Now.Year;
                    }
                    else if (lastSemester == true)
                    {
                        // Last was Spring → new one is Fall of same year
                        newSemester = false;
                        newYear = lastYear;
                    }
                    else
                    {
                        // Last was Fall → new one is Spring of next year
                        newSemester = true;
                        newYear = lastYear + 1;
                    }

                    // Insert new semester
                    string insertQuery = @"
                        INSERT INTO Semester (DegreePlanID, Semester, SchoolYear)
                        VALUES (@dpid, @sem, @year);";

                    using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        cmd.Parameters.AddWithValue("@sem", newSemester);
                        cmd.Parameters.AddWithValue("@year", newYear.ToString());
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Added new semester: {(newSemester ? "Spring" : "Fall")} {newYear}",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh data
                    LoadData(degreePlanId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding semester: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnRmvSem_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to remove the most recent semester?\nThis will also remove all classes within it.",
                "Confirm Remove Semester",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=Database/stdmngsys.db;Version=3;"))
                {
                    conn.Open();

                    // Count semesters for this Degree Plan
                    string countQuery = "SELECT COUNT(*) FROM Semester WHERE DegreePlanID = @dpid;";
                    int semesterCount = 0;

                    using (SQLiteCommand cmd = new SQLiteCommand(countQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        semesterCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (semesterCount <= 4)
                    {
                        MessageBox.Show("You cannot have fewer than 4 semesters in a degree plan.",
                            "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Find the most recent semester
                    string lastSemQuery = @"
                        SELECT SemesterID FROM Semester
                        WHERE DegreePlanID = @dpid
                        ORDER BY SchoolYear DESC, Semester DESC
                        LIMIT 1;";

                    int semesterIdToRemove = -1;
                    using (SQLiteCommand cmd = new SQLiteCommand(lastSemQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            semesterIdToRemove = Convert.ToInt32(result);
                    }

                    if (semesterIdToRemove == -1)
                    {
                        MessageBox.Show("No semester found to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Remove related semester classes first
                    string deleteClassesQuery = "DELETE FROM SemesterClass WHERE SemesterID = @sid;";
                    using (SQLiteCommand cmd = new SQLiteCommand(deleteClassesQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", semesterIdToRemove);
                        cmd.ExecuteNonQuery();
                    }

                    // Remove semester
                    string deleteSemesterQuery = "DELETE FROM Semester WHERE SemesterID = @sid;";
                    using (SQLiteCommand cmd = new SQLiteCommand(deleteSemesterQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", semesterIdToRemove);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Semester and its classes removed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh data
                    LoadData(degreePlanId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing semester: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnAddClass_Click(object sender, EventArgs e)
        {
            // Validate selection
            if (cmbClass.SelectedItem == null)
            {
                MessageBox.Show("Please select a class to add.", "No Class Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbSem.SelectedIndex < 0 || cmbYear.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a semester and year first.", "Missing Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm
            DialogResult confirm = MessageBox.Show(
                "Add the selected class to this semester?",
                "Confirm Add Class",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                // get selected class id
                int classId = Convert.ToInt32(((dynamic)cmbClass.SelectedItem).Value);
                bool selectedSemester = string.Equals(cmbSem.Text, "Spring", StringComparison.OrdinalIgnoreCase);
                string selectedYear = cmbYear.SelectedItem.ToString();

                using (SQLiteConnection conn = new SQLiteConnection("Data Source=Database/stdmngsys.db;Version=3;"))
                {
                    conn.Open();

                    // Find SemesterID for this degreePlanId + semester + year
                    string semIdQuery = @"
                        SELECT SemesterID
                        FROM Semester
                        WHERE DegreePlanID = @dpid AND Semester = @sem AND SchoolYear = @year
                        LIMIT 1";
                    object semIdObj;
                    using (var cmd = new SQLiteCommand(semIdQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        cmd.Parameters.AddWithValue("@sem", selectedSemester);
                        cmd.Parameters.AddWithValue("@year", selectedYear);
                        semIdObj = cmd.ExecuteScalar();
                    }

                    if (semIdObj == null)
                    {
                        MessageBox.Show("Target semester not found. Please make sure the semester/year exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int semesterId = Convert.ToInt32(semIdObj);

                    // Ensure the class isn't already assigned to this degree plan (safety)
                    string existsQuery = @"
                        SELECT 1
                        FROM SemesterClass sc
                        JOIN Semester s ON sc.SemesterID = s.SemesterID
                        WHERE sc.ClassID = @cid AND s.DegreePlanID = @dpid";
                    using (var check = new SQLiteCommand(existsQuery, conn))
                    {
                        check.Parameters.AddWithValue("@cid", classId);
                        check.Parameters.AddWithValue("@dpid", degreePlanId);
                        var exists = check.ExecuteScalar();
                        if (exists != null)
                        {
                            MessageBox.Show("This class is already assigned to the degree plan.", "Already Assigned", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    // Insert into SemesterClass
                    string insertQuery = "INSERT INTO SemesterClass (SemesterID, ClassID) VALUES (@sid, @cid);";
                    using (var insert = new SQLiteCommand(insertQuery, conn))
                    {
                        insert.Parameters.AddWithValue("@sid", semesterId);
                        insert.Parameters.AddWithValue("@cid", classId);
                        insert.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                // Refresh UI
                UpdateSemesterAndClasses();
                MessageBox.Show("Class added to semester.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding class: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveClass_Click(object sender, EventArgs e)
        {
            // Validate selection
            if (dgvSemester.CurrentRow == null)
            {
                MessageBox.Show("Please select a class row in the semester grid to remove.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbSem.SelectedIndex < 0 || cmbYear.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a semester and year first.", "Missing Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Grab identifying values from selected row
            string courseNumber = dgvSemester.CurrentRow.Cells["CourseNumber"].Value?.ToString();
            string className = dgvSemester.CurrentRow.Cells["ClassName"].Value?.ToString();
            if (string.IsNullOrEmpty(courseNumber) || string.IsNullOrEmpty(className))
            {
                MessageBox.Show("Selected row does not contain valid class identifiers.", "Invalid Row", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Remove {courseNumber} - {className} from this semester?",
                "Confirm Remove Class",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                bool selectedSemester = string.Equals(cmbSem.Text, "Spring", StringComparison.OrdinalIgnoreCase);
                string selectedYear = cmbYear.SelectedItem.ToString();

                using (SQLiteConnection conn = new SQLiteConnection("Data Source=Database/stdmngsys.db;Version=3;"))
                {
                    conn.Open();

                    // Find the SemesterClassID for this degree plan + semester + year + class identifiers
                    string findSemClassQuery = @"
                        SELECT sc.SemesterClassID
                        FROM SemesterClass sc
                        JOIN Semester s ON sc.SemesterID = s.SemesterID
                        JOIN Classes c ON sc.ClassID = c.ClassID
                        WHERE s.DegreePlanID = @dpid
                          AND s.Semester = @sem
                          AND s.SchoolYear = @year
                          AND c.CourseNumber = @cnum
                          AND c.ClassName = @cname
                        LIMIT 1;";

                    object semClassIdObj;
                    using (var cmd = new SQLiteCommand(findSemClassQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        cmd.Parameters.AddWithValue("@sem", selectedSemester);
                        cmd.Parameters.AddWithValue("@year", selectedYear);
                        cmd.Parameters.AddWithValue("@cnum", courseNumber);
                        cmd.Parameters.AddWithValue("@cname", className);
                        semClassIdObj = cmd.ExecuteScalar();
                    }

                    if (semClassIdObj == null)
                    {
                        MessageBox.Show("Could not locate the semester-class record to delete.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int semClassId = Convert.ToInt32(semClassIdObj);

                    // Delete the SemesterClass row
                    string deleteQuery = "DELETE FROM SemesterClass WHERE SemesterClassID = @scid;";
                    using (var del = new SQLiteCommand(deleteQuery, conn))
                    {
                        del.Parameters.AddWithValue("@scid", semClassId);
                        del.ExecuteNonQuery();
                    }

                    conn.Close();
                }

                // Refresh UI
                UpdateSemesterAndClasses();
                MessageBox.Show("Class removed from semester.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing class: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    mainMenu.LoadPage(new SearchStudent());
                }
            }
            // If 'No', simply do nothing (stay on the page)
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Confirm before leaving
            DialogResult result = MessageBox.Show(
                "Are you sure you want to finish editing and return to the search page?",
                "Finish Editing",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Return to Search Student page
                var mainMenu = this.FindForm() as MainMenu;
                if (mainMenu != null)
                {
                    mainMenu.LoadPage(new SearchStudent());
                }
            }
        }

        private void cmbSem_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSemesterAndClasses();
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSemesterAndClasses();
            cmbSem.Items.Clear();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=Database/stdmngsys.db;Version=3;"))
                {
                    conn.Open();

                    string semQuery = "SELECT Semester FROM Semester WHERE DegreePlanID = @dpid AND SchoolYear = @year ORDER BY SemesterID ASC;";

                    using (SQLiteCommand cmd = new SQLiteCommand(semQuery, conn)) // Use existing open connection
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        cmd.Parameters.AddWithValue("@year", cmbYear.SelectedItem.ToString());

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bool semValue = reader.GetBoolean(reader.GetOrdinal("Semester"));
                                string semText = semValue ? "Spring" : "Fall";
                                cmbSem.Items.Add(new { Text = semText, Value = semValue });
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (cmbSem.Items.Count > 0)
                cmbSem.SelectedIndex = 0;
        }

        private void UpdateSemesterAndClasses()
        {
            if (cmbSem.SelectedIndex < 0 || cmbYear.SelectedIndex < 0)
                return; // wait until both are selected

            bool selectedSemester = string.Equals(cmbSem.Text, "Spring", StringComparison.OrdinalIgnoreCase); ; // Fall = 0, Spring = 1
            Console.Write(string.Equals(cmbSem.Text, "Spring", StringComparison.OrdinalIgnoreCase));
            string selectedYear = cmbYear.SelectedItem.ToString();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=Database/stdmngsys.db;Version=3;"))
                {
                    conn.Open();

                    // Load the DataGridView for that semester + year
                    LoadSemesterClasses(conn, degreePlanId, selectedSemester, selectedYear);

                    // Refresh class list for this specific semester + year

                    string classQuery = @"
                            SELECT c.ClassID, c.CourseNumber, c.ClassName
                            FROM Classes c
                            WHERE c.Semester = @sem
                              AND c.ClassID NOT IN (
                                  SELECT sc.ClassID
                                  FROM SemesterClass sc
                                  JOIN Semester s ON sc.SemesterID = s.SemesterID
                                  WHERE s.DegreePlanID = @dpid
                              )
                            ORDER BY c.CourseNumber ASC;";

                    using (SQLiteCommand cmd = new SQLiteCommand(classQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        cmd.Parameters.AddWithValue("@sem", selectedSemester);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            cmbClass.Items.Clear();
                            while (reader.Read())
                            {
                                int classId = Convert.ToInt32(reader["ClassID"]);
                                string courseNum = reader["CourseNumber"].ToString();
                                string className = reader["ClassName"].ToString();
                                string display = $"{courseNum} – {className}";

                                cmbClass.Items.Add(new { Text = display, Value = classId });
                            }

                            cmbClass.DisplayMember = "Text";
                            cmbClass.ValueMember = "Value";
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSemester_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvSemester.Rows[e.RowIndex];
            string courseNumber = row.Cells["CourseNumber"].Value?.ToString();
            string className = row.Cells["ClassName"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(courseNumber) || string.IsNullOrWhiteSpace(className))
            {
                MessageBox.Show("Selected row does not contain a valid class to adjust.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                $"Adjust grade for {courseNumber} - {className}?",
                "Adjust Grade",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            // Pre-select current grade in the GraderForm
            string currentGrade = row.Cells["Grade"].Value?.ToString();

            using (var grader = new GraderForm(currentGrade))
            {
                if (grader.ShowDialog(this) != DialogResult.OK) return;

                string selectedGrade = grader.SelectedGrade;
                if (string.IsNullOrEmpty(selectedGrade)) return;

                try
                {
                    bool selectedSemester = string.Equals(cmbSem.Text, "Spring", StringComparison.OrdinalIgnoreCase);
                    string selectedYear = cmbYear.SelectedItem.ToString();

                    using (SQLiteConnection conn = new SQLiteConnection("Data Source=Database/stdmngsys.db;Version=3;"))
                    {
                        conn.Open();

                        // Find the SemesterClassID for this degree plan + semester + year + class identifiers
                        string findSemClassQuery = @"
                            SELECT sc.SemesterClassID
                            FROM SemesterClass sc
                            JOIN Semester s ON sc.SemesterID = s.SemesterID
                            JOIN Classes c ON sc.ClassID = c.ClassID
                            WHERE s.DegreePlanID = @dpid
                              AND s.Semester = @sem
                              AND s.SchoolYear = @year
                              AND c.CourseNumber = @cnum
                              AND c.ClassName = @cname
                            LIMIT 1;";

                        object semClassIdObj;
                        using (var cmd = new SQLiteCommand(findSemClassQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                            cmd.Parameters.AddWithValue("@sem", selectedSemester);
                            cmd.Parameters.AddWithValue("@year", selectedYear);
                            cmd.Parameters.AddWithValue("@cnum", courseNumber);
                            cmd.Parameters.AddWithValue("@cname", className);
                            semClassIdObj = cmd.ExecuteScalar();
                        }

                        if (semClassIdObj == null)
                        {
                            MessageBox.Show("Could not locate the semester-class record to update.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        int semClassId = Convert.ToInt32(semClassIdObj);

                        string updateQuery = "UPDATE SemesterClass SET Grade = @grade WHERE SemesterClassID = @scid;";
                        using (var updateCmd = new SQLiteCommand(updateQuery, conn))
                        {
                            updateCmd.Parameters.AddWithValue("@grade", selectedGrade);
                            updateCmd.Parameters.AddWithValue("@scid", semClassId);
                            updateCmd.ExecuteNonQuery();
                        }

                        conn.Close();
                    }

                    UpdateSemesterAndClasses();
                    MessageBox.Show("Grade updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating grade: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
