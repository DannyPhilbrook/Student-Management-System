using System.Windows;
using System.Windows.Controls;

namespace StudentManagementSystem.App
{
    public partial class MainWindow : Window
    {
        public Frame MainFrame { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            MainFrame = this.FindName("ContentFrame") as Frame;
            // Don't navigate here - let App.xaml.cs handle it
        }
    }
}
