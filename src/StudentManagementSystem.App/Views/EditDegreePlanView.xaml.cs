using StudentManagementSystem.App.ViewModels;
using StudentManagementSystem.Domain;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

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

        // XAML expects this exact handler name/signature for MouseDoubleClick.
        // Forward to your existing async Task helper so existing logic is reused.
        private async void AssignedClassesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await AssignedClassesDataGrid_MouseDoubleClickAsync(sender, e);
        }

        // XAML expects this exact handler name/signature for SelectionChanged.
        // Keep ViewModel.SelectedSemesterClass in sync when the grid selection changes.
        private void AssignedClassesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg == null) return;

            var vm = DataContext as EditDegreePlanViewModel;
            if (vm == null) return;

            vm.SelectedSemesterClass = dg.SelectedItem as SemesterClass;
        }

        // Existing helper (unchanged) — invoked by the public handler above
        private async Task AssignedClassesDataGrid_MouseDoubleClickAsync(object sender, MouseButtonEventArgs e)
        {
            var dg = sender as DataGrid;
            if (dg == null) return;

            var selected = dg.SelectedItem as SemesterClass;
            if (selected == null) return;

            // Show grade dialog
            var dlg = new GradeDialog(selected.Grade);
            dlg.Owner = Window.GetWindow(this);
            if (dlg.ShowDialog() == true)
            {
                var vm = DataContext as EditDegreePlanViewModel;
                if (vm == null) return;

                // Call the ViewModel's public update method to persist the grade and refresh the UI
                await vm.UpdateGradeAsync(selected, dlg.SelectedGrade);
            }
        }
    }
}