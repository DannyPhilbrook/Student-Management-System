using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StudentManagementSystem.Domain;

namespace StudentManagementSystem.Services.Interfaces
{
    public interface IDegreePlanService
    {
        Task<DegreePlan> GetDegreePlanByIdAsync(int degreePlanId);
        Task<IEnumerable<DegreePlan>> GetDegreePlansByStudentIdAsync(int studentId);
        Task<DegreePlan> CreateDegreePlanAsync(int studentId);
        Task<DegreePlan> UpdateDegreePlanAsync(DegreePlan degreePlan);
        Task<bool> DeleteDegreePlanAsync(int degreePlanId);

        // Semester management
        Task<IEnumerable<Semester>> GetSemestersByDegreePlanIdAsync(int degreePlanId);
        Task<Semester> AddSemesterAsync(int degreePlanId, bool isSpringSemester, string schoolYear);
        Task<bool> RemoveSemesterAsync(int semesterId);

        // Class assignment management
        Task<IEnumerable<SemesterClass>> GetClassesBySemesterAsync(int semesterId);
        Task<IEnumerable<SemesterClass>> GetClassesBySemesterAndYearAsync(int degreePlanId, bool isSpringSemester, string schoolYear);
        Task<SemesterClass> AssignClassToSemesterAsync(int semesterId, int classId);
        Task<bool> RemoveClassFromSemesterAsync(int semesterClassId);
        Task<SemesterClass> UpdateGradeAsync(int semesterClassId, string grade);
    }
}