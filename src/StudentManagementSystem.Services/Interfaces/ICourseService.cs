using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int classID);
        Task<Course> GetCourseByNumberAsync(string courseNumber);
        Task<IEnumerable<string>> GetDistinctLabelsAsync();
        Task<Course> AddCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int classID);
    }
}