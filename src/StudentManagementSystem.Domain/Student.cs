using System;

namespace StudentManagementSystem.Domain
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool StartingSemester { get; set; }  // false=Fall, true=Spring
        public string Notes { get; set; }
        public StudentStatus StudentStatus { get; set; }
        public string SchoolYear { get; set; }
        public int? DegreePlanID { get; set; }

        // Computed property for display
        public string FullName => $"{FirstName} {LastName}";

        public Student()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Notes = string.Empty;
            SchoolYear = DateTime.Now.Year.ToString();
            StudentStatus = StudentStatus.Waiting;
            StartingSemester = false; // Fall by default
        }

        public Student(string firstName, string lastName, bool startingSemester, string notes, StudentStatus status, string schoolYear)
        {
            FirstName = firstName;
            LastName = lastName;
            StartingSemester = startingSemester;
            Notes = notes;
            StudentStatus = status;
            SchoolYear = schoolYear;
        }
    }
}
