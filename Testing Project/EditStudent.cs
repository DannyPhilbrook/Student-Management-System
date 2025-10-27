using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private int studentID;
        private string dbPath = "Data Source=Database/stdmngsys.db;Version=3;";
        public EditStudent(int StudentID, string FirstName, string LastName)
        {
            InitializeComponent();

            stdFNametb.Text = FirstName;
            stdLNametb.Text = LastName;
            studentID = StudentID;
        }
    }
}
