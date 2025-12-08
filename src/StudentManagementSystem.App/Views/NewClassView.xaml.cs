using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StudentManagementSystem.App.ViewModels;
using System;

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
                var viewModel = DataContext as NewClassViewModel;
                if (viewModel != null)
                {
                    viewModel.SelectedLabel = comboBox.SelectedItem.ToString();
                }
            }
        }

        // Handler wired from XAML: fires on key press while ComboBox has focus
        private void CourseLabel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CommitComboText();
                // Move focus so LostFocus handlers run and the ComboBox closes its popup (if open)
                var scope = FocusManager.GetFocusScope(this);
                FocusManager.SetFocusedElement(scope, this as IInputElement);
                e.Handled = true;
            }
        }

        // Handler wired from XAML: fires when ComboBox loses focus
        private void CourseLabel_LostFocus(object sender, RoutedEventArgs e)
        {
            CommitComboText();
        }

        // Shared commit routine: ensure typed text is added to AvailableLabels and selected
        private void CommitComboText()
        {
            if (DataContext is NewClassViewModel vm)
            {
                var text = (vm.SelectedLabel ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(text)) return;

                // Avoid duplicates (case-insensitive)
                string existing = null;
                foreach (var item in vm.AvailableLabels)
                {
                    if (string.Equals(item, text, System.StringComparison.OrdinalIgnoreCase))
                    {
                        existing = item;
                        break;
                    }
                }

                if (existing == null)
                {
                    vm.AvailableLabels.Add(text);
                    vm.SelectedLabel = text;
                }
                else
                {
                    vm.SelectedLabel = existing; // use existing casing
                }
            }
        }

        // Prevent digits from being typed into the CourseLabel ComboBox
        private void CourseLabel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Any(char.IsDigit))
            {
                e.Handled = true;
            }
        }

        // Prevent pasting digits into the CourseLabel ComboBox
        private void CourseLabel_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var pasteText = e.DataObject.GetData(DataFormats.Text) as string ?? string.Empty;
                if (pasteText.Any(char.IsDigit))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        // Prevent non-digits from being typed into CourseNumber
        private void CourseNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Any(ch => !char.IsDigit(ch)))
            {
                e.Handled = true;
            }
        }

        // Prevent pasting non-digits into CourseNumber
        private void CourseNumber_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var pasteText = e.DataObject.GetData(DataFormats.Text) as string ?? string.Empty;
                if (string.IsNullOrEmpty(pasteText) || pasteText.Any(ch => !char.IsDigit(ch)))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
