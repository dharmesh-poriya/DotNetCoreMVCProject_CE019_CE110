using System.Collections.Generic;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public interface ISubjectRepository
    {
        Subject Add(Subject subject);
        Subject GetById(int Id);
        Subject GetBySubjectCode(string code);
        IEnumerable<Subject> GetAllByBranchId(int BranchId);
        IEnumerable<Subject> GetAllBySemesterBranchId(int SemesterNo, int BranchId);
    }
}
