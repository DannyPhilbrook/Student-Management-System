using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;
using StudentManagementSystem.App.Views;
using StudentManagementSystem.App.ViewModels;

namespace StudentManagementSystem.App.Navigation
{
    public class FrameNavigationService : INavigationService
    {
        private readonly Frame _frame;
        private readonly Dictionary<Type, Type> _viewMap;

        public FrameNavigationService(Frame frame)
        {
            _frame = frame;
            _viewMap = new Dictionary<Type, Type>
            {
                { typeof(MainMenuViewModel), typeof(MainMenuView) },
                { typeof(NewStudentViewModel), typeof(NewStudentView) },
                { typeof(SearchStudentsViewModel), typeof(SearchStudentsView) },
                { typeof(EditStudentViewModel), typeof(EditStudentView) },
                { typeof(CoursesViewModel), typeof(CoursesView) },
                { typeof(NewClassViewModel), typeof(NewClassView) },
                { typeof(EditClassViewModel), typeof(EditClassView) },
                { typeof(EditDegreePlanViewModel), typeof(EditDegreePlanView) }
            };
        }

        public void NavigateTo<T>() where T : class
        {
            NavigateTo<T>(null);
        }

        public void NavigateTo<T>(object parameter) where T : class
        {
            if (_viewMap.TryGetValue(typeof(T), out Type viewType))
            {
                var view = Activator.CreateInstance(viewType) as Page;

                if (view != null)
                {
                    // Create ViewModel with services from ServiceLocator
                    var viewModel = CreateViewModel<T>();

                    if (viewModel != null)
                    {
                        view.DataContext = viewModel;

                        // Handle parameter passing for specific ViewModels
                        if (parameter != null)
                        {
                            if (viewModel is EditStudentViewModel editStudentVM && parameter is Domain.Student student)
                            {
                                editStudentVM.LoadStudent(student);
                            }
                            else if (viewModel is EditDegreePlanViewModel editDegreePlanVM && parameter is DegreePlanParameter dpParam)
                            {
                                // Load degree plan asynchronously
                                _ = editDegreePlanVM.LoadDegreePlanAsync(dpParam.DegreePlanId, dpParam.StudentName);
                            }
                            else if (viewModel is EditClassViewModel editClassVM && parameter is Domain.Course course)
                            {
                                editClassVM.LoadCourse(course);
                            }
                        }
                    }

                    _frame.Navigate(view);
                }
            }
        }

        private object CreateViewModel<T>() where T : class
        {
            var viewModelType = typeof(T);

            // Create ViewModels with their dependencies
            if (viewModelType == typeof(MainMenuViewModel))
                return new MainMenuViewModel(ServiceLocator.NavigationService);

            if (viewModelType == typeof(NewStudentViewModel))
                return new NewStudentViewModel(ServiceLocator.NavigationService, ServiceLocator.StudentService);

            if (viewModelType == typeof(SearchStudentsViewModel))
                return new SearchStudentsViewModel(ServiceLocator.NavigationService, ServiceLocator.StudentService);

            if (viewModelType == typeof(EditStudentViewModel))
                return new EditStudentViewModel(ServiceLocator.NavigationService, ServiceLocator.StudentService);

            if (viewModelType == typeof(CoursesViewModel))
                return new CoursesViewModel(ServiceLocator.NavigationService, ServiceLocator.CourseService);

            if (viewModelType == typeof(NewClassViewModel))
                return new NewClassViewModel(ServiceLocator.NavigationService, ServiceLocator.CourseService);

            if (viewModelType == typeof(EditClassViewModel))
                return new EditClassViewModel(ServiceLocator.NavigationService, ServiceLocator.CourseService);

            if (viewModelType == typeof(EditDegreePlanViewModel))
                return new EditDegreePlanViewModel(ServiceLocator.NavigationService, ServiceLocator.DegreePlanService, ServiceLocator.CourseService);

            return null;
        }

        public void GoBack()
        {
            if (!_frame.CanGoBack)
                return;

            // Subscribe to the Navigated event once so we can detect the destination page
            // and trigger a refresh if it's SearchStudentsViewModel.
            NavigatedEventHandler handler = null;
            handler = (sender, args) =>
            {
                try
                {
                    if (_frame.Content is Page page && page.DataContext is SearchStudentsViewModel searchVm)
                    {
                        // Trigger the view model to reload data
                        searchVm.RefreshStudents();
                    }
                }
                finally
                {
                    // Unsubscribe to avoid memory leaks / multiple calls
                    _frame.Navigated -= handler;
                }
            };

            _frame.Navigated += handler;

            // Perform the actual back navigation
            _frame.GoBack();
        }

        public bool CanGoBack => _frame.CanGoBack;
    }

    // Helper class for passing degree plan parameters
    public class DegreePlanParameter
    {
        public int DegreePlanId { get; set; }
        public string StudentName { get; set; }
    }
}
