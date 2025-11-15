using System.Windows.Controls;
using StudentManagementSystem.App.ViewModels;

namespace StudentManagementSystem.App.Views
{
    public partial class SearchStudentsView : Page
    {
        public SearchStudentsView()
        {
            InitializeComponent();
            DataContext = new SearchStudentsViewModel(ServiceLocator.NavigationService, ServiceLocator.StudentService);
        }
    }
}
