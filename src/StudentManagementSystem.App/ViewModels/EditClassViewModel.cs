using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using StudentManagementSystem.App.Navigation;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.App.ViewModels
{
    public class EditClassViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ICourseService _courseService;

        private int _classId;
        private string _courseNumber;
        private string _courseName;
        private string _label;
        private bool _semester;

        private bool _isLoading;
        private ObservableCollection<string> _availableLabels;

        public EditClassViewModel(INavigationService navigationService, ICourseService courseService, Course course = null)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));

            _availableLabels = new ObservableCollection<string>();

            // Load labels for dropdown
            _ = LoadLabelsAsync();

            if (course != null)
                LoadCourse(course);
        }

        public int ClassId
        {
            get => _classId;
            set
            {
                _classId = value;
                OnPropertyChanged();
            }
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

        public string Label
        {
            get => _label;
            set
            {
                _label = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedLabel));
            }
        }

        public string SelectedLabel
        {
            get => _label;
            set
            {
                if (_label != value)
                {
                    _label = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(Label));
                }
            }
        }

        public bool Semester
        {
            get => _semester;
            set
            {
                if (_semester == value) return;
                _semester = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSpringSemester));
                OnPropertyChanged(nameof(IsFallSemester));
            }
        }

        public bool IsSpringSemester
        {
            get => _semester;
            set
            {
                if (_semester == value) return;
                _semester = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Semester));
                OnPropertyChanged(nameof(IsFallSemester));
            }
        }

        public bool IsFallSemester
        {
            get => !_semester;
            set
            {
                bool newSemester = !value;
                if (_semester == newSemester) return;
                _semester = newSemester;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Semester));
                OnPropertyChanged(nameof(IsSpringSemester));
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

        public void LoadCourse(Course course)
        {
            if (course != null)
            {
                ClassId = course.ClassID;
                CourseNumber = course.CourseNumber;
                CourseName = course.CourseName;
                Label = course.Label;
                Semester = course.Semester;
            }
        }

        public ICommand SaveCommand => new RelayCommand(async () =>
        {
            if (string.IsNullOrWhiteSpace(CourseNumber) || string.IsNullOrWhiteSpace(CourseName))
            {
                return;
            }

            try
            {
                IsLoading = true;

                var course = new Course(CourseNumber, CourseName, Label, Semester)
                {
                    ClassID = ClassId
                };
                var confirm = await _courseService.UpdateCourseAsync(course);
                
                if (confirm == null)
                {
                    System.Windows.MessageBox.Show(
                        $"A course with ID {course.CourseNumber} and label {course.Label} already exists.",
                        "Duplicate Course Number",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Warning);
                    return;
                }

                _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error updating course: {ex.Message}",
                    "Database Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error updating course: {ex.Message}");
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
    }
}