using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using StudentManagementSystem.App.Navigation;
using StudentManagementSystem.Domain;
using StudentManagementSystem.Services.Interfaces;
using System.Collections.Generic;
using System.Windows;
using StudentManagementSystem.App.Views;

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
        private string _newSemesterYear;
        private bool _isNewSemesterSpring;
        private ObservableCollection<string> _availableYears;
        private string _selectedYear;
        private ObservableCollection<string> _availableSemestersForYear;
        private string _selectedSemesterType;

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
            _availableYears = new ObservableCollection<string>();
            _availableSemestersForYear = new ObservableCollection<string>();
            _studentName = string.Empty;
            _newSemesterYear = DateTime.Now.Year.ToString();
            _isNewSemesterSpring = false; // Default to Fall
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

        // When a semester item in the ListBox is selected, update the Year and SemesterType
        // so the rest of the UI switches to that semester automatically.
        public Semester SelectedSemester
        {
            get => _selectedSemester;
            set
            {
                if (_selectedSemester == value) return;

                _selectedSemester = value;
                OnPropertyChanged();

                if (_selectedSemester != null)
                {
                    // Derive year and semester text from the selected Semester
                    string year = _selectedSemester.SchoolYear ?? string.Empty;
                    string semType = _selectedSemester.SemesterValue ? "Spring" : "Fall";

                    // Update backing fields directly to avoid triggering the full selection-change round-trip.
                    // Raise property changed notifications so bindings update.
                    if (_selectedYear != year)
                    {
                        _selectedYear = year;
                        OnPropertyChanged(nameof(SelectedYear));
                    }

                    if (_selectedSemesterType != semType)
                    {
                        _selectedSemesterType = semType;
                        OnPropertyChanged(nameof(SelectedSemesterType));
                    }

                    // Refresh classes for this (now selected) semester asynchronously.
                    // Fire-and-forget is fine here; any exceptions are handled inside UpdateSemesterClassesAsync.
                    _ = UpdateSemesterClassesAsync();
                }
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

        public string NewSemesterYear
        {
            get => _newSemesterYear;
            set
            {
                _newSemesterYear = value;
                OnPropertyChanged();
            }
        }

        public bool IsNewSemesterSpring
        {
            get => _isNewSemesterSpring;
            set
            {
                _isNewSemesterSpring = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNewSemesterFall));
            }
        }

        public bool IsNewSemesterFall
        {
            get => !_isNewSemesterSpring;
            set
            {
                _isNewSemesterSpring = !value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNewSemesterSpring));
            }
        }

        public ObservableCollection<string> AvailableYears
        {
            get => _availableYears;
            set
            {
                _availableYears = value;
                OnPropertyChanged();
            }
        }

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged();
                    if (!string.IsNullOrEmpty(value))
                    {
                        _ = UpdateSemestersForYearAsync();
                    }
                }
            }
        }

        public ObservableCollection<string> AvailableSemestersForYear
        {
            get => _availableSemestersForYear;
            set
            {
                _availableSemestersForYear = value;
                OnPropertyChanged();
            }
        }

        public string SelectedSemesterType
        {
            get => _selectedSemesterType;
            set
            {
                if (_selectedSemesterType != value)
                {
                    _selectedSemesterType = value;
                    OnPropertyChanged();
                    if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(SelectedYear))
                    {
                        _ = UpdateSemesterClassesAsync();
                    }
                }
            }
        }

        // Commands
        public ICommand AddSemesterCommand => new RelayCommand(async () =>
        {
            if (string.IsNullOrWhiteSpace(NewSemesterYear))
            {
                System.Windows.MessageBox.Show("Please enter a school year.", "Validation", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            string semesterText = IsNewSemesterSpring ? "Spring" : "Fall";
            var confirm = System.Windows.MessageBox.Show(
                $"Add {semesterText} {NewSemesterYear} to this degree plan?",
                "Confirm Add Semester",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Question);

            if (confirm == System.Windows.MessageBoxResult.Yes)
            {
                try
                {
                    var newSemester = await AddSemesterAsync(IsNewSemesterSpring, NewSemesterYear.Trim());
                    
                    // DegreePlanService now returns null to indicate a duplicate; show a friendly message here.
                    if (newSemester == null)
                    {
                        System.Windows.MessageBox.Show(
                            "That semester already exists for this degree plan.",
                            "Duplicate Semester",
                            System.Windows.MessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Warning);
                        return;
                    }
                    
                    // Only refresh UI when a new semester was actually created
                    if (newSemester != null)
                    {
                        // Refresh years list
                        var years = Semesters.Select(s => s.SchoolYear).Distinct().OrderBy(y => y).ToList();
                        AvailableYears.Clear();
                        foreach (var year in years)
                        {
                            AvailableYears.Add(year);
                        }
                        
                        // Select the new semester's year and type
                        SelectedYear = NewSemesterYear.Trim();
                        await UpdateSemestersForYearAsync();
                        
                        System.Windows.MessageBox.Show(
                            $"Added new semester: {semesterText} {NewSemesterYear}",
                            "Success",
                            System.Windows.MessageBoxButton.OK,
                            System.Windows.MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(
                        $"Error adding semester: {ex.Message}",
                        "Database Error",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Error);
                }
            }
        });

        public ICommand RemoveSemesterCommand => new RelayCommand(async () =>
        {
            if (!string.IsNullOrEmpty(SelectedYear) && !string.IsNullOrEmpty(SelectedSemesterType))
            {
                bool isSpring = SelectedSemesterType == "Spring";
                var semester = Semesters.FirstOrDefault(s => 
                    s.SchoolYear == SelectedYear && s.SemesterValue == isSpring);

                if (semester != null)
                {
                    var confirm = System.Windows.MessageBox.Show(
                        $"Are you sure you want to remove {semester.DisplayText}?\nThis will also remove all classes within it.",
                        "Confirm Remove Semester",
                        System.Windows.MessageBoxButton.YesNo,
                        System.Windows.MessageBoxImage.Warning);

                    if (confirm == System.Windows.MessageBoxResult.Yes)
                    {
                        await RemoveSemesterAsync(semester);
                        // Refresh the view
                        var years = Semesters.Select(s => s.SchoolYear).Distinct().OrderBy(y => y).ToList();
                        AvailableYears.Clear();
                        foreach (var year in years)
                        {
                            AvailableYears.Add(year);
                        }
                        if (AvailableYears.Count > 0)
                        {
                            SelectedYear = AvailableYears[0];
                        }
                    }
                }
            }
        }, () => !string.IsNullOrEmpty(SelectedYear) && !string.IsNullOrEmpty(SelectedSemesterType));

        public ICommand AddClassToSemesterCommand => new RelayCommand(async () =>
        {
            if (!string.IsNullOrEmpty(SelectedYear) && !string.IsNullOrEmpty(SelectedSemesterType) && SelectedAvailableCourse != null)
            {
                await AddClassToSemesterAsync(SelectedAvailableCourse);
            }
        }, () => !string.IsNullOrEmpty(SelectedYear) && !string.IsNullOrEmpty(SelectedSemesterType) && SelectedAvailableCourse != null);

        public ICommand RemoveClassFromSemesterCommand => new RelayCommand(async () =>
        {
            if (SelectedSemesterClass != null)
            {
                var confirm = System.Windows.MessageBox.Show(
                    $"Are you sure you want to remove {SelectedSemesterClass.Course?.DisplayText} from this semester?",
                    "Confirm Remove Class",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

                if (confirm == System.Windows.MessageBoxResult.Yes)
                {
                    await RemoveClassFromSemesterAsync(SelectedSemesterClass);
                }
            }
        }, () => SelectedSemesterClass != null);

        // UpdateGradeCommand now will show the grade editor (via VM) and persist changes.
        public ICommand UpdateGradeCommand => new RelayCommand<SemesterClass>(async semesterClass =>
        {
            if (semesterClass != null)
            {
                await ShowGradeEditorAsync(semesterClass);
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

                // Load all semesters
                var semesters = await _degreePlan_service_GetSemestersSafe(degreePlanId);
                Semesters.Clear();
                foreach (var semester in semesters)
                {
                    Semesters.Add(semester);
                }

                // Load distinct years
                var years = semesters.Select(s => s.SchoolYear).Distinct().OrderBy(y => y).ToList();
                AvailableYears.Clear();
                foreach (var year in years)
                {
                    AvailableYears.Add(year);
                }

                // Select first year by default
                if (AvailableYears.Count > 0)
                {
                    var firstYear = AvailableYears[0];
                    if (_selectedYear != firstYear)
                    {
                        _selectedYear = firstYear;
                        OnPropertyChanged(nameof(SelectedYear));
                        await UpdateSemestersForYearAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error loading degree plan: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        // small helper to avoid null returns from service
        private async Task<List<Semester>> _degreePlan_service_GetSemestersSafe(int degreePlanId)
        {
            var sems = await _degreePlanService.GetSemestersByDegreePlanIdAsync(degreePlanId);
            return sems?.ToList() ?? new List<Semester>();
        }

        private async Task UpdateSemestersForYearAsync()
        {
            if (string.IsNullOrEmpty(SelectedYear)) return;

            try
            {
                // Filter semesters for the selected year
                var semestersForYear = Semesters
                    .Where(s => s.SchoolYear == SelectedYear)
                    .OrderBy(s => s.SemesterID)
                    .ToList();

                AvailableSemestersForYear.Clear();
                foreach (var semester in semestersForYear)
                {
                    AvailableSemestersForYear.Add(semester.SemesterText);
                }

                // Select first semester for that year
                if (AvailableSemestersForYear.Count > 0)
                {
                    var firstSemester = AvailableSemestersForYear[0];
                    if (_selectedSemesterType != firstSemester)
                    {
                        _selectedSemesterType = firstSemester;
                        OnPropertyChanged(nameof(SelectedSemesterType));
                        await UpdateSemesterClassesAsync();
                    }
                }
                else
                {
                    if (_selectedSemesterType != null)
                    {
                        _selectedSemesterType = null;
                        OnPropertyChanged(nameof(SelectedSemesterType));
                        SemesterClasses.Clear();
                        AvailableCourses.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error updating semesters: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private async Task UpdateSemesterClassesAsync()
        {
            if (string.IsNullOrEmpty(SelectedYear) || string.IsNullOrEmpty(SelectedSemesterType)) return;

            try
            {
                // Find the semester matching the selected year and type
                bool isSpring = SelectedSemesterType == "Spring";
                var semester = Semesters.FirstOrDefault(s => 
                    s.SchoolYear == SelectedYear && s.SemesterValue == isSpring);

                if (semester == null)
                {
                    SemesterClasses.Clear();
                    AvailableCourses.Clear();
                    return;
                }

                // Load classes for this specific semester
                var classes = await _degreePlanService.GetClassesBySemesterAndYearAsync(
                    DegreePlanId, isSpring, SelectedYear);
                SemesterClasses.Clear();
                foreach (var semesterClass in classes)
                {
                    SemesterClasses.Add(semesterClass);
                }

                // Load available courses for this semester type, excluding already assigned anywhere in the same degree plan
                var allCourses = (await _courseService.GetCoursesBySemesterAsync(isSpring))?.ToList() ?? new List<Course>();

                // Get all assigned class ids for the current degree plan across all semesters
                var semesterTasks = Semesters.Select(s => _degreePlanService.GetClassesBySemesterAsync(s.SemesterID)).ToArray();
                var assignedClassIdsSet = new HashSet<int>();
                if (semesterTasks.Length > 0)
                {
                    var results = await Task.WhenAll(semesterTasks);
                    foreach (var semClasses in results)
                    {
                        if (semClasses == null) continue;
                        foreach (var sc in semClasses)
                        {
                            assignedClassIdsSet.Add(sc.ClassID);
                        }
                    }
                }

                // Filter out any course already assigned in the degree plan
                var available = allCourses.Where(c => !assignedClassIdsSet.Contains(c.ClassID)).ToList();

                AvailableCourses.Clear();
                foreach (var course in available)
                {
                    AvailableCourses.Add(course);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error updating classes: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }


        private async Task<Semester> AddSemesterAsync(bool isSpringSemester, string schoolYear)
        {
            try
            {
                var newSemester = await _degreePlanService.AddSemesterAsync(DegreePlanId, isSpringSemester, schoolYear);
                Semesters.Add(newSemester);
                SelectedSemester = newSemester;
                return newSemester;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error adding semester: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                return null;
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
                    AvailableCourses.Clear();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error removing semester: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private async Task AddClassToSemesterAsync(Course course)
        {
            try
            {
                // Find the semester matching the selected year and type
                bool isSpring = SelectedSemesterType == "Spring";
                var semester = Semesters.FirstOrDefault(s => s.SchoolYear == SelectedYear && s.SemesterValue == isSpring);

                if (semester == null)
                {
                    System.Windows.MessageBox.Show(
                        "Could not find the selected semester.",
                        "Error",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Error);
                    return;
                }

                var semesterClass = await _degreePlanService.AssignClassToSemesterAsync(semester.SemesterID, course.ClassID);
                
                // Refresh the view
                await UpdateSemesterClassesAsync();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error adding class to semester: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private async Task RemoveClassFromSemesterAsync(SemesterClass semesterClass)
        {
            try
            {
                bool success = await _degreePlanService.RemoveClassFromSemesterAsync(semesterClass.SemesterClassID);
                if (success)
                {
                    // Refresh the view
                    await UpdateSemesterClassesAsync();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error removing class from semester: {ex.Message}",
                    "Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        // Public: show a grade editor dialog (VM-level) and persist the selected grade.
        public async Task ShowGradeEditorAsync(SemesterClass semesterClass)
        {
            if (semesterClass == null) return;

            try
            {
                // Create and show the GradeDialog (view) from the VM.
                // This keeps the view-model entry point here so code-behind doesn't need to perform DB work.
                var dialog = new GradeDialog(semesterClass.Grade);
                var owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
                if (owner != null) dialog.Owner = owner;

                bool? res = dialog.ShowDialog();
                if (res == true && !string.IsNullOrWhiteSpace(dialog.SelectedGrade))
                {
                    await UpdateGradeAsync(semesterClass, dialog.SelectedGrade);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Error editing grade: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Public: persist a grade for a semester-class and refresh local collection.
        public async Task UpdateGradeAsync(SemesterClass semesterClass, string grade)
        {
            try
            {
                var updated = await _degreePlanService.UpdateGradeAsync(semesterClass.SemesterClassID, grade);
                if (updated != null)
                {
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
                System.Windows.MessageBox.Show(
                    $"Error updating grade: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}