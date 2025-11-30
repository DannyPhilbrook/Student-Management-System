using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

        public NewClassViewModel(INavigationService navigationService, ICourseService courseService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));

            _courseNumber = string.Empty;
            _courseName = string.Empty;
            _selectedLabel = string.Empty;
            _availableLabels = new ObservableCollection<string>();

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

        public bool IsSpringSemester
        {
            get => _isSpringSemester;
            set
            {
                _isSpringSemester = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SemesterText));
            }
        }

        public string SemesterText => IsSpringSemester ? "Spring" : "Fall";

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
            if (!ValidateInput())
            {
                // TODO: Show validation error
                System.Diagnostics.Debug.WriteLine("Please fill out all required fields.");
                return;
            }

            try
            {
                IsLoading = true;

                var course = new Course(CourseNumber, CourseName, SelectedLabel, IsSpringSemester);
                await _courseService.AddCourseAsync(course);

                // TODO: Show success message
                System.Diagnostics.Debug.WriteLine($"Course {course.DisplayText} added successfully!");
                _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                // TODO: Show error dialog
                System.Diagnostics.Debug.WriteLine($"Error adding course: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        });

        public ICommand CancelCommand => new RelayCommand(() =>
        {
            _navigationService.GoBack();
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
