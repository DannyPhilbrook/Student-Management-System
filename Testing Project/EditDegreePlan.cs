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
        public EditDegreePlan(int degreePlanId)
        {
            InitializeComponent();
            LoadData(degreePlanId);
        }

        private void LoadData(int degreePlanId)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=Database/stdmngsys.db;Version=3;"))
                {
                    conn.Open();

                    //
                    // 1️⃣ Load Semesters into cmbSem
                    //
                    cmbSem.Items.Clear();
                    cmbSem.Items.Add(new { Text = "Fall", Value = false });
                    cmbSem.Items.Add(new { Text = "Spring", Value = true });
                    cmbSem.DisplayMember = "Text";
                    cmbSem.ValueMember = "Value";

                    //
                    // 2️⃣ Load distinct SchoolYears into cmbYear
                    //
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

                    //
                    // 3️⃣ Auto-select the first semester + year for this plan
                    //
                    string firstSemQuery = "SELECT Semester, SchoolYear FROM Semester WHERE DegreePlanID = @dpid ORDER BY SemesterID ASC LIMIT 1;";
                    bool? firstSemester = null;
                    string firstYear = "";

                    using (SQLiteCommand cmd = new SQLiteCommand(firstSemQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@dpid", degreePlanId);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                firstSemester = reader.GetBoolean(reader.GetOrdinal("Semester"));
                                firstYear = reader.GetString(reader.GetOrdinal("SchoolYear"));
                            }
                        }
                    }

                    if (firstSemester != null)
                    {
                        cmbSem.SelectedIndex = firstSemester.Value ? 1 : 0;
                        cmbYear.SelectedItem = firstYear;

                        //
                        // 4️⃣ Load the DataGridView for that semester
                        //
                        LoadSemesterClasses(conn, degreePlanId, firstSemester.Value, firstYear);
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
                SELECT c.CourseID, c.CourseNumber, c.CourseName, sc.Grade
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

        }

        private void btnRmvSem_Click(object sender, EventArgs e)
        {

        }

        private void btnAddClass_Click(object sender, EventArgs e)
        {

        }

        private void btnRemoveClass_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }
    }
}
