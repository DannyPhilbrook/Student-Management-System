using System;

namespace StudentManagementSystem.App.Navigation
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : class;
        void NavigateTo<T>(object parameter) where T : class;
        void GoBack();
        bool CanGoBack { get; }
    }
}
