using System.Collections;
using System.Collections.Generic;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public interface IStudentRepository
    {
        Student Add(Student student);
        Student GetById(int Id);
        Student GetByUniversityId(string UniversityId);
        Student GetByLoginId(int LoginId);
        IEnumerable<Student> GetByBranchId(int BranchId);
    }
}
