using System.Collections.Generic;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public interface IStudentSubjectRepository
    {
        IEnumerable<StudentSubject> getBySemesterNoStudentId(int semesterNo, int studentId);
        IEnumerable<StudentSubject> GetBySubjectId(int subjectId);
        StudentSubject Update(StudentSubject studentSubject);
        StudentSubject GetById(int id);
        StudentSubject Add(StudentSubject studentSubject);
    }
}
