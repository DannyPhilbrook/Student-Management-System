using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;
using StudentManagementSystem.App.Views;

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
                { typeof(ViewModels.MainMenuViewModel), typeof(MainMenuView) },
                { typeof(ViewModels.NewStudentViewModel), typeof(NewStudentView) },
                { typeof(ViewModels.SearchStudentsViewModel), typeof(SearchStudentsView) },
                { typeof(ViewModels.CoursesViewModel), typeof(CoursesView) },
                { typeof(ViewModels.NewClassViewModel), typeof(NewClassView) },
                { typeof(ViewModels.EditClassViewModel), typeof(EditClassView) }
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
                    _frame.Navigate(view, parameter);
                }
            }
        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }

        public bool CanGoBack => _frame.CanGoBack;
    }
}
