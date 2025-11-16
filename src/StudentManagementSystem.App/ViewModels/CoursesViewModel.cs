using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudentManagementSystem.App.Navigation;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.App.ViewModels
{
    public class CoursesViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ICourseService _courseService;

        private ObservableCollection<Course> _courses;
        private Course _selectedCourse;
        private bool _isLoading;

        public CoursesViewModel(INavigationService navigationService, ICourseService courseService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));

            _courses = new ObservableCollection<Course>();

            // Load courses on startup
            _ = LoadCoursesAsync();
        }

        public ObservableCollection<Course> Courses
        {
            get => _courses;
            set
            {
                _courses = value;
                OnPropertyChanged();
            }
        }

        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                OnPropertyChanged();
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

        public ICommand LoadCoursesCommand => new RelayCommand(async () => await LoadCoursesAsync());

        public ICommand NewCourseCommand => new RelayCommand(() =>
        {
            _navigationService.NavigateTo<NewClassViewModel>();
        });

        public ICommand EditCourseCommand => new RelayCommand<Course>(course =>
        {
            if (course != null)
            {
                // TODO: Navigate with parameter
                System.Diagnostics.Debug.WriteLine($"Edit course: {course.DisplayText}");
                // _navigationService.NavigateTo<EditClassViewModel>(course);
            }
        });

        public ICommand DeleteCourseCommand => new RelayCommand<Course>(async course =>
        {
            if (course != null)
            {
                await DeleteCourseAsync(course);
            }
        });

        public ICommand BackCommand => new RelayCommand(() =>
        {
            _navigationService.GoBack();
        });

        private async Task LoadCoursesAsync()
        {
            try
            {
                IsLoading = true;
                var results = await _courseService.GetAllCoursesAsync();

                Courses.Clear();
                foreach (var course in results)
                {
                    Courses.Add(course);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error loading courses: {ex.Message}",
                    "Database Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteCourseAsync(Course course)
        {
            var result = System.Windows.MessageBox.Show(
                "Are you sure you want to delete this class?",
                "Confirm Delete",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result != System.Windows.MessageBoxResult.Yes)
                return;

            try
            {
                IsLoading = true;
                bool success = await _courseService.DeleteCourseAsync(course.ClassID);

                if (success)
                {
                    Courses.Remove(course);
                    System.Windows.MessageBox.Show(
                        "Class deleted successfully!",
                        "Success",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error deleting class: {ex.Message}",
                    "Database Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
