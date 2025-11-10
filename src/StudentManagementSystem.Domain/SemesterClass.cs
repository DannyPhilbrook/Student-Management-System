using System;

namespace StudentManagementSystem.Domain
{
    public class SemesterClass
    {
        public int SemesterClassID { get; set; }
        public int SemesterID { get; set; }
        public int ClassID { get; set; }
        public string Grade { get; set; }  // A, B, C, D, F

        // Navigation properties (populated when needed)
        public Course Course { get; set; }

        public SemesterClass()
        {
            Grade = string.Empty;
        }

        public SemesterClass(int semesterID, int classID, string grade = "")
        {
            SemesterID = semesterID;
            ClassID = classID;
            Grade = grade;
        }
    }
}