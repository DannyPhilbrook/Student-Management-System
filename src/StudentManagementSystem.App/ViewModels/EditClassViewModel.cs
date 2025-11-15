using System;
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

        public EditClassViewModel(INavigationService navigationService, ICourseService courseService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));

            _courseNumber = string.Empty;
            _courseName = string.Empty;
            _label = string.Empty;
            _semester = false;
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
            }
        }

        public bool Semester
        {
            get => _semester;
            set
            {
                _semester = value;
                OnPropertyChanged();
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
                var course = new Course(CourseNumber, CourseName, Label, Semester)
                {
                    ClassID = ClassId
                };
                await _courseService.UpdateCourseAsync(course);

                _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating course: {ex.Message}");
            }
        });

        public ICommand CancelCommand => new RelayCommand(() =>
        {
            _navigationService.GoBack();
        });
    }
}