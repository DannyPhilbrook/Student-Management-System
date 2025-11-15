using System.Windows.Controls;
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
    }
}
