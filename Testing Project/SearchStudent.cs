using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testing_Project
{
    public partial class SearchStudent : UserControl
    {
        private string dbPath = "Data Source=Database/stdmngsys.db;Version=3;";
        public SearchStudent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = this.FindForm() as MainMenu;
            if (mainMenu != null)
            {
                mainMenu.LoadPage(new NewStudent());
            }
        }

        private void menubtn_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = this.FindForm() as MainMenu;
            if (mainMenu != null)
            {
                mainMenu.LoadPage(new MainPage());
            }
        }

        private void srchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    conn.Open();

                    // Base query
                    string query = "SELECT StudentID, FirstName, LastName " +
                                   "FROM Student Where 1=1";



                    // Build filters dynamically
                    List<SQLiteParameter> parameters = new List<SQLiteParameter>();

                    if (!string.IsNullOrWhiteSpace(stdntFNametb.Text))
                    {
                        query += " AND LOWER(FirstName) LIKE @name";
                        parameters.Add(new SQLiteParameter("@name", "%" + stdntFNametb.Text.Trim().ToLower() + "%"));
                    }

                    if (!string.IsNullOrWhiteSpace(stdntLNametb.Text))
                    {
                        query += " AND LOWER(LastName) LIKE @label";
                        parameters.Add(new SQLiteParameter("@label", "%" + stdntLNametb.Text.Trim().ToLower() + "%"));
                    }

                    if (!string.IsNullOrWhiteSpace(stdntIdtb.Text))
                    {
                        query += " AND StudentID LIKE @number";
                        parameters.Add(new SQLiteParameter("@number", "%" + stdntIdtb.Text.Trim() + "%"));
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());

                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt;

                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching classes: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void stdntLNametb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (like backspace), letters, and whitespace
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void stdntFNametb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (like backspace), letters, and whitespace
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void stdntIdtb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (like backspace) and numbers
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Step 1: Confirm edit intention
                DialogResult confirm = MessageBox.Show(
                    "Are you sure you want to edit the Student or the Degree Plan?",
                    "Confirm Edit",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm == DialogResult.Yes)
                {
                    // Step 2: Ask what to edit
                    DialogResult choice = MessageBox.Show(
                        "Would you like to edit the Student? (Select 'No' to edit the Degree Plan)",
                        "Choose Edit Target",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    var mainMenu = this.FindForm() as MainMenu;
                    if (mainMenu != null)
                    {
                        if (choice == DialogResult.Yes)
                        {
                            // Edit Student
                            mainMenu.LoadPage(new EditStudent(
                                Convert.ToInt32(row.Cells["StudentID"].Value),
                                row.Cells["FirstName"].Value.ToString(),
                                row.Cells["LastName"].Value.ToString()
                            ));
                        }
                        else if (choice == DialogResult.No)
                        {
                            // ✅ Edit Degree Plan — fetch it dynamically
                            int studentId = Convert.ToInt32(row.Cells["StudentID"].Value);
                            int degreePlanId = GetDegreePlanIdByStudentId(studentId);

                            if (degreePlanId != -1)
                            {
                                mainMenu.LoadPage(new EditDegreePlan(degreePlanId));
                            }
                            else
                            {
                                MessageBox.Show(
                                    "This student does not have a Degree Plan.",
                                    "No Degree Plan Found",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                            }
                        }
                    }
                }
            }
        }

        // Helper function to get DegreePlanID by StudentID
        private int GetDegreePlanIdByStudentId(int studentId)
        {
            int degreePlanId = -1;

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(
                    "SELECT DegreePlanID FROM DegreePlan WHERE StudentID = @StudentID", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        degreePlanId = Convert.ToInt32(result);
                }
            }

            return degreePlanId;
        }

    }
}
