using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StudentAttendanceManagementSystem.Models.DAO
{
    public class SQLStudentSubjectRepository : IStudentSubjectRepository
    {
        private readonly AppDbContext context;
        public SQLStudentSubjectRepository(AppDbContext context)
        {
            this.context = context;
        }

        IEnumerable<StudentSubject> IStudentSubjectRepository.getBySemesterNoStudentId(int semesterNo, int studentId)
        {
            return context.StudentSubject.Where(m => m.SemesterNo==semesterNo && m.StudentId==studentId);
        }
        
        IEnumerable<StudentSubject> IStudentSubjectRepository.GetBySubjectId(int subjectId)
        {
            return context.StudentSubject.Where(m => m.SubjectId == subjectId);
        }

        StudentSubject IStudentSubjectRepository.GetById(int id)
        {
            return context.StudentSubject.FirstOrDefault(m => m.Id == id);
        }
        StudentSubject IStudentSubjectRepository.Update(StudentSubject studentSubject)
        {
            var StudentSubject = context.StudentSubject.Attach(studentSubject);
            StudentSubject.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return studentSubject;
        }
        StudentSubject IStudentSubjectRepository.Add(StudentSubject newstusub)
        {
            context.StudentSubject.Add(newstusub);
            context.SaveChanges();
            return newstusub;
        }
    }
}
