using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> GetStudentByIdAsync(int studentID);
        Task<Student> GetStudentByStudentIdAsync(string studentId);
        Task<IEnumerable<Student>> SearchStudentsAsync(string name, string studentId, StudentStatus? status, string semester);
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int studentID);
    }
}