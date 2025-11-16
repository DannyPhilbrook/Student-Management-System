using System;
using System.Collections.Generic;
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
        private string _studentId;
        private bool _isSpringSemester;
        private StudentStatus _status;
        private string _comments;
        private string _schoolYear;
        private bool _isLoading;


        public NewStudentViewModel(INavigationService navigationService, IStudentService studentService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));

            _status = StudentStatus.Waiting; // Default to Waiting like WinForms
            _firstName = string.Empty;
            _lastName = string.Empty;
            _studentId = string.Empty;
            _isSpringSemester = false; // Default to Fall
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

        public string StudentId
        {
            get => _studentId;
            set
            {
                _studentId = value;
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
                OnPropertyChanged(nameof(IsFallSemester));
            }
        }

        public bool IsFallSemester
        {
            get => !_isSpringSemester;
            set
            {
                _isSpringSemester = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSpringSemester));
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
            // Show confirmation dialog like WinForms
            var result = System.Windows.MessageBox.Show(
                "Are you sure you wish to create this Student?",
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

                // Parse StudentID - in WinForms it's stored as text but we'll convert to int if possible
                if (!int.TryParse(StudentId, out int studentIdInt))
                {
                    System.Windows.MessageBox.Show(
                        "Student ID must be a valid number.",
                        "Validation Error",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Error);
                    return;
                }

                var student = new Student(
                    FirstName,
                    LastName,
                    IsSpringSemester, // StartingSemester bool
                    Comments,
                    Status,
                    SchoolYear)
                {
                    StudentID = studentIdInt
                };

                await _studentService.AddStudentAsync(student, SchoolYear);

                System.Windows.MessageBox.Show(
                    $"Student {student.FullName} added successfully!",
                    "Success",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);

                _navigationService.GoBack();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error adding student: {ex.Message}",
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

        private bool ValidateInput()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(StudentId) &&
                   !string.IsNullOrWhiteSpace(SchoolYear);
        }
    }
}