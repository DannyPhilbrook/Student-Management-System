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

        private int _id;
        private string _courseNumber;
        private string _courseName;

        public EditClassViewModel(INavigationService navigationService, ICourseService courseService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
            
            _courseNumber = string.Empty;
            _courseName = string.Empty;
        }

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
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

        public void LoadCourse(Course course)
        {
            if (course != null)
            {
                Id = course.Id;
                CourseNumber = course.CourseNumber;
                CourseName = course.CourseName;
            }
        }

        public ICommand SaveCommand => new RelayCommand(async () =>
        {
            if (string.IsNullOrWhiteSpace(CourseNumber) || string.IsNullOrWhiteSpace(CourseName))
            {
                // TODO: Show validation error message
                return;
            }

            try
            {
                var course = new Course(CourseNumber, CourseName) { Id = Id };
                await _courseService.UpdateCourseAsync(course);
                
                // TODO: Show success message
                _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                // TODO: Show error message
                System.Diagnostics.Debug.WriteLine($"Error updating course: {ex.Message}");
            }
        });

        public ICommand CancelCommand => new RelayCommand(() =>
        {
            _navigationService.GoBack();
        });
    }
}
