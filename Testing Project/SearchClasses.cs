using System.Data.SQLite;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Testing_Project
{
    public partial class SearchClasses : UserControl
    {
        private string dbPath = "Data Source=Database/stdmngsys.db;Version=3;";
        public SearchClasses()
        {
            InitializeComponent();
        }

        private void menubtn_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = this.FindForm() as MainMenu;
            if (mainMenu != null)
            {
                mainMenu.LoadPage(new MainPage());
            }
        }

        private void btnMkClass_Click(object sender, EventArgs e)
        {
            MainMenu mainMenu = this.FindForm() as MainMenu;
            if (mainMenu != null)
            {
                mainMenu.LoadPage(new NewClass());
            }
        }

        private void NameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (like backspace), letters, and whitespace
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void LabelTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (like backspace), letters, and whitespace
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void NumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control characters (like backspace) and digits
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ignore the input
            }
        }

        private void SearchClass_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(dbPath))
                {
                    conn.Open();

                    // Base query
                    string query = "SELECT ClassID, CourseNumber, ClassName, Label, " +
                                   "CASE WHEN Semester = 1 THEN 'Spring' ELSE 'Fall' END AS Semester " +
                                   "FROM Classes WHERE 1=1";



                    // Build filters dynamically
                    List<SQLiteParameter> parameters = new List<SQLiteParameter>();

                    if (!string.IsNullOrWhiteSpace(NameTextBox.Text))
                    {
                        query += " AND LOWER(ClassName) LIKE @name";
                        parameters.Add(new SQLiteParameter("@name", "%" + NameTextBox.Text.Trim().ToLower() + "%"));
                    }

                    if (!string.IsNullOrWhiteSpace(LabelTextBox.Text))
                    {
                        query += " AND LOWER(Label) LIKE @label";
                        parameters.Add(new SQLiteParameter("@label", "%" + LabelTextBox.Text.Trim().ToLower() + "%"));
                    }

                    if (!string.IsNullOrWhiteSpace(NumberTextBox.Text))
                    {
                        query += " AND CourseNumber LIKE @number";
                        parameters.Add(new SQLiteParameter("@number", "%" + NumberTextBox.Text.Trim() + "%"));
                    }

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());

                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt;
                        // Hide the ClassID column but keep it accessible in code
                        if (dataGridView1.Columns.Contains("ClassID"))
                        {
                            dataGridView1.Columns["ClassID"].Visible = false;
                        }

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

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Ensure user clicked a valid row (not header)
            if (e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you wish to edit this class?",
                    "Confirm Edit",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    // Get selected row
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // Navigate to EditClass
                    var mainMenu = this.FindForm() as MainMenu;
                    if (mainMenu != null)
                    {
                        mainMenu.LoadPage(new EditClass(Convert.ToInt32(row.Cells["ClassID"].Value),
                        row.Cells["CourseNumber"].Value.ToString(),
                        row.Cells["ClassName"].Value.ToString(),
                        row.Cells["Label"].Value.ToString(),
                        row.Cells["Semester"].Value.ToString()));
                    }
                }
            }
        }
    }
}
