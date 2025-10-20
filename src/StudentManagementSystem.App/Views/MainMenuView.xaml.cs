using System.Windows.Controls;
using StudentManagementSystem.App.ViewModels;

namespace StudentManagementSystem.App.Views
{
    public partial class MainMenuView : Page
    {
        public MainMenuView()
        {
            InitializeComponent();
            DataContext = new MainMenuViewModel(ServiceLocator.NavigationService);
        }
    }
}
