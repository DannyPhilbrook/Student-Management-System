using System;
using System.Windows.Input;
using StudentManagementSystem.App.Navigation;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.App.ViewModels
{
    public class EditStudentViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IStudentService _studentService;

        private int _studentId;
        private string _firstName;
        private string _lastName;
        private bool _startingSemester;
        private string _notes;
        private StudentStatus _studentStatus;
        private string _schoolYear;

        public EditStudentViewModel(INavigationService navigationService, IStudentService studentService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));

            _firstName = string.Empty;
            _lastName = string.Empty;
            _notes = string.Empty;
            _schoolYear = DateTime.Now.Year.ToString();
            _studentStatus = StudentStatus.Active;
            _startingSemester = true;
        }

        public int StudentId
        {
            get => _studentId;
            set
            {
                _studentId = value;
                OnPropertyChanged();
            }
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

        public bool StartingSemester
        {
            get => _startingSemester;
            set
            {
                _startingSemester = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SemesterText));
                OnPropertyChanged(nameof(IsFallSemester));
            }
        }

        public bool IsFallSemester
        {
            get => !_startingSemester;
            set
            {
                _startingSemester = !value;
                OnPropertyChanged(nameof(StartingSemester));
                OnPropertyChanged(nameof(SemesterText));
                OnPropertyChanged();
            }
        }

        public string SemesterText => StartingSemester ? "Spring" : "Fall";

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        public StudentStatus StudentStatus
        {
            get => _studentStatus;
            set
            {
                _studentStatus = value;
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

        public Array StudentStatusValues => Enum.GetValues(typeof(StudentStatus));

        public ICommand SaveCommand => new RelayCommand(async () =>
        {
            if (!ValidateInput())
            {
                System.Diagnostics.Debug.WriteLine("Please fill out all required fields.");
                return;
            }

            try
            {
                var student = new Student(
                    FirstName,
                    LastName,
                    StartingSemester,
                    Notes,
                    StudentStatus,
                    SchoolYear)
                {
                    StudentID = StudentId
                };

                await _studentService.UpdateStudentAsync(student);

                System.Diagnostics.Debug.WriteLine($"Student {student.FullName} updated successfully!");
                _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating student: {ex.Message}");
            }
        });

        public ICommand DeleteCommand => new RelayCommand(async () =>
        {
            try
            {
                bool success = await _studentService.DeleteStudentAsync(StudentId);

                if (success)
                {
                    System.Diagnostics.Debug.WriteLine($"Student deleted successfully!");
                    _navigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting student: {ex.Message}");
            }
        });

        public ICommand CancelCommand => new RelayCommand(() =>
        {
            _navigationService.GoBack();
        });

        public void LoadStudent(Student student)
        {
            if (student != null)
            {
                StudentId = student.StudentID;
                FirstName = student.FirstName;
                LastName = student.LastName;
                StartingSemester = student.StartingSemester;
                Notes = student.Notes;
                StudentStatus = student.StudentStatus;
                SchoolYear = student.SchoolYear;
            }
        }

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(SchoolYear);
        }
    }
}
