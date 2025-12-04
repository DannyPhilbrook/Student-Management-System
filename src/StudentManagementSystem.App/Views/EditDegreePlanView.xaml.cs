using StudentManagementSystem.App.ViewModels;
using StudentManagementSystem.Domain;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            // No longer needed - ComboBoxes removed by Danny 
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

        // Existing helper (unchanged) � invoked by the public handler above
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