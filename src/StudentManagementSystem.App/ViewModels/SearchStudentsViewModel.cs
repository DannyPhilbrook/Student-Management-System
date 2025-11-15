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
    public class SearchStudentsViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IStudentService _studentService;

        private string _searchName;
        private string _searchStudentId;
        private StudentStatus? _searchStatus;
        private string _searchSemester;
        private ObservableCollection<Student> _students;
        private Student _selectedStudent;
        private bool _isLoading;

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

        public StudentStatus? SearchStatus
        {
            get => _searchStatus;
            set
            {
                _searchStatus = value;
                OnPropertyChanged();
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
                // TODO: Navigate to edit student view with parameter
                System.Diagnostics.Debug.WriteLine($"Edit student: {student.FullName}");
                // _navigationService.NavigateTo<EditStudentViewModel>(student);
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

        private async Task SearchStudentsAsync()
        {
            try
            {
                IsLoading = true;
                var results = await _studentService.SearchStudentsAsync(SearchName, SearchStudentId, SearchStatus, SearchSemester);
                
                Students.Clear();
                foreach (var student in results)
                {
                    Students.Add(student);
                }
            }
            catch (Exception ex)
            {
                // TODO: Show error dialog
                System.Diagnostics.Debug.WriteLine($"Error searching students: {ex.Message}");
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
            }
            catch (Exception ex)
            {
                // TODO: Show error dialog
                System.Diagnostics.Debug.WriteLine($"Error loading students: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteStudentAsync(Student student)
        {
            try
            {
                // TODO: Show confirmation dialog
                // For now, just delete directly
                IsLoading = true;
                bool success = await _studentService.DeleteStudentAsync(student.StudentID);
                
                if (success)
                {
                    Students.Remove(student);
                    // TODO: Show success message
                    System.Diagnostics.Debug.WriteLine($"Student {student.FullName} deleted successfully");
                }
            }
            catch (Exception ex)
            {
                // TODO: Show error dialog
                System.Diagnostics.Debug.WriteLine($"Error deleting student: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
