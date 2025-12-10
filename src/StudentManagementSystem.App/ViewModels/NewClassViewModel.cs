using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using StudentManagementSystem.App.Navigation;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.App.ViewModels
{
    public class NewClassViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ICourseService _courseService;

        private string _courseNumber;
        private string _courseName;
        private string _selectedLabel;
        private bool _isSpringSemester;
        private bool _isLoading;
        private ObservableCollection<string> _availableLabels;
        private string _selectedSemester;

        public List<string> SemesterOptions { get; } = new List<string> { "Fall", "Spring" };

        public NewClassViewModel(INavigationService navigationService, ICourseService courseService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));

            _courseNumber = string.Empty;
            _courseName = string.Empty;
            _selectedLabel = string.Empty;
            _availableLabels = new ObservableCollection<string>();
            _selectedSemester = "Fall"; // Default to Fall

            // Load existing labels for dropdown
            _ = LoadLabelsAsync();
        }

        public string CourseNumber
        {
            get => _courseNumber;
            set
            {
                _courseNumber = value;
                OnPropertyChanged();
            }
        }

        public string CourseName
        {
            get => _courseName;
            set
            {
                _courseName = value;
                OnPropertyChanged();
            }
        }

        public string SelectedLabel
        {
            get => _selectedLabel;
            set
            {
                _selectedLabel = value;
                OnPropertyChanged();
            }
        }

        public string SemesterText
        {
            get => _selectedSemester;
            set
            {
                if (value != null)
                {
                    _selectedSemester = value;
                    _isSpringSemester = value.Equals("Spring", StringComparison.OrdinalIgnoreCase);
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsSpringSemester));
                }
            }
        }

        public bool IsSpringSemester
        {
            get => _isSpringSemester;
            set
            {
                _isSpringSemester = value;
                _selectedSemester = value ? "Spring" : "Fall";
                OnPropertyChanged();
                OnPropertyChanged(nameof(SemesterText));
                OnPropertyChanged(nameof(IsFallSemester));
            }
        }

        public bool IsFallSemester
        {
            get => !_isSpringSemester;
            set
            {
                _isSpringSemester = !value;
                _selectedSemester = value ? "Fall" : "Spring";
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSpringSemester));
                OnPropertyChanged(nameof(SemesterText));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> AvailableLabels
        {
            get => _availableLabels;
            set
            {
                _availableLabels = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand => new RelayCommand(async () =>
        {
            var result = System.Windows.MessageBox.Show(
                "Are you sure you wish to create this Course?",
                "Confirmation",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (result != System.Windows.MessageBoxResult.Yes)
                return;

            if (!ValidateInput())
            {
                System.Windows.MessageBox.Show(
                    "Please fill out all required fields.",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                return;
            }

            try
            {
                IsLoading = true;

                var course = new Course(CourseNumber, CourseName, SelectedLabel, IsSpringSemester);
                var confirm = await _courseService.AddCourseAsync(course);

                if (confirm == null)
                {
                    System.Windows.MessageBox.Show(
                        $"A course with ID {course.CourseNumber} and class label {course.Label} already exists.",
                        "Duplicate Course Number",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Warning);
                    return;
                }

                System.Windows.MessageBox.Show(
                    "Course added successfully!",
                    "Success",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);

                _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error adding course: {ex.Message}",
                    "Database Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        });

        public ICommand CancelCommand => new RelayCommand(() =>
        {
            var result = System.Windows.MessageBox.Show(
                "Are you sure you wish to cancel creation?",
                "Warning",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Exclamation);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                _navigationService.GoBack();
            }
        });

        private async Task LoadLabelsAsync()
        {
            try
            {
                var labels = await _courseService.GetDistinctLabelsAsync();

                AvailableLabels.Clear();
                foreach (var label in labels)
                {
                    AvailableLabels.Add(label);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading labels: {ex.Message}");
            }
        }

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(CourseNumber) &&
                   !string.IsNullOrWhiteSpace(CourseName) &&
                   !string.IsNullOrWhiteSpace(SelectedLabel);
        }
    }
}
