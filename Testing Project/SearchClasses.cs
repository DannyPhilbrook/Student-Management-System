using OfficeOpenXml;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace Testing_Project
{
    public partial class SearchClasses : UserControl
    {
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
                // Set the ExcelPackage.LicenseContext for EPPlus
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or Commercial if applicable

                using (var package = new ExcelPackage(new FileInfo("packages/DegreePlan.xlsx")))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet

                    DataTable dt = new DataTable();

                    // Add columns to the DataTable based on Excel headers
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        dt.Columns.Add(worksheet.Cells[1, col].Text);
                    }

                    // Populate the DataTable with data from Excel
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++) // Start from row 2 to skip headers
                    {
                        DataRow newRow = dt.Rows.Add();
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            newRow[col - 1] = worksheet.Cells[row, col].Text;
                        }
                    }

                    dataGridView1.DataSource = dt; // Set the DataTable as the DataGridView's data source
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Excel data: " + ex.Message);
            }
        }
    }
}
