using System.Windows.Controls;
using StudentManagementSystem.App.ViewModels;

namespace StudentManagementSystem.App.Views
{
    public partial class CoursesView : Page
    {
        public CoursesView()
        {
            InitializeComponent();
            DataContext = new CoursesViewModel(ServiceLocator.NavigationService, ServiceLocator.CourseService);
            
            // Load courses when view loads
            Loaded += (s, e) => ((CoursesViewModel)DataContext).LoadCoursesCommand.Execute(null);
        }
    }
}
