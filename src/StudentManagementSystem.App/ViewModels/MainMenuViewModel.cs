using System;
using System.Windows.Input;
using StudentManagementSystem.App.Navigation;

namespace StudentManagementSystem.App.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;

        public MainMenuViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public ICommand GoToNewStudentCommand => new RelayCommand(() =>
        {
            _navigationService.NavigateTo<NewStudentViewModel>();
        });

        public ICommand GoToSearchStudentsCommand => new RelayCommand(() =>
        {
            _navigationService.NavigateTo<SearchStudentsViewModel>();
        });

        public ICommand GoToCoursesCommand => new RelayCommand(() =>
        {
            _navigationService.NavigateTo<CoursesViewModel>();
        });
    }
}
