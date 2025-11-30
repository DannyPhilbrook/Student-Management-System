using System;
using System.Windows;

namespace StudentManagementSystem.App.Views
{
    /// <summary>
    /// Interaction logic for GradeDialog.xaml
    /// </summary>
    public partial class GradeDialog : Window
    {
        public string SelectedGrade { get; private set; }

        public GradeDialog(string currentGrade = null)
        {
            InitializeComponent();

            // Pre-select current grade if provided
            if (!string.IsNullOrWhiteSpace(currentGrade))
            {
                foreach (var item in GradeCombo.Items)
                {
                    if ((item as System.Windows.Controls.ComboBoxItem)?.Content?.ToString() == currentGrade)
                    {
                        GradeCombo.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedGrade = (GradeCombo.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content?.ToString();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
