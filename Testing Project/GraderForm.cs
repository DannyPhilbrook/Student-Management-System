using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testing_Project
{
    public partial class GraderForm : Form
    {
        public string SelectedGrade { get; private set; }

        public GraderForm(string currentGrade = null)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(currentGrade))
            {
                int i = cmbGrade.Items.IndexOf(currentGrade);
                if (i >= 0) cmbGrade.SelectedIndex = i;
            }
            if (cmbGrade.SelectedIndex < 0) cmbGrade.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SelectedGrade = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedGrade = cmbGrade.SelectedItem?.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}