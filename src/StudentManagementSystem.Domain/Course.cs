using System;

namespace StudentManagementSystem.Domain
{
    public class Course
    {
        public int ClassID { get; set; }
        public string CourseNumber { get; set; }
        public string CourseName { get; set; }
        public string Label { get; set; }  // Department/Category label
        public bool Semester { get; set; }  // false=Fall, true=Spring

        // Computed property for display
        public string DisplayText => $"{CourseNumber} - {CourseName}";
        public string SemesterText => Semester ? "Spring" : "Fall";

        public Course()
        {
            CourseNumber = string.Empty;
            CourseName = string.Empty;
            Label = string.Empty;
            Semester = false; // Fall by default
        }

        public Course(string courseNumber, string courseName, string label, bool semester)
        {
            CourseNumber = courseNumber;
            CourseName = courseName;
            Label = label;
            Semester = semester;
        }
    }
}
