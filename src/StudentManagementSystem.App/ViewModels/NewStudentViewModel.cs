using System;
using System.Windows.Input;
using StudentManagementSystem.App.Navigation;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.App.ViewModels
{
    public class NewStudentViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IStudentService _studentService;

        private string _firstName;
        private string _lastName;
        private string _semester;
        private StudentStatus _status;
        private string _comments;
        private string _schoolYear;
        private bool _isLoading;

        public NewStudentViewModel(INavigationService navigationService, IStudentService studentService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));

            _status = StudentStatus.Active;
            _firstName = string.Empty;
            _lastName = string.Empty;
            _semester = string.Empty;
            _comments = string.Empty;
            _schoolYear = DateTime.Now.Year.ToString();
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string FullName => $"{FirstName} {LastName}".Trim();

        public string Semester
        {
            get => _semester;
            set
            {
                _semester = value;
                OnPropertyChanged();
            }
        }

        public StudentStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public string Comments
        {
            get => _comments;
            set
            {
                _comments = value;
                OnPropertyChanged();
            }
        }

        public string SchoolYear
        {
            get => _schoolYear;
            set
            {
                _schoolYear = value;
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

        public ICommand SubmitCommand => new RelayCommand(async () =>
        {
            if (!ValidateInput())
            {
                // TODO: Show validation error message
                System.Diagnostics.Debug.WriteLine("Please fill out all required fields.");
                return;
            }

            try
            {
                IsLoading = true;

                var student = new Student(
                    FirstName,
                    LastName,
                    Semester.Equals("Spring", StringComparison.OrdinalIgnoreCase), // StartingSemester bool
                    Comments,
                    Status,
                    SchoolYear);

                await _studentService.AddStudentAsync(student);

                // TODO: Show success message
                System.Diagnostics.Debug.WriteLine($"Student {student.FullName} added successfully!");
                _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                // TODO: Show error dialog
                System.Diagnostics.Debug.WriteLine($"Error adding student: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        });

        public ICommand CancelCommand => new RelayCommand(() =>
        {
            // TODO: Show confirmation dialog if there are unsaved changes
            _navigationService.GoBack();
        });

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(Semester) &&
                   !string.IsNullOrWhiteSpace(SchoolYear);
        }
    }
}