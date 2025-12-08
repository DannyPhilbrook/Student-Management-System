using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StudentManagementSystem.App.ViewModels;

namespace StudentManagementSystem.App.Views
{
    public partial class EditClassView : Page
    {
        public EditClassView()
        {
            InitializeComponent();
            // Do not overwrite DataContext here; navigation code likely injects the correct VM.
        }

        private void CourseLabel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem != null)
            {
                if (DataContext is EditClassViewModel vm)
                {
                    vm.Label = comboBox.SelectedItem.ToString();
                }
            }
        }

        // Commit typed text when Enter is pressed
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

        // Commit typed text when the control loses focus
        private void CourseLabel_LostFocus(object sender, RoutedEventArgs e)
        {
            CommitComboText();
        }

        // Adds the typed label to the VM's AvailableLabels collection if missing and selects it.
        private void CommitComboText()
        {
            if (DataContext is EditClassViewModel vm)
            {
                var text = (vm.Label ?? string.Empty).Trim();
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
                    vm.Label = text;
                }
                else
                {
                    vm.Label = existing; // use existing casing
                }
            }
        }

        // Wired from XAML: prevent letters in CourseNumber TextBox (allow digits and control keys)
        private void CourseNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Text))
            {
                e.Handled = false;
                return;
            }

            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }

            e.Handled = false;
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
