using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagementSystem.App.Views
{
    public partial class EditStudentView : Page
    {
        public EditStudentView()
        {
            InitializeComponent();
        }

        // Prevent letters in the School Year textbox (allow digits and control input)
        private void SchoolYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsControl(e.Text, 0) && !char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        // Prevent numbers in the FirstName textbox (allow letters, whitespace, control)
        private void FirstName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsControl(e.Text, 0) && !char.IsLetter(e.Text, 0) && !char.IsWhiteSpace(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        // Prevent numbers in the LastName textbox (allow letters, whitespace, control)
        private void LastName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsControl(e.Text, 0) && !char.IsLetter(e.Text, 0) && !char.IsWhiteSpace(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}