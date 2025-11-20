using System.Windows;
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

            
        private void CourseLabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
            {
                // Update the SelectedLabel when an item is selected from the dropdown
                var viewModel = DataContext as NewClassViewModel;
                if (viewModel != null)
                {
                    viewModel.SelectedLabel = comboBox.SelectedItem.ToString();
                }
            }
        }
    }
}
