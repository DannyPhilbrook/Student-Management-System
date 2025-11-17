using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using StudentManagementSystem.App.ViewModels;

namespace StudentManagementSystem.App.Views
{
    public partial class NewStudentView : Page
    {
        public NewStudentView()
        {
            InitializeComponent();
            DataContext = new NewStudentViewModel(ServiceLocator.NavigationService, ServiceLocator.StudentService);
        }

        // Validation: Allow only letters and whitespace for names
        private void Name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow control characters (like backspace), letters, and whitespace
            if (!char.IsControl(e.Text, 0) && !char.IsLetter(e.Text, 0) && !char.IsWhiteSpace(e.Text, 0))
            {
                e.Handled = true; // Ignore the input
            }
        }

        // Validation: Allow only digits for StudentID
        private void StudentId_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow control characters (like backspace) and numbers
            if (!char.IsControl(e.Text, 0) && !char.IsDigit(e.Text, 0))
            {
                e.Handled = true; // Ignore the input
            }
        }

        // Validation: Allow only digits for SchoolYear
        private void SchoolYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow control characters (like backspace) and numbers
            if (!char.IsControl(e.Text, 0) && !char.IsDigit(e.Text, 0))
            {
                e.Handled = true; // Ignore the input
            }
        }
    }
}
