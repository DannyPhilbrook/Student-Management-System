using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StudentManagementSystem.App.Views
{
    public partial class EditDegreePlanView : Page
    {
        public EditDegreePlanView()
        {
            InitializeComponent();
            this.Loaded += EditDegreePlanView_Loaded;
        }

        private void EditDegreePlanView_Loaded(object sender, RoutedEventArgs e)
        {
            // Force ComboBox bindings to refresh
            if (YearComboBox != null)
            {
                var binding = BindingOperations.GetBinding(YearComboBox, ComboBox.ItemsSourceProperty);
                if (binding != null)
                {
                    BindingOperations.SetBinding(YearComboBox, ComboBox.ItemsSourceProperty, binding);
                }
            }
            if (SemesterComboBox != null)
            {
                var binding = BindingOperations.GetBinding(SemesterComboBox, ComboBox.ItemsSourceProperty);
                if (binding != null)
                {
                    BindingOperations.SetBinding(SemesterComboBox, ComboBox.ItemsSourceProperty, binding);
                }
            }
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var viewModel = DataContext as ViewModels.EditDegreePlanViewModel;
                if (viewModel != null && comboBox.SelectedItem != null)
                {
                    // Only update if the value actually changed to avoid infinite loops
                    string newValue = comboBox.SelectedItem.ToString();
                    if (viewModel.SelectedYear != newValue)
                    {
                        viewModel.SelectedYear = newValue;
                    }
                }
            }
        }

        private void SemesterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                var viewModel = DataContext as ViewModels.EditDegreePlanViewModel;
                if (viewModel != null && comboBox.SelectedItem != null)
                {
                    // Only update if the value actually changed to avoid infinite loops
                    string newValue = comboBox.SelectedItem.ToString();
                    if (viewModel.SelectedSemesterType != newValue)
                    {
                        viewModel.SelectedSemesterType = newValue;
                    }
                }
            }
        }
    }
}