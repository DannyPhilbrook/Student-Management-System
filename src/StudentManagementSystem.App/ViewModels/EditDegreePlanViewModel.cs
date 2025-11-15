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
    public class EditDegreePlanViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IDegreePlanService _degreePlanService;
        private readonly ICourseService _courseService;

        private int _degreePlanId;
        private int _studentId;
        private string _studentName;
        private ObservableCollection<Semester> _semesters;
        private Semester _selectedSemester;
        private ObservableCollection<SemesterClass> _semesterClasses;
        private SemesterClass _selectedSemesterClass;
        private ObservableCollection<Course> _availableCourses;
        private Course _selectedAvailableCourse;

        public EditDegreePlanViewModel(
            INavigationService navigationService,
            IDegreePlanService degreePlanService,
            ICourseService courseService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _degreePlanService = degreePlanService ?? throw new ArgumentNullException(nameof(degreePlanService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));

            _semesters = new ObservableCollection<Semester>();
            _semesterClasses = new ObservableCollection<SemesterClass>();
            _availableCourses = new ObservableCollection<Course>();
            _studentName = string.Empty;
        }

        // Properties
        public int DegreePlanId
        {
            get => _degreePlanId;
            set
            {
                _degreePlanId = value;
                OnPropertyChanged();
            }
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

        public string StudentName
        {
            get => _studentName;
            set
            {
                _studentName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Semester> Semesters
        {
            get => _semesters;
            set
            {
                _semesters = value;
                OnPropertyChanged();
            }
        }

        public Semester SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                _selectedSemester = value;
                OnPropertyChanged();
                _ = LoadSemesterClassesAsync();
            }
        }

        public ObservableCollection<SemesterClass> SemesterClasses
        {
            get => _semesterClasses;
            set
            {
                _semesterClasses = value;
                OnPropertyChanged();
            }
        }

        public SemesterClass SelectedSemesterClass
        {
            get => _selectedSemesterClass;
            set
            {
                _selectedSemesterClass = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Course> AvailableCourses
        {
            get => _availableCourses;
            set
            {
                _availableCourses = value;
                OnPropertyChanged();
            }
        }

        public Course SelectedAvailableCourse
        {
            get => _selectedAvailableCourse;
            set
            {
                _selectedAvailableCourse = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand AddSemesterCommand => new RelayCommand(async () =>
        {
            // TODO: Show dialog to get semester type (Spring/Fall) and year
            // For now, default to Spring and current year
            await AddSemesterAsync(true, DateTime.Now.Year.ToString());
        });

        public ICommand RemoveSemesterCommand => new RelayCommand(async () =>
        {
            if (SelectedSemester != null)
            {
                // TODO: Show confirmation dialog
                await RemoveSemesterAsync(SelectedSemester);
            }
        }, () => SelectedSemester != null);

        public ICommand AddClassToSemesterCommand => new RelayCommand(async () =>
        {
            if (SelectedSemester != null && SelectedAvailableCourse != null)
            {
                await AddClassToSemesterAsync(SelectedAvailableCourse);
            }
        }, () => SelectedSemester != null && SelectedAvailableCourse != null);

        public ICommand RemoveClassFromSemesterCommand => new RelayCommand(async () =>
        {
            if (SelectedSemesterClass != null)
            {
                // TODO: Show confirmation dialog
                await RemoveClassFromSemesterAsync(SelectedSemesterClass);
            }
        }, () => SelectedSemesterClass != null);

        public ICommand UpdateGradeCommand => new RelayCommand<SemesterClass>(async semesterClass =>
        {
            if (semesterClass != null)
            {
                // TODO: Show dialog to input grade
                // For now, just a placeholder
                string grade = "A"; // This should come from a dialog
                await UpdateGradeAsync(semesterClass, grade);
            }
        });

        public ICommand BackCommand => new RelayCommand(() =>
        {
            _navigationService.GoBack();
        });

        // Helper Methods
        public async Task LoadDegreePlanAsync(int degreePlanId, string studentName)
        {
            try
            {
                DegreePlanId = degreePlanId;
                StudentName = studentName;

                // Load semesters
                var semesters = await _degreePlanService.GetSemestersByDegreePlanIdAsync(degreePlanId);
                Semesters.Clear();
                foreach (var semester in semesters)
                {
                    Semesters.Add(semester);
                }

                // Load all available courses
                var courses = await _courseService.GetAllCoursesAsync();
                AvailableCourses.Clear();
                foreach (var course in courses)
                {
                    AvailableCourses.Add(course);
                }

                // Select first semester by default
                if (Semesters.Count > 0)
                {
                    SelectedSemester = Semesters[0];
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading degree plan: {ex.Message}");
            }
        }

        private async Task LoadSemesterClassesAsync()
        {
            if (SelectedSemester == null) return;

            try
            {
                var classes = await _degreePlanService.GetClassesBySemesterAsync(SelectedSemester.SemesterID);
                SemesterClasses.Clear();
                foreach (var semesterClass in classes)
                {
                    SemesterClasses.Add(semesterClass);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading semester classes: {ex.Message}");
            }
        }

        private async Task AddSemesterAsync(bool isSpringSemester, string schoolYear)
        {
            try
            {
                var newSemester = await _degreePlanService.AddSemesterAsync(DegreePlanId, isSpringSemester, schoolYear);
                Semesters.Add(newSemester);
                SelectedSemester = newSemester;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding semester: {ex.Message}");
            }
        }

        private async Task RemoveSemesterAsync(Semester semester)
        {
            try
            {
                bool success = await _degreePlanService.RemoveSemesterAsync(semester.SemesterID);
                if (success)
                {
                    Semesters.Remove(semester);
                    SemesterClasses.Clear();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing semester: {ex.Message}");
            }
        }

        private async Task AddClassToSemesterAsync(Course course)
        {
            try
            {
                var semesterClass = await _degreePlanService.AssignClassToSemesterAsync(SelectedSemester.SemesterID, course.ClassID);
                SemesterClasses.Add(semesterClass);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding class to semester: {ex.Message}");
            }
        }

        private async Task RemoveClassFromSemesterAsync(SemesterClass semesterClass)
        {
            try
            {
                bool success = await _degreePlanService.RemoveClassFromSemesterAsync(semesterClass.SemesterClassID);
                if (success)
                {
                    SemesterClasses.Remove(semesterClass);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing class from semester: {ex.Message}");
            }
        }

        private async Task UpdateGradeAsync(SemesterClass semesterClass, string grade)
        {
            try
            {
                var updated = await _degreePlanService.UpdateGradeAsync(semesterClass.SemesterClassID, grade);
                if (updated != null)
                {
                    // Update the local object
                    int index = SemesterClasses.IndexOf(semesterClass);
                    if (index >= 0)
                    {
                        SemesterClasses[index] = updated;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating grade: {ex.Message}");
            }
        }
    }
}