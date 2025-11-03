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

                    List<int> selectedStatuses = new List<int>();

                    if (ckbWaiting.Checked) selectedStatuses.Add(0);
                    if (ckbInactive.Checked) selectedStatuses.Add(1);
                    if (ckbActive.Checked) selectedStatuses.Add(2);
                    if (ckbGraduated.Checked) selectedStatuses.Add(3);

                    if (selectedStatuses.Count > 0)
                    {
                        // Create something like "AND (StudentStatus = 0 OR StudentStatus = 2 OR StudentStatus = 3)"
                        string statusConditions = string.Join(" OR ", selectedStatuses.Select((s, i) => $"StudentStatus = @status{i}"));
                        query += " AND (" + statusConditions + ")";

                        // Add parameters for each checked box
                        for (int i = 0; i < selectedStatuses.Count; i++)
                        {
                            parameters.Add(new SQLiteParameter($"@status{i}", selectedStatuses[i]));
                        }
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
                DialogResult result = MessageBox.Show(
                    "Are you sure you wish to edit this Student?",
                    "Confirm Edit",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    // Get selected row
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    int studentID = Convert.ToInt32(row.Cells["StudentID"].Value);

                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                        {
                            conn.Open();

                            // Base query
                            string query = "SELECT StudentID, FirstName, LastName, StartingSemester, Notes, StudentStatus " +
                                           "FROM Student Where StudentID = @StudentID";


                            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@StudentID", studentID);

                                using (SQLiteDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        int StudentID = Convert.ToInt32(reader["StudentID"]);
                                        string FirstName = reader["FirstName"]?.ToString() ?? "";
                                        string LastName = reader["LastName"]?.ToString() ?? "";
                                        bool StartingSemester = Convert.ToBoolean(reader["StartingSemester"]);
                                        string Notes = reader["Notes"]?.ToString() ?? "";
                                        int StudentStatus = Convert.ToInt32(reader["StudentStatus"]);

                                        // Navigate to EditStudent
                                        var mainMenu = this.FindForm() as MainMenu;
                                        if (mainMenu != null)
                                        {
                                            mainMenu.LoadPage(new EditStudent(StudentID, FirstName, LastName, StartingSemester, Notes, StudentStatus));
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Student not found.", "Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                       
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error searching students: " + ex.Message, "Database Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
