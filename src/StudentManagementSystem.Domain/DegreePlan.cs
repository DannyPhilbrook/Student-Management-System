using System;
using System.Collections.Generic;

namespace StudentManagementSystem.Domain
{
    public class DegreePlan
    {
        public int DegreePlanID { get; set; }
        public int StudentID { get; set; }

        // Navigation properties (populated when needed)
        public List<Semester> Semesters { get; set; }

        public DegreePlan()
        {
            Semesters = new List<Semester>();
        }
    }
}