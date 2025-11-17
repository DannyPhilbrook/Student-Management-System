using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StudentManagementSystem.App.ViewModels;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.App.Views
{
    public partial class SearchStudentsView : Page
    {
        private SearchStudentsViewModel _viewModel;

        public SearchStudentsView()
        {
            InitializeComponent();
            _viewModel = new SearchStudentsViewModel(ServiceLocator.NavigationService, ServiceLocator.StudentService);
            DataContext = _viewModel;
            Loaded += SearchStudentsView_Loaded;
        }

        private void SearchStudentsView_Loaded(object sender, RoutedEventArgs e)
        {
            // Refresh student list when view is loaded (e.g., returning from edit)
            // Only refresh if we're not on initial load (to avoid double-loading)
            if (_viewModel != null && !_viewModel.IsInitialLoad)
            {
                _viewModel.RefreshStudents();
            }
        }

        // Auto-search when status checkbox is checked/unchecked
        private void StatusCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            // The ViewModel property setter will trigger the search automatically
        }

        private void StatusCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            // The ViewModel property setter will trigger the search automatically
        }

        // Double-click to edit student
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is Student student)
            {
                var viewModel = DataContext as SearchStudentsViewModel;
                viewModel?.EditStudentCommand.Execute(student);
            }
        }
    }
}
