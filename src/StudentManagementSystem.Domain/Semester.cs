using System;
using System.Collections.Generic;

namespace StudentManagementSystem.Domain
{
    public class Semester
    {
        public int SemesterID { get; set; }
        public int DegreePlanID { get; set; }
        public bool SemesterValue { get; set; }  // false=Fall, true=Spring
        public string SchoolYear { get; set; }

        // Navigation properties
        public List<SemesterClass> Classes { get; set; }

        // Computed properties
        public string SemesterText => SemesterValue ? "Spring" : "Fall";
        public string DisplayText => $"{SemesterText} {SchoolYear}";

        public Semester()
        {
            Classes = new List<SemesterClass>();
            SchoolYear = DateTime.Now.Year.ToString();
            SemesterValue = false; // Fall by default
        }

        public Semester(int degreePlanID, bool semesterValue, string schoolYear)
        {
            DegreePlanID = degreePlanID;
            SemesterValue = semesterValue;
            SchoolYear = schoolYear;
            Classes = new List<SemesterClass>();
        }
    }
}