using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudentManagementSystem.App.Navigation;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.App.ViewModels
{
    public class SearchStudentsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IStudentService _studentService;

        private string _searchName;
        private string _searchStudentId;
        private string _searchSemester;
        private ObservableCollection<Student> _students;
        private Student _selectedStudent;
        private bool _isLoading;
        private bool _isInitialLoad = true; // Flag to prevent auto-search on initial load
        
        // Status checkboxes
        private bool _isWaitingChecked;
        private bool _isInactiveChecked;
        private bool _isActiveChecked;
        private bool _isGraduatedChecked;

        public SearchStudentsViewModel(INavigationService navigationService, IStudentService studentService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));

            _students = new ObservableCollection<Student>();
            _searchName = string.Empty;
            _searchStudentId = string.Empty;
            _searchSemester = string.Empty;

            // Load all students on startup
            _ = LoadAllStudentsAsync();
        }

        public string SearchName
        {
            get => _searchName;
            set
            {
                _searchName = value;
                OnPropertyChanged();
            }
        }

        public string SearchStudentId
        {
            get => _searchStudentId;
            set
            {
                _searchStudentId = value;
                OnPropertyChanged();
            }
        }

        public bool IsWaitingChecked
        {
            get => _isWaitingChecked;
            set
            {
                if (_isWaitingChecked != value)
                {
                    _isWaitingChecked = value;
                    OnPropertyChanged();
                    // Auto-search when checkbox changes (but not on initial load)
                    if (!_isInitialLoad)
                    {
                        _ = SearchStudentsAsync();
                    }
                }
            }
        }

        public bool IsInactiveChecked
        {
            get => _isInactiveChecked;
            set
            {
                if (_isInactiveChecked != value)
                {
                    _isInactiveChecked = value;
                    OnPropertyChanged();
                    // Auto-search when checkbox changes (but not on initial load)
                    if (!_isInitialLoad)
                    {
                        _ = SearchStudentsAsync();
                    }
                }
            }
        }

        public bool IsActiveChecked
        {
            get => _isActiveChecked;
            set
            {
                if (_isActiveChecked != value)
                {
                    _isActiveChecked = value;
                    OnPropertyChanged();
                    // Auto-search when checkbox changes (but not on initial load)
                    if (!_isInitialLoad)
                    {
                        _ = SearchStudentsAsync();
                    }
                }
            }
        }

        public bool IsGraduatedChecked
        {
            get => _isGraduatedChecked;
            set
            {
                if (_isGraduatedChecked != value)
                {
                    _isGraduatedChecked = value;
                    OnPropertyChanged();
                    // Auto-search when checkbox changes (but not on initial load)
                    if (!_isInitialLoad)
                    {
                        _ = SearchStudentsAsync();
                    }
                }
            }
        }

        public string SearchSemester
        {
            get => _searchSemester;
            set
            {
                _searchSemester = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
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

        public Array StudentStatusValues => Enum.GetValues(typeof(StudentStatus));

        public ICommand SearchCommand => new RelayCommand(async () => await SearchStudentsAsync());

        public ICommand LoadAllStudentsCommand => new RelayCommand(async () => await LoadAllStudentsAsync());

        public ICommand EditStudentCommand => new RelayCommand<Student>(student =>
        {
            if (student != null)
            {
                // Navigate to EditStudentViewModel with the student object
                _navigationService.NavigateTo<EditStudentViewModel>(student);
            }
        });

        public ICommand DeleteStudentCommand => new RelayCommand<Student>(async student =>
        {
            if (student != null)
            {
                await DeleteStudentAsync(student);
            }
        });

        public ICommand BackCommand => new RelayCommand(() =>
        {
            _navigationService.GoBack();
        });

        public ICommand GoToNewStudentCommand => new RelayCommand(() =>
        {
            _navigationService.NavigateTo<NewStudentViewModel>();
        });

        // Public method to refresh students (called when returning from edit)
        public void RefreshStudents()
        {
            _ = LoadAllStudentsAsync();
        }

        private async Task SearchStudentsAsync()
        {
            try
            {
                IsLoading = true;
                
                // Collect selected statuses
                var selectedStatuses = new List<StudentStatus>();
                if (IsWaitingChecked) selectedStatuses.Add(StudentStatus.Waiting);
                if (IsInactiveChecked) selectedStatuses.Add(StudentStatus.Inactive);
                if (IsActiveChecked) selectedStatuses.Add(StudentStatus.Active);
                if (IsGraduatedChecked) selectedStatuses.Add(StudentStatus.Graduated);

                // Allow searching by name and/or ID regardless of checkbox state
                // If no filters at all (no checkboxes, no name, no ID), show all students
                bool hasNameFilter = !string.IsNullOrWhiteSpace(SearchName);
                bool hasIdFilter = !string.IsNullOrWhiteSpace(SearchStudentId);
                bool hasStatusFilter = selectedStatuses.Count > 0;

                if (!hasNameFilter && !hasIdFilter && !hasStatusFilter)
                {
                    // No filters - show all students
                    var allStudents = await _studentService.GetAllStudentsAsync();
                    Students.Clear();
                    foreach (var student in allStudents)
                    {
                        Students.Add(student);
                    }
                }
                else
                {
                    // Has at least one filter - perform search
                    var results = await _studentService.SearchStudentsAsync(
                        hasNameFilter ? SearchName : string.Empty, 
                        hasIdFilter ? SearchStudentId : string.Empty, 
                        hasStatusFilter ? selectedStatuses : null, 
                        SearchSemester);

                    Students.Clear();
                    foreach (var student in results)
                    {
                        Students.Add(student);
                    }
                }

                // Force UI update - notify all related properties
                OnPropertyChanged(nameof(Students));
                OnPropertyChanged(nameof(HasSearchResults));
                OnPropertyChanged(nameof(ShowNoResultsMessage));
                OnPropertyChanged(nameof(NoResultsMessage));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error searching students: {ex.Message}",
                    "Database Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadAllStudentsAsync()
        {
            try
            {
                IsLoading = true;
                var results = await _studentService.GetAllStudentsAsync();

                Students.Clear();
                foreach (var student in results)
                {
                    Students.Add(student);
                }

                // Mark initial load as complete
                _isInitialLoad = false;

                // Force UI update - notify all related properties
                OnPropertyChanged(nameof(Students));
                OnPropertyChanged(nameof(HasSearchResults));
                OnPropertyChanged(nameof(ShowNoResultsMessage));
                OnPropertyChanged(nameof(NoResultsMessage));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error loading students: {ex.Message}",
                    "Database Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // Properties for no results message
        public bool HasSearchResults => Students.Count > 0;

        public bool ShowNoResultsMessage => Students.Count == 0 && !IsLoading;

        public string NoResultsMessage => "No students found matching your search criteria.";

        public bool IsInitialLoad => _isInitialLoad;

        private async Task DeleteStudentAsync(Student student)
        {
            var result = System.Windows.MessageBox.Show(
                "Are you sure you want to delete this student and all related degree plans?",
                "Confirm Delete",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (result != System.Windows.MessageBoxResult.Yes)
                return;

            try
            {
                IsLoading = true;
                bool success = await _studentService.DeleteStudentAsync(student.StudentID);

                if (success)
                {
                    Students.Remove(student);
                    System.Windows.MessageBox.Show(
                        "Student and all related data deleted successfully!",
                        "Success",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error deleting student: {ex.Message}",
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
