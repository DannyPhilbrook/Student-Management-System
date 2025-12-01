using StudentManagementSystem.App.ViewModels;
using StudentManagementSystem.Domain;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagementSystem.App.Views
{
    public partial class EditClassView : Page
    {
        public EditClassView()
        {
            InitializeComponent();
            DataContext = new EditClassViewModel(ServiceLocator.NavigationService, ServiceLocator.CourseService);
        }

        public void OnNavigatedTo(object parameter)
        {
            // Load course data if passed as parameter
            if (parameter is Course course)
            {
                ((EditClassViewModel)DataContext).LoadCourse(course);
            }
        }
        // Prevent letters in the course number box (allow digits and control input)
        private void CourseNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsControl(e.Text, 0) && !char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
    }
}
