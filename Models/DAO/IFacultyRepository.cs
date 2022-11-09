using System.Collections;
using System.Collections.Generic;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public interface IFacultyRepository
    {
        Faculty GetById(int Id);
        Faculty GetByLoginId(int loginId);
        IEnumerable<Faculty> GetByBranchId(int branchId);
        IEnumerable<Faculty> GetAll(int branchId);
        Faculty GetBySubjectId(int subjectId);
        Faculty GetByEmail(string email);
        Faculty Add(Faculty faculty);
    }
}
