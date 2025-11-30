using System.Windows.Controls;
using StudentManagementSystem.App.ViewModels;
using StudentManagementSystem.Domain;

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
    }
}
