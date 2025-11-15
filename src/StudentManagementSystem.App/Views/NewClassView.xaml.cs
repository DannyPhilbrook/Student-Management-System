using System.Windows.Controls;
using StudentManagementSystem.App.ViewModels;

namespace StudentManagementSystem.App.Views
{
    public partial class NewClassView : Page
    {
        public NewClassView()
        {
            InitializeComponent();
            DataContext = new NewClassViewModel(ServiceLocator.NavigationService, ServiceLocator.CourseService);
        }
    }
}
