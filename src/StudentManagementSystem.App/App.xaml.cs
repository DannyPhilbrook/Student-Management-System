using System;
using System.Windows;
using StudentManagementSystem.App.Navigation;
using StudentManagementSystem.App.Views;
using StudentManagementSystem.Services.Implementations;
using StudentManagementSystem.Services.Interfaces;

namespace StudentManagementSystem.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize services
            var studentService = new StudentService();
            var courseService = new CourseService();
            var degreePlanService = new DegreePlanService();

            // Initialize navigation service
            var mainWindow = new MainWindow();
            var navigationService = new FrameNavigationService(mainWindow.MainFrame);

            // Set up dependency injection (simple approach)
            ServiceLocator.StudentService = studentService;
            ServiceLocator.CourseService = courseService;
            ServiceLocator.DegreePlanService = degreePlanService;
            ServiceLocator.NavigationService = navigationService;

            // Show main window
            mainWindow.Show();
        }
    }

    // Simple service locator for dependency injection
    public static class ServiceLocator
    {
        public static IStudentService StudentService { get; set; }
        public static ICourseService CourseService { get; set; }
        public static IDegreePlanService DegreePlanService { get; set; }
        public static INavigationService NavigationService { get; set; }
    }
}

